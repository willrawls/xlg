using System.Collections.Generic;

namespace MetX.Standard.Library
{
    public class InMemoryCache<T> where T : class
    {
        public readonly Dictionary<string, T> Dictionary = new();
        public T this[string cacheKey]
        {
            get
            {
                if (cacheKey.IsEmpty())
                    return null;
                cacheKey = cacheKey.ToLower();
                
                return Dictionary.ContainsKey(cacheKey) ? Dictionary[cacheKey] : null;
            }
            set
            {
                if (cacheKey.IsEmpty())
                    return;
                cacheKey = cacheKey.ToLower();

                if (Dictionary.ContainsKey(cacheKey))
                    Dictionary[cacheKey] = value;
                else
                    Dictionary.Add(cacheKey, value);
            }
        }

        public bool Contains(string cacheKey)
        {
            if (cacheKey.IsEmpty())
                return false;
            cacheKey = cacheKey.ToLower();
                
            return Dictionary.ContainsKey(cacheKey);
        }
    }
}