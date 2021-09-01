using System;
using System.Xml.Serialization;

namespace MetX.Standard.Metadata
{
    [Serializable]
    public class View
    {
        [XmlAttribute] public string Schema { get; set; }
        [XmlAttribute] public string ViewName { get; set; }
        [XmlAttribute] public string TSQL { get; set; }
    }
}