using MetX.Standard.Library;
using MetX.Standard.Library.ML;

namespace MetX.Standard.XDString.Generics;

public class AssocArray2D<T> : AssocArray1D<AssocArray1D<T>> where T : class, new()
{
    public T this[string d1, string d2]
    {
        get => this[d1].Item[d2].Item;
        set => this[d1].Item[d2].Item = value;
    }
    
    public override string ToXml()
    {
        var xml = Xml.ToXml(this, true, ExtraTypes);
        var targetNameOfRootElement = typeof(AssocArray2D<T>).Name;
        if(targetNameOfRootElement != "AssocArray")
            xml = xml.Replace("<AssocArray", $"<{targetNameOfRootElement}")
                .Replace("</AssocArray", $"</{targetNameOfRootElement}");
        return xml;
    }}