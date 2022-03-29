using System;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Techniques
{
    [Serializable]
    public class Variable : Particle
    {
        [XmlAttribute]
        public string Value;
    }
}