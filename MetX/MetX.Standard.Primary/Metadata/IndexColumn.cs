using System;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Metadata
{
    [Serializable]
    public class IndexColumn
    {
        [XmlAttribute] public string IndexColumnName;
        [XmlAttribute] public string Location;
        [XmlAttribute] public string PropertyName;
    }
}