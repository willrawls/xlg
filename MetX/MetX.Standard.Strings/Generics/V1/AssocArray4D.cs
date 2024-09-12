using System;
using System.IO;
using System.Runtime.CompilerServices;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

namespace MetX.Standard.Strings.Generics.V1;

public class AssocArray4D<T4DParent,T3DParent,T2DParent, T1DParent, TItem> 
    : AssocArray1D<T4DParent, AssocArray3D<T3DParent, T2DParent, T1DParent, TItem>> 
    where T4DParent : class, new()
    where TItem : class, new()
    where T3DParent : class, new()
    where T2DParent : class, new()
    where T1DParent : class, new()
{
    private static Type ActualType = typeof(AssocArray1D<T4DParent, AssocArray3D<T3DParent, T2DParent, T1DParent, TItem>>);

    public static string ActualName => "AssocArray"; // ActualType.Name.Replace("`1", "Of") + typeof(TItem).Name;

    public TItem this[string d1, string d2, string d3, string d4]
    {
        get => this[d1].Item[d2].Item[d3].Item[d4].Item;
        set => this[d1].Item[d2].Item[d3].Item[d4].Item = value;
    }
    
    public AssocArray1D<T1DParent, TItem> this[string d1, string d2, string d3]
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

    /*
    public override string ToXml(bool removeNamespaces = true, bool normalizeRootNodeName = true)
    {
        var xml = base.ToXml(removeNamespaces, normalizeRootNodeName);
        if(Name.IsEmpty()) return xml;

        if (Name != ActualName && normalizeRootNodeName)
            xml = xml
                .Replace($"<{ActualName}", $"<{Name}")
                .Replace($"</{ActualName}", $"</{Name}")
                .Replace($" Name=\"{Name}\"", "")
                ;

        return xml;
    }
    */

    public new static TConcreteType FromXml<TConcreteType>(string xml) 
        where TConcreteType : class, new()
    {
        var name = xml.TokenBetween("<", ">").FirstToken();
        if (name != ActualName)
            xml = xml
                .Replace($"<{name}", $"<{ActualName} Name=\"{name}\"")
                .Replace($"</{name}", $"</{ActualName}")
                ;

        using var sr = new StringReader(xml);
        return GetSerializer(ActualType, ExtraTypes<TConcreteType>()).Deserialize(sr) as TConcreteType;
    }

}