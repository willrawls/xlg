using System;
using MetX.Standard.Strings.Generics;
using MetX.Standard.Strings.Interfaces;
using MetX.Standard.Strings.ML;

namespace MetX.Standard.Strings.Extensions;

public static class AssocSheetExtensions
{
    #region Three different types

    public static string ToXml<TFirstAxis, TSecondAxis, TItem>(
        this AssocSheet<TFirstAxis, TSecondAxis, TItem> assocSheet, Type[] extraTypes = null)
        where TFirstAxis : class, IAssocItem , new()
        where TSecondAxis : class, IAssocItem, new()
        where TItem : class, IAssocItem, new()
    {
        if (assocSheet == null || assocSheet.Count == 0)
            return "";

        extraTypes = Xml.ExtraTypes<TFirstAxis, TSecondAxis, TItem, string, AssocSheet<TFirstAxis, TSecondAxis, TItem>>(extraTypes);
        var xml = Xml.ToXml(assocSheet, true, extraTypes);
        return xml;
    }

    public static AssocSheet<TFirstAxis, TSecondAxis, TItem> FromXml<TFirstAxis, TSecondAxis, TItem>(string xml)
        where TFirstAxis : class, IAssocItem, new() 
        where TSecondAxis : class, IAssocItem, new()
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
        if (assocSheet == null || assocSheet.Count == 0)
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
        where TAxis : class, IAssocItem , new()
        where TItem : class, IAssocItem, new()
    {
        if (assocSheet == null || assocSheet.Count == 0)
            return "";

        var xml = Xml.ToXml(assocSheet);
        return xml;
    }

    public static AssocSheet<TAxis, TItem> FromXml<TAxis, TItem>(string xml)
        where TAxis : class, IAssocItem , new()
        where TItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocSheet<TAxis, TItem>>(xml);
    }
    #endregion
}