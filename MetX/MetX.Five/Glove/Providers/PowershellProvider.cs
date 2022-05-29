using MetX.Five.Glove.Gatherers;
using MetX.Five.Glove.Interfaces;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Five.Glove.Providers
{
    // ReSharper disable once UnusedType.Global
    public class PowershellProvider : IProvide 
    {
        #region IProvide Members

        public string ProviderName => "PowerShellProvider";

        public string ObjectName => "MetX.Standard.Data.Factory.PowerShellProvider";

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
