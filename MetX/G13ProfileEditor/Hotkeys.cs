using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Hotkeys
    {
        [XmlAttribute]
        public string xmlns= "http://www.logitech.com/Cassandra/2010.1/Macros/Hotkeys";

        [XmlElement("do")] public List<Do> Does;
    }
}