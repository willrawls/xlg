// ReSharper disable UnusedParameter.Local

using MetX.Standard.Generation;
using MetX.Standard.Interfaces;
using MetX.Standard.Library.Strings;
using MetX.Standard.Pipelines;
using MetX.Windows;
using MetX.Windows.Library;

namespace MetX.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;

    using ICSharpCode.TextEditor;
    using ICSharpCode.TextEditor.Document;
    using ICSharpCode.TextEditor.Gui.CompletionWindow;
    using MetX.Standard.Scripts;

    using Microsoft.Win32;

    public partial class QuickScriptToolWindow : ScriptRunningWindow
    {
        public static readonly List<QuickScriptOutput> OutputWindows = new List<QuickScriptOutput>();
        public static RegistryKey AppDataRegistry;
        public CodeCompletionWindow CompletionWindow;
        public XlgQuickScript CurrentScript;
        public bool Updating;

        private TextArea _textArea;

        public QuickScriptToolWindow(IGenerationHost host, XlgQuickScript script)
        {
            Host = host;
            InitializeComponent();
            InitializeEditor();
            CurrentScript = script;
        }

        public string WordBeforeCaret
        {
            get
            {
                if (ScriptEditor.Text != null && (ScriptEditor.Text.Length == 0 || _textArea.Caret.Column == 0))
                {
                    return string.Empty;
                }

                var lines = ScriptEditor.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                var linePart = lines[_textArea.Caret.Line].Substring(0, _textArea.Caret.Column).LastToken();
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

        private string LineAtCaret
        {
            get
            {
                if (Text.Length == 0 || _textArea.Caret.Column == 0) return string.Empty;
                var line = Text.TokenAt(
                    _textArea.Caret.Line,
                    Environment.NewLine,
                    StringComparison.InvariantCulture);

                return line;
            }
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

        public void DisplayExpandedQuickScriptSourceInNotepad()
        {
            try
            {
                if (CurrentScript == null)
                {
                    return;
                }
                UpdateScriptFromForm();

                var settings = ScriptEditor.Current.BuildSettings(false, Host);
                var result = settings.QuickScriptTemplate.ActualizeCode(settings);

                var source = result.OutputFiles["QuickScriptProcessor"].Value;
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

        public void ShowCodeCompletion(string[] items)
        {
            if (items != null && items.Length > 0)
            {
                var completionDataProvider = new CompletionDataProvider(items);
                CompletionWindow = CodeCompletionWindow.ShowCompletionWindow(this, ScriptEditor, string.Empty, completionDataProvider, '.');
                if (CompletionWindow != null)
                {
                    CompletionWindow.Closed += CompletionWindowClosed;
                }
            }
        }

        public void UpdateScriptFromForm()
        {
            if (CurrentScript == null)
            {
                return;
            }

            CurrentScript.Script = ScriptEditor.Text;
        }

        private void CompletionWindowClosed(object source, EventArgs e)
        {
            if (CompletionWindow != null)
            {
                CompletionWindow.Closed -= CompletionWindowClosed;
                CompletionWindow.Dispose();
                CompletionWindow = null;
            }
        }

        private void InitializeEditor()
        {
            ScriptEditor.FindAndReplaceForm = new FindAndReplaceForm(ScriptEditor, Host);
            _textArea = ScriptEditor.ActiveTextAreaControl.TextArea;
            _textArea.KeyEventHandler += ProcessKey;
            _textArea.KeyUp += TextAreaOnKeyUp;

            var fsmProvider = new FileSyntaxModeProvider(AppDomain.CurrentDomain.BaseDirectory);
            HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.
            ScriptEditor.SetHighlighting("QuickScript"); // Activate the highlighting, use the name from the SyntaxDefinition node.
            ScriptEditor.Refresh();
        }

        private void InsertMissingSections()
        {
            _textArea.InsertString("Members:\n\tList<string> Items = new List<string>();\n\n~~Start:\n\n~~Body:\n\n~~Finish:\n\n");
        }

        private bool ProcessKey(char ch)
        {
            switch (ch)
            {
                case '~':
                    if (LineAtCaret == CsProjGeneratorOptions.Delimiter)
                    {
                        InsertMissingSections();
                    }

                    break;
            }

            // switch (ch)
            // {
            // case '.':
            // if (WordBeforeCaret == "this")
            // {
            // ShowThisCodeCompletion();
            // }
            // break;
            // case '~':
            // ShowScriptCommandCodeCompletion();
            // break;
            // }
            return false;
        }

        private void QuickScriptToolWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveQuickScript_Click(sender, null);
        }

        private void QuickScriptToolWindow_Load(object sender, EventArgs e)
        {
            UpdateLastKnownPath();
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
                Window.Host ??= Host;
                Context.RunQuickScript(this, CurrentScript, null, Host);
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

                if (Host.Context.Scripts == null) return;

                UpdateScriptFromForm();

                SaveDestinationFilePathDialog.FileName = Host.Context.Scripts.FilePath;
                SaveDestinationFilePathDialog.InitialDirectory = Host.Context.Scripts.FilePath.TokensBeforeLast(@"\");
                SaveDestinationFilePathDialog.AddExtension = true;
                SaveDestinationFilePathDialog.CheckPathExists = true;
                SaveDestinationFilePathDialog.DefaultExt = ".xlgq";
                SaveDestinationFilePathDialog.Filter = "Qk Scrptr files (*.xlgq)|*.xlgq;All files (*.*)|*.*";
                SaveDestinationFilePathDialog.ShowDialog(this);
                if (!string.IsNullOrEmpty(SaveDestinationFilePathDialog.FileName))
                {
                    Host.Context.Scripts.FilePath = SaveDestinationFilePathDialog.FileName;
                    Host.Context.Scripts.Save(Dirs.ScriptArchivePath);
                    Text = "Qk Scrptr - " + Host.Context.Scripts.FilePath;
                    UpdateLastKnownPath();
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
                    {
                        SaveAs_Click(null, null);
                    }
                    else
                    {
                        Host.Context.Scripts.Save(Dirs.ScriptArchivePath);
                    }
                }
            }
            catch (Exception exception)
            {
                Host.MessageBox.Show(exception.ToString());
            }
        }

/*
        private void ShowScriptCommandCodeCompletion()
        {
            ShowCodeCompletion(new[] { "~:", "~Members:", "~Start:", "~Body:", "~Finish:", "~BeginString:", "~EndString:" });
        }
*/      
        private void ShowThisCodeCompletion()
        {
            ShowCodeCompletion(new[] { "Output", "Lines", "AllText", "DestinationFilePath", "InputFilePath", "LineCount", "OpenNotepad", "Ask" });
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
            else
            {
                // No modifiers
                switch (e.KeyCode)
                {
                    case Keys.F5:
                        e.Handled = true;
                        RunQuickScript_Click(null, null);
                        break;
                }
            }
        }
/*
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
*/
        private void UpdateLastKnownPath()
        {
            if (Host.Context.Scripts == null || string.IsNullOrEmpty(Host.Context.Scripts.FilePath) || !File.Exists(Host.Context.Scripts.FilePath))
            {
                return;
            }

            var openedKey = false;
            if (AppDataRegistry == null)
            {
                AppDataRegistry = Application.UserAppDataRegistry;
                openedKey = true;
            }

            if (AppDataRegistry == null)
            {
                return;
            }

            AppDataRegistry.SetValue("LastQuickScriptPath", Host.Context.Scripts.FilePath, RegistryValueKind.String);

            if (!openedKey || AppDataRegistry == null)
            {
                return;
            }

            AppDataRegistry.Close();
            AppDataRegistry = null;
        }

        private void ViewGeneratedCode_Click(object sender, EventArgs e)
        {
            DisplayExpandedQuickScriptSourceInNotepad();
        }

        private void ViewIndependentGeneratedCode_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentScript == null)
                    return;

                UpdateScriptFromForm();
                var settings = CurrentScript.BuildSettings(false, Host);
                var result = settings.ActualizeAndCompile();
                QuickScriptWorker.ViewText(Host, result.FinalDetails(out var keyLines), false);
                if (!result.CompileSuccessful) return;
                
                MessageBoxResult messageBoxResult = Host.MessageBox.Show(
                    $@"
Executable generated successfully in: 

    {result.DestinationExecutableFilePath}

Would you like to run it now? 
  (Yes to run, No to open the output folder, Cancel to do nothing)", "RUN EXE?", MessageBoxChoices.YesNoCancel);

                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        Process.Start(new ProcessStartInfo(result.DestinationExecutableFilePath)
                        {
                            UseShellExecute = true,
                            WorkingDirectory = result.Settings.ProjectFolder,
                        });
                        break;

                    case MessageBoxResult.No:
                        QuickScriptWorker.ViewFolderInExplorer(result.Settings.ProjectFolder, Host);
                        break;

                    default:
                        // Do nothing
                        break;
                }
            }
            catch (Exception exception)
            {
                Host.MessageBox.Show(exception.ToString());
            }
        }
    }
}
