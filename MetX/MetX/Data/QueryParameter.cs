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
            this.ParameterName = parameterName;
            this.ParameterValue = value;
            this.Direction = ParameterDirection.Input;
        }

        public QueryParameter(string parameterName, object value, DbType dataType)
        {
            this.ParameterName = parameterName;
            this.ParameterValue = value;
            this.DataType = dataType;
            this.Direction = ParameterDirection.Input;
        }

        public QueryParameter(string parameterName, object value, ParameterDirection Direction)
        {
            this.ParameterName = parameterName;
            this.ParameterValue = value;
            this.Direction = Direction;
        }

        public QueryParameter(string parameterName, object value, DbType dataType, ParameterDirection Direction)
        {
            this.ParameterName = parameterName;
            this.ParameterValue = value;
            this.DataType = dataType;
            this.Direction = Direction;
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