using System.Xml.Serialization;

namespace MetX.Console.Tests.Standard.XDString;

public class GeorgeItem
{
    [XmlAttribute]
    public string ItemName {get; set; }
}