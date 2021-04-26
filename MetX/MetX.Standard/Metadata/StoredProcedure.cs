using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class StoredProcedure
    {
        [XmlAttribute] public string Location;


        [XmlAttribute] public string MethodName;


        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Parameter",
            typeof(Parameter),
            Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public List<Parameter> Parameters;


        [XmlAttribute] public string StoredProcedureName;
    }
}