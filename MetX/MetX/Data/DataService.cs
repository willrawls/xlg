using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using System.Configuration;
using System.Web.Configuration;
using System.Data.Common;
using System.Data;
using System.Reflection;

using MetX.IO;

namespace MetX.Data
{
    
    public class DataService
    {
        #region Provider-specific bits

        public DataProvider Provider;
        public GatherProvider Gatherer;

        public ProviderTypeEnum ProviderType;

        public ConnectionStringSettings Settings;
        public string ProviderAssemblyString;
        public string MetXProviderAssemblyString;
        public string MetXObjectName;

        public static DataService Instance;
        public static ConnectionStringSettingsCollection ConnectionStrings;
        private static Dictionary<string, DataService> Services = new Dictionary<string, DataService>();
        private static Dictionary<string, IProvide> IProviders = new Dictionary<string, IProvide>();

        public static DataProvider GetProvider(string ConnectionStringName)
        {
            return GetDataService(ConnectionStringName).Provider;
        }

        public DataService(ConnectionStringSettings ConnectionSettings)
        {
            Settings = ConnectionSettings;
            Setup();
        }

        public void Setup()
        {
            IProvide MetXProvider = null;
            if (!IProviders.ContainsKey(Settings.ProviderName.ToLower()))
            {
                MetXObjectName = "MetX.Data.Factory." + Settings.ProviderName.Replace(".", "_");
                Assembly MetXProviderAssembly = Assembly.Load(MetXObjectName);
                Type MetXProviderType = MetXProviderAssembly.GetType(MetXObjectName, true);
                MetXProvider = System.Activator.CreateInstance(MetXProviderType, true) as IProvide;
                IProviders.Add(Settings.ProviderName.ToLower(), MetXProvider);
                MetXProviderAssemblyString = MetXProvider.ProviderAssemblyString;
                ProviderAssemblyString = MetXProviderAssembly.FullName;
            }
            else
                MetXProvider = IProviders[Settings.ProviderName.ToLower()];

            ProviderType = MetXProvider.ProviderType;
            if (MetXProvider.ProviderType == ProviderTypeEnum.Data || MetXProvider.ProviderType == ProviderTypeEnum.DataAndGather)
            {
                Provider = MetXProvider.GetNewDataProvider(Settings.ConnectionString);
                Provider.Initialize(MetXProvider.ProviderName, new System.Collections.Specialized.NameValueCollection());

                if (Provider == null)
                    throw new ProviderException("Unknown provider: " + Settings.ProviderName);
            }
            if (MetXProvider.ProviderType == ProviderTypeEnum.Gather || MetXProvider.ProviderType == ProviderTypeEnum.DataAndGather)
            {
                Gatherer = MetXProvider.GetNewGatherProvider();
                if (Gatherer == null)
                    throw new ProviderException("Unknown gatherer: " + Settings.ProviderName);
            }

            Services.Add(Settings.Name, this);
            if (Instance == null)
                Instance = this;

            /*
            switch(Settings.ProviderName)
            {
                case "System.Data.SqlClient":
                    Provider = new SqlDataProvider(Settings.ConnectionString);
                    Provider.Initialize("SqlDataProvider", new System.Collections.Specialized.NameValueCollection());
                    break;
                //case "MySql.Data.MySqlClient":
                //    Provider = new MySqlDataProvider(Settings.ConnectionString);
                //    Provider.Initialize("MySqlDataProvider", new System.Collections.Specialized.NameValueCollection());                    
                //    break;
                case "Sybase.Data.AseClient":
                    Provider = new SybaseDataProvider(Settings.ConnectionString);
                    Provider.Initialize("SybaseDataProvider", new System.Collections.Specialized.NameValueCollection());                    
                    break;
                //case "MetX.Data.FWTClient":
                //    Provider = new MetX.Data.FWTDataProvider(Settings.ConnectionString);
                //    Provider.Initialize("FWTDataProvider", new System.Collections.Specialized.NameValueCollection());
                //    break;
            }
            */
        }

        public static void ClearDataServiceCache()
        {
            if (Services == null)
                Services = new Dictionary<string, DataService>();
            Services.Clear();
        }

