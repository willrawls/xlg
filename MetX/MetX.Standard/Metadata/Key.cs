using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class Key
    {
        [XmlArrayItem("Column", typeof(Column),
            Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public List<Column> Columns;


        [XmlAttribute] public string IsPrimary;


        [XmlAttribute] public string Location;


        [XmlAttribute] public string Name;
    }
}