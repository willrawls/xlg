using MetX.Standard.Library.ML;

namespace MetX.Standard.XDString.Generics;

public class AssocArray3D<T> : AssocArray2D<AssocArray1D<T>> where T : class, new()
{
    public T this[string d1, string d2, string d3]
    {
        get => this[d1].Item[d2].Item[d3].Item;
        set => this[d1].Item[d2].Item[d3].Item = value;
    }
    

    public new AssocArray1D<T> this[string d1, string d2]
    {
        get => this[d1].Item[d2].Item;
        set => this[d1].Item[d2].Item = value;
    }

    public override string ToXml()
    {
        var xml = Xml.ToXml(this, true, ExtraTypes);
        var targetNameOfRootElement = typeof(AssocArray3D<T>).Name;
        if(targetNameOfRootElement != "AssocArray")
            xml = xml.Replace("<AssocArray", $"<{targetNameOfRootElement}")
                .Replace("</AssocArray", $"</{targetNameOfRootElement}");
        return xml;
    }
}