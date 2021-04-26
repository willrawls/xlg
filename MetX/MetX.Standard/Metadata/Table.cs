using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class Table
    {
        [XmlAttribute] public string ClassName;

        [XmlArrayItem("Column", typeof(Column))]
        public List<Column> Columns;
        
        [XmlArrayItem("Index", typeof(Index))]
        public List<Index> Indexes;

        [XmlArrayItem("Key", typeof(Key))]
        public List<Key> Keys;


        [XmlAttribute] public string PrimaryKeyColumnName;


        [XmlAttribute] public string RowCount;


        [XmlAttribute] public string TableName;
    }
}