using System;
using System.Text;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings.Generics;

[Serializable]
public class AssocItemOfT<T> : BasicAssocItem, IAssocItem<T> where T : class, new()
{
    public T Item { get; set; }

    public AssocItemOfT()
    {
        Item = new T();
    }

    public AssocItemOfT(string key, T item = default, string value = null, Guid? id = null, string name = null, int number = 0) 
        : base(key, value, id, name)
    {
        Number = number;
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
