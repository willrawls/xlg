using System.Text;

namespace MetX.Standard.Strings
{
    public class StringWriterWithEncoding : System.IO.StringWriter
    {
        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
            : base(sb)
        {
            Encoding = encoding;
        }

        public override Encoding Encoding { get; }
    }
}