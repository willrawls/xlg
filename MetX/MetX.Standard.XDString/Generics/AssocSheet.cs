using System;
using System.Xml.Serialization;
using MetX.Standard.XDString.Interfaces;
using MetX.Standard.XDString.Support;

namespace MetX.Standard.XDString.Generics;

public class AssocSheet : AssocSheet<AssocType<string>>
{

}

public class AssocSheet<TItem> 
    where TItem : class, IAssocItem, new()
{
    public AssocArrayOfT<AssocArrayOfT<TItem>> FirstAxis = new();

    public TItem this[string firstAxisKey, string secondAxisKey]
    {
        get
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return null;

            return FirstAxis[firstAxisKey].Item[secondAxisKey].Item;
        }
        set
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return;

            FirstAxis[firstAxisKey].Item[secondAxisKey].Item = value;
        }
    }

    public TItem this[TItem first, TItem second]
    {
        get
        {
            if (first == null 
                || second == null
                || first.Key.IsEmpty() 
                || second.Key.IsEmpty())
                return null;

            return FirstAxis[first.Key].Item[second.Key].Item;
        }
        set
        {
            if (first == null 
                || second == null
                || first.Key.IsEmpty() 
                || second.Key.IsEmpty())
                return;

            FirstAxis[first.Key].Item[second.Key].Item = value;
        }
    }

    public TItem this[Guid firstAxisId, Guid secondAxisId]
    {
        get
        {
            if (firstAxisId == Guid.Empty || secondAxisId == Guid.Empty)
                return null;

            return FirstAxis[firstAxisId].Item[secondAxisId].Item;
        }
        set
        {
            if (firstAxisId == Guid.Empty || secondAxisId == Guid.Empty)
                return;

            FirstAxis[firstAxisId].Item[secondAxisId].Item = value;
        }
    }

}

public class AssocSheet<TAxis, TItem> 
    where TAxis : class, IAssocItem
    where TItem : class, IAssocItem, new()
{
    public AssocArrayOfT<AssocArrayOfT<TItem>> FirstAxis = new();

    public TItem this[string firstAxisKey, string secondAxisKey]
    {
        get
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return null;

            return FirstAxis[firstAxisKey].Item[secondAxisKey].Item;
        }
        set
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return;

            FirstAxis[firstAxisKey].Item[secondAxisKey].Item = value;
        }
    }

    public TItem this[TAxis first, TAxis second]
    {
        get
        {
            if (first == null 
                || second == null
                || first.Key.IsEmpty() 
                || second.Key.IsEmpty())
                return null;

            return FirstAxis[first.Key].Item[second.Key].Item;
        }
        set
        {
            if (first == null 
                || second == null
                || first.Key.IsEmpty() 
                || second.Key.IsEmpty())
                return;

            FirstAxis[first.Key].Item[second.Key].Item = value;
        }
    }

    public TItem this[Guid firstAxisId, Guid secondAxisId]
    {
        get
        {
            if (firstAxisId == Guid.Empty || secondAxisId == Guid.Empty)
                return null;

            return FirstAxis[firstAxisId].Item[secondAxisId].Item;
        }
        set
        {
            if (firstAxisId == Guid.Empty || secondAxisId == Guid.Empty)
                return;

            FirstAxis[firstAxisId].Item[secondAxisId].Item = value;
        }
    }
}

public class AssocSheet<TFirstAxis, TSecondAxis, TItem> : IAssocItem
    where TFirstAxis : class, IAssocItem 
    where TSecondAxis : class, IAssocItem
    where TItem : class, IAssocItem, new()
{
    public AssocArrayOfT<AssocArrayOfT<TItem>> FirstAxis = new();

    [XmlAttribute] public string Key { get; set; }
    [XmlAttribute] public string Value { get; set; }
    [XmlAttribute] public string Name { get; set; }
    [XmlAttribute] public Guid ID { get; set; }
    [XmlAttribute] public int Number { get; set; }
    [XmlAttribute] public string Category { get; set; }

    public AssocSheet()
    {
        ID = Guid.NewGuid();
        Key = ID.ToString("N");
    }

    public AssocSheet(string key, string value = null, string name = null, Guid? id = null)
    {
        Value = value;
        Name = name;
        ID = id ?? Guid.NewGuid();
        Key = key ?? ID.ToString("N");
    }

    public TItem this[string firstAxisKey, string secondAxisKey]
    {
        get
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return null;

            return FirstAxis[firstAxisKey].Item[secondAxisKey].Item;
        }
        set
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return;

            FirstAxis[firstAxisKey].Item[secondAxisKey].Item = value;
        }
    }

    public TItem this[TFirstAxis first, TSecondAxis second]
    {
        get
        {
            if (first == null 
                || second == null
                || first.Key.IsEmpty() 
                || second.Key.IsEmpty())
                return null;

            return FirstAxis[first.Key].Item[second.Key].Item;
        }
        set
        {
            if (first == null 
                || second == null
                || first.Key.IsEmpty() 
                || second.Key.IsEmpty())
                return;

            FirstAxis[first.Key].Item[second.Key].Item = value;
        }
    }

    public TItem this[Guid firstAxisId, Guid secondAxisId]
    {
        get
        {
            if (firstAxisId == Guid.Empty || secondAxisId == Guid.Empty)
                return null;

            return FirstAxis[firstAxisId].Item[secondAxisId].Item;
        }
        set
        {
            if (firstAxisId == Guid.Empty || secondAxisId == Guid.Empty)
                return;

            FirstAxis[firstAxisId].Item[secondAxisId].Item = value;
        }
    }
}