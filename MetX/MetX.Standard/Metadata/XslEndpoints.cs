using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class XslEndpoints
    {
        [XmlAttribute] public string Folder;


        [XmlAttribute] public string Path;


        [XmlAttribute] public string VirtualDir;


        [XmlAttribute] public string VirtualPath;


        [XmlAttribute] public string xlgPath;


        [XmlElement("XslEndpoints")] public List<XslEndpoints> XslEndpoints1;
    }
}