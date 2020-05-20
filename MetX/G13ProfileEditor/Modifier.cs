using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Modifier : KdmBase
    {
        [XmlAttribute] public string value;
    }
}