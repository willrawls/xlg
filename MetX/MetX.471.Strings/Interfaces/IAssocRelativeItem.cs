using System;

namespace MetX._471.Strings.Interfaces;

public interface IAssocRelativeItem<T> : IAssocRelativeItem, IAssocItem<T>
{
}

public interface IAssocRelativeItem : IAssocItem
{
    public Guid Parent { get; set; }
    public Guid Left { get; set; }
    public Guid Right { get; set; }
}