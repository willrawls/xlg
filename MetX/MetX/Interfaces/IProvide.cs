using MetX.Data;

namespace MetX.Interfaces
{
    public interface IProvide
    {
        ProviderTypeEnum ProviderType { get; }

        string ProviderName { get; }

        string ObjectName { get; }

        string ProviderAssemblyString { get; }

        DataService GetNewDataService(string ConnectionName);

        DataProvider GetNewDataProvider(string ConnectionString);

        GatherProvider GetNewGatherProvider();
    }
}