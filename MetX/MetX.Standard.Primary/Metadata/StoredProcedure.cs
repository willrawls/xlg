using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Metadata
{
    [Serializable]
    public class StoredProcedure
    {
        [XmlAttribute] public string StoredProcedureName;
        [XmlAttribute] public string Location;
        [XmlAttribute] public string MethodName;

        [XmlArray(ElementName = "Parameters")]
        [XmlArrayItem("Parameter", typeof(Parameter))]
        public List<Parameter> Parameters;

        [XmlAttribute] public string SchemaName;
        [XmlAttribute] public string Definition;
        [XmlAttribute] public string Body;
    }
}