using System;
using System.Xml.Serialization;

namespace MetX.Techniques
{
    [Serializable]
    public class FileParticle : Particle
    {
        [XmlAttribute] public int Size;
        [XmlAttribute] public DateTime Created;
        [XmlAttribute] public DateTime Modified;
        [XmlAttribute] public string Filename;
        [XmlAttribute] public string Content;
    }
}