using MetX.Standard.Strings.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetX.Standard.Strings.Generics.V3
{
    public class AssocCube : AssocCubeOf<BasicAssocItem>
    {
    }

    public class FoldedAssocArray2D : AssocArrayOfT<AssocArrayOfT<BasicAssocItem>>
    {
    }

    public class FoldedAssocArray2D<TItem> : AssocArrayOfT<AssocArrayOfT<TItem>> where TItem : class, IAssocItem, new()
    {
    }

    public class FoldedAssocArray3D : AssocArrayOfT<FoldedAssocArray2D>
    {
    }

    public class FoldedAssocArray3D<TItem> : AssocArrayOfT<FoldedAssocArray2D<TItem>>
        where TItem : class, IAssocItem, new()
    {
    }

    public class AssocSheets : AssocSheet<BasicAssocItem, AssocSheet<BasicAssocItem, BasicAssocItem>> { }

    public class AssocSheets<TItem> : AssocSheet<BasicAssocItem, AssocSheet<BasicAssocItem, TItem>>
        where TItem : class, IAssocItem, new()
    { }

    public class AssocSheets<TAxises, TItem> : AssocSheet<TAxises, AssocSheet<TAxises, TItem>>
        where TAxises : class, IAssocItem, new()
        where TItem : class, IAssocItem, new()
    { }

    public class AssocSheets<TFirstAxis, TSecondAxis, TItem> : AssocSheet<TFirstAxis, AssocSheet<TSecondAxis, TItem>>
        where TFirstAxis : class, IAssocItem, new()
        where TSecondAxis : class, IAssocItem, new()
        where TItem : class, IAssocItem, new()
    {
    };


}