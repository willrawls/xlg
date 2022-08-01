using System;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings;

[Serializable]
public class AssocRelativeItem : BasicAssocItem, IAssocRelativeItem
{
    public Guid Parent { get; set; }
    public Guid Left { get; set; }
    public Guid Right { get; set; }

    public AssocRelativeItem()
    {
    }

    public AssocRelativeItem(string key, string value = "", Guid? id = null, string name = null, Guid? parent = null, Guid? left = null, Guid? right = null)
        : base(key, value, id, name)
    {
        Parent = parent ?? Guid.Empty;
        Left = left ?? Guid.Empty;
        Right = right ?? Guid.Empty;
    }

}