using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Metadata;

[Serializable]
public class Relationship
{
    [XmlAttribute] public string Name { get; set; }
    [XmlAttribute] public string Type { get; set; }
    
    [XmlAttribute] public string LeftSchema { get; set; }
    [XmlAttribute] public string LeftTable { get; set; }

    [XmlAttribute] public string RightTable { get; set; }
    [XmlAttribute] public string RightSchema { get; set; }

    [XmlArray(ElementName = "Fields")]
    [XmlArrayItem(typeof(RelationshipField), ElementName = "Field")]
    public List<RelationshipField> Fields { get; set; }

    [XmlArray(ElementName = "Tags")]
    [XmlArrayItem(typeof(string), ElementName = "Tag")]
    public List<string> Tags { get; set; }

    [XmlAttribute] public string RightKey { get; set; }
}