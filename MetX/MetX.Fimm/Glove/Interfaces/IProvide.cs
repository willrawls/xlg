using MetX.Fimm.Glove.Providers;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Fimm.Glove.Interfaces
{
    public interface IProvide
    {
        ProviderTypeEnum ProviderType { get; }

        string ProviderName { get; }

        string ObjectName { get; }

        string ProviderAssemblyString { get; }

        DataService GetNewDataService(string connectionName);

        DataProvider GetNewDataProvider(string connectionString);

        GatherProvider GetNewGatherProvider();
    }
}