using System.Xml.Serialization;
using MetX.Standard.XDString;

namespace MetX.Console.Tests.Standard.XDString;

public class GeorgeItem
{
    [XmlAttribute]
    public string ItemName {get; set; }
}
