using System;
using System.Collections.Generic;

namespace MetX.Standard.Library
{
    [Serializable]
    public class AssocArray : AssocItem
    {
        public object SyncRoot { get; }= new();
        protected SortedDictionary<string, AssocItem> Pairs = new();

        public AssocArray() { }
        public AssocArray(string key, string value = "", Guid? id = null, string name = "", IAssocItem parent = null) 
            : base(key, value, id, name, parent) { }

        public AssocItem this[string key]
        {
            get
            {
                lock(SyncRoot)
                {
                    AssocItem assocItem;
                    var assocKey = key.ToAssocKey();
                    if (!Pairs.ContainsKey(assocKey))
                    {
                        assocItem = new AssocItem(key, "");
                        Pairs.Add(assocKey, assocItem);
                    }
                    else
                        assocItem = Pairs[assocKey];
                    return assocItem;
                }
            }
            set
            {
                lock(SyncRoot)
                {
                    var assocKey = key.ToAssocKey();
                    if (!Pairs.ContainsKey(assocKey))
                        Pairs.Add(assocKey, value);
                    else
                        Pairs[assocKey] = value;
                }
            }
        }
    }
}