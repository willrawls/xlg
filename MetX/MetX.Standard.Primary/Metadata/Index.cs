using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Metadata
{
    [Serializable]
    public class Index
    {
        [XmlAttribute] public string IndexName;
        [XmlAttribute] public string IsClustered;
        [XmlAttribute] public string Location;
        [XmlAttribute] public string PropertyName;
        [XmlAttribute] public string SingleColumnIndex;

        [XmlArray(ElementName = "IndexColumns")]
        [XmlArrayItem("IndexColumn", typeof(IndexColumn))]
        public List<IndexColumn> IndexColumns;
    }
}