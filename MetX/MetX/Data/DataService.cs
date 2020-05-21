using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Configuration;
using System.Web.Configuration;
using System.Data;
using System.Reflection;
using MetX.Interfaces;

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
        private static Dictionary<string, DataService> _mServices = new Dictionary<string, DataService>();
        private static readonly Dictionary<string, IProvide> MProviders = new Dictionary<string, IProvide>();

        public DataService(ConnectionStringSettings connectionSettings)
        {
            Settings = connectionSettings;
            Setup();
        }

        public void Setup()
        {
            IProvide metXProvider;
            if (!MProviders.ContainsKey(Settings.ProviderName.ToLower()))
            {
                MetXObjectName = "MetX.Data.Factory." + Settings.ProviderName.Replace(".", "_");
                var metXProviderAssembly = Assembly.Load(MetXObjectName);
                var metXProviderType = metXProviderAssembly.GetType(MetXObjectName, true);
                metXProvider = Activator.CreateInstance(metXProviderType, true) as IProvide;
                if (metXProvider == null) throw new ProviderException("Unable to instantiate: " + metXProviderType.FullName);
                MProviders.Add(Settings.ProviderName.ToLower(), metXProvider);
                MetXProviderAssemblyString = metXProvider.ProviderAssemblyString;
                ProviderAssemblyString = metXProviderAssembly.FullName;
            }
            else
                metXProvider = MProviders[Settings.ProviderName.ToLower()];

            ProviderType = metXProvider.ProviderType;
            if (metXProvider.ProviderType == ProviderTypeEnum.Data || metXProvider.ProviderType == ProviderTypeEnum.DataAndGather)
            {
                Provider = metXProvider.GetNewDataProvider(Settings.ConnectionString);
                Provider.Initialize(metXProvider.ProviderName, new System.Collections.Specialized.NameValueCollection());

                if (Provider == null)
                    throw new ProviderException("Unknown provider: " + Settings.ProviderName);
            }
            if (metXProvider.ProviderType == ProviderTypeEnum.Gather || metXProvider.ProviderType == ProviderTypeEnum.DataAndGather)
            {
                Gatherer = metXProvider.GetNewGatherProvider();
                if (Gatherer == null)
                    throw new ProviderException("Unknown gatherer: " + Settings.ProviderName);
            }

            _mServices.Add(Settings.Name, this);
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

        /// <summary>
        /// Typically called from DAL code.
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        public static DataService GetDataService(string connectionStringName)
        {
            if (_mServices.ContainsKey(connectionStringName))
                return _mServices[connectionStringName];

            if (ConnectionStrings == null)
                ConnectionStrings = WebConfigurationManager.ConnectionStrings;

            var ret = new DataService();
            ret.Settings = ConnectionStrings[connectionStringName];
            if (ret.Settings == null) return null;

            ret.Setup();
            return ret;
        }

        /// <summary>
        /// Typically used to generate code when an app.config or web.config isn't available or desired.
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="connectionString"></param>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static DataService GetDataServiceManually(string connectionStringName, string connectionString, string providerName)
        {
            if (_mServices.ContainsKey(connectionStringName))
                return _mServices[connectionStringName];

            return new DataService(new ConnectionStringSettings(connectionStringName, connectionString, providerName));
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
        public string[] GetSpList()
        {
            return Provider.GetSpList();
        }
        
        /// <param name="fkColumn">C#CD: </param>
        /// <returns>C#CD: </returns>
        public string GetForeignKeyTableName(string fkColumn)
        {
            return Provider.GetForeignKeyTableName(fkColumn);
        }
        
        /// <param name="spName">C#CD: </param>
        /// <returns>C#CD: </returns>
        public IDataReader GetSpParams(string spName)
        {
            return Provider.GetSpParams(spName);
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
