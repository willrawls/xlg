using System;

namespace MetX.Standard.XDString.Interfaces;

public interface IAssocItem
{
    string Key { get; set; }

    string Value { get; set; }
    string Name { get; set; }
    Guid ID { get; set; }
}

public interface IAssocItem<T> : IAssocItem
{
    T Item { get; set; }
}