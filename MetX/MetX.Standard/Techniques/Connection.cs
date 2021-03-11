using System;
using System.Xml.Serialization;

namespace MetX.Standard.Techniques
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