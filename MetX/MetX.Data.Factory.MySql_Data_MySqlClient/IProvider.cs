using System;
using System.Collections.Generic;
using System.Text;

using MetX;
using MetX.Data;

namespace MetX.Data.Factory
{
    public class MySql_Data_MySqlClient : IProvide
    {
        #region IProvide Members

        public DataProvider GetNewDataProvider(string ConnectionString)
        {
            return new MetX.Data.MySqlDataProvider(ConnectionString);
        }

        public DataService GetNewDataService(string ConnectionName)
        {
            return MetX.Data.DataService.GetDataService(ConnectionName);
        }

        public string ObjectName
        {
            get { return "MetX.Data.MySqlDataProvider"; }
        }

        public string ProviderName
        {
            get { return "MySqlDataProvider"; }
        }

        public string ProviderAssemblyString
        {
            get { return "MySql.Data"; }
        }

        public ProviderTypeEnum ProviderType
        {
            get { return ProviderTypeEnum.Data; }
        }

        #endregion

        #region IProvide Members


        public GatherProvider GetNewGatherProvider()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
