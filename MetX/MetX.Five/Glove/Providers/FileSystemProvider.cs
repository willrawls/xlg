using System;
using MetX.Five.Glove.Gatherers;
using MetX.Five.Glove.Interfaces;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Five.Glove.Providers
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
