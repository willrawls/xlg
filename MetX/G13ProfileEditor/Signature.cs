using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Signature
    {

        [XmlAttribute] public string value;
        [XmlAttribute] public string executble;
        [XmlAttribute] public string key;
        [XmlAttribute] public string name;

    }
}