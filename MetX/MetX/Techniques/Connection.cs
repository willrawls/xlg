using System;
using System.Xml.Serialization;

namespace MetX.Techniques
{
    [Serializable]
    public class Connection : Particle
    {
        [XmlAttribute]
        public string ConnetionString;
        [XmlAttribute]
        public string Provider;
    }
}