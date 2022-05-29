using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data;
using System.IO;
using System.Text;
using MetX.Five.Glove.Data;
using MetX.Five.Glove.Gatherers;
using MetX.Standard.Library;
using MetX.Standard.Primary.Metadata;

namespace MetX.Five.Glove.Providers
{
    public abstract class DataProvider : ProviderBase
    {
        protected string ConnectionString;

        public abstract StoredProcedureResult GetStoredProcedureResult(QueryCommand cmd);

        public abstract IDataReader GetReader(QueryCommand cmd);

        public abstract DataSet ToDataSet(QueryCommand cmd);

        public DataSet ToDataSet(string selectQueryText) { return ToDataSet(new QueryCommand(selectQueryText)); }

        public DataSet ToDataSet(string selectQueryText, InMemoryCache<string> cache)
        {
            DataSet ds;
            var cacheKey = "DP" + selectQueryText.GetHashCode();
            var dsXml = cache[cacheKey];
            if (dsXml == null)
            {
                ds = ToDataSet(selectQueryText);
                if (ds != null)
                {
                    var sb = new StringBuilder();
                    using (var sw = new StringWriter(sb))
                        ds.WriteXml(sw);
                    cache[cacheKey] =  sb.ToString();
                }
            }
            else
            {
                ds = new DataSet();
                using var sr = new StringReader(dsXml);
                ds.ReadXml(sr);
            }
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;
            return ds;
        }

        public abstract string ToXml(string tagName, string tagAttributes, string sql);

        public abstract object ExecuteScalar(QueryCommand cmd);

        public abstract int ExecuteQuery(QueryCommand cmd);

        public abstract TableSchema.Table GetTableSchema(OwnerTablePair pair);

        public abstract StoredProcedure[] GetStoredProcedureList();
        public abstract List<OwnerTablePair> GetTableList();
        public abstract View[] GetViews();

        public abstract IDataReader GetSpParams(string spName);

        public abstract List<Relationship> GetRelationships();

        public abstract DbType GetDbType(string dataType);

        public abstract IDbCommand GetCommand(QueryCommand qry);

        public bool KeepConnectionOpen;
        public int CommandTimeout = 30;
        public int CacheTimeout = 60;
        public IDbConnection LastConnection;

        public IDbConnection GetConnection()
        {
            if (LastConnection == null || LastConnection.State != ConnectionState.Open)
                LastConnection = NewConnection();
            return LastConnection;
        }

        public abstract IDbConnection NewConnection();

        public void CloseConnection()
        {
            if (LastConnection != null && LastConnection.State != ConnectionState.Closed)
            {
                LastConnection.Close();
                LastConnection.Dispose();
            }
            LastConnection = null;
        }

        public string ValidIdentifier(string identifier)
        {
            if (identifier != null)
                return "[" + identifier + "]";
            return null;
        }

        public string TopStatement => " TOP ";

        public string CommandSeparator => "; ";

        public virtual string SelectStatement(string top, int page, QueryType qType)
        {
            if (!string.IsNullOrEmpty(top) && qType == QueryType.Select)
                return "SELECT " + TopStatement + " " + top + " ";
            return "SELECT ";
        }

        public abstract string HandlePage(string query, int offset, int limit, QueryType qType);

        public DataRow ToDataRow(string sql)
        {
            var rows = ToDataRows(sql);
            if (rows == null || rows.Count == 0)
                return null;
            return rows[0];
        }

        public DataRowCollection ToDataRows(string sql)
        {
            var ds = ToDataSet(new QueryCommand(sql));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0].Rows;
        }

        /// <summary>Converts a SQL statement into a series of elements via SQLXML. If a "FOR XML" phrase is not found "FOR XML AUTO" is added to the SQL</summary>
        /// <param name="sql">The SQL to convert to an xml string</param>
        /// <returns>The xml string attribute based representation of the SQL statement</returns>
        public string ToXml(string sql)
        {
            return ToXml(null, null, sql);
        }
    }
}