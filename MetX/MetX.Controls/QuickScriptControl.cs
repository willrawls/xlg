// ReSharper disable UnusedVariable

using MetX.Windows.Library;

namespace MetX.Controls
{
    using System;
    using System.Windows.Forms;

    using ICSharpCode.TextEditor;
    using ICSharpCode.TextEditor.Document;
    using ICSharpCode.TextEditor.Gui.CompletionWindow;

    using Properties;
    using MetX.Standard.Library;
    using MetX.Standard.Scripts;

    public partial class QuickScriptControl : TextEditorControl
    {
        public XlgQuickScript Current = null;

        public TextArea _codeArea;
        public CodeCompletionWindow _completionWindow;

        public QuickScriptControl()
        {
            InitializeComponent();
            InitializeEditor();
            FindAndReplaceForm = new FindAndReplaceForm(this);
        }

        public string LineAtCaret
        {
            get
            {
                if (Text.Length == 0 || _codeArea.Caret.Column == 0) return string.Empty;
                var line = Text.TokenAt(
                    _codeArea.Caret.Line + 1,
                    Environment.NewLine,
                    StringComparison.InvariantCulture);

                return line;
            }
        }

        public string WordBeforeCaret
        {
            get
            {
                if (_codeArea.Caret.Column == 0 || Text.Length == 0)
                {
                    return string.Empty;
                }

                var linePart = LineAtCaret.Substring(0, _codeArea.Caret.Column).LastToken();
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

                return linePart.Trim();
            }
        }

        public void ShowCodeCompletion(string[] items)
        {
            if (items != null && items.Length > 0)
            {
                var completionDataProvider = new CompletionDataProvider(items);
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

        public void CodeAreaOnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Space:
                        ShowThisCodeCompletion();
                        break;
                    
                    /*
                    case Keys.F:
                        FindNext();
                        break;
                    case Keys.H:
                        if (e.Shift)
                            ReplaceAll();
                        break;
                */
                }
            }
            else
            {
                // No modifiers
                switch (e.KeyCode)
                {
                    case Keys.F5:
                        var form = FindForm() as ScriptRunningWindow;
                        if (form != null)
                        {
                            e.Handled = true;
                            form.RunQuickScript(Current);
                        }

                        break;
                }
            }
        }

        public void FindNext()
        {
            var dialog = new AskForStringDialog();
            var toFind = dialog.Ask("What would you like to find?", "REPLACE ALL - TO FIND", LastFind);
            if (toFind.IsEmpty())
                return;

            var rest = Text.Substring(_codeArea.Caret.Offset);
            if (rest.IsNotEmpty())
            {
                var index = rest.IndexOf(toFind, StringComparison.InvariantCultureIgnoreCase);
                if (index > 0)
                {
                    var realIndex = index + _codeArea.Caret.Offset;
                    var newCaret = new Caret(_codeArea);
                    _codeArea.Caret.UpdateCaretPosition();
                    //_codeArea.Caret.
                }
            }
        }

        public string LastFind;
        public string LastReplace;
        public readonly FindAndReplaceForm FindAndReplaceForm;

        public void ReplaceAll()
        {
            var dialog = new AskForStringDialog();
            var toFind = dialog.Ask("What would you like to find?", "REPLACE ALL - TO FIND", LastFind);
            if (toFind.IsEmpty())
                return;
            
            var replacement = dialog.Ask($"Replace with what?\n\nTo Find: {toFind}", "REPLACE ALL - CHOOSE REPLACEMENT", LastReplace);
            
            LastFind = toFind;
            LastReplace = replacement;

            Text = Text.Replace(toFind, replacement);
        }

        public void CompletionWindowClosed(object source, EventArgs e)
        {
            if (_completionWindow != null)
            {
                _completionWindow.Closed -= CompletionWindowClosed;
                _completionWindow.Dispose();
                _completionWindow = null;
            }
        }

        public void InitializeEditor()
        {
            _codeArea = ActiveTextAreaControl.TextArea;

            _codeArea.KeyEventHandler += ProcessKey;
            _codeArea.KeyUp += CodeAreaOnKeyUp;

            var fsmProvider =
                new FileSyntaxModeProvider(AppDomain.CurrentDomain.BaseDirectory);
            HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.
            SetHighlighting("QuickScript");

            // Activate the highlighting, use the name from the SyntaxDefinition node.
            Refresh();
        }

        public void InsertMissingSections()
        {
            _codeArea.InsertString(Resources.StringToInsertOnTripleTilde);
        }

        public bool ProcessKey(char ch)
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
                    var wordBeforeCaret = WordBeforeCaret;
                    var lineAtCaret = LineAtCaret;

                    if (wordBeforeCaret == "Ask" && !lineAtCaret.Contains(")"))
                    {
                        var location = _codeArea.Caret.Column;
                        _codeArea.InsertString("(\"What string to look for?\", \" \");");
                        _codeArea.Caret.Column = location + 30;
                        return true;
                    }

                    if (wordBeforeCaret == "Choose" && !lineAtCaret.Contains(")"))
                    {
                        var location = _codeArea.Caret.Column;
                        _codeArea.InsertString("(choices, selectedIndex, \"Please select one from the list\", \"SELECT ONE\");");
                        _codeArea.Caret.Column = location + 9;
                        return true;
                    }

                    if (wordBeforeCaret == "MultipleChoice" && !lineAtCaret.Contains(")"))
                    {
                        var location = _codeArea.Caret.Column;
                        _codeArea.InsertString("(choices, initiallySelectedIndices, \"Please select one or more items from the list\", \"MULTIPLE CHOICE\");");
                        _codeArea.Caret.Column = location + 9;
                        return true;
                    }

                    if (wordBeforeCaret == "MessageBox" && !lineAtCaret.Contains(")"))
                    {
                        var location = _codeArea.Caret.Column;
                        _codeArea.InsertString(".Show(\"\");");
                        _codeArea.Caret.Column = location + 7;
                        return true;
                    }

                    break;
            }
            return false;
        }

        public void ShowThisCodeCompletion()
        {
            ShowCodeCompletion(
                new[]
                    {
                        "Output", "Lines", "AllText", "DestinationFilePath", "InputFilePath", "LineCount",
                        "OpenNotepad", "Ask"
                    });
        }

        public void menuEditFind_Click(object sender, EventArgs e)
        {
            FindAndReplaceForm.ShowFor(false);
        }

        public void menuEditReplace_Click(object sender, EventArgs e)
        {
            FindAndReplaceForm.ShowFor(true);
        }

        public void menuFindAgain_Click(object sender, EventArgs e)
        {
            FindAndReplaceForm.FindNext(true, false, $"Search text «{FindAndReplaceForm.LookFor}» not found.");
        }
        public void menuFindAgainReverse_Click(object sender, EventArgs e)
        {
            FindAndReplaceForm.FindNext(true, true, $"Search text «{FindAndReplaceForm.LookFor}» not found.");
        }

        public void QuickScriptControl_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public void QuickScriptControl_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F:
                    if (e.Control)
                    {
                        FindAndReplaceForm.ShowFor(false);
                    }
                    break;
                case Keys.H:
                    if (e.Control)
                    {
                        FindAndReplaceForm.ShowFor(true);
                    }
                    break;
            }
        }
    }
}
