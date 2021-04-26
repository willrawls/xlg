using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class Columns
    {
        [XmlElement("Column", Form = XmlSchemaForm.Unqualified)]
        public List<Column> Column;
    }
}