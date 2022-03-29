using System.Collections.Generic;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Gui;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace MetX.Controls
{
	public class CompletionDataProvider : ICompletionDataProvider
	{
		readonly ICompletionData[] _mCompletionData;
		
		public CompletionDataProvider(IList<string> items)
		{
		    _mCompletionData = new ICompletionData[items.Count];
		    for (var index = 0; index < items.Count; index++) 
                _mCompletionData[index] = new DefaultCompletionData(items[index],items[index],0);
		}

	    public ImageList ImageList { get; } = new ImageList();

	    public string PreSelection => null;

        public int DefaultIndex => 0;

        public CompletionDataProviderKeyResult ProcessKey(char key)
		{
			if (char.IsLetterOrDigit(key)) {
				return CompletionDataProviderKeyResult.NormalKey;
			}
			return CompletionDataProviderKeyResult.InsertionKey;
		}
		
		public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
		{
			textArea.Caret.Position = textArea.Document.OffsetToPosition(insertionOffset);
			return data.InsertAction(textArea, key);
		}
		
		public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
		{
			return _mCompletionData;
		}
	}
}
