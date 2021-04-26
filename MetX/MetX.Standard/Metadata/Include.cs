using System;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class Include
    {
        [XmlAttribute] public string Name;
    }
}