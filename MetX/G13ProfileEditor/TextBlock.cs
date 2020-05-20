using System;
using System.Xml;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class TextBlock
    {
        [XmlAttribute]
        public string xmlns = "http://www.logitech.com/Cassandra/2010.2/Macros/TextBlock";

        [XmlElement("text")]
        public Text Text;

        public TextBlock(Text text)
        {
            Text = text;
        }

        public TextBlock()
        {
        }
    }
}