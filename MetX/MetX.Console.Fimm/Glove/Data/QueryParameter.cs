using System;
using System.Data;
using System.Xml.Serialization;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

namespace MetX.Fimm.Glove.Data
{
    [Serializable]
    public class QueryParameter
    {
        [XmlAttribute("Name")] public string ParameterName;
        [XmlIgnore] public object ParameterValue;
        [XmlAttribute("Type")] public DbType DataType;
        [XmlAttribute("Direction")] public ParameterDirection Direction;

        public QueryParameter() { }
        public QueryParameter(string parameterName, object value)
        {
            ParameterName = parameterName;
            ParameterValue = value;
            Direction = ParameterDirection.Input;
        }

        public QueryParameter(string parameterName, object value, DbType dataType)
        {
            ParameterName = parameterName;
            ParameterValue = value;
            DataType = dataType;
            Direction = ParameterDirection.Input;
        }

        public QueryParameter(string parameterName, object value, ParameterDirection direction)
        {
            ParameterName = parameterName;
            ParameterValue = value;
            Direction = direction;
        }

        public QueryParameter(string parameterName, object value, DbType dataType, ParameterDirection direction)
        {
            ParameterName = parameterName;
            ParameterValue = value;
            DataType = dataType;
            Direction = direction;
        }

        [XmlAttribute]
        public string Value 
        { 
            get => ParameterValue == null ? "(DbNull.Value)" : ParameterValue.AsStringFromObject();
            set => ParameterValue = value == "(DbNull.Value)" ? null : value;
        }
    }
}