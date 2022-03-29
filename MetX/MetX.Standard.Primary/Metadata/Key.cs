using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Metadata
{
    [Serializable]
    public class Key
    {
        [XmlAttribute] public string IsPrimary;
        [XmlAttribute] public string Location;
        [XmlAttribute] public string Name;

        [XmlArray(ElementName = "Columns")]
        [XmlArrayItem("Column", typeof(KeyColumn))]
        public List<KeyColumn> Columns;
    }
}