using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Movement
    {
        [XmlAttribute] public int acceleration;
        [XmlAttribute] public int speed;
    }
}