using System.Xml.Serialization;
using MetX.Standard.Strings;

namespace MetX.Console.Tests.Standard.XDString.TestingClasses;

public class GeorgeAssocItem : TimeTrackingAssocItem
{
    [XmlAttribute]
    public string GeorgeAssocItemName {get; set; }

    public GeorgeAssocItem()
    {
        Key = "George1DArray";
    }
}