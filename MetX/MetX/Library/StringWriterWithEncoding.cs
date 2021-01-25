using System.Text;

namespace MetX.Library
{
    public class StringWriterWithEncoding : System.IO.StringWriter
    {
        Encoding _mEncoding;
        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
            : base(sb)
        {
            _mEncoding = encoding;
        }

        public override Encoding Encoding => _mEncoding;
    }
}