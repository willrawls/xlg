using System;
using System.Xml.Serialization;

namespace MetX.Techniques
{
    [Serializable]
    public class Variable : Particle
    {
        [XmlAttribute]
        public string Value;
    }
}