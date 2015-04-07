using System;
using System.Xml.Serialization;

namespace MetX.Data.Techniques
{
    [Serializable]
    public class Reference : Particle
    {
        [XmlAttribute]
        public string Content;
        [XmlAttribute]
        public string Value;
    }

    public class Particle
    {
        [XmlAttribute]
        public Guid Id;
        [XmlAttribute]
        public int Index;
        [XmlAttribute]
        public string Name;
        [XmlIgnore]
        public ParticleList Parent;
        [XmlAttribute]
        public ParticleType Quality;
    }
}