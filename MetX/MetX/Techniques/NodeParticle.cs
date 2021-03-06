using System;
using System.Xml.Serialization;

namespace MetX.Techniques
{
    [Serializable]
    public class NodeParticle : Particle
    {
        [XmlArray(ElementName = "Nodes")]
        [XmlArrayItem(typeof(NodeParticle), ElementName = "Node")]
        public ParticleList<NodeParticle> Nodes;
    }
}