﻿using System;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings.Generics;

public class AssocCubeOf<TAxis, TItem> : AssocCubeOf<TAxis, TAxis, TAxis, TItem>
    where TAxis : class, IAssocItem
    where TItem : class, IAssocItem, new()
{
}

public class AssocCubeOf<TFirstAxis, TSecondAxis, TThirdAxis, TItem>
    where TFirstAxis : class, IAssocItem 
    where TSecondAxis : class, IAssocItem
    where TThirdAxis : class, IAssocItem
    where TItem : class, IAssocItem, new()
{
    public AssocArrayOfT<AssocArrayOfT<AssocArrayOfT<TItem>>> FirstAxis = new();

    public TItem this[string firstAxisKey, string secondAxisKey, string thirdAxisKey]
    {
        get
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return null;

            return FirstAxis[firstAxisKey].Item[secondAxisKey].Item[thirdAxisKey].Item;
        }
        set
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return;

            FirstAxis[firstAxisKey].Item[secondAxisKey].Item[thirdAxisKey].Item = value;
        }
    }

    public TItem this[TFirstAxis first, TSecondAxis second, TThirdAxis third]
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
            if (firstAxisId == Guid.Empty 
                || secondAxisId == Guid.Empty
                || thirdAxisId == Guid.Empty)
                return null;

            return FirstAxis[firstAxisId].Item[secondAxisId].Item[thirdAxisId].Item;
        }
        set
        {
            if (firstAxisId == Guid.Empty 
                || secondAxisId == Guid.Empty
                || thirdAxisId == Guid.Empty)
                return;

            FirstAxis[firstAxisId].Item[secondAxisId].Item[thirdAxisId].Item = value;
        }
    }
}