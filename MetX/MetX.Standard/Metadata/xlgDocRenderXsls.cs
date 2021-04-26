using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class xlgDocRenderXsls
    {
        [XmlAttribute] public string Path;
        [XmlAttribute] public string UrlExtension;

        public List<Include> Include;
        public List<Exclude> Exclude;
    }
}