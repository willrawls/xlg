using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Metadata
{
    [Serializable]
    public class Table
    {
        [XmlAttribute] public string ClassName;
        [XmlAttribute] public string PrimaryKeyColumnName;
        [XmlAttribute] public string RowCount;
        [XmlAttribute] public string TableName;

        [XmlArray(ElementName = "Columns")]
        [XmlArrayItem(typeof(Column), ElementName = "Column")]
        public List<Column> Columns;
        
        [XmlArray(ElementName = "Indexes")]
        [XmlArrayItem(typeof(Index), ElementName = "Index")]
        public List<Index> Indexes;

        [XmlArray(ElementName = "Keys")]
        [XmlArrayItem("Key", typeof(Key))]
        public List<Key> Keys;
    }
}