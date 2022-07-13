using System;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Metadata;

[Serializable]
public class RelationshipField
{
    [XmlAttribute] public string Left { get; set; }
    [XmlAttribute] public string Right { get; set; }
    [XmlAttribute] public int LeftPosition { get; set; }
    [XmlAttribute] public int RightPosition { get; set; }
}