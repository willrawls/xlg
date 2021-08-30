using System;
using System.Collections.Generic;
using MetX.Standard.Library.Generics;

namespace MetX.Standard.Library
{
    [Serializable]
    public class AssocArrayList : AssocItem
    {
        public object SyncRoot { get; }= new();
        public AssocArray<AssocArray> Pairs { get; set; } = new();

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
                    return Pairs[key].Item;
                }
            }
            set
            {
                lock(SyncRoot)
                {
                    Pairs[key].Item = value;
                }
            }
        }
    }
}