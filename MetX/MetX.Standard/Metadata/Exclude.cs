using System;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class Exclude
    {
        [XmlAttribute] public string Name;
    }
}