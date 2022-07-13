using System;
using System.Text;
using System.Xml.Serialization;
using MetX.Standard.XDString.Interfaces;

namespace MetX.Standard.XDString.Generics;

[Serializable]
public class AssocItem<T> : AssocItem, IAssocItem<T> where T : class, new()
{
    public T Item { get; set; }

    public AssocItem()
    {
        Item = new T();
    }

    public AssocItem(string key, T item = default, string value = null, Guid? id = null, string name = null) 
        : base(key, value, id, name)
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