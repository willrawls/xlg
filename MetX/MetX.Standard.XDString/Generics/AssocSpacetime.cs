using System;
using System.Xml.Serialization;
using MetX.Standard.XDString.Interfaces;

namespace MetX.Standard.XDString.Generics;

public class AssocSpacetime : AssocSheet<DateTimeAssocType, AssocCube<AssocItem>, VectorAssocType>, IAssocItem
{
    [XmlAttribute] public string Key { get; set; }
    [XmlAttribute] public string Value { get; set; }
    [XmlAttribute] public string Name { get; set; }
    [XmlAttribute] public Guid ID { get; set; }
}