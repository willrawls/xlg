using System;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings.Generics;


[Serializable]
public class AssocRelativeItem<T> : AssocRelativeItem, IAssocItem<T> where T : new()
{
    public T Item { get; set; }

    public AssocRelativeItem()
    {
    }

    public AssocRelativeItem(string key, T item = default, string value = "", Guid? id = null, string name = null, Guid? parent = null, Guid? left = null, Guid? right = null)
        : base(key, value, id, name)
    {
        Item = item ?? new T();
        Parent = parent ?? Guid.Empty;
        Left = left ?? Guid.Empty;
        Right = right ?? Guid.Empty;
    }

}
