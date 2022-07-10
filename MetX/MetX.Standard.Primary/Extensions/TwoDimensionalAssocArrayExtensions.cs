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
        this AssocSheet<TFirstAxis, TSecondAxis, TItem> assocSheet)
        where TFirstAxis : class, IAssocItem 
        where TSecondAxis : class, IAssocItem
        where TItem : class, IAssocItem, new()
    {
        if (assocSheet == null || assocSheet.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(assocSheet);
        return xml;
    }

    public static AssocSheet<TFirstAxis, TSecondAxis, TItem> FromXml<TFirstAxis, TSecondAxis, TItem>(string xml)
        where TFirstAxis : class, IAssocItem 
        where TSecondAxis : class, IAssocItem
        where TItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocSheet<TFirstAxis, TSecondAxis, TItem>>(xml);
    }
    #endregion

    #region One type

    public static string ToXml<T>(this AssocSheet<T> assocSheet)
        where T : class, IAssocItem, new()
    {
        if (assocSheet == null || assocSheet.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(assocSheet);
        return xml;
    }

    public static AssocSheet<TAssocItem> FromXml<TAssocItem>(string xml)
        where TAssocItem : class, IAssocItem , new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocSheet<TAssocItem>>(xml);
    }
    #endregion

    #region One type for axis, one for item
    public static string ToXml<TAxis, TItem>(
        this AssocSheet<TAxis, TItem> assocSheet)
        where TAxis : class, IAssocItem 
        where TItem : class, IAssocItem, new()
    {
        if (assocSheet == null || assocSheet.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(assocSheet);
        return xml;
    }

    public static AssocSheet<TAxis, TItem> FromXml<TAxis, TItem>(string xml)
        where TAxis : class, IAssocItem 
        where TItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocSheet<TAxis, TItem>>(xml);
    }
    #endregion
}