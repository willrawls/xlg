using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Natural
    {
        [XmlAttribute]
        public string xmlns = "http://www.logitech.com/Cassandra/2010.1/Macros/Natural";
    }
}