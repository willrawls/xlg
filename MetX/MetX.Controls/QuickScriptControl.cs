namespace MetX.Controls
{
    using System;
    using System.Windows.Forms;

    using ICSharpCode.TextEditor;
    using ICSharpCode.TextEditor.Document;
    using ICSharpCode.TextEditor.Gui.CompletionWindow;

    using Properties;
    using Library;
    using Scripts;

    public partial class QuickScriptControl : TextEditorControl
    {
        public XlgQuickScript Current = null;

        private TextArea _codeArea;
        private CodeCompletionWindow _completionWindow;

        public QuickScriptControl()
        {
            InitializeComponent();
            InitializeEditor();
        }

        private string LineAtCaret
        {
            get
            {
                if ((Text.Length == 0) || (_codeArea.Caret.Column == 0)) return string.Empty;
                string line = Text.TokenAt(
                    _codeArea.Caret.Line + 1,
                    Environment.NewLine,
                    StringComparison.InvariantCulture);

                return line;
            }
        }

        private string WordBeforeCaret
        {
            get
            {
                if ((_codeArea.Caret.Column == 0) || (Text.Length == 0))
                {
                    return string.Empty;
                }

                string linePart = LineAtCaret.Substring(0, _codeArea.Caret.Column).LastToken();
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
                _completionWindow = CodeCompletionWindow.ShowCompletionWindow(
                    FindForm(),
                    this,
                    string.Empty,
                    completionDataProvider,
                    '.');
                if (_completionWindow != null)
                {
                    _completionWindow.Closed += CompletionWindowClosed;
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
            if (_completionWindow != null)
            {
                _completionWindow.Closed -= CompletionWindowClosed;
                _completionWindow.Dispose();
                _completionWindow = null;
            }
        }

        private void InitializeEditor()
        {
            _codeArea = ActiveTextAreaControl.TextArea;

            _codeArea.KeyEventHandler += ProcessKey;
            _codeArea.KeyUp += CodeAreaOnKeyUp;

            FileSyntaxModeProvider fsmProvider =
                new FileSyntaxModeProvider(AppDomain.CurrentDomain.BaseDirectory);
            HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.
            SetHighlighting("QuickScript");

            // Activate the highlighting, use the name from the SyntaxDefinition node.
            Refresh();
        }

        private void InsertMissingSections()
        {
            _codeArea.InsertString(Resources.StringToInsertOnTripleTilde);
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
                    string wordBeforeCaret = WordBeforeCaret;
                    string lineAtCaret = LineAtCaret;

                    if ((wordBeforeCaret == "Ask") && !lineAtCaret.Contains(")"))
                    {
                        int location = _codeArea.Caret.Column;
                        _codeArea.InsertString("(\"What string to look for?\", \" \");");
                        _codeArea.Caret.Column = location + 30;
                        return true;
                    }

                    if ((wordBeforeCaret == "Choose") && !lineAtCaret.Contains(")"))
                    {
                        int location = _codeArea.Caret.Column;
                        _codeArea.InsertString("(choices, selectedIndex, \"Please select one from the list\", \"SELECT ONE\");");
                        _codeArea.Caret.Column = location + 9;
                        return true;
                    }

                    if ((wordBeforeCaret == "MultipleChoice") && !lineAtCaret.Contains(")"))
                    {
                        int location = _codeArea.Caret.Column;
                        _codeArea.InsertString("(choices, initiallySelectedIndices, \"Please select one or more items from the list\", \"MULTIPLE CHOICE\");");
                        _codeArea.Caret.Column = location + 9;
                        return true;
                    }

                    if ((wordBeforeCaret == "MessageBox") && !lineAtCaret.Contains(")"))
                    {
                        int location = _codeArea.Caret.Column;
                        _codeArea.InsertString(".Show(\"\");");
                        _codeArea.Caret.Column = location + 7;
                        return true;
                    }

                    break;
            }
            return false;
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
