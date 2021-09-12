using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MetX.Standard.Library.Generics;

namespace MetX.Standard.Library
{
    [Serializable]
    public class AssocArrayList : AssocItem
    {
        [XmlIgnore]
        public object SyncRoot { get; }= new();
        public SortedDictionary<string, AssocArray> Pairs = new();

        public AssocArrayList(){ }

        public AssocArrayList(string key, string name = null, string value = null, Guid? id = null, IAssocItem parent = null) 
            : base(key, value, id, name, parent)
        {
        }

        public AssocArray this[string key]
        {
            get
            {
                lock(SyncRoot)
                {
                    AssocArray assocArray;
                    var assocKey = key.ToAssocKey();
                    if (!Pairs.ContainsKey(assocKey))
                    {
                        assocArray = new AssocArray(key);
                        Pairs.Add(assocKey, assocArray);
                    }
                    else
                    {
                        assocArray = Pairs[assocKey];
                    }
                    return assocArray;
                }
            }
            set
            {
                lock(SyncRoot)
                {
                    var assocKey = key.ToAssocKey();
                    if (Pairs.ContainsKey(assocKey))
                        Pairs[assocKey] = value;
                    else
                        Pairs.Add(assocKey, value);
                }
            }
        }

    }
}