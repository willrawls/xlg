﻿using MetX.Fimm;
using MetX.Fimm.Scripts;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Generics;
using MetX.Windows.Controls;
using MetX.Windows.Library;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using NHotPhrase.WindowsForms;

namespace MetX.MAUI.Flimm;

public partial class MainPage : ContentPage, IRunQuickScript
{
    public XlgQuickScriptFile QuickScriptFile;
    public bool Updating;

    public HotPhraseManagerForWinForms PhraseManager { get; set; } = new();

    public int LastChoice { get; set; }

    public XlgQuickScript SelectedScript =>
        SelectedScriptIndex >= 0
            ? Host.Context.Scripts[SelectedScriptIndex]
            : null;

    public int SelectedScriptIndex { get; set; }

    public string LastSuccessfulCloneFolder { get; set; }

    public Guid CurrentId { get; set; } = Guid.Empty;

    public AssocSheet States { get; set; }

    public IGenerationHost Host { get; set; }
    public static IGenerationHost HostInstance { get; set; }
    public List<XlgQuickScript> Scripts { get; set; }

    public MainPage()
    {
        try
        {
            string ClipboardText()
            {
                return Clipboard.Default.HasText ? Clipboard.Default.GetTextAsync().Result : "";
            }

            Host = new MauiFormGenerationHost<MainPage>(this, ClipboardText);
            var filePath = Shared.Dirs.LastScriptFilePath;
            LoadQuickScriptsFile(filePath);

            try
            {
                Updating = true;

                //InputParam.GotFocus += InputParam_GotFocus;
                //DestinationParam.GotFocus += DestinationParam_GotFocus;

                InitializeHotPhrases();
                Updating = false;
            }
            catch (Exception e)
            {
                Content = new Label {Text = e.ToString()};
            }

            InitializeComponent();

            var selectedIndex = RefreshListOfScripts();
            SelectedScriptIndex = selectedIndex;

            QuickScriptList.SetBinding(ItemsView.ItemsSourceProperty, "Scripts");
            QuickScriptList.RowHeight = 50;

            QuickScriptList.ItemTemplate = new DataTemplate(() =>
            {
                var cell = new TextCell
                {
                    Height = 30
                };
                cell.SetBinding(TextCell.TextProperty, "Name");
                return cell;
            });

            Updating = true;
            Scripts = QuickScriptFile; // ScriptsToModel();
            Task.Run(() =>
            {
                QuickScriptList.BindingContext = SelectedScript;
                CurrentId = SelectedScript?.Id ?? Guid.Empty;

                QuickScriptList.ItemsSource = Scripts;
                QuickScriptList.SelectedItem = SelectedScript;
                Updating = false;

                UpdateForm(Host.Context.Scripts.Default);
            });
        }
        catch (Exception e)
        {
            if (Content != null)
            {
                var oldText = (Content as Label)?.Text;
                Content = new Label
                {
                    Text = $"{e}\n\n-----\n\n" + oldText
                };
            }
            else
            {
                Content = new Label {Text = e.ToString()};
            }
        }
    }

