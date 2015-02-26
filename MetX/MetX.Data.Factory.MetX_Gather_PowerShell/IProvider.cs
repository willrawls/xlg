using System;
using System.Collections.Generic;
using System.Text;

using MetX;
using MetX.Data;

namespace MetX.Data.Factory
{
    public class IProvider : IProvide 
    {
        #region IProvide Members

        public string ProviderName
        {
            get { return "PowerShellProvider"; }
        }

        public string ObjectName
        {
            get { return "MetX.Gather.PowerShell"; }
        }

        public string ProviderAssemblyString
        {
            get { return string.Empty; }
        }

        public DataService GetNewDataService(string ConnectionName)
        {
            return null;
        }

        public DataProvider GetNewDataProvider(string ConnectionString)
        {
            return null;
        }

        public ProviderTypeEnum ProviderType
        {
            get { return ProviderTypeEnum.Gather; }
        }

        public GatherProvider GetNewGatherProvider()
        {
            return new Gather.PowerShell();
        }

        #endregion
    }
}
