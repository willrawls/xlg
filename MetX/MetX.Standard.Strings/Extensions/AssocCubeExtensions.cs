using MetX.Standard.Strings.Generics;
using MetX.Standard.Strings.Interfaces;
using MetX.Standard.Strings.ML;

namespace MetX.Standard.Strings.Extensions;

public static class AssocCubeExtensions
{
    #region All different types

    public static string ToXml<TFirstAxis, TSecondAxis, TThirdAxis, TItem>(
        this AssocCubeOf<TFirstAxis, TSecondAxis, TThirdAxis, TItem> AssocCube)
        where TFirstAxis : class, IAssocItem, new() 
        where TSecondAxis : class, IAssocItem, new()
        where TThirdAxis : class, IAssocItem, new()
        where TItem : class, IAssocItem, new()
    {
        if (AssocCube == null || AssocCube.Count == 0)
            return "";

        var xml = Xml.ToXml(AssocCube);
        return xml;
    }

    public static AssocCubeOf<TFirstAxis, TSecondAxis, TThirdAxis, TItem> 
        FromXml<TFirstAxis, TSecondAxis, TThirdAxis, TItem>(string xml)
        where TFirstAxis : class, IAssocItem, new() 
        where TSecondAxis : class, IAssocItem, new()
        where TThirdAxis : class, IAssocItem, new()
        where TItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocCubeOf<TFirstAxis, TSecondAxis, TThirdAxis, TItem>>(xml);
    }
    #endregion

    #region One type

    public static string ToXml<T>(this AssocCubeOf<T> AssocCube)
        where T : class, IAssocItem, new()
    {
        if (AssocCube == null || AssocCube.Count == 0)
            return "";

        var xml = Xml.ToXml(AssocCube);
        return xml;
    }

    public static AssocCubeOf<TAssocItem> FromXml<TAssocItem>(string xml)
        where TAssocItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocCubeOf<TAssocItem>>(xml);
    }
    #endregion

    #region One type for axis, one for item
    public static string ToXml<TAxis, TItem>(
        this AssocCubeOfT2<TAxis, TItem> assocCube)
        where TAxis : class, IAssocItem, new() 
        where TItem : class, IAssocItem, new()
    {
        if (assocCube == null || assocCube.Count == 0)
            return "";

        var xml = Xml.ToXml(assocCube);
        return xml;
    }

    public static AssocCubeOfT2<TAxis, TItem> FromXml<TAxis, TItem>(string xml)
        where TAxis : class, IAssocItem, new() 
        where TItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocCubeOfT2<TAxis, TItem>>(xml);
    }
    #endregion
}