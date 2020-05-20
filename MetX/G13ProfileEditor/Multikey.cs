using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Multikey
    {
        [XmlAttribute]
        public string xmlns = "http://www.logitech.com/Cassandra/2010.1/Macros/MultiKey";

        [XmlArrayItem("key", typeof(Key))]
        [XmlArrayItem("delay", typeof(Delay))]
        [XmlArrayItem("modifier", typeof(Modifier))]
        public List<KdmBase> Keys;

    }
}