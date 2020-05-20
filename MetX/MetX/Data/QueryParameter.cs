using System;
using System.Data;
using System.Xml.Serialization;
using MetX.Library;

namespace MetX.Data
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
            this.Direction = direction;
        }

        public QueryParameter(string parameterName, object value, DbType dataType, ParameterDirection direction)
        {
            ParameterName = parameterName;
            ParameterValue = value;
            DataType = dataType;
            this.Direction = direction;
        }

        [XmlAttribute]
        public string Value 
        { 
            get 
            {
                if (ParameterValue == null)
                    return "(DbNull.Value)";
                return ParameterValue.AsString(); 
            }
            set
            {
                if (value == "(DbNull.Value)")
                    ParameterValue = null;
                else
                    ParameterValue = value;
            }
        }
    }
}