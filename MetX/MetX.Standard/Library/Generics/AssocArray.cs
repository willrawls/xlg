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
                    var assocKey = key.ToAssocKey();
                    if (!Items.ContainsKey(assocKey))
                    {
                        Items[assocKey] = assocItem = new AssocItem<T>(key);
                    }
                    else
                        assocItem = Items[assocKey];
                    return assocItem;
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    var assocKey = key.ToAssocKey();
                    if (!Items.ContainsKey(assocKey))
                        Items.Add(assocKey, new AssocItem<T>(key, value?.Item));
                    else
                        Items[assocKey] = value;
                }
            }
        }
    }
}