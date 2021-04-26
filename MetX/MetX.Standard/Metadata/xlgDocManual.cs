using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MetX.Five.Metadata;
using MetX.Standard.Library;

namespace MetX.Standard.Metadata
{
    [Serializable]
    public class xlgDoc : SerializesToXml<xlgDoc>
    {
        [XmlAttribute] public string ConnectionStringName;
        [XmlAttribute] public string DatabaseProvider;
        [XmlAttribute] public string IncludeNamespace;
        [XmlAttribute] public string MetXAssemblyString;
        [XmlAttribute] public string MetXObjectName;
        [XmlAttribute] public string MetXProviderAssemblyString;
        [XmlAttribute] public string Namespace;
        [XmlAttribute] public string Now;
        [XmlAttribute] public string OutputFolder;
        [XmlAttribute] public string ProviderAssemblyString;
        [XmlAttribute] public string ProviderName;
        [XmlAttribute] public string VDirName;
        [XmlAttribute] public string XlgInstanceID;

        public List<StoredProcedures> StoredProcedures;
        public List<XslEndpoints> XslEndpoints;
        public List<xlgDocRender> Render;
        public List<Tables> Tables;
    }
}