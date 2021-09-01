using System;
using System.Collections.Generic;

namespace MetX.Standard.Library.Generics
{
    [Serializable]
    public class AssocArrayList<T> : AssocItem<T>  where T : class, new() //: AssocArray<AssocArray<T>> where T : class, new()
    {
        public AssocArrayList() { }
        /*
        public AssocArrayList(string key, AssocArray<T> item = default, string value = null, Guid? id = null, string name = null, IAssocArray parent = null) 
            : base(key, item, value, id, name, parent)
        {
        }

        public AssocArrayList(string key, AssocArray<T> item) 
            : base(key, new AssocArray<T>(key))
        {
        }
    */

        public object SyncRoot = new();
        public SortedDictionary<string, AssocArray<T>> Items = new();

        public AssocArray<T> this[string key]
        {
            get
            {
                lock (SyncRoot)
                {
                    AssocArray<T> assocArray;
                    var assocKey = key.ToAssocKey();
                    if (!Items.ContainsKey(assocKey))
                    {
                        assocArray = new AssocArray<T>(key);
                        Items.Add(assocKey, assocArray);
                    }
                    else
                        assocArray = Items[assocKey];
                    return assocArray;
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    var assocKey = key.ToAssocKey();
                    if (!Items.ContainsKey(assocKey))
                    {
                        var assocArray = new AssocArray<T>(key);
                        Items.Add(assocKey, assocArray);
                    }
                    else
                        Items[assocKey] = value;
                }
            }
        }

    }
}