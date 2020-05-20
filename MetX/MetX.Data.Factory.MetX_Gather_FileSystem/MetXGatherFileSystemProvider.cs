using System;
using System.Collections.Generic;
using System.Text;

using MetX;
using MetX.Data;
using MetX.Interfaces;

namespace MetX.Data.Factory
{
    public class MetXGatherFileSystemProvider : IProvide
    {
        #region IProvide Members

        public string ProviderName
        {
            get { return "FileSystemProvider"; }
        }

        public string ObjectName
        {
            get { return "MetX.Gather.FileSystem"; }
        }

        public string ProviderAssemblyString
        {
            get { return string.Empty; }
        }

        public DataService GetNewDataService(string ConnectionName)
        {
             throw new Exception("The method or operation is not implemented.");
        }

        public DataProvider GetNewDataProvider(string ConnectionString)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ProviderTypeEnum ProviderType
        {
            get { return ProviderTypeEnum.Gather; }
        }

        public GatherProvider GetNewGatherProvider()
        {
            return new Gather.FileSystem();
        }

        #endregion
    }
}
