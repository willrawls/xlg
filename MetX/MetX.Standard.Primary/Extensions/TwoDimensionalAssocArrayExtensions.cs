using System.Runtime.ExceptionServices;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.ML;
using MetX.Standard.XDString.Generics;
using MetX.Standard.XDString.Interfaces;

namespace MetX.Standard.Primary.Extensions;

public static class TwoDimensionalAssocArrayExtensions
{
    #region Three different types

    public static string ToXml<TFirstAxis, TSecondAxis, TItem>(
        this TwoDimensionalAssocArray<TFirstAxis, TSecondAxis, TItem> assocArray)
        where TFirstAxis : class, IAssocItem 
        where TSecondAxis : class, IAssocItem
        where TItem : class, IAssocItem, new()
    {
        if (assocArray == null || assocArray.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(assocArray);
        return xml;
    }

    public static TwoDimensionalAssocArray<TFirstAxis, TSecondAxis, TItem> FromXml<TFirstAxis, TSecondAxis, TItem>(string xml)
        where TFirstAxis : class, IAssocItem 
        where TSecondAxis : class, IAssocItem
        where TItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<TwoDimensionalAssocArray<TFirstAxis, TSecondAxis, TItem>>(xml);
    }
    #endregion

    #region One type

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
    #endregion

    #region One type for axis, one for item
    public static string ToXml<TAxis, TItem>(
        this TwoDimensionalAssocArray<TAxis, TItem> twoDimensionalAssocArray)
        where TAxis : class, IAssocItem 
        where TItem : class, IAssocItem, new()
    {
        if (twoDimensionalAssocArray == null || twoDimensionalAssocArray.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(twoDimensionalAssocArray);
        return xml;
    }

    public static TwoDimensionalAssocArray<TAxis, TItem> FromXml<TAxis, TItem>(string xml)
        where TAxis : class, IAssocItem 
        where TItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<TwoDimensionalAssocArray<TAxis, TItem>>(xml);
    }
    #endregion
}