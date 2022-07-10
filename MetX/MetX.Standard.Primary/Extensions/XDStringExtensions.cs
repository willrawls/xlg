using System.Runtime.ExceptionServices;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.ML;
using MetX.Standard.XDString.Generics;
using MetX.Standard.XDString.Interfaces;

namespace MetX.Standard.Primary.Extensions;

public static class XDStringExtensions
{
    public static string ToXml<TFirstAxis, TSecondAxis, TItem>(
        this TwoDimensionalAssocArray<TFirstAxis, TSecondAxis, TItem> twoDimensionalAssocArray2d)
        where TFirstAxis : class, IAssocItem 
        where TSecondAxis : class, IAssocItem
        where TItem : class, IAssocItem, new()
    {
        if (twoDimensionalAssocArray2d == null || twoDimensionalAssocArray2d.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(twoDimensionalAssocArray2d);
        return xml;
    }

    public static TwoDimensionalAssocArray<TFirstAxis, TSecondAxis, TItem> FromXml<TFirstAxis, TSecondAxis, TItem>(
        string xml)
        where TFirstAxis : class, IAssocItem 
        where TSecondAxis : class, IAssocItem
        where TItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<TwoDimensionalAssocArray<TFirstAxis, TSecondAxis, TItem>>(xml);
    }

    public static string ToXml<T>(this TwoDimensionalAssocArray<T> twoDimensionalAssocArray)
        where T : class, IAssocItem, new()
    {
        if (twoDimensionalAssocArray == null || twoDimensionalAssocArray.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(twoDimensionalAssocArray);
        return xml;
    }

    public static TwoDimensionalAssocArray<TAssocItem> FromXml<TAssocItem>(string xml)
        where TAssocItem : class, IAssocItem , new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<TwoDimensionalAssocArray<TAssocItem>>(xml);
    }
}