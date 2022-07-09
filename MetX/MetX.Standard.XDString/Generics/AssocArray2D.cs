using System;
using System.Linq;
using System.Runtime.CompilerServices;
using MetX.Standard.XDString.Interfaces;
using MetX.Standard.XDString.Support;

namespace MetX.Standard.XDString.Generics;

public class AssocArray2D<T2DParent, T1DParent, TItem> : AssocArray1D<T2DParent, AssocArray1D<T1DParent, TItem>> where TItem : class, new() where T2DParent : class where T1DParent : class
{
    public TItem this[string d1, string d2]
    {
        get => this[d1].Item[d2].Item;
        set => this[d1].Item[d2].Item = value;
    }
    
    public override string ToXml(bool removeNamespaces, bool normalizeRootNodeName)
    {
        var xml = base.ToXml(removeNamespaces, normalizeRootNodeName);
        var targetNameOfRootElement = typeof(AssocArray2D<T2DParent, T1DParent, TItem>).Name;
        if(targetNameOfRootElement != "AssocArray" && normalizeRootNodeName)
            xml = xml
                .Replace("<AssocArray", $"<{targetNameOfRootElement}")
                .Replace("</AssocArray", $"</{targetNameOfRootElement}");
        return xml;
    }

}

public class AssocArray2dRethink<TFirstAxis, TSecondAxis, TItem>
    where TFirstAxis : class, IAssocItem 
    where TSecondAxis : class, IAssocItem
    where TItem : class, IAssocItem, new()
{
    public AssocArray<AssocArray<TItem>> FirstAxis = new();

    public TItem this[string firstAxisKey, string secondAxisKey]
    {
        get
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return null;

            return FirstAxis[firstAxisKey].Item[secondAxisKey].Item;
        }
        set
        {
            if (firstAxisKey.IsEmpty() || secondAxisKey.IsEmpty())
                return;

            var f = FirstAxis[firstAxisKey];
            var s = f.Item[secondAxisKey];
             s.Item = value;
        }
    }


    /*
    public override string ToXml(bool removeNamespaces, bool normalizeRootNodeName)
    {
        var xml = base.ToXml(removeNamespaces, normalizeRootNodeName);
        var targetNameOfRootElement = typeof(AssocArray2D<TFirstAxis, TSecondAxis, TItem>).Name;
        if(targetNameOfRootElement != "AssocArray" && normalizeRootNodeName)
            xml = xml
                .Replace("<AssocArray", $"<{targetNameOfRootElement}")
                .Replace("</AssocArray", $"</{targetNameOfRootElement}");
        return xml;
    }
    */

}