using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using MetX.Data;
using MetX.Library;

namespace MetX.Controls
{
    public partial class QuickScriptControl : TextEditorControl
    {
        public QuickScriptControl()
        {
            InitializeComponent();
            InitializeEditor();
        }

        public XlgQuickScript Current = null;
        private TextArea textArea;
        private CodeCompletionWindow completionWindow;

        private void InitializeEditor()
        {
            textArea = ActiveTextAreaControl.TextArea;
            textArea.KeyEventHandler += ProcessKey;
            textArea.KeyUp += TextAreaOnKeyUp;

            FileSyntaxModeProvider fsmProvider = new FileSyntaxModeProvider(AppDomain.CurrentDomain.BaseDirectory);
            HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.
            SetHighlighting("QuickScript"); // Activate the highlighting, use the name from the SyntaxDefinition node.
            Refresh();
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
            else // No modifiers
            {
                switch (e.KeyCode)
                {
                    case Keys.F5:
                        ScriptRunningWindow form = FindForm() as ScriptRunningWindow;
                        if(form != null)
                        {
                            e.Handled = true;
                            form.RunQuickScript(Current);
                        }
                        break;
                }
            }
        }

        private bool ProcessKey(char ch)
        {
            switch (ch)
            {
                case '.':
                    if (WordBeforeCaret == "this") ShowThisCodeCompletion();
                    break;
                case '~':
                    ShowScriptCommandCodeCompletion();
                    break;
            }
            return false;
        }

        private void ShowScriptCommandCodeCompletion() { ShowCodeCompletion(new[] { "~:", "~Members:", "~Start:", "~Body:", "~Finish:", "~BeginString:", "~EndString:" }); }

        private void ShowThisCodeCompletion() { ShowCodeCompletion(new[] { "Output", "Lines", "AllText", "DestionationFilePath", "InputFilePath", "LineCount", "OpenNotepad", "Ask" }); }

        public void ShowCodeCompletion(string[] items)
        {
            if (items != null && items.Length > 0)
            {
                CompletionDataProvider completionDataProvider = new CompletionDataProvider(items);
                completionWindow = CodeCompletionWindow.ShowCompletionWindow(FindForm(), this, String.Empty, completionDataProvider, '.');
                if (completionWindow != null)
                {
                    completionWindow.Closed += CompletionWindowClosed;
                }
            }
        }

        private string WordBeforeCaret
        {
            get
            {
                if (Text.Length == 0 || textArea.Caret.Column == 0) return string.Empty;
                string[] lines = Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                string linePart = lines[textArea.Caret.Line].Substring(0, textArea.Caret.Column).LastToken();
                int i;
                for (i = linePart.Length - 1; i >= 0; i--)
                {
                    if (!char.IsLetterOrDigit(linePart[i])) break;
                }
                if (i > 0 && i < linePart.Length) linePart = linePart.Substring(i);
                return linePart.ToLower().Trim();
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
    }
}
