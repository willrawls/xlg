using System;
using System.Xml.Serialization;
using MetX.Standard.XDString.Interfaces;
using MetX.Standard.XDString.Support;

namespace MetX.Standard.XDString.Generics;

public class AssocCube<TItem> : IAssocItem
    where TItem : class, IAssocItem, new()
{
    [XmlAttribute] public string Key { get; set; }
    [XmlAttribute] public string Value { get; set; }
    [XmlAttribute] public string Name { get; set; }
    [XmlAttribute] public Guid ID { get; set; }

    public AssocCube(string key = null, string value = null, string name = null, Guid? id = null)
    {
        Key = key;
        Value = value;
        Name = name;
        ID = id ?? Guid.NewGuid();
    }

    public AssocCube()
    {
        ID = Guid.NewGuid();
    }

    public AssocArray<AssocArray<AssocArray<TItem>>> FirstAxis = new();

    public TItem this[string firstAxisKey, string secondAxisKey, string thirdAxisKey]
    {
        get
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty() || thirdAxisKey.IsEmpty())
                return null;

            return FirstAxis[firstAxisKey].Item[secondAxisKey].Item[thirdAxisKey].Item;
        }
        set
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty() || thirdAxisKey.IsEmpty())
                return;

            FirstAxis[firstAxisKey].Item[secondAxisKey].Item[thirdAxisKey].Item = value;
        }
    }

    public TItem this[TItem first, TItem second, TItem third]
    {
        get
        {
            if (first == null 
                || second == null
                || third == null
                || first.ID == Guid.Empty
                || second.ID == Guid.Empty
                || third.ID == Guid.Empty)
                return null;

            return FirstAxis[first.ID].Item[second.ID].Item[third.ID].Item;
        }
        set
        {
            if (first == null 
                || second == null
                || third == null
                || first.ID == Guid.Empty
                || second.ID == Guid.Empty
                || third.ID == Guid.Empty)
                return;

            FirstAxis[first.ID].Item[second.ID].Item[third.ID].Item = value;
        }
    }

    public TItem this[Guid firstAxisId, Guid secondAxisId, Guid thirdAxisId]
    {
        get
        {
            if (firstAxisId == Guid.Empty || secondAxisId == Guid.Empty || thirdAxisId == Guid.Empty)
                return null;

            return FirstAxis[firstAxisId].Item[secondAxisId].Item[thirdAxisId].Item;
        }
        set
        {
            if (firstAxisId == Guid.Empty || secondAxisId == Guid.Empty || thirdAxisId == Guid.Empty)
                return;

            FirstAxis[firstAxisId].Item[secondAxisId].Item[thirdAxisId].Item = value;
        }
    }
}