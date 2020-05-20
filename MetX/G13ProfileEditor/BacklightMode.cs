using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class BacklightMode
    {
        [XmlAttribute] public string color;
        [XmlAttribute] public int shiftstate;
    }
}