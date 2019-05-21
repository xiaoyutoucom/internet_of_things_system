
using System;
using Mongodb.Data;

namespace Mongodb
{
    public partial class MongoDBDataProviderManager : BaseDataProviderManager
    {
        public MongoDBDataProviderManager(DataSettings settings) : base(settings)
        {
        }

        public override IDataProvider LoadDataProvider()
        {
            var providerName = Settings.DataProvider;
            if (!String.IsNullOrWhiteSpace(providerName) && providerName.ToLowerInvariant() != "mongodb")
            {
                return null;
            }
            return new MongoDBDataProvider();
        }
    }
}
