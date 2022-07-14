using System.Xml.Serialization;

namespace MetX.Standard.XDString.Generics;

public class VectorAssocType : AssocItem
{
    [XmlAttribute] public decimal Speed { get; set; }
    [XmlAttribute] public decimal Spin { get; set; }
    [XmlAttribute] public decimal Arc { get; set; }
    [XmlAttribute] public decimal Deformation { get; set; }

}