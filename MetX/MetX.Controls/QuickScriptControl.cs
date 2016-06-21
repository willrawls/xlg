namespace MetX.Controls
{
    using System;
    using System.Windows.Forms;

    using ICSharpCode.TextEditor;
    using ICSharpCode.TextEditor.Document;
    using ICSharpCode.TextEditor.Gui.CompletionWindow;

    using MetX.Controls.Properties;
    using MetX.Library;
    using MetX.Scripts;

    public partial class QuickScriptControl : TextEditorControl
    {
        public XlgQuickScript Current = null;

        private TextArea CodeArea;
        private CodeCompletionWindow completionWindow;

        public QuickScriptControl()
        {
            InitializeComponent();
            InitializeEditor();
        }

        private string LineAtCaret
        {
            get
            {
                if ((this.Text.Length == 0) || (this.CodeArea.Caret.Column == 0)) return string.Empty;
                string line = Text.TokenAt(
                    this.CodeArea.Caret.Line + 1,
                    Environment.NewLine,
                    StringComparison.InvariantCulture);

                return line;
            }
        }

        private string WordBeforeCaret
        {
            get
            {
                if ((this.CodeArea.Caret.Column == 0) || (this.Text.Length == 0))
                {
                    return string.Empty;
                }

                string linePart = LineAtCaret.Substring(0, this.CodeArea.Caret.Column).LastToken();
                int i;
                for (i = linePart.Length - 1; i >= 0; i--)
                {
                    if (!char.IsLetterOrDigit(linePart[i]))
                    {
                        break;
                    }
                }

                if ((i > 0) && (i < linePart.Length))
                {
                    linePart = linePart.Substring(i);
                }

                return linePart.Trim();
            }
        }

        public void ShowCodeCompletion(string[] items)
        {
            if ((items != null) && (items.Length > 0))
            {
                CompletionDataProvider completionDataProvider = new CompletionDataProvider(items);
                completionWindow = CodeCompletionWindow.ShowCompletionWindow(
                    FindForm(),
                    this,
                    string.Empty,
                    completionDataProvider,
                    '.');
                if (completionWindow != null)
                {
                    completionWindow.Closed += CompletionWindowClosed;
                }
            }
        }

        private void CodeAreaOnKeyUp(object sender, KeyEventArgs e)
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
                        ScriptRunningWindow form = FindForm() as ScriptRunningWindow;
                        if (form != null)
                        {
                            e.Handled = true;
                            form.RunQuickScript(Current);
                        }

                        break;
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

        private void InitializeEditor()
        {
            this.CodeArea = ActiveTextAreaControl.TextArea;

            this.CodeArea.KeyEventHandler += ProcessKey;
            this.CodeArea.KeyUp += this.CodeAreaOnKeyUp;

            FileSyntaxModeProvider fsmProvider =
                new FileSyntaxModeProvider(AppDomain.CurrentDomain.BaseDirectory);
            HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.
            SetHighlighting("QuickScript");

            // Activate the highlighting, use the name from the SyntaxDefinition node.
            Refresh();
        }

        private void InsertMissingSections()
        {
            this.CodeArea.InsertString(Resources.StringToInsertOnTripleTilde);
        }

        private bool ProcessKey(char ch)
        {
            switch (ch)
            {
                case '~':
                    if (LineAtCaret == "~~")
                    {
                        InsertMissingSections();
                        return true;
                    }

                    break;

                case '(':
                    string wordBeforeCaret = this.WordBeforeCaret;
                    string lineAtCaret = this.LineAtCaret;

                    if ((wordBeforeCaret == "Ask") && !lineAtCaret.Contains(")"))
                    {
                        int location = this.CodeArea.Caret.Column;
                        this.CodeArea.InsertString("(\"What string to look for?\", \" \");");
                        this.CodeArea.Caret.Column = location + 30;
                        return true;
                    }

                    if ((wordBeforeCaret == "Choose") && !lineAtCaret.Contains(")"))
                    {
                        int location = this.CodeArea.Caret.Column;
                        this.CodeArea.InsertString("(choices, selectedIndex, \"Please select one from the list\", \"SELECT ONE\");");
                        this.CodeArea.Caret.Column = location + 9;
                        return true;
                    }

                    if ((wordBeforeCaret == "MultipleChoice") && !lineAtCaret.Contains(")"))
                    {
                        int location = this.CodeArea.Caret.Column;
                        this.CodeArea.InsertString("(choices, initiallySelectedIndices, \"Please select one or more items from the list\", \"MULTIPLE CHOICE\");");
                        this.CodeArea.Caret.Column = location + 9;
                        return true;
                    }

                    if ((wordBeforeCaret == "MessageBox") && !lineAtCaret.Contains(")"))
                    {
                        int location = this.CodeArea.Caret.Column;
                        this.CodeArea.InsertString(".Show(\"\");");
                        this.CodeArea.Caret.Column = location + 7;
                        return true;
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

        private void ShowScriptCommandCodeCompletion()
        {
            ShowCodeCompletion(
                new[] { "~:", "~Members:", "~Start:", "~Body:", "~Finish:", "~BeginString:", "~EndString:" });
        }

        private void ShowThisCodeCompletion()
        {
            ShowCodeCompletion(
                new[]
                    {
                        "Output", "Lines", "AllText", "DestionationFilePath", "InputFilePath", "LineCount",
                        "OpenNotepad", "Ask"
                    });
        }
    }
}
