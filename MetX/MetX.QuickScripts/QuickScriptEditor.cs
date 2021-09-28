using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using MetX.Standard;
using MetX.Standard.Interfaces;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Pipelines;
using MetX.Windows.Library;
using NHotkey;
using NHotkey.WindowsForms;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using NHotPhrase.WindowsForms;

namespace XLG.QuickScripts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;

    using MetX.Controls;
    using MetX.Standard.Library;
    using MetX.Standard.Scripts;

    using Microsoft.Win32;

    public partial class QuickScriptEditor : ScriptRunningWindow
    {
        public bool DestinationParamAlreadyFocused;
        public bool InputParamAlreadyFocused;
        public bool Updating;

        public HotPhraseManagerForWinForms Manager { get; set; } = new();

        public QuickScriptEditor(string filePath)
        {
            InitializeComponent();

            InputParam.GotFocus += InputParam_GotFocus;
            InputParam.LostFocus += InputParam_LostFocus;

            DestinationParam.GotFocus += DestinationParam_GotFocus;
            DestinationParam.LostFocus += DestinationParam_LostFocus;

            Host = new WinFormGenerationHost<QuickScriptEditor>(this, Clipboard.GetText);
            
            LoadQuickScriptsFile(filePath);
            InitializeHotKeys();
        }


        private void InitializeHotKeys()
        {
            Manager.Keyboard.AddOrReplace("RunCurrentQuickScript", new() { PKey.Control, PKey.Shift, PKey.Shift }, OnRunCurrentQuickScript);
            Manager.Keyboard.AddOrReplace("PickAndRunQuickScript", new() { PKey.Control, PKey.Alt, PKey.Alt }, OnPickAndRunQuickScript);
        }

        private void OnPickAndRunQuickScript(object? sender, PhraseEventArguments e)
        {
            try
            {
                if (ScriptEditor.Current == null)
                {
                    return;
                }

                UpdateScriptFromForm();

                var chooseQuickScript = new ChooseOneDialog();
                var choices = Host.Context.Scripts.ScriptNames();
                if (LastChoice > choices.Length)
                {
                    LastChoice = 0;
                }
                var choice = chooseQuickScript.Ask(choices, "Run which quick script?", "CHOOSE QUICK SCRIPT TO RUN", LastChoice);
                if (choice >= 0)
                {
                    LastChoice = choice;
                    var selectedScript = Host.Context.Scripts[choice];
                    RunQuickScript(this, selectedScript, null);
                }

            }
            catch (Exception exception)
            {
                Host.MessageBox.Show(exception.ToString());
            }

            e.Handled = true;            
        }

        private void OnRunCurrentQuickScript(object sender, PhraseEventArguments e)
        {
            try
            {
                // Same as clicking the Run button
                RunQuickScript_Click(null, null);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            e.Handled = true;
        }

        public int LastChoice { get; set; } = 0;


        public XlgQuickScript SelectedScript => QuickScriptList.SelectedItem as XlgQuickScript;

        public void DisplayExpandedQuickScriptSourceInNotepad(bool independent)
        {
            try
            {
                if (ScriptEditor.Current == null)
                {
                    return;
                }

                UpdateScriptFromForm();
                var source = ScriptEditor.Current.ToCSharp(independent, ContextBase.Default.Templates["Native"]);
                if (!string.IsNullOrEmpty(source))
                {
                    QuickScriptWorker.ViewText(Host, source, true);
                }
            }
            catch (Exception e)
            {
                Host.MessageBox.Show(e.ToString());
            }
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
            ScriptEditor.Current.NativeTemplateName = NativeTemplateList.Text.AsString("Native");
            ScriptEditor.Current.ExeTemplateName = ExeTemplateList.Text.AsString("Exe");
            Host.Context.Scripts.Default = ScriptEditor.Current;
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

            var answer = Host.MessageBox.Show(
                "This will permanently delete the current script.\n\tAre you sure this is what you want to do?",
                "DELETE SCRIPT", MessageBoxChoices.YesNo);
            if (answer == MessageBoxResult.Yes)
            {
                Updating = true;
                var script = ScriptEditor.Current;
                try
                {
                    QuickScriptList.Items.Remove(script);
                    Host.Context.Scripts.Remove(script);
                }
                finally
                {
                    Updating = false;
                }

                if (Host.Context.Scripts.Count == 0)
                {
                    script = new XlgQuickScript("First script");
                    Host.Context.Scripts.Add(script);
                    Host.Context.Scripts.Default = script;
                }
                else if (Host.Context.Scripts.Default == script)
                {
                    Host.Context.Scripts.Default = Host.Context.Scripts[0];
                }

                RefreshLists();
                UpdateFormWithScript(Host.Context.Scripts.Default);
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
                // ReSharper disable once StringLiteralTypo
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
            if (DestinationParamAlreadyFocused || DestinationParam.SelectionLength != 0) return;

            DestinationParamAlreadyFocused = true;
            DestinationParam.SelectAll();
        }

        private void EditDestinationFilePath_Click(object sender, EventArgs e)
        {
            QuickScriptWorker.ViewFile(Host, DestinationParam.Text);
        }

        private void EditInputFilePath_Click(object sender, EventArgs e)
        {
            QuickScriptWorker.ViewFile(Host, InputParam.Text);
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

            EditInputFilePath.Enabled = InputParam.Enabled && input != "Web Address";
            BrowseInputFilePath.Enabled = InputParam.Enabled && input != "Web Address";
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
            if (InputParamAlreadyFocused || InputParam.SelectionLength != 0) return;

            InputParamAlreadyFocused = true;
            InputParam.SelectAll();
        }

        private void LoadQuickScriptsFile(string filePath)
        {
            Host.Context ??= new Context(Host);
            var xlgQuickScriptFile = XlgQuickScriptFile.Load(filePath);
            Host.Context.Scripts = xlgQuickScriptFile;

            if (Host.Context.Scripts.Count == 0)
            {
                var script = new XlgQuickScript("First script", QuickScriptWorker.FirstScript);
                Host.Context.Scripts.Add(script);
                Host.Context.Scripts.Default = script;
                script = new XlgQuickScript("Example / Tutorial", QuickScriptWorker.ExampleTutorialScript);
                Host.Context.Scripts.Add(script);
                Host.Context.Scripts.Save();
            }

            UpdateLastKnownPath();

            RefreshLists();
            UpdateFormWithScript(Host.Context.Scripts.Default);
            Text = "Quick Script - " + filePath;
        }

        private void NewQuickScript_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            if (Host.Context.Scripts != null)
            {
                var name = string.Empty;
                var answer = Host.InputBox("New Script Name", "Please enter the name for the new script.", ref name);
                if (answer != MessageBoxResult.OK || (name ?? string.Empty).Trim() == string.Empty)
                {
                    return;
                }

                var script = string.Empty;
                XlgQuickScript newScript = null;
                if (ScriptEditor.Current != null)
                {
                    answer = Host.MessageBox.Show("Would you like to clone the current script?", "CLONE SCRIPT?", MessageBoxChoices.YesNoCancel);
                    switch (answer)
                    {
                        case MessageBoxResult.Cancel:
                            return;

                        case MessageBoxResult.Yes:
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

                    Host.Context.Scripts.Add(newScript);
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
            OpenInputFilePathDialog.InitialDirectory = Host.Context.Scripts.FilePath.TokensBeforeLast(@"\");
            OpenInputFilePathDialog.AddExtension = true;
            OpenInputFilePathDialog.CheckFileExists = false;
            OpenInputFilePathDialog.CheckPathExists = true;
            OpenInputFilePathDialog.DefaultExt = ".xlgq";
            OpenInputFilePathDialog.Filter = "Quick script files (*.xlgq)|*.xlgq|All files (*.*)|*.*";
            OpenInputFilePathDialog.Multiselect = false;
            OpenInputFilePathDialog.ShowDialog(this);
            if (!string.IsNullOrEmpty(OpenInputFilePathDialog.FileName))
            {
                Host.Context.Scripts.Save();
                LoadQuickScriptsFile(OpenInputFilePathDialog.FileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenInputFilePathDialog.FileName = string.Empty;
            OpenInputFilePathDialog.InitialDirectory = Host.Context.Scripts.FilePath.TokensBeforeLast(@"\");
            OpenInputFilePathDialog.AddExtension = true;
            OpenInputFilePathDialog.CheckFileExists = true;
            OpenInputFilePathDialog.CheckPathExists = true;
            OpenInputFilePathDialog.DefaultExt = ".xlgq";
            OpenInputFilePathDialog.Filter = "Quick script files (*.xlgq)|*.xlgq|All files (*.*)|*.*";
            OpenInputFilePathDialog.Multiselect = false;
            OpenInputFilePathDialog.ShowDialog(this);
            if (!string.IsNullOrEmpty(OpenInputFilePathDialog.FileName))
            {
                Host.Context.Scripts.Save();
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
                foreach (var script in Host.Context.Scripts)
                {
                    QuickScriptList.Items.Add(script);
                    if (Host.Context.Scripts.Default != null && script == Host.Context.Scripts.Default)
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
                Host.MessageBox.Show(exception.ToString());
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

                if (Host.Context.Scripts != null)
                {
                    UpdateScriptFromForm();

                    SaveDestinationFilePathDialog.FileName = Host.Context.Scripts.FilePath;
                    SaveDestinationFilePathDialog.InitialDirectory = Host.Context.Scripts.FilePath.TokensBeforeLast(@"\");
                    SaveDestinationFilePathDialog.AddExtension = true;
                    SaveDestinationFilePathDialog.CheckPathExists = true;
                    SaveDestinationFilePathDialog.DefaultExt = ".xlgq";
                    SaveDestinationFilePathDialog.Filter = "Quick script files (*.xlgq)|*.xlgq|All files (*.*)|*.*";
                    SaveDestinationFilePathDialog.ShowDialog(this);
                    if (!string.IsNullOrEmpty(SaveDestinationFilePathDialog.FileName))
                    {
                        Host.Context.Scripts.FilePath = SaveDestinationFilePathDialog.FileName;
                        Host.Context.Scripts.Save();
                        Text = "Quick Scriptr - " + Host.Context.Scripts.FilePath;
                        UpdateLastKnownPath();
                    }
                }
            }
            catch (Exception exception)
            {
                Host.MessageBox.Show(exception.ToString());
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

                if (Host.Context.Scripts != null)
                {
                    UpdateScriptFromForm();
                    if (string.IsNullOrEmpty(Host.Context.Scripts.FilePath))
                        SaveAs_Click(null, null);
                    else Host.Context.Scripts.Save();
                }
            }
            catch (Exception exception)
            {
                Host.MessageBox.Show(exception.ToString());
            }
        }

        private void ShowInputOutputOptions_Click(object sender, EventArgs e)
        {
            ShowInputOutputOptions.Checked = !ShowInputOutputOptions.Checked;

            InputOptions.Visible = ShowInputOutputOptions.Checked;
            OutputOptions.Visible = ShowInputOutputOptions.Checked;
        }

        private void testFuncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Determine order of the other
            var choices = new string[0];
            string[] selection = { "C", "A", "B" };

            var dialog = new ChooseOrderDialog();
            var items = dialog.Ask(choices, selection);

            if (items != null && items.Length > 0)
            {
                var x = string.Empty;
                foreach (var item in items)
                {
                    x += item + "\r\n";
                }

                Host.MessageBox.Show(x);
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

            index = NativeTemplateList.FindString(selectedScript.NativeTemplateName);
            NativeTemplateList.SelectedIndex = index > -1
                ? index
                : NativeTemplateList.Items.Add(selectedScript.NativeTemplateName);

            index = ExeTemplateList.FindString(selectedScript.ExeTemplateName);
            ExeTemplateList.SelectedIndex = index > -1
                ? index
                : ExeTemplateList.Items.Add(selectedScript.ExeTemplateName);

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
            if (Host.Context.Scripts == null || string.IsNullOrEmpty(Host.Context.Scripts.FilePath) ||
                !File.Exists(Host.Context.Scripts.FilePath)) return;
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

            Context.AppDataRegistry.SetValue("LastQuickScriptPath", Host.Context.Scripts.FilePath, RegistryValueKind.String);

            if (!openedKey || Context.AppDataRegistry == null)
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

        private void findMenuItem_Click(object sender, EventArgs e)
        {
            ScriptEditor.FindAndReplaceForm.ShowFor(false);
        }

        private void replaceMenuItem_Click(object sender, EventArgs e)
        {
            ScriptEditor.FindAndReplaceForm.ShowFor(true);
        }

        private void ViewIndependentGeneratedCode_Click(object sender, EventArgs e)
        {
            try
            {
                if (ScriptEditor.Current == null)
                {
                    return;
                }

                UpdateScriptFromForm();

                var settings = ScriptEditor.Current.BuildSettings(true, false, Host);
                //var result = settings.QuickScriptTemplate.ActualizeCode(settings);
                var result = settings.ActualizeAndCompile();
                QuickScriptWorker.ViewText(Host, result.FinalDetails(), false);
                if (!result.CompileSuccessful) return;

                var location = result.DestinationExecutableFilePath;
                if (location.IsEmpty()) return;

                if ( MessageBoxResult.Yes == Host.MessageBox.Show(
                    "Executable generated successfully at: " + location + Environment.NewLine +
                    Environment.NewLine +
                    "Would you like to run it now? (Yes to run, No to view source).", "RUN EXE OR VIEW FOLDER?", MessageBoxChoices.YesNo))
                {
                    var arguments = $"/k \"{location}\"";
                    Process.Start(new ProcessStartInfo("cmd.exe", arguments )
                    {
                        UseShellExecute = true,
                        WorkingDirectory = result.Settings.OutputFolder,
                        Arguments = ScriptEditor.Current.AsParameters(),
                        Verb = "runas",
                    });
                }
                else
                {
                    var outputFile = Path.Combine(settings.OutputFolder, "QuickScriptProcessor.cs");
                    QuickScriptWorker.ViewFile(Host, outputFile);
                }
            }
            catch (Exception exception)
            {
                Host.MessageBox.Show(exception.ToString());
            }
        }
    }
}
