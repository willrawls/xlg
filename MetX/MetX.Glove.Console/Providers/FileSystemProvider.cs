using System;
using MetX.Standard.Primary.Interfaces;
using XLG.Pipeliner.Gatherers;
using XLG.Pipeliner.Interfaces;

namespace XLG.Pipeliner.Providers
{
    // ReSharper disable once UnusedType.Global
    public class FileSystemProvider : IProvide
    {
        #region IProvide Members

        public string ProviderName => "FileSystemProvider";

        public string ObjectName => "MetX.Standard.Data.Factory.FileSystemProvider";

        public string ProviderAssemblyString => string.Empty;

        public DataService GetNewDataService(string connectionName)
        {
             throw new Exception("The method or operation is not implemented.");
        }

        public DataProvider GetNewDataProvider(string connectionString)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ProviderTypeEnum ProviderType => ProviderTypeEnum.Gather;

        public GatherProvider GetNewGatherProvider()
        {
            return new FileSystemGatherer();
        }

        #endregion
    }
}
