using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

namespace MetX.Windows.Controls;

public class ConsoleContext : ContextBase
{
    public static readonly List<QuickScriptOutput> OutputWindows = new();
    public static bool ScriptIsRunning { get; private set; }

    private static readonly object MScriptSyncRoot = new();


    public ConsoleContext(IGenerationHost host = null) : base(Shared.Dirs.CurrentTemplateFolderPath, host)
    {
    }

    public static void ViewInNewQuickScriptOutputWindow(IRunQuickScript caller, XlgQuickScript script, string title, string output)
    {
        var quickScriptOutput = new QuickScriptOutput(script, caller, title, output, caller.ToolWindow.Host);
        OutputWindows.Add(quickScriptOutput);
        caller.ToolWindow.Show(quickScriptOutput);
    }

    public static QuickScriptOutput ViewInNewQuickScriptOutputWindow(string title, string text, bool addLineNumbers, List<int> keyLines, IGenerationHost host, QuickScriptOutput putNextToThisWindow = null)
    {
        var quickScriptOutput = QuickScriptOutput.View(title, text, addLineNumbers, keyLines, !addLineNumbers, host, putNextToThisWindow);
        OutputWindows.Add(quickScriptOutput);
        return quickScriptOutput;
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

    public static void RunQuickScript(ScriptRunningWindow caller, XlgQuickScript scriptToRun, IShowText targetOutput, IGenerationHost host)
    {
        if (caller.InvokeRequired)
        {
            caller.Invoke(new Action<ScriptRunningWindow, XlgQuickScript, IShowText, IGenerationHost>(RunQuickScript), caller, scriptToRun, targetOutput, host);
            return;
        }

        var lockTaken = false;
        Monitor.TryEnter(MScriptSyncRoot, ref lockTaken);
        if (!lockTaken) return;

        try
        {
            ScriptIsRunning = true;
            if (scriptToRun.Destination == QuickScriptDestination.File)
            {
                if (string.IsNullOrEmpty(scriptToRun.DestinationFilePath))
                {
                    MessageBox.Show(caller, "Please supply an output filename.", "OUTPUT FILE PATH REQUIRED");
                    caller.SetFocus("DestinationParam");
                    return;
                }

                if (!File.Exists(scriptToRun.DestinationFilePath))
                {
                    Directory.CreateDirectory(scriptToRun.DestinationFilePath.TokensBeforeLast(@"\"));
                }
            }

            var runResult = Run(caller, host.Context, scriptToRun, host, scriptToRun.Destination != QuickScriptDestination.TextBox);
            if (runResult.InputMissing)
                caller.SetFocus("InputParam");
            //if (runResult.ErrorOutput.IsNotEmpty() && runResult.ErrorOutput.Contains(": error"))
            //    host.MessageBox.Show(runResult.ErrorOutput);

            if (!runResult.KeepGoing || runResult.GatheredOutput.IsEmpty())
                return;

            try
            {
                switch (scriptToRun.Destination)
                {
                    case QuickScriptDestination.TextBox:
                        if (targetOutput == null)
                        {
                            ViewInNewQuickScriptOutputWindow(
                                caller,
                                scriptToRun,
                                scriptToRun.Name + " at " + DateTime.Now.ToString("G"),
                                runResult.GatheredOutput);
                        }
                        else
                        {
                            targetOutput.Title = scriptToRun.Name + " at " + DateTime.Now.ToString("G");
                            targetOutput.TextToShow = runResult.GatheredOutput;
                            //targetOutput.TextToShow = runResult.QuickScriptProcessor.Output.ToString();
                        }

                        break;

                    case QuickScriptDestination.Clipboard:
                        // Nothing to do. The executable did this
                        /* 
                            Clipboard.Clear();
                            var textForClipboard = runResult.QuickScriptProcessor.OutputStringBuilder.ToString();
                            Clipboard.SetText(textForClipboard);
                            */
                        break;
                        
                    case QuickScriptDestination.Notepad:
                        // Nothing to do. The executable did this
                        /*
                            QuickScriptWorker.ViewFile(caller.Host, runResult.QuickScriptProcessor.Output.FilePath);
                            */
                        break;

                    case QuickScriptDestination.File:
                        // Nothing to do. The executable did this
                        /*
                            runResult.QuickScriptProcessor.Output?.Finish();
                            runResult.QuickScriptProcessor.Output = null;
                            QuickScriptWorker.ViewFile(caller.Host, scriptToRun.DestinationFilePath);
                            */
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
            Monitor.Exit(MScriptSyncRoot);
        }
    }

    private static RunResult Run(ScriptRunningWindow caller, ContextBase @base, XlgQuickScript scriptToRun, IGenerationHost host, bool fireAndForget)
    {
        var wallaby = new Wallaby(host);
        var result = wallaby.BuildActualizeAndCompileQuickScript(scriptToRun);

        if (!result.CompileSuccessful)
        {
            CloseAllWindows();
            QuickScriptOutput.LastLocation = Rectangle.Empty;

            var source = result.OutputFiles["QuickScriptProcessor.cs"].Value;
            var finalDetails = result.FinalDetails(out var keyLines);
                
            var sourceCodeWindow = ViewInNewQuickScriptOutputWindow("Source for QuickScriptProcessor.cs", source, true, keyLines, host);
            sourceCodeWindow.Find("|Error");

            ViewInNewQuickScriptOutputWindow("Error detail / Compile results", finalDetails, false, null, host, sourceCodeWindow);

            return new RunResult
            {
                ActualizationResult = result,
                ErrorOutput = finalDetails,
            };
        }

        string parameters = result.Settings.Script.AsParameters();

        string errorOutput = "";
        if (fireAndForget)
        {
            FileSystem.FireAndForget(result.DestinationExecutableFilePath, parameters, Environment.GetEnvironmentVariable("TEMP"), ProcessWindowStyle.Hidden);
        }

        var runResult = new RunResult
        {
            ActualizationResult = result,
            GatheredOutput = fireAndForget
                ? ""
                : FileSystem.GatherOutputAndErrors(result.DestinationExecutableFilePath, parameters, out errorOutput, Environment.GetEnvironmentVariable("TEMP"), 30, ProcessWindowStyle.Hidden),
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
}