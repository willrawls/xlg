// ReSharper disable UnusedVariable

using System.Collections.Generic;
using ICSharpCode.TextEditor.UserControls;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.Strings;
using MetX.Windows.Library;

namespace MetX.Controls
{
    using System;
    using System.Windows.Forms;

    using ICSharpCode.TextEditor;
    using ICSharpCode.TextEditor.Document;
    using ICSharpCode.TextEditor.Gui.CompletionWindow;
    using MetX.Standard.Scripts;

    public partial class QuickScriptControl : TextEditorControl
    {
        public XlgQuickScript Current = null;
        public TextArea _codeArea;
        public CodeCompletionWindow _completionWindow;

        static QuickScriptControl()
        {
            CodeCompletionStrings = new List<string>()
            {
                "~~Start:\n\t",
                "~~Members:\n\t",
                "~~ReadInput:\n\t",
                "~~ProcessLine:\n\tif(line.Contains(\"$filter\"))\n\t\t{\n\t\t~~:%line%$end\n\t\t}\n",
                "~~Finish:\n\t",
                "~~:%$end%",
                "~~:%line%\n",
                "~~Using:\nSystem.",
                "~~To: $filename",
                "~~AppendTo: $filename",
                "~~Ask: \"$title\" \"$description\" \"$default\" ",
                "~~BeginString:\n",
                "~~EndString:\n",
                Helpers.SlashSlashBlockLeftDelimiter + "\n", 
                Helpers.SlashSlashBlockRightDelimiter + "\n",
                "Output.AppendLine(\"$end\");",
                "line",
                "number",
                "DestinationFilePath",
                "InputFilePath",
                "LineCount",
                "if($condition)\n{\n\t$end\n}\n",
            };
        }

        public QuickScriptControl()
        {
            InitializeComponent();
            InitializeEditor();
            ToolWindow.ChangeTheme(ColorScheme.DarkThemeOne, Controls);
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
            if (items.IsEmpty()) return;

            var completionDataProvider = new CompletionDataProvider(items);
            _completionWindow = CodeCompletionWindow.ShowCompletionWindow(
                FindForm(),
                this,
                string.Empty,
                completionDataProvider,
                '.');

            if (_completionWindow != null) 
                _completionWindow.Closed += CompletionWindowClosed;
        }

        public void CodeAreaOnKeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Control || e.KeyCode != Keys.Oemtilde) return;
            
            ShowThisCodeCompletion();
            e.Handled = true;
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
        public FindAndReplaceForm FindAndReplaceForm;

        public static List<string> CodeCompletionStrings {get; set;}

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

        /*
        public void InsertMissingSections()
        {
            _codeArea.InsertString(Resources.StringToInsertOnTripleTilde);
        }
        */

        public bool ProcessKey(char ch)
        {
            switch (ch)
            {
                /*
                case '~':
                    if (LineAtCaret == CsProjGeneratorOptions.Delimiter)
                    {
                        return true;
                    }
                    break;
                */

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
            ShowCodeCompletion(CodeCompletionStrings.ToArray());
        }

        public void menuEditFind_Click(object sender, EventArgs e)
        {
            FindAndReplaceForm.ShowFor(this, false);
        }

        public void menuEditReplace_Click(object sender, EventArgs e)
        {
            FindAndReplaceForm.ShowFor(this, true);
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
        }
    }
}
