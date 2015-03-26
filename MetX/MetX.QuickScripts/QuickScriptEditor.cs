using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using MetX.Data;
using MetX.Library;
using Microsoft.Win32;

namespace XLG.QuickScripts
{
    public partial class QuickScriptEditor : Form
    {
        public static readonly List<QuickScriptOutput> OutputWindows = new List<QuickScriptOutput>();

        public XlgQuickScript SelectedScript { get { return QuickScriptList.SelectedItem as XlgQuickScript; } }

        private readonly object m_ScriptSyncRoot = new object();
        private readonly TextArea textArea;
        private CodeCompletionWindow completionWindow;
        public XlgQuickScript CurrentScript = null;
        protected bool ScriptIsRunning;
        public XlgQuickScriptFile Scripts;
        public bool Updating;

        public QuickScriptEditor(string filePath)
        {
            InitializeComponent();

            textArea = ScriptEditor.ActiveTextAreaControl.TextArea;
            textArea.KeyEventHandler += ProcessKey;
            textArea.KeyUp += TextAreaOnKeyUp;

            FileSyntaxModeProvider fsmProvider = new FileSyntaxModeProvider(AppDomain.CurrentDomain.BaseDirectory);
            HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.
            ScriptEditor.SetHighlighting("QuickScript"); // Activate the highlighting, use the name from the SyntaxDefinition node.
            //ScriptEditor.SetHighlighting("C#");
            ScriptEditor.Refresh();

            LoadQuickScriptsFile(filePath);
        }