        /// <summary>
        /// Typically called from DAL code.
        /// </summary>
        /// <param name="ConnectionStringName"></param>
        /// <returns></returns>
        public static DataService GetDataService(string ConnectionStringName)
        {
            if (Services.ContainsKey(ConnectionStringName))
                return Services[ConnectionStringName];

            if (ConnectionStrings == null)
                ConnectionStrings = WebConfigurationManager.ConnectionStrings;

            DataService ret = new DataService();
            ret.Settings = ConnectionStrings[ConnectionStringName];
            ret.Setup();
            return ret;
        }

        /// <summary>
        /// Typically used to generate code when an app.config or web.config isn't available or desired.
        /// </summary>
        /// <param name="ConnectionStringName"></param>
        /// <param name="ConnectionString"></param>
        /// <param name="ProviderName"></param>
        /// <returns></returns>
        public static DataService GetDataServiceManually(string ConnectionStringName, string ConnectionString, string ProviderName)
        {
            if (Services.ContainsKey(ConnectionStringName))
                return Services[ConnectionStringName];

            return new DataService(new ConnectionStringSettings(ConnectionStringName, ConnectionString, ProviderName));
        }

        public DataService()
        {
        }

        ///// <summary>
        ///// Not meant to be used publically except to allow static readonly instancing
        ///// </summary>
        ///// <param name="ConnectionStringName"></param>
        //public DataService(string ConnectionStringName)
        //{
        //    DataService temp = GetDataService(ConnectionStringName);
        //    Settings = temp.Settings;
        //    Provider = temp.Provider;
        //}
        #endregion

        /// <summary>
        /// Returns an IDataReader using the passed-in command
        /// </summary>
        /// <param name="cmd">C#CD: </param>
        /// <returns>IDataReader</returns>
        public IDataReader GetReader(QueryCommand cmd)
        {
            return Provider.GetReader(cmd);
        }

        /// <summary>
        /// Returns an IDataReader using the passed-in command
        /// </summary>
        /// <param name="cmd">C#CD: </param>
        /// <returns>IDataReader</returns>
        public StoredProcedureResult GetStoredProcedureResult(QueryCommand cmd)
        {
            return Provider.GetStoredProcedureResult(cmd);
        }

        /// <summary>
        /// Returns a DataSet based on the passed-in command
        /// </summary>
        /// <param name="cmd">C#CD: </param>
        /// <returns>C#CD: </returns>
        public DataSet ToDataSet(QueryCommand cmd)
        {
            return Provider.ToDataSet(cmd);

        }
        /// <summary>
        /// Returns a scalar object based on the passed-in command
        /// </summary>
        /// <param name="cmd">C#CD: </param>
        /// <returns>C#CD: </returns>
        public object ExecuteScalar(QueryCommand cmd)
        {
            return Provider.ExecuteScalar(cmd);
        }
        /// <summary>
        /// Executes a pass-through query on the DB
        /// </summary>
        /// <param name="cmd">C#CD: </param>
        /// <returns>C#CD: </returns>
        public int ExecuteQuery(QueryCommand cmd)
        {
            return Provider.ExecuteQuery(cmd);
        }

        
        /// <param name="tableName">C#CD: </param>
        /// <returns>C#CD: </returns>
        public TableSchema.Table GetTableSchema(string tableName)
        {
            return Provider.GetTableSchema(tableName);
        }

        
        /// <returns>C#CD: Returns a sorted list of tables</returns>
        public string[] GetTables()
        {
            return Provider.GetTableList();
        }
        
        /// <returns>C#CD: </returns>
        public string[] GetSPList()
        {
            return Provider.GetSPList();
        }
        
        /// <param name="fkColumn">C#CD: </param>
        /// <returns>C#CD: </returns>
        public string GetForeignKeyTableName(string fkColumn)
        {
            return Provider.GetForeignKeyTableName(fkColumn);
        }
        
        /// <param name="spName">C#CD: </param>
        /// <returns>C#CD: </returns>
        public IDataReader GetSPParams(string spName)
        {
            return Provider.GetSPParams(spName);
        }
        
        /// <param name="dataType">C#CD: </param>
        /// <returns>C#CD: </returns>
        public DbType GetDbType(string dataType)
        {
            return Provider.GetDbType(dataType);
        }

        
        /// <returns>C#CD: </returns>
        public string GetClientType()
        {
            return Provider.GetType().Name;
        }

        
        /// <param name="qry">C#CD: </param>
        /// <returns>C#CD: </returns>
        internal IDbCommand GetIDbCommand(QueryCommand qry)
        {
            return Provider.GetCommand(qry);
        }

    }
}
