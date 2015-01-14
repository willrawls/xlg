using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;

namespace MetX.Data
{

    
    public class DataProviderCollection : System.Configuration.Provider.ProviderCollection
    {
        
        public new DataProvider this[string name]
        {
            get { return (DataProvider)base[name]; }
        }

        
        /// <param name="provider">C#CD: </param>
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (!(provider is DataProvider))
                throw new ArgumentException
                    ("Invalid provider type", "provider");

            base.Add(provider);
        }
    }

    
    public abstract class DataProvider : ProviderBase
    {
        protected string connectionString;

        public abstract StoredProcedureResult GetStoredProcedureResult(QueryCommand cmd);
        public abstract IDataReader GetReader(QueryCommand cmd);
        
		public abstract DataSet ToDataSet(QueryCommand cmd);
		public virtual DataSet ToDataSet(string selectQueryText) { return ToDataSet(new QueryCommand(selectQueryText)); }
		public virtual DataSet ToDataSet(string selectQueryText, System.Web.Caching.Cache cache)
		{
			DataSet ds = null;
			string cacheKey = "DP" + selectQueryText.GetHashCode().ToString();
			string dsXml = (string)cache[cacheKey];
			if (dsXml == null)
			{
				ds = ToDataSet(selectQueryText);
				if (ds != null)
				{
					StringBuilder sb = new StringBuilder();
					using (StringWriter sw = new StringWriter(sb))
						ds.WriteXml(sw);
					cache.Add(cacheKey, sb.ToString(), null, DateTime.Now.AddMinutes(CacheTimeout), 
						System.Web.Caching.Cache.NoSlidingExpiration, 
						System.Web.Caching.CacheItemPriority.AboveNormal, null);
				}
			}
			else
			{
				ds = new DataSet();
				using (StringReader sr = new StringReader(dsXml))
					ds.ReadXml(sr);
			}
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count == 0)
                return null;
			return ds;
		}

		public abstract string ToXml(string tagName, string tagAttributes, string sql);
        public abstract object ExecuteScalar(QueryCommand cmd);
        public abstract int ExecuteQuery(QueryCommand cmd);
        public abstract TableSchema.Table GetTableSchema(string tableName);
        public abstract string[] GetSPList();
        public abstract string[] GetTableList();
        public abstract IDataReader GetSPParams(string spName);
        public abstract string GetForeignKeyTableName(string fkColumnName);
        public abstract DbType GetDbType(string dataType);
        public abstract IDbCommand GetCommand(QueryCommand qry);

        public bool KeepConnectionOpen;
        public int CommandTimeout = 30;
		public int CacheTimeout = 60;
        public IDbConnection LastConnection;
        public virtual IDbConnection GetConnection()
        {
            if (LastConnection == null || LastConnection.State != ConnectionState.Open)
                LastConnection = NewConnection();
            return LastConnection;
        }
        public abstract IDbConnection NewConnection();
        public virtual void CloseConnection()
        {
            if (LastConnection != null && LastConnection.State != ConnectionState.Closed)
            {
                LastConnection.Close();
                LastConnection.Dispose();
            }
            LastConnection = null;
        }

        public virtual string ValidIdentifier(string identifier)
        {
            if(identifier != null) // && identifier.IndexOf(" ") > 0)
                return "[" + identifier + "]";
            return identifier;
        }

        public virtual string TopStatement 
        {
            get
            {
                return " TOP ";
            }
        }

        public virtual string CommandSeparator 
        {
            get
            {
                return "; ";
            }
        }

        public virtual string SelectStatement( string top, int page, QueryType qType )
        {
            if (!string.IsNullOrEmpty(top) && qType == QueryType.Select)
                return "SELECT " + TopStatement + " " + top + " ";
            return "SELECT ";
        }

        public abstract string HandlePage(string query, int offset, int limit, QueryType qType);

        public DataRow ToDataRow(string sql)
        {
            DataRowCollection rows = ToDataRows(sql);
            if (rows == null || rows.Count == 0)
                return null;
            return rows[0];
        }

        public DataRowCollection ToDataRows(string sql)
        {
            DataSet ds = ToDataSet(new QueryCommand(sql));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0].Rows;
        }

		public int RetrieveSingleIntegerValue(string sql)
        {
            return Worker.nzInteger(ExecuteScalar(new QueryCommand(sql)));
        }
        public object ExecuteScalar(string sql)
        {
            return ExecuteScalar(new QueryCommand(sql));
        }

        /// <summary>Converts a SQL statement into a series of elements via SQLXML. If a "FOR XML" phrase is not found "FOR XML AUTO" is added to the SQL</summary>
        /// <param name="sql">The SQL to convert to an xml string</param>
        /// <returns>The xml string attribute based representation of the SQL statement</returns>
        public string ToXml(string sql)
        {
            return ToXml(null, null, sql);
        }
/*
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            ////get the connection string from the web.config
            //string connectionStringName = config["connectionStringName"].ToString();
            
            //if (DataService.Config == null)
            //    this.connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            //else
            //    this.connectionString = DataService.Config.ConnectionStrings.ConnectionStrings[connectionStringName].ConnectionString;
        }*/

    }


}
