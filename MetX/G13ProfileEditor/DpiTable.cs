using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class DpiTable
    {
        [XmlAttribute] public int shiftindex;
        [XmlAttribute] public int defaultindex;
        [XmlAttribute] public int syncxy;

        [XmlElement("dpi")] public List<Dpi> Dpis;
    }
}