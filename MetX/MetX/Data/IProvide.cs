using System;
using System.Collections.Generic;
using System.Text;

namespace MetX.Data
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

    public enum ProviderTypeEnum
    {
        Unknown,
        Data,
        Gather,
        DataAndGather
    }
}
