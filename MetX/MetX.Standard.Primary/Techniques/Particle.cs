using System;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Techniques
{
    [Serializable]
    public class Particle
    {
        [XmlAttribute]
        public Guid Id;
        [XmlAttribute]
        public int Index;
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public ParticleType Quality;

        //        [XmlIgnore]
        //        public ParticleList Parent;
    }
}