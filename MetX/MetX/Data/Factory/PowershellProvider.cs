using System;
using System.Collections.Generic;
using System.Text;

using MetX;
using MetX.Data;
using MetX.Interfaces;

namespace MetX.Data.Factory
{
    public class PowershellProvider : IProvide 
    {
        #region IProvide Members

        public string ProviderName => "PowerShellProvider";

        public string ObjectName => "MetX.Data.Factory.PowerShellProvider";

        public string ProviderAssemblyString => string.Empty;

        public DataService GetNewDataService(string connectionName)
        {
            return null;
        }

        public DataProvider GetNewDataProvider(string connectionString)
        {
            return null;
        }

        public ProviderTypeEnum ProviderType => ProviderTypeEnum.Gather;

        public GatherProvider GetNewGatherProvider()
        {
            return new PowerShellGatherer();
        }

        #endregion
    }
}
