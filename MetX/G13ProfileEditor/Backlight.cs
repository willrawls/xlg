using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Backlight
    {
        [XmlAttribute] public string devicemodel;
        [XmlElement("mode")] public List<BacklightMode> Modes;
    }
}