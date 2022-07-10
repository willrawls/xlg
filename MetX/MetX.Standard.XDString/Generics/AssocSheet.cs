using System;
using MetX.Standard.XDString.Interfaces;
using MetX.Standard.XDString.Support;

namespace MetX.Standard.XDString.Generics;

public class AssocSheet<TItem> 
    where TItem : class, IAssocItem, new()
{
    public AssocArray<AssocArray<TItem>> FirstAxis = new();

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
    public AssocArray<AssocArray<TItem>> FirstAxis = new();

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

public class AssocSheet<TFirstAxis, TSecondAxis, TItem>
    where TFirstAxis : class, IAssocItem 
    where TSecondAxis : class, IAssocItem
    where TItem : class, IAssocItem, new()
{
    public AssocArray<AssocArray<TItem>> FirstAxis = new();

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