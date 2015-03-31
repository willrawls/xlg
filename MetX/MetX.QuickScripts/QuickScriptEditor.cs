using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using MetX.Controls;
using MetX.Data;
using MetX.Interfaces;
using MetX.Library;
using Microsoft.Win32;
using WeifenLuo.WinFormsUI.Docking;
using XLG.QuickScripts;

namespace XLG.QuickScripts
{
    public partial class QuickScriptEditor : ScriptRunningToolWindow //, IRunQuickScript
    {
        public XlgQuickScript SelectedScript { get { return this.QuickScriptList.SelectedItem as XlgQuickScript; } }

        private TextArea textArea;
        private CodeCompletionWindow completionWindow;
        public XlgQuickScript CurrentScript = null;
        public bool Updating;

        public QuickScriptEditor(string filePath)
        {
            InitializeComponent();
            InitializeEditor();
            LoadQuickScriptsFile(filePath);
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
                    if (WordBeforeCaret == "this") ShowThisCodeCompletion();
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
                completionWindow = CodeCompletionWindow.ShowCompletionWindow(this, ScriptEditor, String.Empty, completionDataProvider, '.');
                if (completionWindow != null)
                {
                    completionWindow.Closed += CompletionWindowClosed;
                }
            }
        }

        private string WordBeforeCaret
        {
            get
            {
                if (ScriptEditor.Text.Length == 0 || textArea.Caret.Column == 0) return string.Empty;
                string[] lines = ScriptEditor.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                string linePart = lines[textArea.Caret.Line].Substring(0, textArea.Caret.Column).LastToken();
                int i;
                for (i = linePart.Length - 1; i >= 0; i--)
                {
                    if (!char.IsLetterOrDigit(linePart[i])) break;
                }
                if (i > 0 && i < linePart.Length) linePart = linePart.Substring(i);
                return linePart.ToLower().Trim();
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

        private void RefreshLists()
        {
            Updating = true;
            try
            {
                QuickScriptList.Items.Clear();
                int defaultIndex = 0;
                foreach (XlgQuickScript script in Context.Scripts)
                {
                    QuickScriptList.Items.Add(script);
                    if (Context.Scripts.Default != null && script == Context.Scripts.Default)
                    {
                        defaultIndex = QuickScriptList.Items.Count - 1;
                    }
                }
                if (defaultIndex > -1)
                {
                    QuickScriptList.SelectedIndex = defaultIndex;
                }
            }
            finally
            {
                Updating = false;
            }
        }

        public void UpdateScriptFromForm()
        {
            if (CurrentScript == null)
            {
                return;
            }

            CurrentScript.Script = ScriptEditor.Text;
            Enum.TryParse(DestinationList.Text.Replace(" ", string.Empty), out CurrentScript.Destination);
            CurrentScript.Input = InputList.Text;
            CurrentScript.SliceAt = SliceAt.Text;
            CurrentScript.DiceAt = DiceAt.Text;
            CurrentScript.InputFilePath = InputParam.Text;
            CurrentScript.DestinationFilePath = DestinationParam.Text;
            CurrentScript.Template = TemplateList.Text;
            Context.Scripts.Default = CurrentScript;
        }

        private void QuickScriptList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }
            UpdateScriptFromForm();
            if (SelectedScript != null)
            {
                UpdateFormWithScript(SelectedScript);
            }
        }

