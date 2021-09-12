using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.Standard.Library.Generics
{
    [Serializable]
    public class AssocArrayList<T> : AssocItem<T>  where T : class, new() //: AssocArray<AssocArray<T>> where T : class, new()
    {
        [XmlIgnore] public object SyncRoot = new();
        public SortedDictionary<string, AssocArray<T>> Items = new();

        public AssocArrayList(string key) : base(key)
        {

        }

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