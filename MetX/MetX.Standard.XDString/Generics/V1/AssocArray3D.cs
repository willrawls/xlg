namespace MetX.Standard.Strings.Generics.V1;

public class AssocArray3D<T3DParent, T2DParent, T1DParent, TItem> 
    : AssocArray1D<T3DParent, AssocArray1D<T2DParent, AssocArray1D<T1DParent, TItem>>> where TItem : class, new() where T1DParent : class where T3DParent : class where T2DParent : class
{
    public TItem this[string d1, string d2, string d3]
    {
        get => this[d1].Item[d2].Item[d3].Item;
        set => this[d1].Item[d2].Item[d3].Item = value;
    }
    

    public new AssocArray1D<T1DParent, TItem> this[string d1, string d2]
    {
        get => this[d1].Item[d2].Item;
        set => this[d1].Item[d2].Item = value;
    }

    public override string ToXml(bool removeNamespaces, bool normalizeRootNodeName)
    {
        var xml = base.ToXml(removeNamespaces, normalizeRootNodeName);
        var targetNameOfRootElement = typeof(AssocArray1D<T3DParent, AssocArray1D<T2DParent, AssocArray1D<T1DParent, TItem>>>).Name;
        if(targetNameOfRootElement != "AssocArray" && normalizeRootNodeName)
            xml = xml
                .Replace("<AssocArray", $"<{targetNameOfRootElement}")
                .Replace("</AssocArray", $"</{targetNameOfRootElement}");
        return xml;
    }
}