using System;
using System.Data;
using System.Text;
using System.Xml.Serialization;

namespace MetX.Standard.Data
{
    [Serializable]
    public class QueryCommand
    {
        [XmlAttribute]
        public CommandType CommandType;
        [XmlAttribute]
        public string CommandSql;
        [XmlArray(ElementName = "Parameters"), XmlArrayItem(ElementName = "Parameter")]
        public QueryParameterCollection Parameters;

        public QueryCommand() { }

        public QueryCommand(string sql)
        {
            CommandSql = sql;
            CommandType = CommandType.Text;
            Parameters = new QueryParameterCollection();
        }

        public void AddParameter(string parameterName, object parameterValue, DbType dataType, ParameterDirection direction)
        {
            if (Parameters == null)
                Parameters = new QueryParameterCollection();
            Parameters.Add(new QueryParameter(parameterName, parameterValue, dataType, direction));
        }

        public void AddParameter(string parameterName, object parameterValue, ParameterDirection direction)
        {
            if (Parameters == null)
                Parameters = new QueryParameterCollection();
            Parameters.Add(new QueryParameter(parameterName, parameterValue, direction));
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

        public string ToXml()
        {
            var xs = new XmlSerializer(typeof(QueryCommand));
            var sb = new StringBuilder();
            var settings = new System.Xml.XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            var xw = System.Xml.XmlWriter.Create(sb, settings);
            xs.Serialize(xw, this);
            sb.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ", string.Empty);
            return sb.ToString();
        }

        public static QueryCommand FromXml(ref string xmlText)
        {
            var xs = new XmlSerializer(typeof(QueryCommand));
            return (QueryCommand)xs.Deserialize(new System.IO.StringReader(xmlText));
        }
    }
}