using System;
using System.Configuration.Provider;

namespace MetX.Data
{
    public class DataProviderCollection : ProviderCollection
    {
        
        public new DataProvider this[string name] => (DataProvider)base[name];


        /// <param name="provider">C#CD: </param>
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (!(provider is DataProvider))
                throw new ArgumentException
                    ("Invalid provider type", "provider");

            base.Add(provider);
        }
    }
}