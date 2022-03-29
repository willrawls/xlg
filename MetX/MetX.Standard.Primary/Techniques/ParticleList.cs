using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Techniques
{
    [Serializable]
    public class ParticleList<T> : List<T> where T : Particle, new()
    {
        [XmlAttribute]
        public Guid Id;
        [XmlAttribute]
        public string Name;

        [XmlIgnore]
        public ParticleList<T> Parent { get; set; }

        public ParticleList()
        {
        }

        public ParticleList(ParticleList<T> parent)
        {
            Parent = parent;
        }

        public ParticleList(ParticleList<T> parent, Guid id)
        {
            Id = id;
            Parent = parent;
        }
    }
}