using System;
using MetX.Standard.Interfaces;

namespace MetX.Standard.Data.Factory
{
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
