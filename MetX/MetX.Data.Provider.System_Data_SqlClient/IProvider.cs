using System;
using System.Collections.Generic;
using System.Text;

using MetX;
using MetX.Data;

namespace MetX.Data.Factory
{
    public class System_Data_SqlClient : IProvide
    {
        #region IProvide Members

        public DataProvider GetNewDataProvider(string ConnectionString)
        {
            return new MetX.Data.SqlDataProvider(ConnectionString);
        }

        public DataService GetNewDataService(string ConnectionName)
        {
            return MetX.Data.DataService.GetDataService(ConnectionName);
        }

        public string ProviderName
        {
            get { return "SqlDataProvider"; }
        }

        public string ObjectName
        {
            get { return "MetX.Data.SqlDataProvider"; }
        }

        public string ProviderAssemblyString
        {
            get { return "System.Data"; }
        }

        public ProviderTypeEnum ProviderType
        {
            get { return ProviderTypeEnum.DataAndGather; }
        }

        public GatherProvider GetNewGatherProvider()
        {
            return new MetX.Gather.SqlToXml();
        }

        #endregion
    }
}
