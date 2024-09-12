using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using MetX.Fimm;
using MetX.Fimm.Scripts;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.IO;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

#pragma warning disable 414
namespace MetX.Windows.Controls
{

    public class GuiContext : ContextBase
    {
        public static readonly List<QuickScriptOutput> OutputWindows = new();
        public static bool ScriptIsRunning { get; private set; }

        private static readonly object _syncRoot = new();


        public GuiContext(IGenerationHost host = null) : base(Shared.Dirs.CurrentTemplateFolderPath, host)
        {
        }

        public static void ViewInNewQuickScriptOutputWindow(IRunQuickScript caller, XlgQuickScript script, string title, string output, IGenerationHost host)
        {
            var quickScriptOutput = new QuickScriptOutput(script, caller, title, output, host);
            OutputWindows.Add(quickScriptOutput);

            if (caller.ToolWindow != null) 
                caller.ToolWindow.Show(quickScriptOutput);
            else if(host != null)
            {
                quickScriptOutput.Bounds = host.Boundary;
                quickScriptOutput.Show();
            }
        }

        public static QuickScriptOutput ViewInNewQuickScriptOutputWindow(string title, string text, bool addLineNumbers, List<int> keyLines, IGenerationHost host, QuickScriptOutput putNextToThisWindow = null)
        {
            if (text.IsEmpty())
                return null;

            var quickScriptOutput = QuickScriptOutput.View(title, text, addLineNumbers, keyLines, !addLineNumbers, host, putNextToThisWindow);
            OutputWindows.Add(quickScriptOutput);
            return quickScriptOutput;
        }

        public static void RunQuickScript(IRunQuickScript caller, XlgQuickScript scriptToRun, IShowText targetOutput, IGenerationHost host)
        {
            var callerIsAWinForm = caller is Form;
            Form callerAsWinForm = null;
            {
                callerAsWinForm = caller as Form;
                if (callerAsWinForm is {InvokeRequired: true})
                {
                    callerAsWinForm.Invoke(
                        new Action<ScriptRunningWindow, XlgQuickScript, IShowText, IGenerationHost>(RunQuickScript),
                        caller, scriptToRun, targetOutput, host);
                    return;
                }
            }
            var lockTaken = false;
            Monitor.TryEnter(_syncRoot, ref lockTaken);
            if (!lockTaken) return;

            try
            {
                ScriptIsRunning = true;
                if (scriptToRun.Destination == QuickScriptDestination.File)
                {
                    if (string.IsNullOrEmpty(scriptToRun.DestinationFilePath))
                    {
                        if(callerAsWinForm != null)
                        {
                            MessageBox.Show(callerAsWinForm, "Please supply an output filename.", "OUTPUT FILE PATH REQUIRED");
                            callerAsWinForm.Controls["DestinationParam"]?.Focus();
                        }
                        else
                            MessageBox.Show("Please supply an output filename.", "OUTPUT FILE PATH REQUIRED");
                        return;
                    }

                    if (!File.Exists(scriptToRun.DestinationFilePath))
                    {
                        Directory.CreateDirectory(scriptToRun.DestinationFilePath.TokensBeforeLast(@"\"));
                    }
                }

                var fireAndForget = scriptToRun.Destination != QuickScriptDestination.TextBox;
                var runResult = Run(caller, scriptToRun, host, fireAndForget);
                if (runResult.InputMissing) 
                    callerAsWinForm?.Controls["InputParam"]?.Focus();

                if (!runResult.KeepGoing || fireAndForget)
                    return;

                if(runResult.GatheredOutput.IsEmpty())
                    runResult.GatheredOutput = "No output was generated.";

                try
                {
                    switch (scriptToRun.Destination)
                    {
                        case QuickScriptDestination.TextBox:
                            if (targetOutput == null)
                            {
                                CloseAllWindows();
                                ViewInNewQuickScriptOutputWindow(
                                    caller,
                                    scriptToRun,
                                    scriptToRun.Name + " at " + DateTime.Now.ToString("G"),
                                    runResult.GatheredOutput,
                                    host);
                            }
                            else
                            {
                                targetOutput.Title = scriptToRun.Name + " at " + DateTime.Now.ToString("G");
                                targetOutput.TextToShow = runResult.GatheredOutput;
                            }

                            break;

                        case QuickScriptDestination.Clipboard:
                        case QuickScriptDestination.Notepad:
                        case QuickScriptDestination.Folder:
                        case QuickScriptDestination.File:
                            // Nothing to do. The executable does this
                            break;
                    }
                }
                catch (Exception ex)
                {
                    host.MessageBox.Show(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                host.MessageBox.Show(ex.ToString());
            }
            finally
            {
                ScriptIsRunning = false;
                Monitor.Exit(_syncRoot);
            }
        }

        private static RunResult Run(
            IRunQuickScript caller, 
            XlgQuickScript scriptToRun, 
            IGenerationHost host, 
            bool fireAndForget)
        {
            var wallaby = new Wallaby(host);
            var result = wallaby.BuildActualizeAndCompileQuickScript(scriptToRun);

            if (!result.CompileSuccessful)
            {
                var source = result.OutputFiles["QuickScriptProcessor.cs"].Value;
                var finalDetails = result.FinalDetails(out var keyLines);

                CloseAllWindows();
                
                var sourceCodeWindow = ViewInNewQuickScriptOutputWindow("Source for QuickScriptProcessor.cs", source, true, keyLines, host);
                sourceCodeWindow?.Find("|Error");

                ViewInNewQuickScriptOutputWindow("Error detail / Compile results", finalDetails, false, null, host, sourceCodeWindow);

                return new RunResult
                {
                    ActualizationResult = result,
                    ErrorOutput = finalDetails,
                };
            }

            string parameters = result.Settings.Script.AsParameters();

            string errorOutput = "";
            string gatherOutputAndErrors = "";
            if (fireAndForget)
            {
                FileSystem.FireAndForget(result.DestinationExecutableFilePath, parameters, Environment.GetEnvironmentVariable("TEMP"), ProcessWindowStyle.Hidden);
            }
            else
            {
                gatherOutputAndErrors = FileSystem.GatherOutputAndErrors(result.DestinationExecutableFilePath, parameters, out errorOutput, Environment.GetEnvironmentVariable("TEMP"), 30, ProcessWindowStyle.Hidden);
            }

            var runResult = new RunResult
            {
                ActualizationResult = result,
                GatheredOutput = gatherOutputAndErrors,
                KeepGoing = true,
                ErrorOutput = errorOutput,
            };
            if (runResult.ErrorOutput.IsNotEmpty())
            {
                runResult.KeepGoing = false;
                return runResult;
            }

            runResult.KeepGoing = true;
            caller.Progress();
            return runResult;
        }

        public static void CloseAllWindows()
        {
            foreach (var window in OutputWindows)
            {
                try
                {
                    window.Close();
                }
                catch
                {
                    // ignored
                }
            }

            OutputWindows.Clear();
        }
    }
}
