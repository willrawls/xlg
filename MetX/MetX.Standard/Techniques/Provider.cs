using System;
using System.Xml.Serialization;

namespace MetX.Standard.Techniques
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