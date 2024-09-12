using System;
using System.Collections.Generic;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;
using MetX.Standard.Strings.Generics.V3;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings.Generics;

public class AssocCubeOf<TAxis, TItem> : AssocCubeOf<TAxis, TAxis, TAxis, TItem>
    where TAxis : class, IAssocItem, new()
    where TItem : class, IAssocItem, new()
{
}

public class AssocCubeOf<TFirstAxis, TSecondAxis, TThirdAxis, TItem> : AssocSheet<TFirstAxis, AssocSheets<TSecondAxis, TThirdAxis, TItem>>
    where TFirstAxis : class, IAssocItem, new()
    where TSecondAxis : class, IAssocItem, new()
    where TThirdAxis : class, IAssocItem, new()
    where TItem : class, IAssocItem, new()
{

    public TItem this[string firstAxisKey, string secondAxisKey, string thirdAxisKey]
    {
        get
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return null;

            return this[firstAxisKey, secondAxisKey, thirdAxisKey];
        }
        set
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return;

            this[firstAxisKey, secondAxisKey, thirdAxisKey] = value;
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

            return this[first.ID, second.ID, third.ID];
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

            this[first.ID, second.ID, third.ID] = value;
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

            return this[firstAxisId, secondAxisId, thirdAxisId];
        }
        set
        {
            if (firstAxisId == Guid.Empty 
                || secondAxisId == Guid.Empty
                || thirdAxisId == Guid.Empty)
                return;

            this[firstAxisId, secondAxisId, thirdAxisId] = value;
        }
    }
}