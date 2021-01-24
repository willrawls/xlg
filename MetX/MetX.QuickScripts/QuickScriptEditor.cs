namespace XLG.QuickScripts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;

    using MetX.Controls;
    using MetX.Library;
    using MetX.Scripts;

    using Microsoft.Win32;

    public partial class QuickScriptEditor : ScriptRunningWindow
    {
        public bool DestinationParamAlreadyFocused;

        public bool InputParamAlreadyFocused;

        public bool Updating;

        public QuickScriptEditor(string filePath)
        {
            InitializeComponent();

            InputParam.GotFocus += InputParam_GotFocus;
            InputParam.LostFocus += InputParam_LostFocus;

            DestinationParam.GotFocus += DestinationParam_GotFocus;
            DestinationParam.LostFocus += DestinationParam_LostFocus;

            LoadQuickScriptsFile(filePath);
        }

        public XlgQuickScript SelectedScript
        {
            get { return QuickScriptList.SelectedItem as XlgQuickScript; }
        }

        public void DisplayExpandedQuickScriptSourceInNotepad(bool independent)
        {
            try
            {
                if (ScriptEditor.Current == null)
                {
                    return;
                }

                UpdateScriptFromForm();
                var source = ScriptEditor.Current.ToCSharp(independent);
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

            if ((Context.Templates.Count == 0) ||
                string.IsNullOrEmpty(Context.Templates[TemplateList.Text].Views["Exe"]))
            {
                MessageBox.Show(this, "Quick script template 'Exe' missing for: " + TemplateList.Text);
                return null;
            }

            if (scriptToRun == null)
            {
                return null;
            }

            var source = scriptToRun.ToCSharp(true);
            var additionalReferences = Context.DefaultTypesForCompiler();
            var compilerResults = XlgQuickScript.CompileSource(source, true, additionalReferences, null);

            if (compilerResults.CompiledSuccessfully)
            {
                var assembly = compilerResults.CompiledAssembly;

                var parentDestination = scriptToRun.DestinationFilePath.TokensBeforeLast(@"\");

                if (string.IsNullOrEmpty(parentDestination)
                    && !string.IsNullOrEmpty(scriptToRun.InputFilePath)
                    && (scriptToRun.Input != "Web Address"))
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

                var metXDllPathSource = Path.Combine(assembly.Location.TokensBeforeLast(@"\"), "MetX.dll");

                parentDestination = Path.Combine(parentDestination, "bin");
                var metXDllPathDest = Path.Combine(parentDestination, "MetX.dll");

                var exeFilePath = Path.Combine(parentDestination, scriptToRun.Name.AsFilename()) + ".exe";
                var csFilePath = exeFilePath.Replace(".exe", ".cs");

                Directory.CreateDirectory(parentDestination);

                if (File.Exists(exeFilePath)) File.Delete(exeFilePath);
                if (File.Exists(csFilePath)) File.Delete(csFilePath);

                File.Copy(assembly.Location, exeFilePath);
                if (!File.Exists(metXDllPathDest))
                    File.Copy(metXDllPathSource, metXDllPathDest);
                File.WriteAllText(csFilePath, source);
                return exeFilePath;
            }

            var sb =
                new StringBuilder("Compilation failure. Errors found include:" + Environment.NewLine
                                  + Environment.NewLine);
            var lines = new List<string>(source.LineList());
            for (var index = 0; index < compilerResults.Failures.Length; index++)
            {
                var error = compilerResults.Failures[index].ToString();
                if (error.Contains("("))
                {
                    error = error.TokensAfterFirst("(").Replace(")", string.Empty);
                }

                sb.AppendLine(index + 1 + ": Line " + error);
                sb.AppendLine();
                if (error.Contains(Environment.NewLine))
                {
                    lines[compilerResults.Failures[index].Location.Line() - 1] += "\t// " + error.Replace(Environment.NewLine, " ");
                }
                else if (compilerResults.Failures[index].Location.Line() == 0)
                {
                    lines[0] += "\t// " + error;
                }
                else
                {
                    lines[compilerResults.Failures[index].Location.Line() - 1] += "\t// " + error;
                }
            }

            MessageBox.Show(sb.ToString());
            QuickScriptWorker.ViewTextInNotepad(lines.Flatten(), true);

            return null;
        }

        public override void Progress(int index = -1)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(Progress), new[] { index });
                return;
            }

            if (index >= 0)
            {
                RunningLabel.Text = "Script Running...";
                ProgressLabel.Text = index.ToString("0000000");
                Update();
            }
            else
            {
                RunningLabel.Text = "Not running.";
                ProgressLabel.Text = "0";
                Update();
            }
        }

        public void UpdateScriptFromForm()
        {
            if (ScriptEditor.Current == null)
            {
                return;
            }

            ScriptEditor.Current.Script = ScriptEditor.Text;
            Enum.TryParse(DestinationList.Text.Replace(" ", string.Empty), out ScriptEditor.Current.Destination);
            ScriptEditor.Current.Input = InputList.Text;
            ScriptEditor.Current.SliceAt = SliceAt.Text;
            ScriptEditor.Current.DiceAt = DiceAt.Text;
            ScriptEditor.Current.InputFilePath = InputParam.Text;
            ScriptEditor.Current.DestinationFilePath = DestinationParam.Text;
            ScriptEditor.Current.Template = TemplateList.Text;
            Context.Scripts.Default = ScriptEditor.Current;
        }

        private void BrowseDestinationFilePath_Click(object sender, EventArgs e)
        {
            SaveDestinationFilePathDialog.FileName = DestinationParam.Text;
            SaveDestinationFilePathDialog.InitialDirectory = DestinationParam.Text.TokensBeforeLast(@"\");
            SaveDestinationFilePathDialog.AddExtension = true;
            SaveDestinationFilePathDialog.DefaultExt = string.Empty;
            SaveDestinationFilePathDialog.CheckPathExists = true;
            SaveDestinationFilePathDialog.Filter = "All files (*.*)|*.*";
            SaveDestinationFilePathDialog.ShowDialog(this);
            if (!string.IsNullOrEmpty(SaveDestinationFilePathDialog.FileName))
            {
                DestinationParam.Text = SaveDestinationFilePathDialog.FileName;
            }
        }

        private void BrowseInputFilePath_Click(object sender, EventArgs e)
        {
            OpenInputFilePathDialog.FileName = InputParam.Text;
            OpenInputFilePathDialog.InitialDirectory = InputParam.Text.TokensBeforeLast(@"\");
            OpenInputFilePathDialog.AddExtension = true;
            OpenInputFilePathDialog.CheckFileExists = true;
            OpenInputFilePathDialog.CheckPathExists = true;
            OpenInputFilePathDialog.DefaultExt = string.Empty;
            OpenInputFilePathDialog.Filter = "All files (*.*)|*.*";
            OpenInputFilePathDialog.Multiselect = false;
            OpenInputFilePathDialog.ShowDialog(this);
            if (!string.IsNullOrEmpty(OpenInputFilePathDialog.FileName))
            {
                InputParam.Text = OpenInputFilePathDialog.FileName;
            }
        }

        private void DeleteScript_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            if (ScriptEditor.Current == null)
            {
                return;
            }

            var answer = MessageBox.Show(this,
                "This will permanently delete the current script.\n\tAre you sure this is what you want to do?",
                "DELETE SCRIPT", MessageBoxButtons.YesNo);
            if (answer == DialogResult.Yes)
            {
                Updating = true;
                var script = ScriptEditor.Current;
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

        private void DestinationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var input = DestinationList.Text;
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

        private void DestinationParam_Enter(object sender, EventArgs e)
        {
            DestinationParam.SelectAll();
        }

        private void DestinationParam_GotFocus(object sender, EventArgs e)
        {
            if (MouseButtons != MouseButtons.None) return;
            DestinationParam.SelectAll();
            DestinationParamAlreadyFocused = true;
        }

        private void DestinationParam_LostFocus(object sender, EventArgs e)
        {
            DestinationParamAlreadyFocused = false;
        }

        private void DestinationParam_MouseUp(object sender, MouseEventArgs e)
        {
            if (DestinationParamAlreadyFocused || (DestinationParam.SelectionLength != 0)) return;

            DestinationParamAlreadyFocused = true;
            DestinationParam.SelectAll();
        }

        private void EditDestinationFilePath_Click(object sender, EventArgs e)
        {
            QuickScriptWorker.ViewFileInNotepad(DestinationParam.Text);
        }

        private void EditInputFilePath_Click(object sender, EventArgs e)
        {
            QuickScriptWorker.ViewFileInNotepad(InputParam.Text);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InputList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var input = InputList.Text;
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

            EditInputFilePath.Enabled = InputParam.Enabled && (input != "Web Address");
            BrowseInputFilePath.Enabled = InputParam.Enabled && (input != "Web Address");
        }

        private void InputParam_Enter(object sender, EventArgs e)
        {
            InputParam.SelectAll();
        }

        private void InputParam_GotFocus(object sender, EventArgs e)
        {
            if (MouseButtons != MouseButtons.None) return;

            InputParam.SelectAll();
            InputParamAlreadyFocused = true;
        }

        private void InputParam_Leave(object sender, EventArgs e)
        {
        }

        private void InputParam_LostFocus(object sender, EventArgs e)
        {
            InputParamAlreadyFocused = false;
        }

        private void InputParam_MouseUp(object sender, MouseEventArgs e)
        {
            if (InputParamAlreadyFocused || (InputParam.SelectionLength != 0)) return;

            InputParamAlreadyFocused = true;
            InputParam.SelectAll();
        }

        private void LoadQuickScriptsFile(string filePath)
        {
            if (Context == null)
                Context = new Context();
            Context.Scripts = XlgQuickScriptFile.Load(filePath);

            if (Context.Scripts.Count == 0)
            {
                var script = new XlgQuickScript("First script", QuickScriptWorker.FirstScript);
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

        private void NewQuickScript_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            if (Context.Scripts != null)
            {
                var name = string.Empty;
                var answer = Ui.InputBoxRef("New Script Name", "Please enter the name for the new script.",
                    ref name);
                if ((answer != DialogResult.OK) || ((name ?? string.Empty).Trim() == string.Empty))
                {
                    return;
                }

                var script = string.Empty;
                XlgQuickScript newScript = null;
                if (ScriptEditor.Current != null)
                {
                    answer = MessageBox.Show(this, "Would you like to clone the current script?", "CLONE SCRIPT?",
                        MessageBoxButtons.YesNoCancel);
                    switch (answer)
                    {
                        case DialogResult.Cancel:
                            return;

                        case DialogResult.Yes:
                            UpdateScriptFromForm();

                            // script = Current.Script;
                            newScript = ScriptEditor.Current.Clone(name);
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenInputFilePathDialog.FileName = string.Empty;
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
            OpenInputFilePathDialog.FileName = string.Empty;
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

        private void QuickScriptEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                foreach (var outputWindow in Context.OutputWindows)
                {
                    outputWindow.Close();
                    outputWindow.Dispose();
                }

                Context.OutputWindows.Clear();
            }
            catch
            {
                // Ignored
            }

            SaveQuickScript_Click(sender, null);
        }

        private void QuickScriptEditor_Load(object sender, EventArgs e)
        {
            UpdateLastKnownPath();
        }

        private void QuickScriptEditor_ResizeEnd(object sender, EventArgs e)
        {
            // 785 - X = 450
            // 785 = X + 450
            // X = 785 - 450
            // X = 335
            if (Width < 787)
                InputParam.Width = 445;
            else
                InputParam.Width = Width - 335;

            DestinationParam.Width = InputParam.Width;
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

        private void RefreshLists()
        {
            Updating = true;
            try
            {
                QuickScriptList.Items.Clear();
                var defaultIndex = 0;
                foreach (var script in Context.Scripts)
                {
                    QuickScriptList.Items.Add(script);
                    if ((Context.Scripts.Default != null) && (script == Context.Scripts.Default))
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

        private void RunQuickScript_Click(object sender, EventArgs e)
        {
            try
            {
                if (ScriptEditor.Current == null)
                {
                    return;
                }

                UpdateScriptFromForm();
                RunQuickScript(this, ScriptEditor.Current, null);
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
                    SaveDestinationFilePathDialog.Filter = "Quick script files (*.xlgq)|*.xlgq|All files (*.*)|*.*";
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

        private void ShowInputOutputOptions_Click(object sender, EventArgs e)
        {
            ShowInputOutputOptions.Checked = !ShowInputOutputOptions.Checked;

            InputOptions.Visible = ShowInputOutputOptions.Checked;
            OutputOutputs.Visible = ShowInputOutputOptions.Checked;
        }

        private void testFuncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Determine order of the other
            var choices = new string[0];
            string[] selection = { "C", "A", "B" };

            var dialog = new ChooseOrderDialog();
            var items = dialog.Ask(choices, selection);

            if ((items != null) && (items.Length > 0))
            {
                var x = string.Empty;
                foreach (var item in items)
                {
                    x += item + "\r\n";
                }

                MessageBox.Show(x);
            }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
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

            var index = InputList.FindString(selectedScript.Input);
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
            ScriptEditor.Current = selectedScript;
        }

        private void UpdateLastKnownPath()
        {
            if ((Context.Scripts == null) || string.IsNullOrEmpty(Context.Scripts.FilePath) ||
                !File.Exists(Context.Scripts.FilePath)) return;
            var openedKey = false;
            if (Context.AppDataRegistry == null)
            {
                Context.AppDataRegistry = Application.UserAppDataRegistry;
                openedKey = true;
            }

            if (Context.AppDataRegistry == null)
            {
                return;
            }

            Context.AppDataRegistry.SetValue("LastQuickScriptPath", Context.Scripts.FilePath, RegistryValueKind.String);

            if (!openedKey || (Context.AppDataRegistry == null))
            {
                return;
            }

            Context.AppDataRegistry.Close();
            Context.AppDataRegistry = null;
        }

        private void ViewGeneratedCode_Click(object sender, EventArgs e)
        {
            DisplayExpandedQuickScriptSourceInNotepad(false);
        }

        private void ViewIndependectGeneratedCode_Click(object sender, EventArgs e)
        {
            // DisplayExpandedQuickScriptSourceInNotepad(true);
            try
            {
                if (ScriptEditor.Current == null)
                {
                    return;
                }

                UpdateScriptFromForm();
                var location = GenerateIndependentQuickScriptExe(ScriptEditor.Current);
                if (location.IsEmpty()) return;

                if (DialogResult.Yes == MessageBox.Show(this,
                    "Executable generated successfully at: " + location + Environment.NewLine +
                    Environment.NewLine +
                    "Would you like to run it now? (No will open the generated file).", "RUN EXE?",
                    MessageBoxButtons.YesNo))
                {
                    Process.Start(new ProcessStartInfo(location)
                    {
                        UseShellExecute = true,
                        WorkingDirectory = location.TokensBeforeLast(@"\")
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

        public string PreviousFind { get; set; }
        public string PreviousReplace { get; set; }
        
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var choices = new string[0];
            string[] selection = { "C", "A", "B" };

            var dialog = new AskForStringDialog(PreviousFind);
            var answer = dialog.Ask("What would you like to find?", "FIND NEXT");

            if (answer.IsNotEmpty())
            {
                //this.ScriptEditor.

                //MessageBox.Show(x);
            }

        }
    }
}
