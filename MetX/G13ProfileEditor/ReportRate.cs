using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class ReportRate
    {
        [XmlAttribute] public int rate;
    }
}