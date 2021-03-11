using System;
using MetX.Standard.Interfaces;

namespace MetX.Standard.Data.Factory
{
    public class CommandLineProvider : IProvide 
    {
        #region IProvide Members

        public string ProviderName => "CommandLineProvider";

        public string ObjectName => "MetX.Data.Factory.CommandLineProvider";

        public string ProviderAssemblyString => string.Empty;

        public DataService GetNewDataService(string connectionName)
        {
            throw new Exception("This gathers, not data.");
        }

        public DataProvider GetNewDataProvider(string connectionString)
        {
            throw new Exception("This gathers, not data.");
        }

        public ProviderTypeEnum ProviderType => ProviderTypeEnum.Gather;

        public GatherProvider GetNewGatherProvider()
        {
            return new CommandLineGatherer();
        }

        #endregion
    }
}
