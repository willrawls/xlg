using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Dpi
    {
        [XmlAttribute] public int enabled;
        [XmlAttribute] public int x;
        [XmlAttribute] public int y;
    }
}