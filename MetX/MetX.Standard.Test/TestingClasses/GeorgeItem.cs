using System.Xml.Serialization;
using MetX.Standard.Strings;

namespace MetX.Standard.Test.TestingClasses;

[Serializable]
public class GeorgeItem : BasicAssocItem
{
    [XmlAttribute]
    public string ItemName { get; set; }
}
