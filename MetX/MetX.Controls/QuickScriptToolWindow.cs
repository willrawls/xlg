using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using MetX.Library;
using MetX.Scripts;
using Microsoft.Win32;

namespace MetX.Controls
{
    public partial class QuickScriptToolWindow : ScriptRunningWindow
    {
        public static RegistryKey AppDataRegistry;
        public static readonly List<QuickScriptOutput> OutputWindows = new List<QuickScriptOutput>();

        public string WordBeforeCaret
        {
            get
            {
                if (ScriptEditor.Text.Length == 0 || textArea.Caret.Column == 0)
                {
                    return string.Empty;
                }
                string[] lines = ScriptEditor.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                string linePart = lines[textArea.Caret.Line].Substring(0, textArea.Caret.Column).LastToken();
                int i;
                for (i = linePart.Length - 1; i >= 0; i--)
                {
                    if (!char.IsLetterOrDigit(linePart[i]))
                    {
                        break;
                    }
                }
                if (i > 0 && i < linePart.Length)
                {
                    linePart = linePart.Substring(i);
                }
                return linePart.ToLower().Trim();
            }
        }

        public CodeCompletionWindow completionWindow;
        public XlgQuickScript CurrentScript = null;
        private TextArea textArea;
        public bool Updating;

        public QuickScriptToolWindow(XlgQuickScript script)
        {
            InitializeComponent();
            InitializeEditor();
            CurrentScript = script;
        }

        private void InitializeEditor()
        {
            textArea = ScriptEditor.ActiveTextAreaControl.TextArea;
            textArea.KeyEventHandler += ProcessKey;
            textArea.KeyUp += TextAreaOnKeyUp;

            FileSyntaxModeProvider fsmProvider = new FileSyntaxModeProvider(AppDomain.CurrentDomain.BaseDirectory);
            HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.
            ScriptEditor.SetHighlighting("QuickScript"); // Activate the highlighting, use the name from the SyntaxDefinition node.
            ScriptEditor.Refresh();
        }

