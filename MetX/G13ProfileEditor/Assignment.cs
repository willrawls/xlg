using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Assignment
    {
        [XmlAttribute] public bool backup;
        [XmlAttribute] public bool original;
        [XmlAttribute] public string contextid;
        [XmlAttribute] public Guid macroguid;

        [XmlAttribute]
        public int shiftstate;
    }
}