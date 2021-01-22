#pragma warning disable 414
namespace MetX.Controls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    using Interfaces;
    using Library;
    using Scripts;

    using Microsoft.Win32;

    public class Context : ContextBase
    {
        public static readonly List<QuickScriptOutput> OutputWindows = new List<QuickScriptOutput>();
        public static RegistryKey AppDataRegistry;
        private static bool ScriptIsRunning;
        private static readonly object MScriptSyncRoot = new object();

        public static BaseLineProcessor GenerateQuickScriptLineProcessor(ContextBase @base, XlgQuickScript scriptToRun)
        {
            if ((@base.Templates.Count == 0) ||
                string.IsNullOrEmpty(@base.Templates[scriptToRun.Template].Views["Native"]))
            {
                MessageBox.Show("Quick script template 'Native' missing: " + scriptToRun.Template);
                return null;
            }

            var source = scriptToRun.ToCSharp(false);

            var additionalReferences = new List<Assembly> {
                                                          Assembly.GetAssembly(typeof(ChooseOrderDialog))
                                                      };

            var compilerResults = XlgQuickScript.CompileSource(source, false, additionalReferences);

            if (compilerResults.Errors.Count <= 0)
            {
                var assembly = compilerResults.CompiledAssembly;
                var quickScriptProcessor =
                    assembly.CreateInstance("MetX.QuickScriptProcessor") as BaseLineProcessor;

                if (quickScriptProcessor != null)
                {
                    quickScriptProcessor.InputFilePath = scriptToRun.InputFilePath;
                    quickScriptProcessor.DestinationFilePath = scriptToRun.DestinationFilePath;
                }

                return quickScriptProcessor;
            }

            var sb =
                new StringBuilder("Compilation failure. Errors found include:"
                                  + Environment.NewLine + Environment.NewLine);
            for (var index = 0; index < compilerResults.Errors.Count; index++)
            {
                sb.AppendLine((index + 1) + ": Line "
                              + compilerResults.Errors[index]
                                  .ToString()
                                  .TokensAfterFirst("(")
                                  .Replace(")", string.Empty));
                sb.AppendLine();
            }

            MessageBox.Show(sb.ToString());
            QuickScriptWorker.ViewTextInNotepad(source, true);

            return null;
        }

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

            if (!openedKey || (AppDataRegistry == null))
            {
                return null;
            }

            AppDataRegistry.Close();
            AppDataRegistry = null;
            return lastKnownPath;
        }

        public static void OpenNewOutput(IRunQuickScript caller, XlgQuickScript script, string title, string output)
        {
            var quickScriptOutput = new QuickScriptOutput(script, caller, title, output);
            OutputWindows.Add(quickScriptOutput);
            quickScriptOutput.Show(caller.Window);
            quickScriptOutput.BringToFront();
        }

        public static void RunQuickScript(ScriptRunningWindow caller, XlgQuickScript scriptToRun, IShowText targetOutput)
        {
            if (caller.InvokeRequired)
            {
                caller.Invoke(new Action<ScriptRunningWindow, XlgQuickScript, IShowText>(RunQuickScript), caller, scriptToRun, targetOutput);
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

                var runResult = Run(caller, ToolWindow.Context, scriptToRun);
                if (runResult.InputMissing)
                    caller.SetFocus("InputParam");
                if (runResult.Error != null)
                    MessageBox.Show(runResult.Error.ToString());
                if (!runResult.KeepGoing || (runResult.QuickScriptProcessor.Output == null) || (runResult.QuickScriptProcessor.Output.Length <= 0))
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
                                    runResult.QuickScriptProcessor.Output.ToString());
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
                            QuickScriptWorker.ViewFileInNotepad(
                                runResult.QuickScriptProcessor.Output.FilePath);

                            // QuickScriptWorker.ViewTextInNotepad(runResult.QuickScriptProcessor.Output.ToString(), false);
                            break;

                        case QuickScriptDestination.File:
                            runResult.QuickScriptProcessor.Output?.Finish();
                            runResult.QuickScriptProcessor.Output = null;
                            QuickScriptWorker.ViewFileInNotepad(scriptToRun.DestinationFilePath);

                            // File.WriteAllText(scriptToRun.DestinationFilePath, runResult.QuickScriptProcessor.Output.ToString());
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ScriptIsRunning = false;
                Monitor.Exit(MScriptSyncRoot);
            }
        }

        private static RunResult Run(ScriptRunningWindow caller, ContextBase @base, XlgQuickScript scriptToRun)
        {
            var result = new RunResult
            {
                QuickScriptProcessor = GenerateQuickScriptLineProcessor(@base, scriptToRun)
            };
            if (result.QuickScriptProcessor == null)
            {
                result.KeepGoing = false;
                return result;
            }

            var inputResult = result.QuickScriptProcessor.ReadInput(scriptToRun.Input);
            switch (inputResult)
            {
                case null:
                    result.KeepGoing = false;
                    return result;

                case false:
                    return result;

                // True keep going
            }

            var outputSetupCorrectly = result.QuickScriptProcessor.SetupOutput(scriptToRun.Destination, ref result.QuickScriptProcessor.DestinationFilePath);
            if (!outputSetupCorrectly)
            {
                if(result.Error == null) 
                    result.Error = new Exception("Output could not be set up correctly. Locked file?");
                return result;
            }

            // Start
            try
            {
                if (!result.QuickScriptProcessor.Start())
                {
                    result.StartReturnedFalse = true;
                    caller.Progress();
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Error = new Exception("Error running Start:" + Environment.NewLine + ex);
                result.KeepGoing = false;
                caller.Progress();
                return result;
            }

            // Process each line
            var index = 0;
            do
            {
                var currLine = result.QuickScriptProcessor.InputStream.ReadLine();
                if (string.IsNullOrEmpty(currLine))
                {
                    continue;
                }

                try
                {
                    if (result.QuickScriptProcessor.ProcessLine(currLine, index))
                    {
                        if (++index % 10 == 0 || index < 10)
                        {
                            caller.Progress(index);
                        }

                        continue;
                    }

                    result.ProcessLineReturnedFalse = true;
                    caller.Progress();
                    return result;
                }
                catch (Exception ex)
                {
                    var answer =
                        MessageBox.Show("Error processing line " + (index + 1) + ":" + Environment.NewLine +
                                        currLine + Environment.NewLine +
                                        Environment.NewLine +
                                        ex, "CONTINUE PROCESSING", MessageBoxButtons.YesNo);
                    if (answer != DialogResult.No)
                    {
                        continue;
                    }

                    result.Error =
                        new Exception(
                            "Error processing line " + (index + 1) + ":" + Environment.NewLine + currLine, ex);
                    caller.Progress();
                    return result;
                }
            }
            while (!result.QuickScriptProcessor.InputStream.EndOfStream);

            try
            {
                if (!result.QuickScriptProcessor.Finish())
                {
                    result.FinishReturnedFalse = true;
                    caller.Progress();
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Error = new Exception("Error running Finish:" + Environment.NewLine + ex);
            }

            result.KeepGoing = true;
            caller.Progress();
            return result;
        }
    }
}
