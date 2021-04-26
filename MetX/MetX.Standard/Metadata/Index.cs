using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class Index
    {
        [XmlAttribute] public string IndexName;
        [XmlAttribute] public string IsClustered;
        [XmlAttribute] public string Location;
        [XmlAttribute] public string PropertyName;
        [XmlAttribute] public string SingleColumnIndex;

        [XmlArrayItem("IndexColumn")]
        public List<IndexColumn> IndexColumns;
    }
}