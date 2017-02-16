using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Target
    {
        [XmlAttribute] public string path;

        public Target()
        {
        }
    }
}