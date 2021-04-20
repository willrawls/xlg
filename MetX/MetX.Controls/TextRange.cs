using ICSharpCode.TextEditor.Document;

namespace MetX.Controls
{
    public class TextRange : AbstractSegment
    {
        IDocument _document;
        public TextRange(IDocument document, int offset, int length)
        {
            _document = document;
            this.offset = offset;
            this.length = length;
        }
    }
}