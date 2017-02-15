using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Assignments
    {
        [XmlAttribute] public string devicecategory;
        [XmlArrayItem("assignment")]
        public List<Assignment> Assignment;
    }
}