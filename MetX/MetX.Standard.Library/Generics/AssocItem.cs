using System;
using System.Text;

namespace MetX.Standard.Library.Generics
{
    [Serializable]
    public class AssocItem<T> : AssocItem, IAssocItem<T> where T : class, new()
    {
        public T Item { get; set; }

        public AssocItem()
        {
            Item = new T();
        }

        public AssocItem(string key, T item = default, string value = null, Guid? id = null, string name = null, IAssocItem parent = null) 
            : base(key, value, id, name, parent)
        {
            Item = item ??  new T();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"      {Key}={Value ?? "nil"}, {ID:N}, {Name ?? "nil"}");
            if(Item != null)
            {
                sb.AppendLine(Item.ToString());
            }
            return sb.ToString();
        }
    }
}