        private void TextAreaOnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Space:
                        ShowThisCodeCompletion();
                        break;
                }
            }
            else // No modifiers
            {
                switch (e.KeyCode)
                {
                    case Keys.F5:
                        e.Handled = true;
                        RunQuickScript_Click(null, null);
                        break;
                }
            }
        }

        private bool ProcessKey(char ch)
        {
            switch (ch)
            {
                case '.':
                    if (WordBeforeCaret == "this")
                    {
                        ShowThisCodeCompletion();
                    }
                    break;
                case '~':
                    ShowScriptCommandCodeCompletion();
                    break;
            }
            return false;
        }

        private void ShowScriptCommandCodeCompletion() { ShowCodeCompletion(new[] { "~:", "~Members:", "~Start:", "~Body:", "~Finish:", "~BeginString:", "~EndString:" }); }

        private void ShowThisCodeCompletion() { ShowCodeCompletion(new[] { "Output", "Lines", "AllText", "DestionationFilePath", "InputFilePath", "LineCount", "OpenNotepad", "Ask" }); }

        public void ShowCodeCompletion(string[] items)
        {
            if (items != null && items.Length > 0)
            {
                CompletionDataProvider completionDataProvider = new CompletionDataProvider(items);
                completionWindow = CodeCompletionWindow.ShowCompletionWindow(this, ScriptEditor, string.Empty, completionDataProvider, '.');
                if (completionWindow != null)
                {
                    completionWindow.Closed += CompletionWindowClosed;
                }
            }
        }

        private void CompletionWindowClosed(object source, EventArgs e)
        {
            if (completionWindow != null)
            {
                completionWindow.Closed -= CompletionWindowClosed;
                completionWindow.Dispose();
                completionWindow = null;
            }
        }

        public void OpenNewOutput(XlgQuickScript script, string title, string output)
        {
            QuickScriptOutput quickScriptOutput = new QuickScriptOutput(script, this, title, output);
            OutputWindows.Add(quickScriptOutput);
            quickScriptOutput.Show(this);
            quickScriptOutput.BringToFront();
        }

        public void UpdateScriptFromForm()
        {
            if (CurrentScript == null)
            {
                return;
            }

            CurrentScript.Script = ScriptEditor.Text;
        }

        private void UpdateFormWithScript(XlgQuickScript selectedScript)
        {
            if (CurrentScript == null)
            {
                return;
            }

            ScriptEditor.Text = selectedScript.Script;
            ScriptEditor.Refresh();
            ScriptEditor.Focus();
            CurrentScript = selectedScript;
        }

        public void DisplayExpandedQuickScriptSourceInNotepad(bool independent)
        {
            try
            {
                if (CurrentScript == null)
                {
                    return;
                }
                UpdateScriptFromForm();
                string source = CurrentScript.ToCSharp(independent);
                if (!string.IsNullOrEmpty(source))
                {
                    QuickScriptWorker.ViewTextInNotepad(source, true);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public BaseLineProcessor GenerateQuickScriptLineProcessor(string templateName)
        {
            if (Context.Templates.Count == 0 ||
                string.IsNullOrEmpty(Context.Templates[templateName].Views["Native"]))
            {
                MessageBox.Show(this, "Quick script template 'Native' missing: " + templateName);
                return null;
            }

            string source = CurrentScript.ToCSharp(false);
            CompilerResults compilerResults = XlgQuickScript.CompileSource(source, false);

            if (compilerResults.Errors.Count <= 0)
            {
                Assembly assembly = compilerResults.CompiledAssembly;
                BaseLineProcessor quickScriptProcessor =
                    assembly.CreateInstance("MetX.QuickScriptProcessor") as BaseLineProcessor;

                if (quickScriptProcessor != null)
                {
                    quickScriptProcessor.InputFilePath = CurrentScript.InputFilePath;
                    quickScriptProcessor.DestinationFilePath = CurrentScript.DestinationFilePath;
                }

                return quickScriptProcessor;
            }

            StringBuilder sb =
                new StringBuilder("Compilation failure. Errors found include:"
                                  + Environment.NewLine + Environment.NewLine);
            for (int index = 0; index < compilerResults.Errors.Count; index++)
            {
                sb.AppendLine((index + 1) + ": Line " + compilerResults.Errors[index]
                    .ToString()
                    .TokensAfterFirst("(")
                    .Replace(")", string.Empty));
                sb.AppendLine();
            }
            MessageBox.Show(sb.ToString());
            QuickScriptWorker.ViewTextInNotepad(source, true);

            return null;
        }

        public string GenerateIndependentQuickScriptExe(string templateName)
        {
            if (InvokeRequired)
            {
                return (string)Invoke(new Func<string, string>(GenerateIndependentQuickScriptExe), templateName);
            }
            if (Context.Templates.Count == 0 ||
                string.IsNullOrEmpty(Context.Templates[templateName].Views["Exe"]))
            {
                MessageBox.Show(this, "Quick script template 'Exe' missing for: " + templateName);
                return null;
            }

            string source = CurrentScript.ToCSharp(true);
            CompilerResults compilerResults = XlgQuickScript.CompileSource(source, true);

            if (compilerResults.Errors.Count <= 0)
            {
                Assembly assembly = compilerResults.CompiledAssembly;

                string parentDestination = CurrentScript.DestinationFilePath.TokensBeforeLast(@"\");

                if (string.IsNullOrEmpty(parentDestination)
                    && !string.IsNullOrEmpty(CurrentScript.InputFilePath)
                    && CurrentScript.Input != "Web Address")
                {
                    parentDestination = CurrentScript.InputFilePath.TokensBeforeLast(@"\");
                }

                if (string.IsNullOrEmpty(parentDestination))
                {
                    parentDestination = Context.Scripts.FilePath.TokensBeforeLast(@"\");
                }

                if (string.IsNullOrEmpty(parentDestination))
                {
                    parentDestination = assembly.Location.TokensBeforeLast(@"\");
                }

                if (!Directory.Exists(parentDestination))
                {
                    return assembly.Location;
                }

                string metXDllPathSource = Path.Combine(assembly.Location.TokensBeforeLast(@"\"), "MetX.dll");

                parentDestination = Path.Combine(parentDestination, "bin");
                string metXDllPathDest = Path.Combine(parentDestination, "MetX.dll");

                string exeFilePath = Path.Combine(parentDestination, CurrentScript.Name.AsFilename()) + ".exe";
                string csFilePath = exeFilePath.Replace(".exe", ".cs");

                Directory.CreateDirectory(parentDestination);

                if (File.Exists(exeFilePath))
                {
                    File.Delete(exeFilePath);
                }
                if (File.Exists(csFilePath))
                {
                    File.Delete(csFilePath);
                }

                File.Copy(assembly.Location, exeFilePath);
                if (!File.Exists(metXDllPathDest))
                {
                    File.Copy(metXDllPathSource, metXDllPathDest);
                }
                File.WriteAllText(csFilePath, source);
                return exeFilePath;
            }

            StringBuilder sb =
                new StringBuilder("Compilation failure. Errors found include:" + Environment.NewLine
                                  + Environment.NewLine);
            List<string> lines = new List<string>(source.LineList());
            for (int index = 0; index < compilerResults.Errors.Count; index++)
            {
                string error = compilerResults.Errors[index].ToString();
                if (error.Contains("("))
                {
                    error = error.TokensAfterFirst("(").Replace(")", string.Empty);
                }
                sb.AppendLine((index + 1) + ": Line " + error);
                sb.AppendLine();
                if (error.Contains(Environment.NewLine))
                {
                    lines[compilerResults.Errors[index].Line - 1] += "\t// " + error.Replace(Environment.NewLine, " ");
                }
                else if (compilerResults.Errors[index].Line == 0)
                {
                    lines[0] += "\t// " + error;
                }
                else
                {
                    lines[compilerResults.Errors[index].Line - 1] += "\t// " + error;
                }
            }
            MessageBox.Show(sb.ToString());
            QuickScriptWorker.ViewTextInNotepad(lines.Flatten(), true);

            return null;
        }

        private void RunQuickScript_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentScript == null)
                {
                    return;
                }
                UpdateScriptFromForm();
                Context.RunQuickScript(this, CurrentScript, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void SaveQuickScript_Click(object sender, EventArgs e)
        {
            try
            {
                if (Updating)
                {
                    return;
                }
                if (Context.Scripts != null)
                {
                    UpdateScriptFromForm();
                    if (string.IsNullOrEmpty(Context.Scripts.FilePath))
                    {
                        SaveAs_Click(null, null);
                    }
                    else
                    {
                        Context.Scripts.Save();
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void ViewGeneratedCode_Click(object sender, EventArgs e) { DisplayExpandedQuickScriptSourceInNotepad(false); }

        private void QuickScriptToolWindow_FormClosing(object sender, FormClosingEventArgs e) { SaveQuickScript_Click(sender, null); }

        private void ViewIndependectGeneratedCode_Click(object sender, EventArgs e)
        {
            //DisplayExpandedQuickScriptSourceInNotepad(true);
            try
            {
                if (CurrentScript == null)
                {
                    return;
                }
                UpdateScriptFromForm();
                string location = GenerateIndependentQuickScriptExe(CurrentScript.Template);
                if (location.IsEmpty())
                {
                    return;
                }

                if (DialogResult.Yes == MessageBox.Show(this,
                    "Executable generated successfully at: " + location + Environment.NewLine +
                    Environment.NewLine +
                    "Would you like to run it now? (No will open the generated file).", "RUN EXE?", MessageBoxButtons.YesNo))
                {
                    Process.Start(new ProcessStartInfo(location)
                    {
                        UseShellExecute = true,
                        WorkingDirectory = location.TokensBeforeLast(@"\"),
                    });
                }
                else
                {
                    QuickScriptWorker.ViewFileInNotepad(location.Replace(".exe", ".cs"));
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                if (Updating)
                {
                    return;
                }
                if (Context.Scripts != null)
                {
                    UpdateScriptFromForm();

                    SaveDestinationFilePathDialog.FileName = Context.Scripts.FilePath;
                    SaveDestinationFilePathDialog.InitialDirectory = Context.Scripts.FilePath.TokensBeforeLast(@"\");
                    SaveDestinationFilePathDialog.AddExtension = true;
                    SaveDestinationFilePathDialog.CheckPathExists = true;
                    SaveDestinationFilePathDialog.DefaultExt = ".xlgq";
                    SaveDestinationFilePathDialog.Filter = "Quick script files (*.xlgq)|*.xlgq;All files (*.*)|*.*";
                    SaveDestinationFilePathDialog.ShowDialog(this);
                    if (!string.IsNullOrEmpty(SaveDestinationFilePathDialog.FileName))
                    {
                        Context.Scripts.FilePath = SaveDestinationFilePathDialog.FileName;
                        Context.Scripts.Save();
                        Text = "Quick Scriptr - " + Context.Scripts.FilePath;
                        UpdateLastKnownPath();
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void QuickScriptToolWindow_Load(object sender, EventArgs e) { UpdateLastKnownPath(); }

        private void UpdateLastKnownPath()
        {
            if (Context.Scripts == null || string.IsNullOrEmpty(Context.Scripts.FilePath) || !File.Exists(Context.Scripts.FilePath))
            {
                return;
            }
            bool openedKey = false;
            if (AppDataRegistry == null)
            {
                AppDataRegistry = Application.UserAppDataRegistry;
                openedKey = true;
            }

            if (AppDataRegistry == null)
            {
                return;
            }
            AppDataRegistry.SetValue("LastQuickScriptPath", Context.Scripts.FilePath, RegistryValueKind.String);

            if (!openedKey || AppDataRegistry == null)
            {
                return;
            }
            AppDataRegistry.Close();
            AppDataRegistry = null;
        }

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