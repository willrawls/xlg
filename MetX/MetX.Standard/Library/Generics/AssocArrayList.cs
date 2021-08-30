using System;

namespace MetX.Standard.Library.Generics
{
    [Serializable]
    public class AssocArrayList<T> : AssocArray<AssocArray<T>> where T : class, new()
    {
        public AssocArrayList() { }
        public AssocArrayList(string key, AssocArray<T> item = default, string value = null, Guid? id = null, string name = null, IAssocItem parent = null) : base(key, item, value, id, name, parent)
        {
        }
    }
}