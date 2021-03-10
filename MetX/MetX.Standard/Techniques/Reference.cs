using System;
using System.Xml.Serialization;

namespace MetX.Techniques
{
    [Serializable]
    public class Reference : Particle
    {
        [XmlAttribute]
        public string Content;
        [XmlAttribute]
        public string Value;
    }
}