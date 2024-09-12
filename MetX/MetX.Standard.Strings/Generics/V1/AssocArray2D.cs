using System;
using System.Xml.Serialization;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings.Generics.V1;

public class AssocArray2D_TItem : AssocArray
{
}

public class AssocArray2D_T1DParent : AssocArray1D<AssocArray2D_T1DParent, AssocArray2D_TItem>
{
}

public class AssocArray2D_T2DParent : AssocArray1D<AssocArray2D_T2DParent, AssocArray2D_T1DParent>
{
}

public class AssocArray2D : AssocArray2D<AssocArray2D_T2DParent, AssocArray2D_T1DParent, AssocArray2D_TItem>
{
}

public class AssocArray2D<T2DParent, T1DParent, TItem> : AssocArray1D<T2DParent, AssocArray1D<T1DParent, TItem>>,
    IAssocItem
    where TItem : class, new()
    where T2DParent : class, new()
    where T1DParent : class, new()
{
    public new string Name;

    public AssocArray2D(string key = "", string value = null, string category = null, string name = null, int number = 0, Guid? id = null)
    {
        Key = key;
        Value = value;
        Category = category;
        Name = name;
        Number = number;
        ID = id ?? Guid.NewGuid();
    }

    public TItem this[string d1, string d2]
    {
        get => this[d1].Item[d2].Item;
        set => this[d1].Item[d2].Item = value;
    }

}
