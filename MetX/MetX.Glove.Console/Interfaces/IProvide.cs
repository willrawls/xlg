using MetX.Standard.Interfaces;
using XLG.Pipeliner.Providers;

namespace XLG.Pipeliner.Interfaces
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