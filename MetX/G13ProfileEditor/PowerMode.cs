using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class PowerMode
    {
        [XmlAttribute] public int value;
    }
}