using MetX.Standard.Data;

namespace MetX.Standard.Interfaces
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