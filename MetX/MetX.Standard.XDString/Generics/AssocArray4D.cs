using System.Xml.Serialization;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.ML;
using MetX.Standard.Library.Strings;

namespace MetX.Standard.XDString.Generics;

public class AssocArray4D<T> : AssocArray2D<AssocArray2D<T>> where T : class, new()
{
    public static string ActualName {get;} = typeof(AssocArray4D<T>).Name.Replace("`1", "Of") + typeof(T).Name;

    [XmlAttribute]
    public string Name {get; set; }

    public T this[string d1, string d2, string d3, string d4]
    {
        get => this[d1].Item[d2].Item[d3].Item[d4].Item;
        set => this[d1].Item[d2].Item[d3].Item[d4].Item = value;
    }
    
    public AssocArray1D<T> this[string d1, string d2, string d3]
    {
        get => this[d1].Item[d2].Item[d3].Item;
        set => this[d1].Item[d2].Item[d3].Item = value;
    }

    public bool ContainsKey(string d1, string d2, string d3, string d4)
    {
        return ContainsKey(d1)
            && this[d1].Item.ContainsKey(d2)
            && this[d1].Item[d2].Item != null
            && this[d1].Item[d2].Item.ContainsKey(d3)
            && this[d1].Item[d2].Item[d3].Item != null
            && this[d1].Item[d2].Item[d3].Item.ContainsKey(d4)
            ;
    }

    public override string ToXml()
    {
        var xml = Xml.ToXml(this, true, ExtraTypes);
        if(Name.IsEmpty()) return xml;

        if (Name != ActualName)
            return xml
                .Replace($"<{ActualName}", $"<{Name}")
                .Replace($"</{ActualName}", $"</{Name}")
                .Replace($" Name=\"{Name}\"", "")
                ;

        return xml;
    }

    public new static AssocArray4D<T> FromXml(string xml)
    {
        var name = xml.TokenBetween("<", ">").FirstToken();
        if (name != ActualName)
            xml = xml
                .Replace($"<{name}", $"<{ActualName} Name=\"{name}\"")
                .Replace($"</{name}", $"</{ActualName}")
                ;

        return Xml.FromXml<AssocArray4D<T>>(xml, ExtraTypes);
    }

}