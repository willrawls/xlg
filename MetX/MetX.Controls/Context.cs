using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using MetX.Standard.IO;
using MetX.Standard.Library.Extensions;

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
                //if(runResult.GatheredOutput.IsNotEmpty())
                //    QuickScriptWorker.ViewText(host, runResult.GatheredOutput, false);
                if (runResult.ErrorOutput.IsNotEmpty() && runResult.ErrorOutput.Contains(": error"))
                    host.MessageBox.Show(runResult.ErrorOutput);

                if (!runResult.KeepGoing || runResult.GatheredOutput.IsEmpty())
                    return;

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
                _scriptIsRunning = false;
                Monitor.Exit(MScriptSyncRoot);
            }
        }

        private static RunResult Run(ScriptRunningWindow caller, ContextBase @base, XlgQuickScript scriptToRun, IGenerationHost host)
        {
            var settings = scriptToRun.BuildSettings(true, false, host);
            var result = settings.ActualizeAndCompile();

            if (!result.CompileSuccessful)
            {
                QuickScriptWorker.ViewText(host, result.FinalDetails(), false);
                return new RunResult
                {
                    ActualizationResult = result,
                    ErrorOutput = result.FinalDetails(),
                };
            }

            string parameters = result.Settings.Script.AsParameters();

            var runResult = new RunResult
            {
                ActualizationResult = result,
                GatheredOutput = FileSystem.GatherOutputAndErrors(result.DestinationExecutableFilePath, parameters, out var errorOutput, Environment.GetEnvironmentVariable("TEMP"), 60, ProcessWindowStyle.Hidden),
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
}