    public void IfNotUpdating(Action action)
    {
        if (Updating
            || Host?.Context?.Scripts == null
            || SelectedScript == null) return;

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

    public void InitializeHotPhrases()
    {
        try
        {
            PhraseManager.Keyboard.AddOrReplace("Pick and Run QuickScript",
                new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.CapsLock, PKey.LShiftKey, PKey.LShiftKey},
                OnPickAndRunQuickScript);

            PhraseManager.Keyboard.AddOrReplace("Run Current QuickScript",
                new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.CapsLock, PKey.LControlKey, PKey.LControlKey},
                OnRunCurrentQuickScript);
        }
        catch
        {
            // Ignored
        }
    }

    public void LoadQuickScriptsFile(string filePath)
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
    }

    public void OnAddScriptButtonClicked(object sender, EventArgs e)
    {
    }

    public void OnBuildCmdLineExeButtonClicked(object sender, EventArgs e)
    {
    }

    private void OnEditorCompleted(object sender, EventArgs e)
    {
    }


    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
    }

    public void OnFindHighlightButtonClicked(object sender, EventArgs e)
    {
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        var item = args.SelectedItem as XlgQuickScript;
    }

    public void OnNewFileButtonClicked(object sender, EventArgs e)
    {
    }

    public void OnOpenButtonClicked(object sender, EventArgs e)
    {
    }

    public void OnPickAndRunQuickScript(object sender, PhraseEventArguments e)
    {
        if (Updating) return;
        if (SelectedScript == null) return;
        try
        {
            UpdateScript();

            var chooseQuickScript = new ChooseFromListDialog();
            var choices = Host.Context.Scripts.ScriptNames();
            if (LastChoice > choices.Length) LastChoice = 0;
            var choice = chooseQuickScript.Ask((int) Y, (int) X, choices, "Run which quick script?",
                "CHOOSE QUICK SCRIPT TO RUN",
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

    public void PostBuild_Click(object sender, EventArgs e)
    {
        if (SelectedScript == null) return;

        var settings = SelectedScript.BuildSettings(false, Host);
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

    public bool ShowPostBuildMenu(ActualizationResult result, bool singlePass, PostBuildAction defaultAction)
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
                answer = dialog.Ask((int) Bounds.Top, (int) Bounds.Left, answer,
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
                            MetX.Standard.Primary.IO.FileSystem.CleanFolder(newCloneFolder);
                            Directory.CreateDirectory(newCloneFolder);
                            // MetX.Standard.Primary.IO.FileSystem.DeepCopy(templateFolder, newCloneFolder);

                            var settings = SelectedScript.BuildSettings(false, this.Host);
                            settings.ProjectFolder = newCloneFolder;
                            var actualizationResult = settings.ActualizeAndCompile();

                            answer = PostBuildAction.DoNothing;

                            var askResult =
                                Host.MessageBox.Show(
                                    $"Project cloned to:\n\t{newCloneFolder}\n\nWould you like to copy this path to the clipboard?",
                                    $"COPY PATH TO CLIPBOARD?",
                                    MessageBoxChoices.YesNo);
                            if (askResult == MessageBoxResult.Yes)
                                Clipboard.SetTextAsync(newCloneFolder);

                            var cloneDevEnv = 
                                MetX.Standard.Primary.IO.FileSystem.LatestVisualStudioDevEnvFilePath();

                            if (cloneDevEnv.IsNotEmpty() && File.Exists(cloneDevEnv))
                            {
                                var cloneProjectFilePath = Path.Combine(newCloneFolder,
                                    result.Settings.ProjectName + ".csproj");
                                MetX.Standard.Primary.IO.FileSystem
                                    .FireAndForget(cloneDevEnv, "\"" + cloneProjectFilePath + "\"",
                                    newCloneFolder);
                            }
                        }

                        break;

                    case PostBuildAction.OpenTempProjectVisualStudio:
                        var devEnv = MetX.Standard.Primary.IO.FileSystem.LatestVisualStudioDevEnvFilePath();
                        if (devEnv.IsNotEmpty() && File.Exists(devEnv))
                        {
                            MetX.Standard.Primary.IO.FileSystem.FireAndForget(devEnv, result.Settings.ProjectFilePath,
                                result.Settings.ProjectFolder);
                            answer = PostBuildAction.DoNothing;
                        }

                        break;

                    case PostBuildAction.CopyProjectFolderPath:
                        Clipboard.SetTextAsync(result.Settings.ProjectFolder);
                        break;
                    case PostBuildAction.CopyExePath:
                        Clipboard.SetTextAsync(result.DestinationExecutableFilePath);
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


    public void OnReplaceButtonClicked(object sender, EventArgs e)
    {
    }

    public void OnRestageTemplatesButtonClicked(object sender, EventArgs e)
    {
    }

    public void OnRunButtonClicked(object sender, EventArgs e)
    {
        if (Updating) return;

        Host.WaitFor(() => { RunQuickScript_Click(null, null); });
    }

    public void OnRunCurrentQuickScript(object sender, PhraseEventArguments e)
    {
        if (Updating) return;

        Host.WaitFor(() => { RunQuickScript_Click(null, null); });
        e.Handled = true;
    }

    public void OnSaveAsButtonClicked(object sender, EventArgs e)
    {
    }

    public void OnSaveButtonClicked(object sender, EventArgs e)
    {
    }

    private void OnScriptSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is not XlgQuickScript script) return;

        UpdateScript();
        if(!Updating) UpdateForm(script);
    }

    public int RefreshListOfScripts()
    {
        var selectedIndex = 0;

        QuickScriptFile ??= Host.Context.Scripts;
        for (var index = 0; index < Host.Context.Scripts.Count; index++)
        {
            var script = Host.Context.Scripts[index];
            if (Host.Context.Scripts.Default == null || script != Host.Context.Scripts.Default) continue;
            selectedIndex = index;
            break;
        }

        return selectedIndex;
    }

    public void RunQuickScript_Click(object sender, EventArgs e)
    {
        if (SelectedScript == null) return;
        Host.WaitFor(() =>
        {
            UpdateScript();
            RunQuickScript(this, SelectedScript, null);
        });
    }

    private List<XlgQuickScriptModel> ScriptsToModel()
    {
        var result = Host.Context.Scripts.Select(script =>
                new XlgQuickScriptModel
                {
                    Name = script.Name
                })
            .ToList();
        return result;
    }


    public void UpdateForm(XlgQuickScript selectedScript)
    {
        if (Updating) return;
        if (selectedScript == null) return;

        try
        {
            Updating = true;
            QuickScriptName.Text = selectedScript.Name;
            TargetFramework.Text = selectedScript.TargetFramework ?? "net7.0-windows";

            var script = selectedScript.Script;
            ScriptEditor.Text = script;

            DestinationList.SelectedItem =
                selectedScript.Destination == QuickScriptDestination.Unknown
                    ? "Text Box"
                    : selectedScript.Destination.ToString().Replace("Box", " Box");

            InputList.SelectedItem = selectedScript.Input;
            /*
            InputList.SelectedIndex = index > -1
                ? index
                : 0;
                */

            /*
            index = SliceAt.FindString(selectedScript.SliceAt);
            SliceAt.SelectedIndex = index > -1
                ? index
                : SliceAt.Items.Add(selectedScript.SliceAt);

            index = DiceAt.FindString(selectedScript.DiceAt);
            DiceAt.SelectedIndex = index > -1
                ? index
                : DiceAt.Items.Add(selectedScript.DiceAt);
                */

            TemplateFolderPath.Text = selectedScript.TemplateName;

            InputParam.Text = selectedScript.InputFilePath;
            //if (InputParam.Text.Length > 0) InputParam.SelectionStart = InputParam.Text.Length;

            DestinationParam.Text = selectedScript.DestinationFilePath;
            //if (DestinationParam.Text.Length > 0) DestinationParam.SelectionStart = DestinationParam.Text.Length;

            //DestinationList_SelectedIndexChanged(null, null);
            //InputList_SelectedIndexChanged(null, null);

            ScriptEditor.Focus();
            ScriptEditor.Text = selectedScript.Script;
        }
        finally
        {
            Updating = false;
        }
    }

    public void UpdateScript()
    {
        if (SelectedScript == null
            || SelectedScript.Id != CurrentId) return;

        IfNotUpdating(() =>
        {
            SelectedScript.Name = QuickScriptName.Text.AsStringFromObject(DateTime.Now.ToString("s"));
            SelectedScript.Script = ScriptEditor.Text;
            Enum.TryParse(DestinationList.SelectedItem.AsStringFromObject().Replace(" ", string.Empty),
                out SelectedScript.Destination);
            SelectedScript.Input = InputList.SelectedItem.AsStringFromObject();
            //SelectedScript.SliceAt = SliceAt.Text;
            //SelectedScript.DiceAt = DiceAt.Text;
            SelectedScript.InputFilePath = InputParam.Text;
            SelectedScript.DestinationFilePath = DestinationParam.Text;
            SelectedScript.TemplateName = TemplateFolderPath.Text.AsStringFromString("Exe");
            SelectedScript.TargetFramework = TargetFramework.Text.AsStringFromString("net7.0-windows");
            Host.Context.Scripts.Default = SelectedScript;
        });
    }

    public void RunQuickScript(IRunQuickScript caller, XlgQuickScript scriptToRun, IShowText targetOutput)
    {
        GuiContext.RunQuickScript(caller, scriptToRun, targetOutput, Host);
    }

    public IToolWindow ToolWindow { get; set; }

    public void Progress(int index = -1)
    {
        
    }
}

public class XlgQuickScriptModel
{
    public string Name { get; set; }
}

