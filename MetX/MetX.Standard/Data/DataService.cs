using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Reflection;
using MetX.Standard.Data.Factory;
using MetX.Standard.Interfaces;
using MetX.Standard.Library;

namespace MetX.Standard.Data
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
        private static Dictionary<string, DataService> _mServices = new();
        private static readonly Dictionary<string, IProvide> InMemoryProviders = new();

        public DataService(string providerName, string parameter1, string parameter2)
        {
            Settings = null;
            WithSetup(providerName, parameter1, parameter2);
        }

        public DataService(ConnectionStringSettings connectionSettings)
        {
            Settings = connectionSettings;
            WithSetup(connectionSettings.ProviderName, connectionSettings.ConnectionString, connectionSettings.ConnectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="name">usually connection name or folder path</param>
        /// <param name="detail">usually connection string or commands</param>
        public DataService WithSetup(string providerName, string name, string detail)
        {
            IProvide metXProvider;
            if (!InMemoryProviders.ContainsKey(providerName.ToLower()))
            {
                MetXObjectName = providerName;
                if (!MetXObjectName.ToLower().EndsWith("provider"))
                    MetXObjectName = $"{MetXObjectName.Replace(".", "")}Provider";

                var metXProviderAssembly = typeof(IProvide).Assembly;
                var metXProviderType = metXProviderAssembly.GetType(MetXObjectName, true);
                
                metXProvider = Activator
                    .CreateInstance(metXProviderType ?? throw new InvalidOperationException(), true) 
                    as IProvide;
                
                if (metXProvider == null) throw new ProviderException("Unable to instantiate: " + metXProviderType.FullName);
                InMemoryProviders.Add(providerName.ToLower(), metXProvider);
                MetXProviderAssemblyString = metXProvider.ProviderAssemblyString;
                ProviderAssemblyString = metXProviderAssembly.FullName;
            }
            else
                metXProvider = InMemoryProviders[providerName.ToLower()];

            ProviderType = metXProvider.ProviderType;
            if (metXProvider.ProviderType == ProviderTypeEnum.Data || metXProvider.ProviderType == ProviderTypeEnum.DataAndGather)
            {
                Provider = metXProvider.GetNewDataProvider(detail);
                Provider.Initialize(metXProvider.ProviderName, new System.Collections.Specialized.NameValueCollection());

                if (Provider == null)
                    throw new ProviderException("Unknown provider: " + providerName);
            }
            else if (metXProvider.ProviderType == ProviderTypeEnum.Gather)
            {
                Gatherer = metXProvider.GetNewGatherProvider();
                if (Gatherer == null)
                    throw new ProviderException("Unknown gatherer: " + providerName);
            }

            _mServices.Add(name, this);
            Instance ??= this;
            return this;
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
                ConnectionStrings = ConfigurationManager.ConnectionStrings;

            var ret = new DataService
            {
                Settings = ConnectionStrings[connectionStringName]
            };
            if (ret.Settings == null)
            {
                return null;
            }

            ret.WithSetup(ret.Settings.ProviderName, ret.Settings.Name, ret.Settings.ConnectionString);
            return ret;
        }

        /// <summary>
        /// Typically used to generate code when an app.config or web.config isn't available or desired.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="detail"></param>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static DataService GetDataServiceManually(string name, string detail, string providerName)
        {
            if (name.IsNotEmpty() && _mServices.ContainsKey(name))
                return _mServices[name];

            if (name.IsEmpty())
                return new DataService().WithSetup(providerName, name, detail);

            return new DataService(new ConnectionStringSettings(name, detail, providerName));
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
