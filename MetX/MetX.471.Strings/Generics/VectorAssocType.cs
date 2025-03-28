﻿using System.Xml.Serialization;

namespace MetX._471.Strings.Generics;

public class VectorAssocType : BasicAssocItem
{
    [XmlAttribute] public decimal Speed { get; set; }
    [XmlAttribute] public decimal Spin { get; set; }
    [XmlAttribute] public decimal Arc { get; set; }
    [XmlAttribute] public decimal Deformation { get; set; }

}