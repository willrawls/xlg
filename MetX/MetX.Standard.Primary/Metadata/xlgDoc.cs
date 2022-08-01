using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.ML;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

// ReSharper disable InconsistentNaming

namespace MetX.Standard.Primary.Metadata
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

        [XmlArray(ElementName = "Relationships")]
        [XmlArrayItem(typeof(Relationship), ElementName = "Relationship")]
        public List<Relationship> Relationships { get; set; }

        public static xlgDoc Empty(string outputFilePath = "")
        {
            var name = outputFilePath.LastPathToken().FirstToken(".");
            var result = new xlgDoc
            {
                StoredProcedures = new List<StoredProcedure>(),
                Tables = new List<Table>(),
                Views = new List<View>(),
                Relationships = new List<Relationship>(),
                XlgInstanceID = Guid.NewGuid().ToString("N"),
                ConnectionStringName = outputFilePath.IsEmpty() 
                    ? "Initial Catalog=YourDatabase;Server=YourServer;"
                    : $"Initial Catalog={name};Server=YourServer;",
                Now = DateTime.Now.ToString("s"),
                DatabaseProvider = "System.Data.SqlClient",
                Namespace = "MetX.Xlg.YourNamespace",
                OutputFolder = outputFilePath.IsEmpty()
                    ? "C:\\SomeOutputFolder"
                    : outputFilePath.TokensBeforeLast(@"\"),
                ProviderName = "System.Data.SqlClient",
            };

            return result;
        }

        public xlgDoc MakeViable()
        {
            StoredProcedures ??= new List<StoredProcedure>();
            Tables ??= new List<Table>();
            Views ??= new List<View>();
            Relationships ??= new List<Relationship>();
            if(XlgInstanceID.IsEmpty()) XlgInstanceID = Guid.NewGuid().ToString("N");
            Now ??= DateTime.Now.ToString("s");
            return this;
        }
    }
}