        private void TextAreaOnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                e.Handled = true;
                RunQuickScript_Click(null, null);
            }
        }

        private bool ProcessKey(char ch)
        {
            string[] items = null;
            if (ch == '.')      items = new[] {"Output", "Lines", "AllText", "DestionationFilePath", "InputFilePath", "LineCount", "OpenNotepad", "Ask"};
            else if (ch == '~') items = new[] {"~:", "~Members:", "~Start:", "~Body:", "~Finish:", "~BeginString:", "~EndString:"};
            if (items != null && items.Length > 0)
            {
                CompletionDataProvider completionDataProvider = new CompletionDataProvider(items);
                completionWindow = CodeCompletionWindow.ShowCompletionWindow(this, ScriptEditor, String.Empty, completionDataProvider, '.');
                if (completionWindow != null) completionWindow.Closed += CompletionWindowClosed;
            }
            return false;
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
                foreach (XlgQuickScript script in Scripts)
                {
                    QuickScriptList.Items.Add(script);
                    if (Scripts.Default != null && script == Scripts.Default)
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

        public void OpenNewOutput(XlgQuickScript script, string title, string output)
        {
            QuickScriptOutput quickScriptOutput = new QuickScriptOutput(this, title, output, script);
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
            Enum.TryParse(DestinationList.Text.Replace(" ", string.Empty), out CurrentScript.Destination);
            CurrentScript.Input = InputList.Text;
            CurrentScript.SliceAt = SliceAt.Text;
            CurrentScript.DiceAt = DiceAt.Text;
            CurrentScript.InputFilePath = InputParam.Text;
            CurrentScript.DestinationFilePath = DestinationParam.Text;
            CurrentScript.Template = TemplateList.Text;
            Scripts.Default = CurrentScript;
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

        public BaseLineProcessor GenerateQuickScriptLineProcessor(XlgQuickScript scriptToRun)
        {
            if (string.IsNullOrEmpty(XlgQuickScript.DependentTemplate))
            {
                MessageBox.Show(this, "Quick script template missing.");
                return null;
            }

            if (scriptToRun == null)
            {
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
                                  .Replace(")", string.Empty));
                sb.AppendLine();
            }
            MessageBox.Show(sb.ToString());
            QuickScriptWorker.ViewTextInNotepad(source, true);

            return null;
        }

        public string GenerateIndependentQuickScriptExe(XlgQuickScript scriptToRun)
        {
            if (InvokeRequired)
            {
                return (string) Invoke(new d_GenerateExe(GenerateIndependentQuickScriptExe), scriptToRun);
            }
            if (string.IsNullOrEmpty(XlgQuickScript.IndependentTemplate))
            {
                MessageBox.Show(this, "Independent Quick script template missing.");
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
                    parentDestination = Scripts.FilePath.TokensBeforeLast(@"\");

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

                if(File.Exists(exeFilePath))    File.Delete(exeFilePath);
                if(File.Exists(csFilePath))     File.Delete(csFilePath);

                File.Copy(assembly.Location, exeFilePath);
                if(!File.Exists(metXDllPathDest))
                    File.Copy(metXDllPathSource, metXDllPathDest );
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
            Scripts = XlgQuickScriptFile.Load(filePath);

            if (Scripts.Count == 0)
            {
                XlgQuickScript script = new XlgQuickScript("First script", QuickScriptWorker.FirstScript);
                Scripts.Add(script);
                Scripts.Default = script;
                script = new XlgQuickScript("Example / Tutorial", QuickScriptWorker.ExampleTutorialScript);
                Scripts.Add(script);
                Scripts.Save();
            }
            UpdateLastKnownPath();

            RefreshLists();
            UpdateFormWithScript(Scripts.Default);
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
                RunQuickScript(CurrentScript);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        public void RunQuickScript(XlgQuickScript scriptToRun, QuickScriptOutput targetOutput = null)
        {
            if (InvokeRequired)
            {
                Invoke(new d_RunQuickScript(RunQuickScript), scriptToRun, targetOutput);
                return;
            }

            bool lockTaken = false;
            Monitor.TryEnter(m_ScriptSyncRoot, ref lockTaken);
            if (!lockTaken)
            {
                return;
            }

            try
            {
                ScriptIsRunning = true;
                if (scriptToRun.Destination == QuickScriptDestination.File)
                {
                    if (string.IsNullOrEmpty(scriptToRun.DestinationFilePath))
                    {
                        MessageBox.Show(this, "Please supply an output filename.", "OUTPUT FILE PATH REQUIRED");
                        DestinationParam.Focus();
                        return;
                    }
                    if (!File.Exists(scriptToRun.DestinationFilePath))
                    {
                        Directory.CreateDirectory(scriptToRun.DestinationFilePath.TokensBeforeLast(@"\"));
                    }
                }

                BaseLineProcessor quickScriptProcessor = GenerateQuickScriptLineProcessor(scriptToRun);
                bool? inputResult = quickScriptProcessor.ReadInput(scriptToRun.Input);
                switch (inputResult)
                {
                    case null:
                        InputParam.Focus();
                        return;
                    case false:
                        return;
                    // True keep going
                }

                // Start
                try
                {
                    if (!quickScriptProcessor.Start())
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error running Start:" + Environment.NewLine + ex);
                }

                // Process each line
                for (int index = 0; index < quickScriptProcessor.Lines.Count; index++)
                {
                    string currLine = quickScriptProcessor.Lines[index];
                    try
                    {
                        if (!quickScriptProcessor.ProcessLine(currLine, index))
                        {
                            return;
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
                            return;
                        }
                    }
                }
                try
                {
                    if (!quickScriptProcessor.Finish())
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error running Finish:" + Environment.NewLine + ex);
                }

                if (quickScriptProcessor.Output == null || quickScriptProcessor.Output.Length <= 0)
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
                                    scriptToRun,
                                    QuickScriptList.Text + " at " + DateTime.Now.ToString("G"),
                                    quickScriptProcessor.Output.ToString());
                            }
                            else
                            {
                                targetOutput.Text = QuickScriptList.Text + " at " + DateTime.Now.ToString("G");
                                targetOutput.Output.Text = quickScriptProcessor.Output.ToString();
                            }
                            break;

                        case QuickScriptDestination.Clipboard:
                            Clipboard.Clear();
                            Clipboard.SetText(quickScriptProcessor.Output.ToString());
                            break;

                        case QuickScriptDestination.Notepad:
                            QuickScriptWorker.ViewTextInNotepad(quickScriptProcessor.Output.ToString(), false);
                            break;

                        case QuickScriptDestination.File:
                            File.WriteAllText(DestinationParam.Text, quickScriptProcessor.Output.ToString());
                            QuickScriptWorker.ViewFileInNotepad(DestinationParam.Text);
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

        private void SaveQuickScript_Click(object sender, EventArgs e)
        {
            try
            {
                if (Updating)
                {
                    return;
                }
                if (Scripts != null)
                {
                    UpdateScriptFromForm();
                    if( string.IsNullOrEmpty(Scripts.FilePath))
                        SaveAs_Click(null, null);
                    else Scripts.Save();
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
            if (Scripts != null)
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
                    Scripts.Add(newScript);
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
                foreach (QuickScriptOutput outputWindow in OutputWindows)
                {
                    outputWindow.Close();
                    outputWindow.Dispose();
                }
                OutputWindows.Clear();
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
                    Scripts.Remove(script);
                }
                finally
                {
                    Updating = false;
                }
                if (Scripts.Count == 0)
                {
                    script = new XlgQuickScript("First script");
                    Scripts.Add(script);
                    Scripts.Default = script;
                }
                else if (Scripts.Default == script)
                {
                    Scripts.Default = Scripts[0];
                }
                RefreshLists();
                UpdateFormWithScript(Scripts.Default);
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

        private delegate void d_RunQuickScript(XlgQuickScript scriptToRun, QuickScriptOutput targetOutput);

        private delegate string d_GenerateExe(XlgQuickScript scriptToRun);

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
                if (Scripts != null)
                {
                    UpdateScriptFromForm();

                    SaveDestinationFilePathDialog.FileName = Scripts.FilePath;
                    SaveDestinationFilePathDialog.InitialDirectory = Scripts.FilePath.TokensBeforeLast(@"\");
                    SaveDestinationFilePathDialog.AddExtension = true;
                    SaveDestinationFilePathDialog.CheckPathExists = true;
                    SaveDestinationFilePathDialog.DefaultExt = ".xlgq";
                    SaveDestinationFilePathDialog.Filter = "Quick script files (*.xlgq)|*.xlgq;All files (*.*)|*.*";
                    SaveDestinationFilePathDialog.ShowDialog(this);
                    if (!string.IsNullOrEmpty(SaveDestinationFilePathDialog.FileName))
                    {
                        Scripts.FilePath = SaveDestinationFilePathDialog.FileName;
                        Scripts.Save();
                        Text = "Quick Scriptr - " + Scripts.FilePath;
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
            OpenInputFilePathDialog.InitialDirectory = Scripts.FilePath.TokensBeforeLast(@"\");
            OpenInputFilePathDialog.AddExtension = true;
            OpenInputFilePathDialog.CheckFileExists = false;
            OpenInputFilePathDialog.CheckPathExists = true;
            OpenInputFilePathDialog.DefaultExt = ".xlgq";
            OpenInputFilePathDialog.Filter = "Quick script files (*.xlgq)|*.xlgq|All files (*.*)|*.*";
            OpenInputFilePathDialog.Multiselect = false;
            OpenInputFilePathDialog.ShowDialog(this);
            if (!string.IsNullOrEmpty(OpenInputFilePathDialog.FileName))
            {
                Scripts.Save();
                LoadQuickScriptsFile(OpenInputFilePathDialog.FileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenInputFilePathDialog.FileName = "";
            OpenInputFilePathDialog.InitialDirectory = Scripts.FilePath.TokensBeforeLast(@"\");
            OpenInputFilePathDialog.AddExtension = true;
            OpenInputFilePathDialog.CheckFileExists = true;
            OpenInputFilePathDialog.CheckPathExists = true;
            OpenInputFilePathDialog.DefaultExt = ".xlgq";
            OpenInputFilePathDialog.Filter = "Quick script files (*.xlgq)|*.xlgq|All files (*.*)|*.*";
            OpenInputFilePathDialog.Multiselect = false;
            OpenInputFilePathDialog.ShowDialog(this);
            if (!string.IsNullOrEmpty(OpenInputFilePathDialog.FileName))
            {
                Scripts.Save();
                LoadQuickScriptsFile(OpenInputFilePathDialog.FileName);
            }
        }

        public static RegistryKey AppDataRegistry;

        private void QuickScriptEditor_Load(object sender, EventArgs e) 
        {
            UpdateLastKnownPath();
        }

        private void UpdateLastKnownPath()
        {
            if (Scripts == null || string.IsNullOrEmpty(Scripts.FilePath) || !File.Exists(Scripts.FilePath)) return;
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
            AppDataRegistry.SetValue("LastQuickScriptPath", Scripts.FilePath, RegistryValueKind.String);

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