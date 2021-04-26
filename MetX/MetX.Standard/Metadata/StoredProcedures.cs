using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class StoredProcedures
    {
        [XmlAttribute] public string ClassName;


        [XmlElement("StoredProcedure",
            Form = XmlSchemaForm.Unqualified)]
        public List<StoredProcedure> StoredProcedure;

        [XmlElement("Include")] public List<Include> Include;
        [XmlElement("Exclude")] public List<Exclude> Exclude;
    }
}