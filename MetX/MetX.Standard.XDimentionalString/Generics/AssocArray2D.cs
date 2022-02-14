namespace MetX.Standard.XDimensionalString.Generics;

public class AssocArray2D<T> : AssocArray1D<AssocArray1D<T>> where T : class, new()
{
    public T this[string d1, string d2]
    {
        get => this[d1].Item[d2].Item;
        set => this[d1].Item[d2].Item = value;
    }
}