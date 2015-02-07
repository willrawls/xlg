using System.Text;

namespace MetX.Library
{
    public class StringWriterWithEncoding : System.IO.StringWriter
    {
        Encoding m_Encoding;
        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
            : base(sb)
        {
            m_Encoding = encoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return m_Encoding;
            }
        }
    }
}