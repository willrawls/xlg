using System.Xml.Serialization;

namespace MetX.Standard.Test.TestingClasses;

[Serializable]
public class GeorgeItem
{
    [XmlAttribute]
    public string ItemName { get; set; }
}
