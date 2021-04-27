using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;
using MetX.Standard.Data;

namespace MetX.Standard.Metadata
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