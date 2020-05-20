using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Keystroke
    {
        [XmlAttribute]
        public string xmlns = "http://www.logitech.com/Cassandra/2010.1/Macros/Keystroke";
        [XmlElement("key")] public List<Key> Keys;
    }
}