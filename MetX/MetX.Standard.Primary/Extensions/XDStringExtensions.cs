using System.Runtime.ExceptionServices;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.ML;
using MetX.Standard.XDString.Generics;
using MetX.Standard.XDString.Interfaces;

namespace MetX.Standard.Primary.Extensions;

public static class XDStringExtensions
{
    public static string ToXml<TFirstAxis, TSecondAxis, TItem>(
        this AssocArray2dRethink<TFirstAxis, TSecondAxis, TItem> assocArray2d)
        where TFirstAxis : class, IAssocItem 
        where TSecondAxis : class, IAssocItem
        where TItem : class, IAssocItem, new()
    {
        if (assocArray2d == null || assocArray2d.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(assocArray2d);
        return xml;
    }

    public static AssocArray2dRethink<TFirstAxis, TSecondAxis, TItem> FromXml<TFirstAxis, TSecondAxis, TItem>(
        string xml)
        where TFirstAxis : class, IAssocItem 
        where TSecondAxis : class, IAssocItem
        where TItem : class, IAssocItem, new()
    {
        if (xml.IsEmpty())
            return new();

        return Xml.FromXml<AssocArray2dRethink<TFirstAxis, TSecondAxis, TItem>>(xml);
    }
}