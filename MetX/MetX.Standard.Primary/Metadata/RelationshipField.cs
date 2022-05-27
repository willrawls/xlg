using System;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Metadata;

[Serializable]
public class RelationshipField
{
    [XmlAttribute] public string Local { get; set; }
    [XmlAttribute] public string Foreign { get; set; }
}