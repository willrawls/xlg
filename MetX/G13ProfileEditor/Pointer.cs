using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Pointer
    {
        [XmlAttribute] public string devicemodel;

        public Mode Mode;
    }
}