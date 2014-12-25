using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Xml.Serialization;

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
                return Worker.nzString(ParameterValue); 
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

    public class QueryParameterCollection : List<QueryParameter> { }
    public class QueryCommandCollection : List<QueryCommand> { }

    [Serializable]
    public class QueryCommand
    {
        [XmlAttribute] public CommandType CommandType;
        [XmlAttribute] public string CommandSql;
        [XmlArray(ElementName="Parameters"), XmlArrayItem(ElementName="Parameter")]
            public QueryParameterCollection Parameters;

        public QueryCommand() { }

        public QueryCommand(string sql)
        {
            this.CommandSql = sql;
            this.CommandType = CommandType.Text;
            this.Parameters = new QueryParameterCollection();
        }

        public void AddParameter(string parameterName, object parameterValue, DbType dataType, ParameterDirection Direction)
        {
            if (Parameters == null)
                Parameters = new QueryParameterCollection();
            Parameters.Add(new QueryParameter(parameterName, parameterValue, dataType, Direction));
        }
        public void AddParameter(string parameterName, object parameterValue, ParameterDirection Direction)
        {
            if (Parameters == null)
                Parameters = new QueryParameterCollection();
            Parameters.Add(new QueryParameter(parameterName, parameterValue, Direction));
        }

        public void AddParameter(string parameterName, object parameterValue, DbType dataType)
        {
            if (Parameters == null)
                Parameters = new QueryParameterCollection();
            Parameters.Add(new QueryParameter(parameterName, parameterValue, dataType));
        }
        public void AddParameter(string parameterName, object parameterValue)
        {
            if (Parameters == null)
                Parameters = new QueryParameterCollection();
            Parameters.Add(new QueryParameter(parameterName, parameterValue));
        }
        //public IDbCommand ToIDbCommand() { return DataService.Instance.GetIDbCommand(this); }

        public string ToXML()
        {
            XmlSerializer xs = new XmlSerializer(typeof(QueryCommand));
            StringBuilder sb = new StringBuilder();
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            System.Xml.XmlWriter xw = System.Xml.XmlTextWriter.Create(sb, settings);
            xs.Serialize(xw, this);
            sb.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ", string.Empty);
            return sb.ToString();
        }

        public static QueryCommand FromXML(ref string xmlText)
        {
            XmlSerializer xs = new XmlSerializer(typeof(QueryCommand));
            return (QueryCommand)xs.Deserialize(new System.IO.StringReader(xmlText));
        }
    }
}
