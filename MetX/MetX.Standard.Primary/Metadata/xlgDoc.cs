using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class Tables
    {
        [XmlElement("Table", Form = XmlSchemaForm.Unqualified)]
        public List<Table> Table;

        [XmlElement("Include")] public List<Include> Include;
    }


    [Serializable]
    public class Table
    {
        [XmlAttribute] public string ClassName;


        [XmlArrayItem("Column", typeof(Column),
            Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public List<List<Column>> Columns;


        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Index", typeof(Index),
            Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public List<List<Index>> Indexes;


        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Key", typeof(Key),
            Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public List<List<Key>> Keys;


        [XmlAttribute] public string PrimaryKeyColumnName;


        [XmlAttribute] public string RowCount;


        [XmlAttribute] public string TableName;
    }


    [Serializable]
    public class Column
    {
        [XmlAttribute] public string AuditField;


        [XmlAttribute] public string AutoIncrement;


        [XmlAttribute] public string ColumnName;


        [XmlAttribute] public string CovertToPart;


        [XmlAttribute] public string CSharpVariableType;


        [XmlAttribute] public string DbType;


        [XmlAttribute] public string Description;


        [XmlAttribute] public string DomainName;


        [XmlAttribute] public string IsDotNetObject;


        [XmlAttribute] public string IsForeignKey;


        [XmlAttribute] public string IsIdentity;


        [XmlAttribute] public string IsIndexed;


        [XmlAttribute] public string IsNullable;


        [XmlAttribute] public string IsPrimaryKey;


        [XmlAttribute] public string Location;


        [XmlAttribute] public string MaxLength;


        [XmlAttribute] public string Precision;


        [XmlAttribute] public string PropertyName;


        [XmlAttribute] public string Scale;


        [XmlAttribute] public string SourceType;


        [XmlAttribute] public string VBVariableType;
    }


    [Serializable]
    public class Key
    {
        [XmlArrayItem("Column", typeof(Column),
            Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public List<Column> Columns;


        [XmlAttribute] public string IsPrimary;


        [XmlAttribute] public string Location;


        [XmlAttribute] public string Name;
    }


    [Serializable]
    public class Index
    {
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("IndexColumn",
            typeof(IndexColumn), Form = XmlSchemaForm.Unqualified,
            IsNullable = false)]
        public List<List<IndexColumn>> IndexColumns;


        [XmlAttribute] public string IndexName;


        [XmlAttribute] public string IsClustered;


        [XmlAttribute] public string Location;


        [XmlAttribute] public string PropertyName;


        [XmlAttribute] public string SingleColumnIndex;
    }


    [Serializable]
    public class IndexColumn
    {
        [XmlAttribute] public string IndexColumnName;


        [XmlAttribute] public string Location;


        [XmlAttribute] public string PropertyName;
    }


    [Serializable]
    public class Include
    {
        [XmlAttribute] public string Name;
    }


    [Serializable]
    public class Columns
    {
        [XmlElement("Column", Form = XmlSchemaForm.Unqualified)]
        public List<Column> Column;
    }


    [Serializable]
    public class StoredProcedures
    {
        [XmlAttribute] public string ClassName;


        [XmlElement("StoredProcedure",
            Form = XmlSchemaForm.Unqualified)]
        public List<StoredProcedure> StoredProcedure;

        [XmlElement("Include")] public List<Include> Include;
        [XmlElement("Exclude")] public List<Exclude> Exclude;
    }


    [Serializable]
    public class StoredProcedure
    {
        [XmlAttribute] public string Location;


        [XmlAttribute] public string MethodName;


        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Parameter",
            typeof(Parameter),
            Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public List<List<Parameter>> Parameters;


        [XmlAttribute] public string StoredProcedureName;
    }


    [Serializable]
    public class Parameter
    {
        [XmlAttribute] public string CovertToPart;


        [XmlAttribute] public string CSharpVariableType;


        [XmlAttribute] public string DataType;


        [XmlAttribute] public string IsDotNetObject;


        [XmlAttribute] public string IsInput;


        [XmlAttribute] public string IsOutput;


        [XmlAttribute] public string Location;


        [XmlAttribute] public string ParameterName;


        [XmlAttribute] public string VariableName;


        [XmlAttribute] public string VBVariableType;
    }


    [Serializable]
    public class Exclude
    {
        [XmlAttribute] public string Name;
    }


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


    [Serializable]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class xlgDoc
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


        [XmlElement("StoredProcedures")] public List<StoredProcedures> StoredProcedures;
        [XmlElement("XslEndpoints")] public List<XslEndpoints> XslEndpoints;

        [XmlElement("Render", Form = XmlSchemaForm.Unqualified)]
        public List<xlgDocRender> Render;


        [XmlElement("Tables")] public List<Tables> Tables;


        [XmlAttribute] public string VDirName;


        [XmlAttribute] public string XlgInstanceID;


        public static xlgDoc LoadFile(string filename)
        {
            var serializer = new XmlSerializer(typeof(xlgDoc));
            var doc = new xlgDoc();
            return doc;
        }
    }


    [Serializable]
    public class xlgDocRender
    {
        [XmlElement("Xsls", Form = XmlSchemaForm.Unqualified)]
        public List<xlgDocRenderXsls> Xsls;

        [XmlElement("Tables")] public List<Tables> Tables;
        [XmlElement("StoredProcedures")] public List<StoredProcedures> StoredProcedures;
    }


    [Serializable]
    public class xlgDocRenderXsls
    {
        [XmlElement("Include")] public List<Include> Include;
        [XmlElement("Exclude")] public List<Exclude> Exclude;


        [XmlAttribute] public string Path;


        [XmlAttribute] public string UrlExtension;
    }


    [Serializable]
    public class XlgDocumentDataSet
    {
        [XmlElement("Columns", typeof(Columns))]
        [XmlElement("Exclude", typeof(Exclude))]
        [XmlElement("Include", typeof(Include))]
        [XmlElement("StoredProcedures", typeof(StoredProcedures))]
        [XmlElement("Tables", typeof(Tables))]
        [XmlElement("XslEndpoints", typeof(XslEndpoints))]
        [XmlElement("xlgDoc", typeof(xlgDoc))]
        public List<object> Items;
    }
}

internal class NewDataSet
{
}