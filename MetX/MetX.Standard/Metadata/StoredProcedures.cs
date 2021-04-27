using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MetX.Standard.Metadata
{
    [Serializable]
    public class StoredProcedures
    {
        [XmlAttribute] public string ClassName;

        [XmlArray("StoredProcedures")]
        [XmlArrayItem(typeof(StoredProcedure), ElementName = "StoredProcedure")]
        public List<StoredProcedure> StoredProcedure;
    }
}