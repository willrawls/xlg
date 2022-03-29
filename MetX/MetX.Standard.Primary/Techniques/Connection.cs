using System;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Techniques
{
    [Serializable]
    public class Connection : Particle
    {
        [XmlAttribute]
        public string ConnectionString;
        [XmlAttribute]
        public string Provider;
    }
}