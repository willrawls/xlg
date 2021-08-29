using System;
using System.Collections.Generic;

namespace MetX.Standard.Library
{
    public class AssocArray 
    {
        public string Name { get; }
        public AssocArrayList Parent { get; }

        public AssocArray(AssocArrayList parent = null, string name = null, bool createParentIfNeeded = true)
        {
            Name = name ?? Guid.NewGuid().ToString("N");
            Parent = parent ?? (createParentIfNeeded ? new AssocArrayList() : null);
        }

        public object SyncRoot = new();

        protected SortedDictionary<string, AssocArrayItem> Pairs = new();
        public string this[string key]
        {
            get
            {
                lock(SyncRoot)
                {
                    AssocArrayItem assocArrayItem;
                    var k = KeyToLegalKey(key);
                    if (!Pairs.ContainsKey(k))
                        Pairs[k] = assocArrayItem = new AssocArrayItem(key);
                    else
                        assocArrayItem = Pairs[k];
                    return assocArrayItem.Item;
                }
            }
            set
            {
                lock(SyncRoot)
                {
                    var k = KeyToLegalKey(key);
                    if (Pairs.ContainsKey(k))
                        Pairs[k].Item = value;
                    else
                        Pairs.Add(k, new AssocArrayItem(this, key, value));
                }
            }
        }

        public string KeyToLegalKey(string k)
        {
            return k.AsString().ToLowerInvariant();
        }
    }

    public class AssocArray<T> where T : class, new()
    {
        public object SyncRoot = new();
        public AssocArrayList<T> Parent { get; }
        protected SortedDictionary<string, AssocArrayItem<T>> Pairs = new();

        public AssocArray(AssocArrayList<T> parent = null)
        {
            Parent = parent;
        }
        
        public T this[string key]
        {
            get
            {
                lock (SyncRoot)
                {
                    AssocArrayItem<T> assocArrayItem;
                    var k = KeyToLegalKey(key);
                    if (!Pairs.ContainsKey(k))
                        Pairs[k] = assocArrayItem = new AssocArrayItem<T>(key);
                    else
                        assocArrayItem = Pairs[k];
                    return assocArrayItem.Item;
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    var k = KeyToLegalKey(key);
                    if (Pairs.ContainsKey(k))
                        Pairs[k].Item = value;
                    else
                        Pairs.Add(k, new AssocArrayItem<T>(key, value));
                }
            }
        }

        public string KeyToLegalKey(string k)
        {
            return k.AsString().ToLowerInvariant();
        }
    }
}