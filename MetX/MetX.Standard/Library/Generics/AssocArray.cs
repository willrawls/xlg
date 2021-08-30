using System;
using System.Collections.Generic;

namespace MetX.Standard.Library.Generics
{
    [Serializable]
    public class AssocArray<T> : AssocItem<T> where T : class, new()
    {
        public object SyncRoot = new();
        public SortedDictionary<string, AssocItem<T>> Items = new();

        public AssocArray() { }
        public AssocArray(string key, T item = default, string value = null, Guid? id = null, string name = null, IAssocItem parent = null) : base(key, item, value, id, name, parent) { }

        public AssocItem<T> this[string key]
        {
            get
            {
                lock (SyncRoot)
                {
                    AssocItem<T> assocItem;
                    var k = key.ToAssocKey();
                    if (!Items.ContainsKey(k))
                        Items[k] = assocItem = new AssocItem<T>(key);
                    else
                        assocItem = Items[k];
                    return assocItem;
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    var k = key.ToAssocKey();
                    if (Items.ContainsKey(k))
                        Items[k] = value;
                    else
                        Items.Add(k, new AssocItem<T>(key, value?.Item));
                }
            }
        }
    }
}