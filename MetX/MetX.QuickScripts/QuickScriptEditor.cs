using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ICSharpCode.TextEditor.UserControls;
using MetX.Fimm;
using MetX.Fimm.Scripts;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.IO;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Generics;
using MetX.Windows.Controls;
using MetX.Windows.Library;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using NHotPhrase.WindowsForms;
using XLG.QuickScripts.Walker;

namespace XLG.QuickScripts;

public partial class QuickScriptEditor : ScriptRunningWindow
{
    public bool Updating;

    public HotPhraseManagerForWinForms PhraseManager { get; set; } = new();

    public int LastChoice { get; set; }


    public XlgQuickScript SelectedScript =>
        QuickScriptList.SelectedItems.Count != 0
            ? QuickScriptList.SelectedItem as XlgQuickScript
            : null;

    public int SelectedScriptIndex =>
        QuickScriptList.SelectedItems.Count != 0
            ? QuickScriptList.SelectedIndex
            : -1;

    public string LastSuccessfulCloneFolder { get; set; }

    public Guid CurrentId { get; set; } = Guid.Empty;

    public AssocSheet States { get; set; }

    public QuickScriptEditor(string filePath)
    {
        Updating = true;
        InitializeComponent();

        ChangeTheme(ColorScheme.DarkThemeOne, Controls);
        ChangeTheme(ColorScheme.DarkThemeOne, ScriptEditor.Controls);

        InputParam.GotFocus += InputParam_GotFocus;
        DestinationParam.GotFocus += DestinationParam_GotFocus;

        Host = new WinFormGenerationHost<QuickScriptEditor>(this, Clipboard.GetText);

        LoadQuickScriptsFile(filePath, false);
        InitializeHotPhrases();
        Updating = false;

        UpdateForm(Host.Context.Scripts.Default);
    }

