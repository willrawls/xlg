using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Key : KdmBase
    {
        [XmlAttribute] public string value;
    }
}