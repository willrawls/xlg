using System;
using System.Xml.Serialization;
using MetX.Standard.Strings.Generics;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Console.Tests.Standard.Strings;

public class AssocSpace : AssocSheet<AssocTime>, IAssocItem
{
    [XmlAttribute] public string Key { get; set;  }
    [XmlAttribute] public string Value { get; set; }
    [XmlAttribute] public string Name { get; set; }
    [XmlAttribute] public Guid ID { get; set; }
    [XmlAttribute] public int Number { get; set; }
    [XmlAttribute] public string Category { get; set; }

    public AssocSpace() : base()
    {
    }
}