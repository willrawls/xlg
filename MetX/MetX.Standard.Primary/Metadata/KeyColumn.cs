using System;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Metadata
{
    [Serializable]
    public class KeyColumn
    {
        [XmlAttribute] public string Column;
        [XmlAttribute] public string Location;
    }
}