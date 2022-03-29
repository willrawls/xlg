using System;
using System.Xml.Serialization;
using MetX.Standard.Primary.Techniques;

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