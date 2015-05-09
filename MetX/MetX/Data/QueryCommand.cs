using System;
using System.Data;
using System.Text;
using System.Xml.Serialization;

namespace MetX.Data
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
            System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(sb, settings);
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