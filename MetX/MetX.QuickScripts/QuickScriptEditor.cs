using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MetX.Controls;
using MetX.Standard.IO;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Pipelines;
using MetX.Standard.Scripts;
using MetX.Windows;
using MetX.Windows.Library;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using NHotPhrase.WindowsForms;

namespace XLG.QuickScripts;

public partial class QuickScriptEditor : ScriptRunningWindow
{
    public bool DestinationParamAlreadyFocused;
    public bool InputParamAlreadyFocused;
    public bool Updating;

    public HotPhraseManagerForWinForms Manager { get; set; } = new();

    public int LastChoice { get; set; }


    public XlgQuickScript SelectedScript => 
        QuickScriptList.SelectedItems.Count != 0
        ? QuickScriptList.SelectedItems[0].Tag as XlgQuickScript
        : null;

    public string LastSuccessfulCloneFolder { get; set; }

    public QuickScriptEditor(string filePath)
    {
        InitializeComponent();
        ChangeTheme(ColorScheme.DarkThemeOne, Controls);
        ChangeTheme(ColorScheme.DarkThemeOne, ScriptEditor.Controls);

        InputParam.GotFocus += InputParam_GotFocus;
        InputParam.LostFocus += InputParam_LostFocus;

        DestinationParam.GotFocus += DestinationParam_GotFocus;
        DestinationParam.LostFocus += DestinationParam_LostFocus;

        Host = new WinFormGenerationHost<QuickScriptEditor>(this, Clipboard.GetText);

        LoadQuickScriptsFile(filePath);
        InitializeHotPhrases();
    }


