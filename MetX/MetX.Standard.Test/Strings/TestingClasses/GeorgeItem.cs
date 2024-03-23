using System;
using System.Xml.Serialization;

namespace MetX.Console.Tests.Standard.Strings.TestingClasses;

[Serializable]
public class GeorgeItem
{
    [XmlAttribute]
    public string ItemName {get; set; }
}
