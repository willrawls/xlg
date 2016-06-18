using System;
using System.Collections.Generic;
using System.Text;

using MetX;
using MetX.Data;
using MetX.Interfaces;

namespace MetX.Data.Factory
{
    public class MetX_Gather_CommandLine : IProvide 
    {
        #region IProvide Members

        public string ProviderName
        {
            get { return "CommandLineProvider"; }
        }

        public string ObjectName
        {
            get { return "MetX.Gather.CommandLine"; }
        }

        public string ProviderAssemblyString
        {
            get { return string.Empty; }
        }

        public DataService GetNewDataService(string ConnectionName)
        {
            throw new Exception("This gathers, not data.");
        }

        public DataProvider GetNewDataProvider(string ConnectionString)
        {
            throw new Exception("This gathers, not data.");
        }

        public ProviderTypeEnum ProviderType
        {
            get { return ProviderTypeEnum.Gather; }
        }

        public GatherProvider GetNewGatherProvider()
        {
            return new Gather.CommandLine();
        }

        #endregion
    }
}
