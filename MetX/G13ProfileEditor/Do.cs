using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Do
    {
        [XmlAttribute] public string task;
    }
}