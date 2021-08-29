using System;
using System.Collections.Generic;

namespace MetX.Standard.Library
{
    public class AssocArrayList<T> : List<AssocArray<T>> where T : class, new()
    {
    }

    public class AssocArrayList
    {
        public string Name { get; set; }
        public object SyncRoot = new();

        public AssocArrayList(string name = null)
        {
            Name = name ?? Guid.NewGuid().ToString("N");
        }

        protected SortedDictionary<string, AssocArray> Pairs = new();
        public AssocArray this[string key]
        {
            get
            {
                lock(SyncRoot)
                {
                    AssocArray assocArray;
                    var k = key.ToAssocKey();
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
                    var k = key.ToAssocKey();
                    if (Pairs.ContainsKey(k))
                        Pairs[k] = value;
                    else
                        Pairs.Add(k, new AssocArray(this, key));
                }
            }
        }
    }
}