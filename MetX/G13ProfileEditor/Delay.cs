using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Delay : KdmBase
    {
        [XmlAttribute] public int milliseconds;
    }
}