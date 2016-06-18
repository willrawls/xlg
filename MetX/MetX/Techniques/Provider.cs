using System;
using System.Xml.Serialization;

namespace MetX.Techniques
{
    [Serializable]
    public class Provider : Particle
    {
        [XmlAttribute]
        public string Filename;
        [XmlAttribute]
        public string Class;
    }
}