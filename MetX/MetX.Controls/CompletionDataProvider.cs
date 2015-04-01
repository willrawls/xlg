using System.Collections.Generic;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace MetX.Controls
{
	public class CompletionDataProvider : ICompletionDataProvider
	{
	    readonly ImageList m_ImageList = new ImageList();

        readonly ICompletionData[] m_CompletionData;
		
		public CompletionDataProvider(IList<string> items)
		{
		    m_CompletionData = new ICompletionData[items.Count];
		    for (int index = 0; index < items.Count; index++) 
                m_CompletionData[index] = new DefaultCompletionData(items[index], 0);
		}

	    public ImageList ImageList {
			get { return m_ImageList; }
		}
		
		public string PreSelection {
			get { return null; }
		}
		
		public int DefaultIndex {
			get { return 0; }
		}
		
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
			return m_CompletionData;
		}
	}
}
