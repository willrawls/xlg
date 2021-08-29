using System.Collections.Generic;
using MetX.Standard.Library;

namespace MetX.Tests.Library
{
    public class AssocArrayList<T> : List<AssocArray<T>> where T : class, new()
    {
    }

    public class AssocArrayList
    {
        public string Name { get; set; }
        public object SyncRoot = new();

        public AssocArrayList(string name)
        {
            Name = name;
        }

        protected SortedDictionary<string, AssocArray> Pairs = new();
        public AssocArray this[string key]
        {
            get
            {
                lock(SyncRoot)
                {
                    AssocArray assocArray;
                    var k = KeyToLegalKey(key);
                    if (!Pairs.ContainsKey(k))
                        Pairs[k] = assocArray = new AssocArray(this, key);
                    else
                        assocArray = Pairs[k];
                    return assocArray;
                }
            }
            set
            {
                lock(SyncRoot)
                {
                    var k = KeyToLegalKey(key);
                    if (Pairs.ContainsKey(k))
                        Pairs[k] = value;
                    else
                        Pairs.Add(k, new AssocArray(this, key));
                }
            }
        }

        public string KeyToLegalKey(string k)
        {
            return k.AsString().ToLowerInvariant();
        }

    }
}