    private void InitializeHotPhrases()
    {
        try
        {
            PhraseManager.Keyboard.AddOrReplace("Pick and Run QuickScript",
                new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.CapsLock, PKey.LShiftKey, PKey.LShiftKey },
                OnPickAndRunQuickScript);

            PhraseManager.Keyboard.AddOrReplace("Run Current QuickScript",
                new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.CapsLock, PKey.LControlKey, PKey.LControlKey },
                OnRunCurrentQuickScript);
        }
        catch
        {
            // Ignored
        }
    }

    private void OnPickAndRunQuickScript(object sender, PhraseEventArguments e)
    {
        if (Updating) return;
        if (ScriptEditor.Current == null) return;
        try
        {
            UpdateScript();

            var chooseQuickScript = new ChooseFromListDialog();
            var choices = Host.Context.Scripts.ScriptNames();
            if (LastChoice > choices.Length) LastChoice = 0;
            var choice = chooseQuickScript.Ask(Top, Left, choices, "Run which quick script?", "CHOOSE QUICK SCRIPT TO RUN",
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
        if (Updating) return;

        Host.WaitFor(() => { RunQuickScript_Click(null, null); });
        e.Handled = true;
    }

    public void DisplayExpandedQuickScriptSourceInNotepad()
    {
        try
        {
            if (ScriptEditor?.Current == null) return;
            UpdateScript();

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

    private void UpdateForm(XlgQuickScript selectedScript)
    {
        if (Updating) return;
        if (selectedScript == null) return;

        try
        {
            Updating = true;
            QuickScriptName.Text = selectedScript.Name;
            TargetFramework.Text = selectedScript.TargetFramework ?? "net7.0-windows";

            var script = selectedScript.Script;
            ScriptEditor.Refresh();
            ScriptEditor.Text = script;
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
            CurrentId = selectedScript.Id;
        }
        finally
        {
            Updating = false;
        }
    }

    public void UpdateScript()
    {
        if (ScriptEditor.Current == null
            || ScriptEditor.Current.Id != CurrentId) return;

        IfNotUpdating(() =>
        {
            ScriptEditor.Current.Name = QuickScriptName.Text.AsStringFromObject(DateTime.Now.ToString("s"));
            ScriptEditor.Current.Script = ScriptEditor.Text;
            Enum.TryParse(DestinationList.Text.Replace(" ", string.Empty), out ScriptEditor.Current.Destination);
            ScriptEditor.Current.Input = InputList.Text;
            ScriptEditor.Current.SliceAt = SliceAt.Text;
            ScriptEditor.Current.DiceAt = DiceAt.Text;
            ScriptEditor.Current.InputFilePath = InputParam.Text;
            ScriptEditor.Current.DestinationFilePath = DestinationParam.Text;
            ScriptEditor.Current.TemplateName = TemplateFolderPath.Text.AsStringFromString("Exe");
            ScriptEditor.Current.TargetFramework = TargetFramework.Text.AsStringFromString("net7.0-windows");
            Host.Context.Scripts.Default = ScriptEditor.Current;
        });
    }

    private void BrowseDestinationFilePath_Click(object sender, EventArgs e)
    {
        if (Updating) return;

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
        if (Updating) return;

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
            InputParam.Text = OpenInputFilePathDialog.FileName;
    }

    public void IfNotUpdating(Action action)
    {
        if (Updating
            || Host?.Context?.Scripts == null
            || ScriptEditor?.Current == null) return;

        try
        {
            Updating = true;
            action();
        }
        catch (Exception exception)
        {
            Host.MessageBox.Show(exception.ToString());
        }
        finally
        {
            Updating = false;
        }
    }

    private void DeleteScript_Click(object sender, EventArgs e)
    {
        IfNotUpdating(() =>
        {
            var answer = Host.MessageBox.Show(
                "This will permanently delete the current script.\n\tAre you sure this is what you want to do?",
                "DELETE SCRIPT", MessageBoxChoices.YesNo);
            if (answer != MessageBoxResult.Yes) return;

            var script = ScriptEditor.Current;

            Host.Context.Scripts.Remove(script);
            RefreshLists();
            QuickScriptList.SelectedItems.Clear();

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
            UpdateForm(Host.Context.Scripts.Default);
        });
    }

    private void DestinationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Updating) return;
        UpdateScript();
    }

    private void DestinationParam_Enter(object sender, EventArgs e)
    {
        if (Updating) return;
        DestinationParam.SelectAll();
    }

    private void DestinationParam_GotFocus(object sender, EventArgs e)
    {
        if (Updating) return;
        if (MouseButtons != MouseButtons.None) return;
        DestinationParam.SelectAll();
    }

    private void EditDestinationFilePath_Click(object sender, EventArgs e)
    {
        if (Updating) return;
        QuickScriptWorker.ViewFile(Host, DestinationParam.Text);
    }

    private void EditInputFilePath_Click(object sender, EventArgs e)
    {
        if (Updating) return;
        QuickScriptWorker.ViewFile(Host, InputParam.Text);
    }

    private void InputList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Updating) return;
        UpdateScript();
    }

    private void InputParam_Enter(object sender, EventArgs e)
    {
        if (Updating) return;
        InputParam.SelectAll();
    }

    private void InputParam_GotFocus(object sender, EventArgs e)
    {
        if (Updating) return;
        if (MouseButtons != MouseButtons.None) return;

        InputParam.SelectAll();
    }

    private void LoadQuickScriptsFile(string filePath, bool updateForm)
    {
        Host.Context ??= new GuiContext(Host);
        var xlgQuickScriptFile = XlgQuickScriptFile.Load(filePath);
        Host.Context.Scripts = xlgQuickScriptFile ?? new XlgQuickScriptFile(filePath);

        if (Host.Context.Scripts.Count == 0)
        {
            DefaultTemplates.BuildDefaultUserScriptsFile(Host.Context.Scripts);
            Host.Context.Scripts.Save(Shared.Dirs.ScriptArchivePath);
        }

        Shared.Dirs.LastScriptFilePath = filePath;

        var selectedIndex = RefreshLists();
        if (SelectedScript == null)
            QuickScriptList.SelectedIndices.Add(selectedIndex);
        if (updateForm)
            UpdateForm(Host.Context.Scripts.Default);
        Text = "Quick Script - " + filePath;
    }

    private void NewQuickScript_Click(object sender, EventArgs e)
    {
        IfNotUpdating(() =>
        {
            UpdateScript();

            var name = string.Empty;
            var answer = Host.InputBox("New Script Name", "Please enter the name for the new script.", ref name);
            if (answer != MessageBoxResult.OK || (name ?? string.Empty).Trim() == string.Empty) return;

            var newScript = new XlgQuickScript(name, DefaultTemplates.SingleFile)
            {
                Input = "Clipboard",
            };

            if (Host.Context.Scripts.Any(s =>
                    string.Equals(s.Name, newScript.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                Host.MessageBox.Show($"A script with the name '{newScript.Name}' already exists.");
                return;
            }

            QuickScriptList.SelectedItems.Clear();
            Host.Context.Scripts.Add(newScript);
            var newIndex = QuickScriptList.Items.Add(newScript);
            QuickScriptList.SelectedIndex = newIndex;
            Updating = false;
            UpdateForm(newScript);
        });
    }

    private void NewScriptFile_Click(object sender, EventArgs e)
    {
        OpenInputFilePathDialog.FileName = string.Empty;
        OpenInputFilePathDialog.InitialDirectory = Host.Context.Scripts.FilePath.TokensBeforeLast(@"\");
        OpenInputFilePathDialog.AddExtension = true;
        OpenInputFilePathDialog.CheckFileExists = false;
        OpenInputFilePathDialog.CheckPathExists = true;
        OpenInputFilePathDialog.DefaultExt = ".xlgq";
        OpenInputFilePathDialog.Filter = "Qk Scrptr files (*.xlgq)|*.xlgq|All files (*.*)|*.*";
        OpenInputFilePathDialog.Multiselect = false;
        OpenInputFilePathDialog.ShowDialog(this);
        if (!string.IsNullOrEmpty(OpenInputFilePathDialog.FileName))
        {
            Host.Context.Scripts.Save(Shared.Dirs.ScriptArchivePath);
            LoadQuickScriptsFile(OpenInputFilePathDialog.FileName, true);
        }
    }

    private void OpenScriptFile_Click(object sender, EventArgs e)
    {
        OpenInputFilePathDialog.FileName = string.Empty;
        OpenInputFilePathDialog.InitialDirectory = Host.Context.Scripts.FilePath.TokensBeforeLast(@"\");
        OpenInputFilePathDialog.AddExtension = true;
        OpenInputFilePathDialog.CheckFileExists = true;
        OpenInputFilePathDialog.CheckPathExists = true;
        OpenInputFilePathDialog.DefaultExt = ".xlgq";
        OpenInputFilePathDialog.Filter = "Qk Scrptr files (*.xlgq)|*.xlgq|All files (*.*)|*.*";
        OpenInputFilePathDialog.Multiselect = false;
        OpenInputFilePathDialog.ShowDialog(this);
        if (!string.IsNullOrEmpty(OpenInputFilePathDialog.FileName))
        {
            Host.Context.Scripts.Save(Shared.Dirs.ScriptArchivePath);
            LoadQuickScriptsFile(OpenInputFilePathDialog.FileName, true);
        }
    }

    private void QuickScriptEditor_FormClosing(object sender, FormClosingEventArgs e)
    {
        try
        {
            foreach (var outputWindow in GuiContext.OutputWindows)
            {
                outputWindow.Close();
                outputWindow.Dispose();
            }

            GuiContext.OutputWindows.Clear();
        }
        catch
        {
            // Ignored
        }

        SaveQuickScriptFile_Click(sender, null);
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

    private int RefreshLists()
    {
        var selectedIndex = 0;
        QuickScriptList.Items.Clear();
        for (var index = 0; index < Host.Context.Scripts.Count; index++)
        {
            var script = Host.Context.Scripts[index];
            QuickScriptList.Items.Add(script);
            if (Host.Context.Scripts.Default == null || script != Host.Context.Scripts.Default) continue;

            selectedIndex = index;
        }

        return selectedIndex;
    }

    private void RefreshNameForSelectedIndexInScriptList()
    {
        var selectedScript = SelectedScript;
        var selectedScriptIndex = SelectedScriptIndex;
        if (selectedScriptIndex < 0 || selectedScriptIndex >= QuickScriptList.Items.Count) return;

        QuickScriptList.Items.RemoveAt(selectedScriptIndex);
        QuickScriptList.Items.Insert(selectedScriptIndex, selectedScript);
        Updating = true;
        QuickScriptList.SelectedIndex = selectedScriptIndex;
        Updating = false;
        QuickScriptList.Refresh();
    }

    private void RunQuickScript_Click(object sender, EventArgs e)
    {
        if (ScriptEditor.Current == null) return;
        Host.WaitFor(() =>
        {
            UpdateScript();
            RunQuickScript(this, ScriptEditor.Current, null);
        });
    }

    private void SaveAs_Click(object sender, EventArgs e)
    {
        IfNotUpdating(() =>
        {
            UpdateScript();

            SaveDestinationFilePathDialog.FileName = Host.Context.Scripts.FilePath;
            SaveDestinationFilePathDialog.InitialDirectory =
                Host.Context.Scripts.FilePath.TokensBeforeLast(@"\");
            SaveDestinationFilePathDialog.AddExtension = true;
            SaveDestinationFilePathDialog.CheckPathExists = true;
            SaveDestinationFilePathDialog.DefaultExt = ".xlgq";
            SaveDestinationFilePathDialog.Filter = "Qk Scrptr files (*.xlgq)|*.xlgq|All files (*.*)|*.*";
            SaveDestinationFilePathDialog.ShowDialog(this);
            if (string.IsNullOrEmpty(SaveDestinationFilePathDialog.FileName)) return;

            Host.Context.Scripts.FilePath = SaveDestinationFilePathDialog.FileName;
            Host.Context.Scripts.Save(Shared.Dirs.ScriptArchivePath);
            Text = "Qk Scrptr - " + Host.Context.Scripts.FilePath;
            Shared.Dirs.LastScriptFilePath = Host.Context.Scripts.FilePath;
        });
    }

    private void SaveQuickScriptFile_Click(object sender, EventArgs e)
    {
        UpdateScript();
        IfNotUpdating(() =>
        {
            if (string.IsNullOrEmpty(Host.Context.Scripts.FilePath))
                SaveAs_Click(null, null);
            else
                Host.Context.Scripts.Save(Shared.Dirs.ScriptArchivePath);
        });
    }

    private void ViewGeneratedCode_Click(object sender, EventArgs e)
    {
        DisplayExpandedQuickScriptSourceInNotepad();
    }

    private void FindMenuItem_Click(object sender, EventArgs e)
    {
        ScriptEditor.FindAndReplaceForm ??= new FindAndReplaceForm();
        ScriptEditor.FindAndReplaceForm.ShowFor(ScriptEditor, false);
    }

    private void ReplaceMenuItem_Click(object sender, EventArgs e)
    {
        ScriptEditor.FindAndReplaceForm ??= new FindAndReplaceForm();
        ScriptEditor.FindAndReplaceForm.ShowFor(ScriptEditor, true);
    }

    private void BuildExe_Click(object sender, EventArgs e)
    {
        if (ScriptEditor.Current == null) return;

        Host.WaitFor(() =>
        {
            UpdateScript();

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
                answer = dialog.Ask(Top, Left, answer,
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
                        var processorsFolder = Shared.Dirs.Paths[Constants.ProcessorsFolderName].Value;
                        var newCloneFolder = Path.Combine(processorsFolder, result.Settings.ProjectName, $@"{DateTime.Now:yyyyMMdd_hhddss}\");

                        var scriptsFolder = Shared.Dirs.Paths[Constants.ScriptsFolderName].Value;
                        var templateFolder = Path.Combine(scriptsFolder, "Templates", result.Settings.Script.TemplateName);

                        if (Host.InputBox("FOLDER TO CLONE INTO", "Path to the target folder", ref newCloneFolder) == MessageBoxResult.OK)
                        {
                            Shared.Dirs.Settings.ToSettingsFile(Constants.ProcessorsFolderName, newCloneFolder);

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
                            throw new Exception("Start Here. Need to resolve all files first (ActualizeCode?)")
                            FileSystem.DeepCopy(templateFolder, newCloneFolder);
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

    private void PostBuild_Click(object sender, EventArgs e)
    {
        if (ScriptEditor.Current == null) return;

        var settings = ScriptEditor.Current.BuildSettings(false, Host);
        settings.UpdateBinPath();
        var filename =
            settings.TemplateNameAsLegalFilenameWithoutExtension.AsFilename(
                settings.ForExecutable ? ".exe" : ".dll");

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
        if (Updating) return;

        UpdateScript();
        UpdateForm(SelectedScript);
        RefreshNameForSelectedIndexInScriptList();
    }

    private void ActionPanel_Click(object sender, EventArgs e)
    {
        if (Updating) return;
        try
        {
            SuspendLayout();
            var actionBarStyle = LeftPanel.ColumnStyles[1];
            var scriptListStyle = LeftPanel.ColumnStyles[2];
            if (actionBarStyle.Width < 20f)
            {
                TopPanel.Height = 240;
                LeftPanel.Width = 420;
                actionBarStyle.Width = 39f;
                scriptListStyle.Width = 58.72f;
            }
            else
            {
                TopPanel.Height = 155;
                LeftPanel.Width = 220;
                actionBarStyle.Width = 15f;
                scriptListStyle.Width = 24f;
            }
        }
        finally
        {
            ResumeLayout();
        }
    }

    private void BrowseTemplateFolderPathButton_Click(object sender, EventArgs e)
    {
        if (Updating) return;
        var path = BrowseForFolder("BrowseTemplateFolderPathButton",
            "Please select an existing Qk Scrptr template folder");
        if (string.IsNullOrEmpty(path))
            return;

        path = path.LastPathToken();
        if (string.IsNullOrEmpty(path))
            return;

        TemplateFolderPath.Text = path;
    }

    private string BrowseForFolder(string folderKey, string title)
    {
        var last = Shared.Dirs.Settings.FromSettingsFile(folderKey);
        if (last.IsNotEmpty()) FolderBrowserDialog.SelectedPath = last;

        FolderBrowserDialog.Description = title;
        if (FolderBrowserDialog.ShowDialog(this) != DialogResult.OK) return null;

        Shared.Dirs.Settings.ToSettingsFile(folderKey, FolderBrowserDialog.SelectedPath);
        return FolderBrowserDialog.SelectedPath;
    }

    private void CloneScriptButton_Click(object sender, EventArgs e)
    {
        if (Updating) return;
        if (Host.Context.Scripts == null) return;

        var name = string.Empty;
        var answer = Host.InputBox("CLONE CURRENT SCRIPT", "Please enter the name for the newly cloned script.",
            ref name);
        if (answer != MessageBoxResult.OK || (name ?? string.Empty).Trim() == string.Empty) return;

        var script = string.Empty;
        if (ScriptEditor.Current != null) UpdateScript();

        var newScript = ScriptEditor.Current!.Clone(name);
        Host.Context.Scripts.Add(newScript);
        UpdateForm(newScript);
        RefreshLists();
    }

    private void CloneTemplateButton_Click(object sender, EventArgs e)
    {
        if (Updating) return;
        Host.WaitFor(() =>
        {
            UpdateScript();

            // Choose source template folder
            var sourcePath = BrowseForFolder("CloneTemplateButton_Source",
                "Please select an existing Qk Scrptr template folder");
            if (string.IsNullOrEmpty(sourcePath))
                return;

            // Choose destination folder (always the base templates folder)
            var destinationPath = Shared.Dirs.CurrentTemplateFolderPath;
            /*
            var destinationPath = BrowseForFolder("CloneTemplateButton_Destination",
                "Please select folder to contain the cloned template folder");
            if (string.IsNullOrEmpty(destinationPath))
                return;
            */


            // Choose new name of template
            var dialog = new AskForStringDialog();
            var answer = dialog.Ask(ParentForm.Top + 50, ParentForm.Left + 50, promptText: "What would you like to call the new template folder?", title: "NEW TEMPLATE NAME");
            if (answer.IsEmpty()) return;

            // Create subfolder (new name) in destination folder
            var newFolderPath = Path.Combine(destinationPath, answer);
            if (Directory.Exists(newFolderPath))
            {
                Host.MessageBox.Show("The destination folder for the new template already exists");
                return;
            }

            var newFolderInfo = Directory.CreateDirectory(newFolderPath);
            if (!newFolderInfo.Exists) return;

            // Deep copy files from source to destination\new name
            if (!FileSystem.DeepCopy(sourcePath, newFolderPath)) return;

            // Update the script's template folder path
            ScriptEditor.Current.TemplateName = answer;
            UpdateForm(ScriptEditor.Current);
            RefreshNameForSelectedIndexInScriptList();

            // Report success and offer to open the destination\new name folder in explorer
            QuickScriptWorker.ViewFolderInExplorer(newFolderPath, Host);
        });
    }

    private void GitHubButton_Click(object sender, EventArgs e)
    {
        if (Updating) return;
        OpenUrl("https://www.github.com/willrawls/xlg");
    }

    private void FeedbackButton_Click(object sender, EventArgs e)
    {
        if (Updating) return;
        OpenUrl("https://github.com/willrawls/xlg/issues/new");
    }

    private void ScriptEditorHelpButton_Click(object sender, EventArgs e)
    {
        if (Updating) return;
        OpenUrl("https://github.com/willrawls/xlg/wiki");
    }

    public static void OpenUrl(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch
        {
            // Ignored
        }
    }

    private void QuickScriptEditor_KeyUp(object sender, KeyEventArgs e)
    {
        if (Updating) return;
        Host.WaitFor(() =>
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    RunQuickScript_Click(sender, null);
                    break;
                case Keys.F6:
                    BuildExe_Click(sender, null);
                    break;
                case Keys.F10:
                    ViewGeneratedCode_Click(sender, null);
                    break;
                case Keys.F12:
                    PostBuild_Click(sender, null);
                    break;

                case Keys.F:
                    if (e.Control)
                        FindMenuItem_Click(sender, null);
                    break;
                case Keys.R:
                    if (e.Control)
                        ReplaceMenuItem_Click(sender, null);
                    break;
                case Keys.S:
                    if (e.Control)
                        SaveQuickScriptFile_Click(sender, null);
                    break;
                case Keys.A:
                    if (e.Control)
                        SaveAs_Click(sender, null);
                    break;
                case Keys.O:
                    if (e.Control)
                        OpenScriptFile_Click(sender, null);
                    break;
                case Keys.N:
                    if (e.Control)
                        NewQuickScript_Click(sender, null);
                    break;
            }
        });
    }

    private void SliceAt_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Updating) return;
        UpdateScript();
    }

    private void DiceAt_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Updating) return;
        UpdateScript();
    }

    private void Button1_Click(object sender, EventArgs e)
    {
        var ideas4Form = new DatabaseWalkerForm();
        ideas4Form.Show();
    }

    private void RestageTemplatesButton_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            if (Host?.MessageBox.Show("RESET ALL XLG TEMPLATES?",
                    "This will reset all the templates in your Documents\\XLG\\ folder to the defaults from the template manager.\n Continue with overwrite?",
                    MessageBoxChoices.YesNo, MessageBoxStatus.Warning, MessageBoxDefault.Button2
                    )
                != MessageBoxResult.Yes) return;

            Shared.Dirs.RestageStaticTemplates();
            Host?.Context.LoadTemplates();
            Host?.MessageBox.Show($"Templates restaged and reloaded into memory");

        }
        catch (Exception exception)
        {
            Host?.MessageBox.Show($"Restage failed\n\n{exception}");
        }
    }
}