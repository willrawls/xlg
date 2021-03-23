using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace MetX.Controls
{
	public partial class FindAndReplaceForm : Form
	{
		public FindAndReplaceForm(QuickScriptControl editor)
		{
			InitializeComponent();
			_search = new TextEditorSearcher();
            Editor = editor;
        }

		TextEditorSearcher _search;
		TextEditorControl _editor;
		TextEditorControl Editor { 
			get => _editor;
            set { 
				_editor = value;
				_search.Document = _editor.Document;
				UpdateTitleBar();
			}
		}

		private void UpdateTitleBar()
		{
			var text = ReplaceMode ? "Find & replace" : "Find";
			if (_editor != null && _editor.FileName != null)
				text += " - " + Path.GetFileName(_editor.FileName);
			if (_search.HasScanRegion)
				text += " (selection only)";
			Text = text;
		}

		public void ShowFor(bool replaceMode)
		{
			_search.ClearScanRegion();
			var sm = Editor.ActiveTextAreaControl.SelectionManager;
			if (sm.HasSomethingSelected && sm.SelectionCollection.Count == 1) {
				var sel = sm.SelectionCollection[0];
				if (sel.StartPosition.Line == sel.EndPosition.Line)
					txtLookFor.Text = sm.SelectedText;
				else
					_search.SetScanRegion(sel);
			} else {
				// Get the current word that the caret is on
				var caret = Editor.ActiveTextAreaControl.Caret;
				var start = TextUtilities.FindWordStart(Editor.Document, caret.Offset);
				var endAt = TextUtilities.FindWordEnd(Editor.Document, caret.Offset);
				txtLookFor.Text = Editor.Document.GetText(start, endAt - start);
			}
			
			ReplaceMode = replaceMode;

			Owner = (Form)Editor.TopLevelControl;
			Show();
			
			txtLookFor.SelectAll();
			txtLookFor.Focus();
		}

		public bool ReplaceMode
		{
			get => txtReplaceWith.Visible;
            set {
				btnReplace.Visible = btnReplaceAll.Visible = value;
				lblReplaceWith.Visible = txtReplaceWith.Visible = value;
				btnHighlightAll.Visible = !value;
				AcceptButton = value ? btnReplace : btnFindNext;
				UpdateTitleBar();
			}
		}

		private void btnFindPrevious_Click(object sender, EventArgs e)
		{
			FindNext(false, true, "Text not found");
		}
		private void btnFindNext_Click(object sender, EventArgs e)
		{
			FindNext(false, false, "Text not found");
		}

		public bool LastSearchWasBackward = false;
		public bool LastSearchLoopedAround;

		public TextRange FindNext(bool viaF3, bool searchBackward, string messageIfNotFound)
		{
			if (string.IsNullOrEmpty(txtLookFor.Text))
			{
				MessageBox.Show("No string specified to look for!");
				return null;
			}
			LastSearchWasBackward = searchBackward;
			_search.LookFor = txtLookFor.Text;
			_search.MatchCase = chkMatchCase.Checked;
			_search.MatchWholeWordOnly = chkMatchWholeWord.Checked;

			var caret = Editor.ActiveTextAreaControl.Caret;
			if (viaF3 && _search.HasScanRegion && !caret.Offset.
				IsInRange(_search.BeginOffset, _search.EndOffset)) {
				// user moved outside of the originally selected region
				_search.ClearScanRegion();
				UpdateTitleBar();
			}

			var startFrom = caret.Offset - (searchBackward ? 1 : 0);
			var range = _search.FindNext(startFrom, searchBackward, out LastSearchLoopedAround);
			if (range != null)
				SelectResult(range);
			else if (messageIfNotFound != null)
				MessageBox.Show(messageIfNotFound);
			return range;
		}

		private void SelectResult(TextRange range)
		{
			var p1 = _editor.Document.OffsetToPosition(range.Offset);
			var p2 = _editor.Document.OffsetToPosition(range.Offset + range.Length);
			_editor.ActiveTextAreaControl.SelectionManager.SetSelection(p1, p2);
			_editor.ActiveTextAreaControl.ScrollTo(p1.Line, p1.Column);
			// Also move the caret to the end of the selection, because when the user 
			// presses F3, the caret is where we start searching next time.
			_editor.ActiveTextAreaControl.Caret.Position = 
				_editor.Document.OffsetToPosition(range.Offset + range.Length);
		}

		Dictionary<TextEditorControl, HighlightGroup> _highlightGroups = new();

		private void btnHighlightAll_Click(object sender, EventArgs e)
		{
			if (!_highlightGroups.ContainsKey(_editor))
				_highlightGroups[_editor] = new HighlightGroup(_editor);
			var group = _highlightGroups[_editor];

			if (string.IsNullOrEmpty(LookFor))
				// Clear highlights
				group.ClearMarkers();
			else {
				_search.LookFor = txtLookFor.Text;
				_search.MatchCase = chkMatchCase.Checked;
				_search.MatchWholeWordOnly = chkMatchWholeWord.Checked;

				var looped = false;
				int offset = 0, count = 0;
				for(;;) {
					var range = _search.FindNext(offset, false, out looped);
					if (range == null || looped)
						break;
					offset = range.Offset + range.Length;
					count++;

					var m = new TextMarker(range.Offset, range.Length, 
							TextMarkerType.SolidBlock, Color.Yellow, Color.Black);
					group.AddMarker(m);
				}
				if (count == 0)
					MessageBox.Show("Search text not found.");
				else
					Close();
			}
		}
		
		private void FindAndReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
		{	// Prevent dispose, as this form can be re-used
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				if (Owner != null)
					Owner.Select(); // prevent another app from being activated instead
				
				e.Cancel = true;
				Hide();
				
				// Discard search region
				_search.ClearScanRegion();
				_editor?.Refresh(); // must repaint manually
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnReplace_Click(object sender, EventArgs e)
		{
			var sm = _editor.ActiveTextAreaControl.SelectionManager;
			if (string.Equals(sm.SelectedText, txtLookFor.Text, StringComparison.OrdinalIgnoreCase))
				InsertText(txtReplaceWith.Text);
			FindNext(false, LastSearchWasBackward, "Text not found.");
		}

		private void btnReplaceAll_Click(object sender, EventArgs e)
		{
			var count = 0;
			// BUG FIX: if the replacement string contains the original search string
			// (e.g. replace "red" with "very red") we must avoid looping around and
			// replacing forever! To fix, start replacing at beginning of region (by 
			// moving the caret) and stop as soon as we loop around.
			_editor.ActiveTextAreaControl.Caret.Position = 
				_editor.Document.OffsetToPosition(_search.BeginOffset);

			_editor.Document.UndoStack.StartUndoGroup();
			try {
				while (FindNext(false, false, null) != null)
				{
					if (LastSearchLoopedAround)
						break;

					// Replace
					count++;
					InsertText(txtReplaceWith.Text);
				}
			} finally {
				_editor.Document.UndoStack.EndUndoGroup();
			}
			if (count == 0)
				MessageBox.Show("No occurrances found.");
			else {
				MessageBox.Show($"Replaced {count} occurrances.");
				Close();
			}
		}

		private void InsertText(string text)
		{
			var textArea = _editor.ActiveTextAreaControl.TextArea;
			textArea.Document.UndoStack.StartUndoGroup();
			try {
				if (textArea.SelectionManager.HasSomethingSelected) {
					textArea.Caret.Position = textArea.SelectionManager.SelectionCollection[0].StartPosition;
					textArea.SelectionManager.RemoveSelectedText();
				}
				textArea.InsertString(text);
			} finally {
				textArea.Document.UndoStack.EndUndoGroup();
			}
		}

		public string LookFor => txtLookFor.Text;
    }
}
