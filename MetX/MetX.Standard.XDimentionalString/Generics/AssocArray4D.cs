namespace MetX.Standard.XDimensionalString.Generics;

public class AssocArray4D<T> : AssocArray3D<AssocArray1D<T>> where T : class, new()
{
    public T this[string d1, string d2, string d3, string d4]
    {
        get => this[d1][d2].Item[d3].Item[d4].Item;
        set => this[d1][d2].Item[d3].Item[d4].Item = value;
    }
    

    public new AssocArray1D<T> this[string d1, string d2, string d3]
    {
        get => this[d1][d2].Item[d3].Item;
        set => this[d1][d2].Item[d3].Item = value;
    }

    public bool ContainsKey(string d1, string d2, string d3, string d4)
    {
        return ContainsKey(d1)
            && this[d1].ContainsKey(d2)
            && this[d1][d2].Item != null
            && this[d1][d2].Item.ContainsKey(d3)
            && this[d1][d2].Item[d3].Item != null
            && this[d1][d2].Item[d3].Item.ContainsKey(d4)
            ;

    }
}