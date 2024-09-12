using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;
using MetX.Standard.Strings.Generics.V1;
using MetX.Standard.Strings.Generics.V3;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings.Generics;

public class AssocSheet : AssocSheet<AssocType<BasicAssocItem>> { }

public class AssocSheet<TAxis, TItem> : AssocArray2D<TAxis, TAxis, TItem>, IAssocItem
    where TAxis : class, IAssocItem, new()
    where TItem : class, IAssocItem, new() { }


public class AssocSheet<TItem> : AssocArrayOfT<AssocArrayOfT<TItem>>
    where TItem : class, IAssocItem, new()
{
    public TItem this[string thisKey, string secondAxisKey]
    {
        get
        {
            if (thisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return null;

            return this[thisKey].Item[secondAxisKey].Item;
        }
        set
        {
            if (thisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return;

            this[thisKey].Item[secondAxisKey].Item = value;
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

            return this[first.ID].Item[second.ID].Item;
        }
        set
        {
            if (first == null 
                || second == null
                || first.Key.IsEmpty() 
                || second.Key.IsEmpty())
                return;

            this[first.ID].Item[second.ID].Item = value;
        }
    }

    public TItem this[Guid thisId, Guid secondAxisId]
    {
        get
        {
            if (thisId == Guid.Empty || secondAxisId == Guid.Empty)
                return null;

            return this[thisId].Item[secondAxisId].Item;
        }
        set
        {
            if (thisId == Guid.Empty || secondAxisId == Guid.Empty)
                return;

            this[thisId].Item[secondAxisId].Item = value;
        }
    }

}

public class AssocSheet<TFirstAxis, TSecondAxis, TItem> : AssocArray2D<TFirstAxis, TSecondAxis, TItem>, IAssocItem
    where TFirstAxis : class, IAssocItem, new()
    where TSecondAxis : class, IAssocItem, new()
    where TItem : class, IAssocItem, new()
{
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

}