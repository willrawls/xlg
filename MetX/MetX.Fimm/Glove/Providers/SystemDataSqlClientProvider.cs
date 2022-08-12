using MetX.Fimm.Glove.Gatherers;
using MetX.Fimm.Glove.Interfaces;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Fimm.Glove.Providers
{
    public class SystemDataSqlClientProvider : IProvide
    {
        #region IProvide Members

        public DataProvider GetNewDataProvider(string connectionString)
        {
            return new SqlDataProvider(connectionString);
        }

        public DataService GetNewDataService(string connectionName)
        {
            return DataService.GetDataService(connectionName);
        }

        public string ProviderName => "SqlDataProvider";

        public string ObjectName => "MetX.Standard.Data.Factory.SqlDataProvider";

        public string ProviderAssemblyString => "System.Data";

        public ProviderTypeEnum ProviderType => ProviderTypeEnum.DataAndGather;

        public GatherProvider GetNewGatherProvider()
        {
            return new SqlToXmlGatherer();
        }

        #endregion
    }
}
