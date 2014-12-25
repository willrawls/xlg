using System;
using System.Collections.Generic;
using System.Text;

using MetX;
using MetX.Data;

namespace MetX.Data.Factory
{
    public class Sybase_Data_AseClient : IProvide
    {
        #region IProvide Members

        public DataProvider GetNewDataProvider(string ConnectionString)
        {
            return new MetX.Data.SybaseDataProvider(ConnectionString);
        }

        public DataService GetNewDataService(string ConnectionName)
        {
            return MetX.Data.DataService.GetDataService(ConnectionName);
        }

        public string ObjectName
        {
            get { return "MetX.Data.SybaseDataProvider"; }
        }

        public string ProviderName
        {
            get { return "SybaseDataProvider"; }
        }

        private static string FullName;
        public string ProviderAssemblyString
        {
            get { return FullName; }
        }

        public Sybase_Data_AseClient()
        {
            if (FullName == null)
                FullName = System.Reflection.Assembly.GetAssembly(typeof(Sybase.Data.AseClient.AseConnection)).FullName;
        }

        public ProviderTypeEnum ProviderType
        {
            get { return ProviderTypeEnum.Data; }
        }

        public GatherProvider GetNewGatherProvider()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
