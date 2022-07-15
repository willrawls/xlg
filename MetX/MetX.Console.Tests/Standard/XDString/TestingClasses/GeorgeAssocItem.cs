using System.Xml.Serialization;
using MetX.Standard.XDString;

namespace MetX.Console.Tests.Standard.XDString.TestingClasses;

public class GeorgeAssocItem : AssocItem
{
    [XmlAttribute]
    public string GeorgeAssocItemName {get; set; }

    public GeorgeAssocItem()
    {
        Key = "George1DArray";
    }
}