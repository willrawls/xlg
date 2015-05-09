using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MetX.Data;
using MetX.Interfaces;
using MetX.Library;
using MetX.Scripts;
using Microsoft.Win32;

namespace MetX.Controls
{
    public class Context : ContextBase
    {
        public static readonly List<QuickScriptOutput> OutputWindows = new List<QuickScriptOutput>();
        private static readonly object m_ScriptSyncRoot = new object();
        protected static bool ScriptIsRunning;

        public static void OpenNewOutput(IRunQuickScript caller, XlgQuickScript script, string title, string output)
        {
            QuickScriptOutput quickScriptOutput = new QuickScriptOutput(script, caller, title, output);
            OutputWindows.Add(quickScriptOutput);
            quickScriptOutput.Show(caller.Window);
            quickScriptOutput.BringToFront();
        }

        public static BaseLineProcessor GenerateQuickScriptLineProcessor(ContextBase @base, XlgQuickScript scriptToRun)
        {
            if (@base.Templates.Count == 0 ||
                String.IsNullOrEmpty(@base.Templates[scriptToRun.Template].Views["Native"]))
            {
                MessageBox.Show("Quick script template 'Native' missing: " + scriptToRun.Template);
                return null;
            }

            string source = scriptToRun.ToCSharp(false);
            CompilerResults compilerResults = XlgQuickScript.CompileSource(source, false);

            if (compilerResults.Errors.Count <= 0)
            {
                Assembly assembly = compilerResults.CompiledAssembly;
                BaseLineProcessor quickScriptProcessor =
                    assembly.CreateInstance("MetX.QuickScriptProcessor") as BaseLineProcessor;

                if (quickScriptProcessor != null)
                {
                    quickScriptProcessor.InputFilePath = scriptToRun.InputFilePath;
                    quickScriptProcessor.DestinationFilePath = scriptToRun.DestinationFilePath;
                }

                return quickScriptProcessor;
            }

            StringBuilder sb =
                new StringBuilder("Compilation failure. Errors found include:"
                                  + Environment.NewLine + Environment.NewLine);
            for (int index = 0; index < compilerResults.Errors.Count; index++)
            {
                sb.AppendLine((index + 1) + ": Line "
                              + compilerResults.Errors[index]
                                  .ToString()
                                  .TokensAfterFirst("(")
                                  .Replace(")", String.Empty));
                sb.AppendLine();
            }
            MessageBox.Show(sb.ToString());
            QuickScriptWorker.ViewTextInNotepad(source, true);

            return null;
        }

        public static void RunQuickScript(ScriptRunningWindow caller, XlgQuickScript scriptToRun, IShowText targetOutput)
        {
            if (caller.InvokeRequired)
            {
                caller.Invoke(new Action<ScriptRunningWindow, XlgQuickScript, IShowText>(RunQuickScript), caller, scriptToRun, targetOutput);
                return;
            }
            bool lockTaken = false;
            Monitor.TryEnter(m_ScriptSyncRoot, ref lockTaken);
            if (!lockTaken) return;

            try
            {
                ScriptIsRunning = true;
                if (scriptToRun.Destination == QuickScriptDestination.File)
                {
                    if (String.IsNullOrEmpty(scriptToRun.DestinationFilePath))
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

                RunResult runResult = Run(ToolWindow.Context, scriptToRun);
                if (runResult.InputMissing)
                    caller.SetFocus("InputParam");
                if (!runResult.KeepGoing || runResult.QuickScriptProcessor.Output == null || runResult.QuickScriptProcessor.Output.Length <= 0)
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
                                OpenNewOutput(caller,
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
                            Clipboard.Clear();
                            Clipboard.SetText(runResult.QuickScriptProcessor.Output.ToString());
                            break;

                        case QuickScriptDestination.Notepad:
                            QuickScriptWorker.ViewTextInNotepad(runResult.QuickScriptProcessor.Output.ToString(), false);
                            break;

                        case QuickScriptDestination.File:
                            File.WriteAllText(scriptToRun.DestinationFilePath, runResult.QuickScriptProcessor.Output.ToString());
                            QuickScriptWorker.ViewFileInNotepad(scriptToRun.DestinationFilePath);
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
                Monitor.Exit(m_ScriptSyncRoot);
            }
        }

        private static RunResult Run(ContextBase @base, XlgQuickScript scriptToRun)
        {
            RunResult result = new RunResult
            {
                QuickScriptProcessor = GenerateQuickScriptLineProcessor(@base, scriptToRun)
            };
            if (result.QuickScriptProcessor == null)
            {
                result.KeepGoing = false;
                return result;
            }
            bool? inputResult = result.QuickScriptProcessor.ReadInput(scriptToRun.Input);
            switch (inputResult)
            {
                case null:
                    result.KeepGoing = false;
                    return result;
                case false:
                    return result;
                // True keep going
            }

            // Start
            try
            {
                if (!result.QuickScriptProcessor.Start())
                {
                    result.StartReturnedFalse = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Error = new Exception("Error running Start:" + Environment.NewLine + ex);
                result.KeepGoing = false;
                return result;
            }

            // Process each line
            for (int index = 0; index < result.QuickScriptProcessor.Lines.Count; index++)
            {
                string currLine = result.QuickScriptProcessor.Lines[index];
                try
                {
                    if (!result.QuickScriptProcessor.ProcessLine(currLine, index))
                    {
                        result.ProcessLineReturnedFalse = true;
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    DialogResult answer = MessageBox.Show("Error processing line " + (index + 1) + ":" + Environment.NewLine +
                                                          currLine + Environment.NewLine +
                                                          Environment.NewLine +
                                                          ex, "CONTINUE PROCESSING", MessageBoxButtons.YesNo);
                    if (answer == DialogResult.No)
                    {
                        result.Error = new Exception("Error processing line " + (index + 1) + ":" + Environment.NewLine + currLine, ex);
                        return result;
                    }
                }
            }
            try
            {
                if (!result.QuickScriptProcessor.Finish())
                {
                    result.FinishReturnedFalse = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Error = new Exception("Error running Finish:" + Environment.NewLine + ex);
            }
            result.KeepGoing = true;
            return result;
        }

        public static RegistryKey AppDataRegistry;

        public static string GetLastKnownPath()
        {
            bool openedKey = false;
            if (AppDataRegistry == null)
            {
                AppDataRegistry = Application.UserAppDataRegistry;
                openedKey = true;
            }

            if (AppDataRegistry == null)
            {
                return null;
            }
            string lastKnownPath = AppDataRegistry.GetValue("LastQuickScriptPath") as string;

            if (!openedKey || AppDataRegistry == null)
            {
                return null;
            }

            AppDataRegistry.Close();
            AppDataRegistry = null;
            return lastKnownPath;
        }
    }
}