    private void InitializeHotPhrases()
    {
        Manager.Keyboard.AddOrReplace("Pick and Run QuickScript",
            new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.LControlKey, PKey.LControlKey, PKey.LShiftKey },
            OnPickAndRunQuickScript);
        Manager.Keyboard.AddOrReplace("Run Current QuickScript",
            new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.LControlKey, PKey.LControlKey, PKey.Alt },
            OnRunCurrentQuickScript);
    }

    private void OnPickAndRunQuickScript(object? sender, PhraseEventArguments e)
    {
        try
        {
            if (ScriptEditor.Current == null) return;

            UpdateScriptFromForm();

            var chooseQuickScript = new ChooseFromListDialog();
            var choices = Host.Context.Scripts.ScriptNames();
            if (LastChoice > choices.Length) LastChoice = 0;
            var choice = chooseQuickScript.Ask(choices, "Run which quick script?", "CHOOSE QUICK SCRIPT TO RUN",
                LastChoice);
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
        Host.WaitFor(() =>
        {
            RunQuickScript_Click(null, null);
        });

        e.Handled = true;
    }

    public void DisplayExpandedQuickScriptSourceInNotepad()
    {
        try
        {
            if (ScriptEditor?.Current == null) return;
            UpdateScriptFromForm();

            var settings = ScriptEditor.Current.BuildSettings(false, Host);
            var result = settings.QuickScriptTemplate.ActualizeCode(settings);

            var source = result.OutputFiles["QuickScriptProcessor"].Value;
            if (!string.IsNullOrEmpty(source)) QuickScriptWorker.ViewText(Host, source, true);
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
        if (ScriptEditor.Current == null) return;

        ScriptEditor.Current.Name = QuickScriptName.Text.AsString(DateTime.Now.ToString("s"));
        ScriptEditor.Current.Script = ScriptEditor.Text;
        Enum.TryParse(DestinationList.Text.Replace(" ", string.Empty), out ScriptEditor.Current.Destination);
        ScriptEditor.Current.Input = InputList.Text;
        ScriptEditor.Current.SliceAt = SliceAt.Text;
        ScriptEditor.Current.DiceAt = DiceAt.Text;
        ScriptEditor.Current.InputFilePath = InputParam.Text;
        ScriptEditor.Current.DestinationFilePath = DestinationParam.Text;
        ScriptEditor.Current.TemplateName = TemplateFolderPath.Text.AsString("Exe");
        //ScriptEditor.Current.TemplateName = TemplateList.Text.AsString("Exe");
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
            DestinationParam.Text = SaveDestinationFilePathDialog.FileName;
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
        if (!string.IsNullOrEmpty(OpenInputFilePathDialog.FileName)) InputParam.Text = OpenInputFilePathDialog.FileName;
    }

    private void DeleteScript_Click(object sender, EventArgs e)
    {
        if (Updating) return;

        if (ScriptEditor.Current == null) return;

        var answer = Host.MessageBox.Show(
            "This will permanently delete the current script.\n\tAre you sure this is what you want to do?",
            "DELETE SCRIPT", MessageBoxChoices.YesNo);
        if (answer == MessageBoxResult.Yes)
        {
            Updating = true;
            XlgQuickScript script = ScriptEditor.Current;
            try
            {
                var index = QuickScriptList.Items.IndexOfKey(script.Name);
                QuickScriptList.Items.RemoveAt(index);
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

    private void InputParam_LostFocus(object sender, EventArgs e)
    {
        InputParamAlreadyFocused = false;
    }

    private void InputParam_MouseUp(object sender, MouseEventArgs e)
    {

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
            Host.Context.Scripts.Save(Dirs.ScriptArchivePath);
        }

        Dirs.LastScriptFilePath = filePath;

        RefreshLists();
        UpdateFormWithScript(Host.Context.Scripts.Default);
        Text = "Quick Script - " + filePath;
    }

    private void NewQuickScript_Click(object sender, EventArgs e)
    {
        if (Updating) return;

        if (Host.Context.Scripts != null)
        {
            var name = string.Empty;
            var answer = Host.InputBox("New Script Name", "Please enter the name for the new script.", ref name);
            if (answer != MessageBoxResult.OK || (name ?? string.Empty).Trim() == string.Empty) return;

            var script = string.Empty;
            XlgQuickScript newScript = null;
            if (ScriptEditor.Current != null)
            {
                answer = Host.MessageBox.Show("Would you like to copy the current script?", "COPY SCRIPT?",
                    MessageBoxChoices.YesNoCancel);
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
                    if (script.IsEmpty())
                        newScript.Input = "Clipboard";
                }

                QuickScriptList.SelectedItems.Clear();
                Host.Context.Scripts.Add(newScript);
                var listViewItem = ScriptToListViewItem(newScript, true);
                QuickScriptList.Items.Add(listViewItem);
                UpdateFormWithScript(newScript);
            }
            finally
            {
                Updating = false;
            }
        }
    }

    private static ListViewItem ScriptToListViewItem(XlgQuickScript newScript, bool selected)
    {
        var listViewItem = new ListViewItem(newScript.Name)
        {
            Tag = newScript,
            Selected = selected,
        };
        return listViewItem;
    }


    private void NewToolStripMenuItem_Click(object sender, EventArgs e)
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
            Host.Context.Scripts.Save(Dirs.ScriptArchivePath);
            LoadQuickScriptsFile(OpenInputFilePathDialog.FileName);
        }
    }

    private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
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
            Host.Context.Scripts.Save(Dirs.ScriptArchivePath);
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

    private void RefreshLists()
    {
        Updating = true;
        try
        {
            QuickScriptList.Items.Clear();
            var defaultIndex = 0;
            foreach (var script in Host.Context.Scripts)
            {
                var listViewItem = ScriptToListViewItem(script, false);
                QuickScriptList.Items.Add(listViewItem);
                if (Host.Context.Scripts.Default != null && script == Host.Context.Scripts.Default)
                {
                    listViewItem.Selected = true;
                }
            }


        }
        finally
        {
            Updating = false;
        }
    }

    private void RunQuickScript_Click(object sender, EventArgs e)
    {
        if (ScriptEditor.Current == null) return;
        Host.WaitFor(() =>
        {
            UpdateScriptFromForm();
            RunQuickScript(this, ScriptEditor.Current, null);
        });
    }

    private void SaveAs_Click(object sender, EventArgs e)
    {
        try
        {
            if (Updating) return;

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
                    Host.Context.Scripts.Save(Dirs.ScriptArchivePath);
                    Text = "qkScrptR - " + Host.Context.Scripts.FilePath;
                    Dirs.LastScriptFilePath = Host.Context.Scripts.FilePath;
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
            if (Updating) return;

            if (Host.Context.Scripts != null)
            {
                UpdateScriptFromForm();
                if (string.IsNullOrEmpty(Host.Context.Scripts.FilePath))
                    SaveAs_Click(null, null);
                else Host.Context.Scripts.Save(Dirs.ScriptArchivePath);
            }
        }
        catch (Exception exception)
        {
            Host.MessageBox.Show(exception.ToString());
        }
    }

    private void UpdateFormWithScript(XlgQuickScript selectedScript)
    {
        if (SelectedScript == null) return;

        QuickScriptName.Text = selectedScript.Name;

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

        TemplateFolderPath.Text = selectedScript.TemplateName;

        InputParam.Text = selectedScript.InputFilePath;
        if (InputParam.Text.Length > 0) InputParam.SelectionStart = InputParam.Text.Length;

        DestinationParam.Text = selectedScript.DestinationFilePath;
        if (DestinationParam.Text.Length > 0) DestinationParam.SelectionStart = DestinationParam.Text.Length;

        DestinationList_SelectedIndexChanged(null, null);
        InputList_SelectedIndexChanged(null, null);

        ScriptEditor.Focus();
        ScriptEditor.Current = selectedScript;
    }

    private void ViewGeneratedCode_Click(object sender, EventArgs e)
    {
        DisplayExpandedQuickScriptSourceInNotepad();
    }

    private void FindMenuItem_Click(object sender, EventArgs e)
    {
        ScriptEditor.FindAndReplaceForm ??= new FindAndReplaceForm(ScriptEditor, Host);
        ScriptEditor.FindAndReplaceForm.ShowFor(false);
    }

    private void ReplaceMenuItem_Click(object sender, EventArgs e)
    {
        ScriptEditor.FindAndReplaceForm ??= new FindAndReplaceForm(ScriptEditor, Host);
        ScriptEditor.FindAndReplaceForm.ShowFor(true);
    }

    private void BuildExe_Click(object sender, EventArgs e)
    {
        if (ScriptEditor.Current == null) return;

        Host.WaitFor(() =>
        {
            UpdateScriptFromForm();

            var settings = ScriptEditor.Current.BuildSettings(false, Host);
            var result = settings.ActualizeAndCompile();
            var finalDetails = result.FinalDetails(out var keyLines);
            if (!finalDetails.Contains("SUCCESS!"))
                QuickScriptWorker.ViewText(Host, finalDetails, false);
            if (!result.CompileSuccessful) return;

            if (LastSuccessfulCloneFolder.IsNotEmpty())
            {
                var pre = $"~~CloneLocation: {LastSuccessfulCloneFolder}";
                ScriptEditor.Current.Script = pre + ScriptEditor.Current.Script;
            }

            ShowPostBuildMenu(result, false, PostBuildAction.OpenTempProjectVisualStudio);
        });
    }

    private bool ShowPostBuildMenu(ActualizationResult result, bool singlePass, PostBuildAction defaultAction)
    {
        try
        {
            if (result == null
                || result.DestinationExecutableFilePath.IsEmpty())
                return true;

            var answer = defaultAction;
            LastSuccessfulCloneFolder = "";

            var passCount = 0;
            while (answer != PostBuildAction.DoNothing
                   || singlePass && ++passCount > 1)
            {
                var dialog = new ChooseEnumFromListBoxDialog<PostBuildAction>(PostBuildAction.DoNothing);
                answer = dialog.Ask(answer,
                    (result.Settings.Simulate
                        ? ""
                        : File.Exists(result.DestinationExecutableFilePath)
                            ? "Executable generated successfully"
                            : "No exe found") +
                    $"\n\n\tFolder: {result.Settings.ProjectFolder}\n\tExe:    {result.DestinationExecutableFilePath}",
                    result.Settings.Simulate
                        ? "WHAT NEXT?"
                        : "SUCCESS. WHAT NEXT?");

                switch (answer)
                {
                    case PostBuildAction.DoNothing:
                        break;

                    case PostBuildAction.RunNow:
                        QuickScriptWorker.RunInCommandLine(result.DestinationExecutableFilePath,
                            result.Settings.BinPath, Host);
                        break;

                    case PostBuildAction.CloneProjectAndOpen:
                        //var lastProcessorsFolder = Dirs.FromRegistry(Dirs.ProcessorsFolderName);
                        var lastProcessorsFolder = Dirs.Paths[Dirs.ProcessorsFolderName].Value;
                        var newCloneFolder = Path.Combine(lastProcessorsFolder, result.Settings.ProjectName,
                            $@"{DateTime.Now:yyyyMMdd_hhddss}\");

                        if (Host.InputBox("FOLDER TO CLONE INTO", "Path to the target folder", ref newCloneFolder) ==
                            MessageBoxResult.OK)
                        {
                            Dirs.ToRegistry(Dirs.ProcessorsFolderName, newCloneFolder);

                            if (Directory.Exists(newCloneFolder))
                            {
                                var overwriteAnswer = "";
                                if (Host.InputBox("OVERWRITE CLONE?",
                                        $"That folder already exists. Click OK to completely overwrite folder:\n  {newCloneFolder}",
                                        ref overwriteAnswer) != MessageBoxResult.OK)
                                    break;
                            }

                            LastSuccessfulCloneFolder = newCloneFolder;
                            FileSystem.CleanFolder(newCloneFolder);
                            Directory.CreateDirectory(newCloneFolder);
                            FileSystem.DeepCopy(result.Settings.ProjectFolder, newCloneFolder);
                            answer = PostBuildAction.DoNothing;

                            var cloneDevEnv = FileSystem.LatestVisualStudioDevEnvFilePath();
                            if (cloneDevEnv.IsNotEmpty() && File.Exists(cloneDevEnv))
                            {
                                var cloneProjectFilePath = Path.Combine(newCloneFolder,
                                    result.Settings.ProjectName + ".csproj");
                                FileSystem.FireAndForget(cloneDevEnv, "\"" + cloneProjectFilePath + "\"",
                                    newCloneFolder);
                            }
                        }

                        break;

                    case PostBuildAction.OpenTempProjectVisualStudio:
                        var devEnv = FileSystem.LatestVisualStudioDevEnvFilePath();
                        if (devEnv.IsNotEmpty() && File.Exists(devEnv))
                        {
                            FileSystem.FireAndForget(devEnv, result.Settings.ProjectFilePath,
                                result.Settings.ProjectFolder);
                            answer = PostBuildAction.DoNothing;
                        }

                        break;

                    case PostBuildAction.CopyProjectFolderPath:
                        Clipboard.SetText(result.Settings.ProjectFolder);
                        break;
                    case PostBuildAction.CopyExePath:
                        Clipboard.SetText(result.DestinationExecutableFilePath);
                        break;

                    case PostBuildAction.OpenBinFolderInCommandLine:
                        QuickScriptWorker.OpenFolderInCommandLine(result.Settings.BinPath, Host);
                        break;
                    case PostBuildAction.OpenBinFolderInExplorer:
                        QuickScriptWorker.ViewFolderInExplorer(result.Settings.BinPath, Host);
                        break;
                    case PostBuildAction.OpenProjectFolderInCommandLine:
                        QuickScriptWorker.OpenFolderInCommandLine(result.Settings.ProjectFolder, Host);
                        break;
                    case PostBuildAction.OpenProjectFolderOnExplorer:
                        QuickScriptWorker.ViewFolderInExplorer(result.Settings.ProjectFolder, Host);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        catch (Exception exception)
        {
            Host.MessageBox.Show(exception.ToString());
        }

        return false;
    }

    private void postToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (ScriptEditor.Current == null) return;

        var settings = ScriptEditor.Current.BuildSettings(true, Host);
        settings.UpdateBinPath();
        var filename =
            settings.TemplateNameAsLegalFilenameWithoutExtension.AsFilename(settings.ForExecutable ? ".exe" : ".dll");

        var result = new ActualizationResult(settings)
        {
            DestinationExecutableFilePath = Path.Combine(settings.BinPath, filename),
            OutputText = ""
        };
        ShowPostBuildMenu(result, false, PostBuildAction.OpenBinFolderInCommandLine);
    }

    private void InputParam_Leave(object sender, EventArgs e)
    {

    }

    private void QuickScriptList_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateScriptFromForm();
        UpdateFormWithScript(SelectedScript);
    }

    private void ActionPanel_Click(object sender, EventArgs e)
    {
        var style = LeftPanel.ColumnStyles[1];
        style.Width = style.Width < 11f ? 42.28f : 10;
    }
}