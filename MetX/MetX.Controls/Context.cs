using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

using MetX.Standard;
using MetX.Standard.Library;
using MetX.Standard.Pipelines;
using MetX.Standard.Scripts;
using MetX.Windows.Library;
using MetX.Standard.Interfaces;

#pragma warning disable 414
namespace MetX.Controls
{

    public class Context : ContextBase
    {
        public static readonly List<QuickScriptOutput> OutputWindows = new List<QuickScriptOutput>();
        public static RegistryKey AppDataRegistry;
        private static bool _scriptIsRunning;
        private static readonly object MScriptSyncRoot = new object();

        public static string GetLastKnownPath()
        {
            var openedKey = false;
            if (AppDataRegistry == null)
            {
                AppDataRegistry = Application.UserAppDataRegistry;
                openedKey = true;
            }

            if (AppDataRegistry == null)
            {
                return null;
            }

            var lastKnownPath = AppDataRegistry.GetValue("LastQuickScriptPath") as string;

            if (!openedKey || AppDataRegistry == null)
            {
                return null;
            }

            AppDataRegistry.Close();
            AppDataRegistry = null;
            return lastKnownPath;
        }

        public Context(IGenerationHost host = null) : base(host)
        {
        }

        public static void OpenNewOutput(IRunQuickScript caller, XlgQuickScript script, string title, string output)
        {
            var quickScriptOutput = new QuickScriptOutput(script, caller, title, output, caller.Window.Host);
            OutputWindows.Add(quickScriptOutput);
            quickScriptOutput.Show(caller.Window);
            quickScriptOutput.BringToFront();
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
                _scriptIsRunning = true;
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

                var runResult = Run(caller, host.Context, scriptToRun, host);
                if (runResult.InputMissing)
                    caller.SetFocus("InputParam");
                if (runResult.Error != null)
                    host.MessageBox.Show(runResult.Error.ToString());
                if (!runResult.KeepGoing || runResult.QuickScriptProcessor?.Output == null || runResult.QuickScriptProcessor.Output.Length == 0)
                {
                    return;
                }

                try
                {
                    switch (scriptToRun.Destination)
                    {
                        case QuickScriptDestination.TextBox:
                            if (targetOutput == null)
                            {
                                OpenNewOutput(
                                    caller,
                                    scriptToRun,
                                    scriptToRun.Name + " at " + DateTime.Now.ToString("G"),
                                    runResult.QuickScriptProcessor.OutputStringBuilder != null
                                        ? runResult.QuickScriptProcessor.OutputStringBuilder.ToString()
                                        : runResult.QuickScriptProcessor.Output.ToString());
                            }
                            else
                            {
                                targetOutput.Title = scriptToRun.Name + " at " + DateTime.Now.ToString("G");
                                targetOutput.TextToShow = runResult.QuickScriptProcessor.Output.ToString();
                            }

                            break;

                        case QuickScriptDestination.Clipboard:
                        {
                            Clipboard.Clear();
                            var textForClipboard = runResult.QuickScriptProcessor.OutputStringBuilder.ToString();
                            Clipboard.SetText(textForClipboard);
                            break;
                        }
                        case QuickScriptDestination.Notepad:

                            // QuickScriptWorker.ViewFileInNotepad(scriptToRun.DestinationFilePath);
                            QuickScriptWorker.ViewFile(caller.Host, runResult.QuickScriptProcessor.Output.FilePath);

                            // QuickScriptWorker.ViewTextInNotepad(runResult.QuickScriptProcessor.Output.ToString(), false);
                            break;

                        case QuickScriptDestination.File:
                            runResult.QuickScriptProcessor.Output?.Finish();
                            runResult.QuickScriptProcessor.Output = null;
                            QuickScriptWorker.ViewFile(caller.Host, scriptToRun.DestinationFilePath);

                            // File.WriteAllText(scriptToRun.DestinationFilePath, runResult.QuickScriptProcessor.Output.ToString());
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
                _scriptIsRunning = false;
                Monitor.Exit(MScriptSyncRoot);
            }
        }

        private static RunResult Run(ScriptRunningWindow caller, ContextBase @base, XlgQuickScript scriptToRun, IGenerationHost host)
        {
            var settings = QuickScriptProcessorFactory.BuildSettings(scriptToRun, false, false, host);
            var actualizeResult = QuickScriptProcessorFactory.ActualizeAndCompile(settings);
            if (!actualizeResult.CompileSuccessful) return new RunResult
            {
                ActualizationResult = actualizeResult,
                Error = new Exception(actualizeResult.CompileErrorText),
            };

            var runResult = new RunResult
            {
                ActualizationResult = actualizeResult,
                QuickScriptProcessor = actualizeResult.AsBaseLineProcessor(),
                KeepGoing = true,
            };
            if (runResult.QuickScriptProcessor == null)
            {
                runResult.KeepGoing = false;
                return runResult;
            }

            runResult.QuickScriptProcessor.Host = caller.Window.Host;
            var inputResult = runResult.QuickScriptProcessor.ReadInput(scriptToRun.Input);
            switch (inputResult)
            {
                case null:
                    runResult.KeepGoing = false;
                    return runResult;

                case false:
                    return runResult;

                // True keep going
            }

            var outputSetupCorrectly = runResult.QuickScriptProcessor.SetupOutput(scriptToRun.Destination, ref runResult.QuickScriptProcessor.DestinationFilePath);
            if (!outputSetupCorrectly)
            {
                if(runResult.Error == null) 
                    runResult.Error = new Exception("Output could not be set up correctly. Locked file?");
                return runResult;
            }

            // Start
            try
            {
                if (!runResult.QuickScriptProcessor.Start())
                {
                    runResult.StartReturnedFalse = true;
                    caller.Progress();
                    return runResult;
                }
            }
            catch (Exception ex)
            {
                runResult.Error = new Exception("Error running Start:" + Environment.NewLine + ex);
                runResult.KeepGoing = false;
                caller.Progress();
                return runResult;
            }

            // Process each line
            var index = 0;
            do
            {
                var currLine = runResult.QuickScriptProcessor.InputStream.ReadLine();
                if (string.IsNullOrEmpty(currLine))
                {
                    continue;
                }

                try
                {
                    if (runResult.QuickScriptProcessor.ProcessLine(currLine, index))
                    {
                        if (++index % 10 == 0 || index < 10)
                        {
                            caller.Progress(index);
                        }

                        continue;
                    }

                    runResult.ProcessLineReturnedFalse = true;
                    caller.Progress();
                    return runResult;
                }
                catch (Exception ex)
                {
                    var answer =
                        host.MessageBox.Show("Error processing line " + (index + 1) + ":" + Environment.NewLine +
                                             currLine + Environment.NewLine +
                                             Environment.NewLine +
                                             ex, "CONTINUE PROCESSING", MessageBoxChoices.YesNo);
                    if (answer != MessageBoxResult.No)
                    {
                        continue;
                    }

                    runResult.Error =
                        new Exception(
                            "Error processing line " + (index + 1) + ":" + Environment.NewLine + currLine, ex);
                    caller.Progress();
                    return runResult;
                }
            }
            while (!runResult.QuickScriptProcessor.InputStream.EndOfStream);

            try
            {
                if (!runResult.QuickScriptProcessor.Finish())
                {
                    runResult.FinishReturnedFalse = true;
                    caller.Progress();
                    return runResult;
                }
            }
            catch (Exception ex)
            {
                runResult.Error = new Exception("Error running Finish:" + Environment.NewLine + ex);
            }

            runResult.KeepGoing = true;
            caller.Progress();
            return runResult;
        }
    }
}
