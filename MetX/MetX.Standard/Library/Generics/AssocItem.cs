using System;
using System.Xml.Serialization;
using MetX.Standard.Web.Virtual.xsl;

namespace MetX.Standard.Library.Generics
{
    [Serializable]
    public class AssocItem<T> : AssocItem, IAssocItem<T> where T : class, new()
    {
        public T Item { get; set; }

        public AssocItem() { }
        public AssocItem(string key, T item = default, string value = null, Guid? id = null, string name = null, IAssocItem parent = null) 
            : base(key, value, id, name, parent)
        {
            Item = item;
        }
    }
}