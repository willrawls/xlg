namespace MetX.Standard.XDimensionalString.Generics;

public class AssocArray3D<T> : AssocArray2D<AssocArray1D<T>> where T : class, new()
{
    public T this[string d1, string d2, string d3]
    {
        get => this[d1][d2].Item[d3].Item;
        set => this[d1][d2].Item[d3].Item = value;
    }
    

    public new AssocArray1D<T> this[string d1, string d2]
    {
        get => this[d1][d2].Item;
        set => this[d1][d2].Item = value;
    }
}