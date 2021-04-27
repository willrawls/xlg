using System;
using System.Collections.Generic;
using System.Xml.Serialization;
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

        [XmlArray(ElementName = "Tables")]
        [XmlArrayItem(typeof(Table), ElementName = "Table")]
        public List<Table> Tables;

        [XmlArray(ElementName = "Views")]
        [XmlArrayItem(typeof(View), ElementName = "View")]
        public List<View> Views;

        [XmlArray(ElementName = "StoredProcedures")]
        [XmlArrayItem(typeof(StoredProcedure), ElementName = "StoredProcedure")]
        public List<StoredProcedure> StoredProcedures;

    }

    [Serializable]
    public class View
    {
        public string Schema { get; set; }
        public string Name { get; set; }
        public string TSQL { get; set; }
    }
}