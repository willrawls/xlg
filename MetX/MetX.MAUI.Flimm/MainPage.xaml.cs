using MetX.Fimm;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings.Generics;
using MetX.Windows.Controls;
using MetX.Windows.Library;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using NHotPhrase.WindowsForms;
using static System.Net.Mime.MediaTypeNames;

namespace MetX.MAUI.Flimm;


public partial class MainPage : ContentPage
{
    public XlgQuickScriptFile Scripts { get; set; }

    public bool Updating;

    public HotPhraseManagerForWinForms PhraseManager { get; set; } = new();

    public int LastChoice { get; set; }

    public XlgQuickScript SelectedScript =>
        Scripts.SelectedItems.Count != 0
            ? QuickScriptList.SelectedItem as XlgQuickScript
            : null;

    public int SelectedScriptIndex =>
        QuickScriptList.SelectedItems.Count != 0
            ? QuickScriptList.SelectedIndex
            : -1;

    public string LastSuccessfulCloneFolder { get; set; }

    public Guid CurrentId { get; set; } = Guid.Empty;

    public AssocSheet States { get; set; }

    public IGenerationHost Host { get; set; }
    public static IGenerationHost HostInstance { get; set; }

	public MainPage(string filePath)
	{
		InitializeComponent();
        Scripts = new XlgQuickScriptFile(filePath);

        Updating = true;
        InitializeComponent();

        //InputParam.GotFocus += InputParam_GotFocus;
        //DestinationParam.GotFocus += DestinationParam_GotFocus;

        Func<string> clipboardText = () =>
        {
            if (Clipboard.Default.HasText)
            {
                return Clipboard.Default.GetTextAsync().Result;
            }

            return "";
        };
        Host = new MauiFormGenerationHost<MainPage>(this, clipboardText);

        LoadQuickScriptsFile(filePath, false);
        InitializeHotPhrases();
        Updating = false;

        UpdateForm(Host.Context.Scripts.Default);
    }

    public void LoadQuickScriptsFile(string filePath, bool updateForm)
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

    public int RefreshLists()
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

    void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        XlgQuickScript item = args.SelectedItem as XlgQuickScript;
    }
	/*
	public void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
	*/

    public void OnRunButtonClicked(object sender, EventArgs e)
    {
        

    }

    public void OnPostBuildActionButtonClicked(object sender, EventArgs e)
    {
        
    }

    public void OnBuildCmdLineExeButtonClicked(object sender, EventArgs e)
    {
        

    }

    public void OnFindHighlightButtonClicked(object sender, EventArgs e)
    {
        

    }

    public void OnReplaceButtonClicked(object sender, EventArgs e)
    {
        

    }

    public void OnSaveButtonClicked(object sender, EventArgs e)
    {
        

    }

    public void OnSaveAsButtonClicked(object sender, EventArgs e)
    {
        


    }

    public void OnOpenButtonClicked(object sender, EventArgs e)
    {
        

    }

    public void OnNewFileButtonClicked(object sender, EventArgs e)
    {
        

    }

    public void OnAddScriptButtonClicked(object sender, EventArgs e)
    {
        

    }

    public void OnRestageTemplatesButtonClicked(object sender, EventArgs e)
    {
        

    }

    public void InitializeHotPhrases()
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
            var choice = chooseQuickScript.Ask(Y, X, choices, "Run which quick script?", "CHOOSE QUICK SCRIPT TO RUN",
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

    public void OnRunCurrentQuickScript(object sender, PhraseEventArguments e)
    {
        if (Updating) return;

        Host.WaitFor(() => { RunQuickScript_Click(null, null); });
        e.Handled = true;
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

    public void UpdateScript()
    {
        if (SelectedScript == null
            || SelectedScript.Id != CurrentId) return;

        IfNotUpdating(() =>
        {
            SelectedScript.Name = QuickScriptName.Text.AsStringFromObject(DateTime.Now.ToString("s"));
            SelectedScript.Script = ScriptEditor.Text;
            Enum.TryParse(DestinationList.Text.Replace(" ", string.Empty), out SelectedScript.Destination);
            SelectedScript.Input = InputList.Text;
            SelectedScript.SliceAt = SliceAt.Text;
            SelectedScript.DiceAt = DiceAt.Text;
            SelectedScript.InputFilePath = InputParam.Text;
            SelectedScript.DestinationFilePath = DestinationParam.Text;
            SelectedScript.TemplateName = TemplateFolderPath.Text.AsStringFromString("Exe");
            SelectedScript.TargetFramework = TargetFramework.Text.AsStringFromString("net7.0-windows");
            Host.Context.Scripts.Default = SelectedScript;
        });
    }


    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        

    }

    private void OnEditorCompleted(object sender, EventArgs e)
    {
        

    }

    private void OnScriptSelected(object sender, SelectedItemChangedEventArgs e)
    {
        XlgQuickScript script = e.SelectedItem as XlgQuickScript;
        if (script == null) return;

        UpdateScript();

    }

    public void RunQuickScript(ContentPage caller, XlgQuickScript scriptToRun, IShowText targetOutput)
    {
        GuiContext.RunQuickScript(caller, scriptToRun, targetOutput, Host);
    }
}

