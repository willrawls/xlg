using System;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
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
}