using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
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
using MetX.Windows.WinApi;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#pragma warning disable 414
namespace MetX.Controls
{

    public class Context : ContextBase
    {
        public static readonly List<QuickScriptOutput> OutputWindows = new();
        public static RegistryKey AppDataRegistry;
        public static bool ScriptIsRunning { get; private set; }
        public const string LastQuickScriptsBasePathKeyName = "LastQuickScriptsBasePathKeyName";

        private static readonly object MScriptSyncRoot = new();
        public const string QuickScriptsClonesFolderName = "Clones";
        public const string QuickScriptsActualFolderName = "Actual";
        public const string QuickScriptsFolderName = "QuickScripts";
        public const string ArchiveFolderName = "Archive";

        public static string LastClonesBasePath => Path.Combine(GetLastQuickScriptsBasePath(), QuickScriptsClonesFolderName);
        public static string LastActualBasePath => Path.Combine(GetLastQuickScriptsBasePath(), QuickScriptsActualFolderName);
        public static string MyDocumentsQuickScriptsBasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Context.QuickScriptsFolderName);
        


        public static string GetLastQuickScriptsBasePath()
        {
            var defaultValue = MyDocumentsQuickScriptsBasePath;

            Directory.CreateDirectory(defaultValue);
            Directory.CreateDirectory(Path.Combine(defaultValue, ArchiveFolderName));

            Directory.CreateDirectory(Path.Combine(defaultValue, QuickScriptsClonesFolderName));
            Directory.CreateDirectory(Path.Combine(defaultValue, QuickScriptsClonesFolderName, ArchiveFolderName));

            Directory.CreateDirectory(Path.Combine(defaultValue, QuickScriptsActualFolderName));
            Directory.CreateDirectory(Path.Combine(defaultValue, QuickScriptsActualFolderName, ArchiveFolderName));

            var openedKey = false;
            if (AppDataRegistry == null)
            {
                AppDataRegistry = Application.UserAppDataRegistry;
                openedKey = true;
            }

            if (AppDataRegistry == null)
                return defaultValue;

            var lastKnownPath = AppDataRegistry.GetValue(LastQuickScriptsBasePathKeyName) as string;

            if (!openedKey || AppDataRegistry == null)
                return defaultValue;

            AppDataRegistry.Close();
            AppDataRegistry = null;

            if (lastKnownPath.IsEmpty())
                return defaultValue;

            Directory.CreateDirectory(Path.Combine(lastKnownPath, QuickScriptsClonesFolderName));
            Directory.CreateDirectory(Path.Combine(lastKnownPath, QuickScriptsActualFolderName));

            return lastKnownPath;
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

        public static void ViewInNewQuickScriptOutputWindow(IRunQuickScript caller, XlgQuickScript script, string title, string output)
        {
            var quickScriptOutput = new QuickScriptOutput(script, caller, title, output, caller.Window.Host);
            OutputWindows.Add(quickScriptOutput);
            quickScriptOutput.Show(caller.Window);
            quickScriptOutput.BringToFront();
        }

        public static QuickScriptOutput ViewInNewQuickScriptOutputWindow(string title, string text, bool addLineNumbers, List<int> keyLines, IGenerationHost host)
        {
            var quickScriptOutput = QuickScriptOutput.View(title, text, addLineNumbers, keyLines, !addLineNumbers, host);
            OutputWindows.Add(quickScriptOutput);
            return quickScriptOutput;
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
            var settings = scriptToRun.BuildSettings(true, false, host);
            var result = settings.ActualizeAndCompile();

            if (!result.CompileSuccessful)
            {
                var source = result.OutputFiles["QuickScriptProcessor.cs"].Value;
                var finalDetails = result.FinalDetails(out var keyLines);
                
                var x = ViewInNewQuickScriptOutputWindow("Source for QuickScriptProcessor.cs", source, true, keyLines, host);
                x.Find("|Error");

                ViewInNewQuickScriptOutputWindow("Error detail / Compile results", finalDetails, false, null, host);

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
}
