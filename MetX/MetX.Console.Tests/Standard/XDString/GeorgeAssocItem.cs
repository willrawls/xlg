﻿using System.Xml.Serialization;
using MetX.Standard.XDString;

namespace MetX.Console.Tests.Standard.XDString;

public class GeorgeAssocItem : AssocItem
{
    [XmlAttribute]
    public string GeorgeAssocItemName {get; set; }

    public GeorgeAssocItem()
    {
        Key = "George";
    }
}