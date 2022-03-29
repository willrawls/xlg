using System;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Metadata
{
    [Serializable]
    public class Parameter
    {
        [XmlAttribute] public string CovertToPart;
        [XmlAttribute] public string CSharpVariableType;
        [XmlAttribute] public string DataType;
        [XmlAttribute] public string IsDotNetObject;
        [XmlAttribute] public string IsInput;
        [XmlAttribute] public string IsOutput;
        [XmlAttribute] public string Location;
        [XmlAttribute] public string ParameterName;
        [XmlAttribute] public string VariableName;
        [XmlAttribute] public string VBVariableType;
    }
}