        private void UpdateFormWithScript(XlgQuickScript selectedScript)
        {
            if (SelectedScript == null)
            {
                return;
            }

            QuickScriptList.Text = selectedScript.Name;
            ScriptEditor.Text = selectedScript.Script;
            ScriptEditor.Refresh();

            DestinationList.Text = selectedScript.Destination == QuickScriptDestination.Unknown
                ? "Text Box"
                : selectedScript.Destination.ToString().Replace("Box", " Box");

            int index = InputList.FindString(selectedScript.Input);
            InputList.SelectedIndex = index > -1
                ? index
                : 0;

            index = SliceAt.FindString(selectedScript.SliceAt);
            SliceAt.SelectedIndex = index > -1
                ? index
                : SliceAt.Items.Add(selectedScript.SliceAt);

            index = DiceAt.FindString(selectedScript.DiceAt);
            DiceAt.SelectedIndex = index > -1
                ? index
                : DiceAt.Items.Add(selectedScript.DiceAt);

            index = TemplateList.FindString(selectedScript.Template);
            TemplateList.SelectedIndex = index > -1
                ? index
                : TemplateList.Items.Add(selectedScript.Template);

            InputParam.Text = selectedScript.InputFilePath;
            if (InputParam.Text.Length > 0)
            {
                InputParam.SelectionStart = InputParam.Text.Length;
            }
            DestinationParam.Text = selectedScript.DestinationFilePath;
            if (DestinationParam.Text.Length > 0)
            {
                DestinationParam.SelectionStart = DestinationParam.Text.Length;
            }

            DestinationList_SelectedIndexChanged(null, null);
            InputList_SelectedIndexChanged(null, null);

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

        public string GenerateIndependentQuickScriptExe(XlgQuickScript scriptToRun)
        {
            if (InvokeRequired)
            {
                return (string)Invoke(new Func<XlgQuickScript, string>(GenerateIndependentQuickScriptExe), scriptToRun);
            }
            if (Context.Templates.Count == 0 ||
                string.IsNullOrEmpty(Context.Templates[TemplateList.Text].Views["Exe"]))
            {
                MessageBox.Show(this, "Quick script template 'Exe' missing for: " + TemplateList.Text);
                return null;
            }

            if (scriptToRun == null)
            {
                return null;
            }
            string source = scriptToRun.ToCSharp(true);
            CompilerResults compilerResults = XlgQuickScript.CompileSource(source, true);

            if (compilerResults.Errors.Count <= 0)
            {
                Assembly assembly = compilerResults.CompiledAssembly;

                string parentDestination = scriptToRun.DestinationFilePath.TokensBeforeLast(@"\");

                if (string.IsNullOrEmpty(parentDestination)
                    && !string.IsNullOrEmpty(scriptToRun.InputFilePath)
                    && scriptToRun.Input != "Web Address")
                {
                    parentDestination = scriptToRun.InputFilePath.TokensBeforeLast(@"\");
                }

                if (string.IsNullOrEmpty(parentDestination))
                    parentDestination = Context.Scripts.FilePath.TokensBeforeLast(@"\");

                if (string.IsNullOrEmpty(parentDestination))
                    parentDestination = assembly.Location.TokensBeforeLast(@"\");

                if (!Directory.Exists(parentDestination))
                {
                    return assembly.Location;
                }

                string metXDllPathSource = Path.Combine(assembly.Location.TokensBeforeLast(@"\"), "MetX.dll");

                parentDestination = Path.Combine(parentDestination, "bin");
                string metXDllPathDest = Path.Combine(parentDestination, "MetX.dll");

                string exeFilePath = Path.Combine(parentDestination, scriptToRun.Name.AsFilename()) + ".exe";
                string csFilePath = exeFilePath.Replace(".exe", ".cs");

                Directory.CreateDirectory(parentDestination);

                if (File.Exists(exeFilePath)) File.Delete(exeFilePath);
                if (File.Exists(csFilePath)) File.Delete(csFilePath);

                File.Copy(assembly.Location, exeFilePath);
                if (!File.Exists(metXDllPathDest))
                    File.Copy(metXDllPathSource, metXDllPathDest);
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

        private void LoadQuickScriptsFile(string filePath)
        {
            if(Context == null)
                Context = new Context();
            Context.Scripts = XlgQuickScriptFile.Load(filePath);

            if (Context.Scripts.Count == 0)
            {
                XlgQuickScript script = new XlgQuickScript("First script", QuickScriptWorker.FirstScript);
                Context.Scripts.Add(script);
                Context.Scripts.Default = script;
                script = new XlgQuickScript("Example / Tutorial", QuickScriptWorker.ExampleTutorialScript);
                Context.Scripts.Add(script);
                Context.Scripts.Save();
            }
            UpdateLastKnownPath();

            RefreshLists();
            UpdateFormWithScript(Context.Scripts.Default);
            Text = "Quick Scriptr - " + filePath;
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
                RunQuickScript(this, CurrentScript, null);
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
                        SaveAs_Click(null, null);
                    else Context.Scripts.Save();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void NewQuickScript_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }
            if (Context.Scripts != null)
            {
                string name = string.Empty;
                DialogResult answer = UI.InputBox("New Script Name", "Please enter the name for the new script.",
                    ref name);
                if (answer != DialogResult.OK || (name ?? string.Empty).Trim() == string.Empty)
                {
                    return;
                }

                string script = string.Empty;
                XlgQuickScript newScript = null;
                if (CurrentScript != null)
                {
                    answer = MessageBox.Show(this, "Would you like to clone the current script?", "CLONE SCRIPT?",
                        MessageBoxButtons.YesNoCancel);
                    switch (answer)
                    {
                        case DialogResult.Cancel:
                            return;
                        case DialogResult.Yes:
                            UpdateScriptFromForm();
                            //script = CurrentScript.Script;
                            newScript = CurrentScript.Clone(name);
                            break;
                    }
                }

                UpdateScriptFromForm();
                Updating = true;
                try
                {
                    if (newScript == null)
                    {
                        newScript = new XlgQuickScript(name, script);
                    }
                    Context.Scripts.Add(newScript);
                    QuickScriptList.Items.Add(newScript);
                    QuickScriptList.SelectedIndex = QuickScriptList.Items.Count - 1;
                    UpdateFormWithScript(newScript);
                }
                finally
                {
                    Updating = false;
                }
            }
        }

        private void ViewGeneratedCode_Click(object sender, EventArgs e) { DisplayExpandedQuickScriptSourceInNotepad(false); }

        private void QuickScriptEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                foreach (QuickScriptOutput outputWindow in MetX.Controls.Context.OutputWindows)
                {
                    outputWindow.Close();
                    outputWindow.Dispose();
                }
                MetX.Controls.Context.OutputWindows.Clear();
            }
            catch
            {
                // Ignored
            }
            SaveQuickScript_Click(sender, null);
        }

        private void DeleteScript_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }
            if (CurrentScript == null)
            {
                return;
            }

            DialogResult answer = MessageBox.Show(this,
                "This will permanently delete the current script.\n\tAre you sure this is what you want to do?",
                "DELETE SCRIPT", MessageBoxButtons.YesNo);
            if (answer == DialogResult.Yes)
            {
                Updating = true;
                XlgQuickScript script = CurrentScript;
                try
                {
                    QuickScriptList.Items.Remove(script);
                    Context.Scripts.Remove(script);
                }
                finally
                {
                    Updating = false;
                }
                if (Context.Scripts.Count == 0)
                {
                    script = new XlgQuickScript("First script");
                    Context.Scripts.Add(script);
                    Context.Scripts.Default = script;
                }
                else if (Context.Scripts.Default == script)
                {
                    Context.Scripts.Default = Context.Scripts[0];
                }
                RefreshLists();
                UpdateFormWithScript(Context.Scripts.Default);
            }
        }

        private void EditInputFilePath_Click(object sender, EventArgs e) { QuickScriptWorker.ViewFileInNotepad(InputParam.Text); }

        private void EditDestinationFilePath_Click(object sender, EventArgs e) { QuickScriptWorker.ViewFileInNotepad(DestinationParam.Text); }

        private void BrowseInputFilePath_Click(object sender, EventArgs e)
        {
            OpenInputFilePathDialog.FileName = InputParam.Text;
            OpenInputFilePathDialog.InitialDirectory = InputParam.Text.TokensBeforeLast(@"\");
            OpenInputFilePathDialog.AddExtension = true;
            OpenInputFilePathDialog.CheckFileExists = true;
            OpenInputFilePathDialog.CheckPathExists = true;
            OpenInputFilePathDialog.DefaultExt = "";
            OpenInputFilePathDialog.Filter = "All files (*.*)|*.*";
            OpenInputFilePathDialog.Multiselect = false;
            OpenInputFilePathDialog.ShowDialog(this);
            if (!string.IsNullOrEmpty(OpenInputFilePathDialog.FileName))
            {
                InputParam.Text = OpenInputFilePathDialog.FileName;
            }
        }

        private void BrowseDestinationFilePath_Click(object sender, EventArgs e)
        {
            SaveDestinationFilePathDialog.FileName = DestinationParam.Text;
            SaveDestinationFilePathDialog.InitialDirectory = DestinationParam.Text.TokensBeforeLast(@"\");
            SaveDestinationFilePathDialog.AddExtension = true;
            SaveDestinationFilePathDialog.DefaultExt = "";
            SaveDestinationFilePathDialog.CheckPathExists = true;
            SaveDestinationFilePathDialog.Filter = "All files (*.*)|*.*";
            SaveDestinationFilePathDialog.ShowDialog(this);
            if (!string.IsNullOrEmpty(SaveDestinationFilePathDialog.FileName))
            {
                DestinationParam.Text = SaveDestinationFilePathDialog.FileName;
            }
        }

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
                string location = GenerateIndependentQuickScriptExe(CurrentScript);
                if (location.IsNullOrEmpty()) return;

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

        private void InputList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string input = InputList.Text;
            if (string.IsNullOrEmpty(input))
            {
                return;
            }
            switch (input.ToLower())
            {
                case "clipboard":
                case "none":
                    InputParam.Enabled = false;
                    break;
                default:
                    InputParam.Enabled = true;
                    break;
            }
            EditInputFilePath.Enabled = InputParam.Enabled && input != "Web Address";
            BrowseInputFilePath.Enabled = InputParam.Enabled && input != "Web Address";
        }

        private void DestinationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string input = DestinationList.Text;
            if (string.IsNullOrEmpty(input))
            {
                return;
            }
            switch (input.ToLower())
            {
                case "text box":
                case "textbox":
                case "notepad":
                case "clipboard":
                case "none":
                    DestinationParam.Enabled = false;
                    break;
                default:
                    DestinationParam.Enabled = true;
                    break;
            }
            EditDestinationFilePath.Enabled = DestinationParam.Enabled;
            BrowseDestinationFilePath.Enabled = DestinationParam.Enabled;
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
        }

        private void ShowInputOutputOptions_Click(object sender, EventArgs e)
        {
            ShowInputOutputOptions.Checked = !ShowInputOutputOptions.Checked;

            InputOptions.Visible = ShowInputOutputOptions.Checked;
            OutputOutputs.Visible = ShowInputOutputOptions.Checked;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenInputFilePathDialog.FileName = "";
            OpenInputFilePathDialog.InitialDirectory = Context.Scripts.FilePath.TokensBeforeLast(@"\");
            OpenInputFilePathDialog.AddExtension = true;
            OpenInputFilePathDialog.CheckFileExists = false;
            OpenInputFilePathDialog.CheckPathExists = true;
            OpenInputFilePathDialog.DefaultExt = ".xlgq";
            OpenInputFilePathDialog.Filter = "Quick script files (*.xlgq)|*.xlgq|All files (*.*)|*.*";
            OpenInputFilePathDialog.Multiselect = false;
            OpenInputFilePathDialog.ShowDialog(this);
            if (!string.IsNullOrEmpty(OpenInputFilePathDialog.FileName))
            {
                Context.Scripts.Save();
                LoadQuickScriptsFile(OpenInputFilePathDialog.FileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenInputFilePathDialog.FileName = "";
            OpenInputFilePathDialog.InitialDirectory = Context.Scripts.FilePath.TokensBeforeLast(@"\");
            OpenInputFilePathDialog.AddExtension = true;
            OpenInputFilePathDialog.CheckFileExists = true;
            OpenInputFilePathDialog.CheckPathExists = true;
            OpenInputFilePathDialog.DefaultExt = ".xlgq";
            OpenInputFilePathDialog.Filter = "Quick script files (*.xlgq)|*.xlgq|All files (*.*)|*.*";
            OpenInputFilePathDialog.Multiselect = false;
            OpenInputFilePathDialog.ShowDialog(this);
            if (!string.IsNullOrEmpty(OpenInputFilePathDialog.FileName))
            {
                Context.Scripts.Save();
                LoadQuickScriptsFile(OpenInputFilePathDialog.FileName);
            }
        }

        private void QuickScriptEditor_Load(object sender, EventArgs e)
        {
            UpdateLastKnownPath();
        }

        private void UpdateLastKnownPath()
        {
            if (Context.Scripts == null || string.IsNullOrEmpty(Context.Scripts.FilePath) || !File.Exists(Context.Scripts.FilePath)) return;
            bool openedKey = false;
            if (MetX.Controls.Context.AppDataRegistry == null)
            {
                MetX.Controls.Context.AppDataRegistry = Application.UserAppDataRegistry;
                openedKey = true;
            }

            if (Context.AppDataRegistry == null)
            {
                return;
            }
            Context.AppDataRegistry.SetValue("LastQuickScriptPath", Context.Scripts.FilePath, RegistryValueKind.String);

            if (!openedKey || Context.AppDataRegistry == null)
            {
                return;
            }
            Context.AppDataRegistry.Close();
            Context.AppDataRegistry = null;
        }
    }
}