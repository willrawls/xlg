using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Mode
    {
        [XmlAttribute] public string color;
        [XmlAttribute] public int shiftstate;

        public ReportRate ReportRate;
        public PowerMode PowerMode;
        public DpiTable DpiTable;
        public Movement Movement;
    }
}