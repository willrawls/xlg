using System;
using MetX.Fimm.Glove.Gatherers;
using MetX.Fimm.Glove.Interfaces;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Fimm.Glove.Providers
{
    // ReSharper disable once UnusedType.Global
    public class CommandLineProvider : IProvide 
    {
        #region IProvide Members

        public string ProviderName => "CommandLineProvider";

        public string ObjectName => "MetX.Standard.Data.Factory.CommandLineProvider";

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
