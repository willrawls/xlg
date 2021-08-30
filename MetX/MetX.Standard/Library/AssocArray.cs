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
        public AssocArray(string key, string value = null, Guid? id = null, string name = null, IAssocItem parent = null) : base(key, value, id, name, parent) { }

        public AssocItem this[string key]
        {
            get
            {
                lock(SyncRoot)
                {
                    AssocItem assocItem;
                    var k = key.ToAssocKey();
                    if (!Pairs.ContainsKey(k))
                        Pairs[k] = assocItem = new AssocItem(key);
                    else
                        assocItem = Pairs[k];
                    return assocItem;
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
                        Pairs.Add(k, value);
                }
            }
        }
    }
}