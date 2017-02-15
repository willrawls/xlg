using IntegenX.RapidLink.Services.Common.Library;
using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    [XmlRoot("profiles")]
    public class Profiles : SerializesToXml<Profiles>
    {
        [XmlAttribute]
        public string xmlns = "http://www.logitech.com/Cassandra/2010.7/Profile";
        [XmlElement("profile")]
        public Profile Profile;
    }
}