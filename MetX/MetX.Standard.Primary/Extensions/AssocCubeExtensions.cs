using MetX.Standard.Library.ML;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Generics;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Primary.Extensions;

public static class AssocCubeExtensions
{
    #region All different types

    public static string ToXml<TFirstAxis, TSecondAxis, TThirdAxis, TItem>(
        this AssocCubeOf<TFirstAxis, TSecondAxis, TThirdAxis, TItem> AssocCube)
        where TFirstAxis : class, IAssocItem 
        where TSecondAxis : class, IAssocItem
        where TThirdAxis : class, IAssocItem
        where TItem : class, IAssocItem, new()
    {
        if (AssocCube == null || AssocCube.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(AssocCube);
        return xml;
    }

    public static AssocCubeOf<TFirstAxis, TSecondAxis, TThirdAxis, TItem> 
        FromXml<TFirstAxis, TSecondAxis, TThirdAxis, TItem>(string xml)
        where TFirstAxis : class, IAssocItem 
        where TSecondAxis : class, IAssocItem
        where TThirdAxis : class, IAssocItem
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
        if (AssocCube == null || AssocCube.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(AssocCube);
        return xml;
    }

    public static AssocCubeOf<TAssocItem> FromXml<TAssocItem>(string xml)
        where TAssocItem : class, IAssocItem , new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocCubeOf<TAssocItem>>(xml);
    }
    #endregion

    #region One type for axis, one for item
    public static string ToXml<TAxis, TItem>(
        this AssocCubeOfT2<TAxis, TItem> assocCube)
        where TAxis : class, IAssocItem 
        where TItem : class, IAssocItem, new()
    {
        if (assocCube == null || assocCube.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(assocCube);
        return xml;
    }

    public static AssocCubeOfT2<TAxis, TItem> FromXml<TAxis, TItem>(string xml)
        where TAxis : class, IAssocItem 
        where TItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocCubeOfT2<TAxis, TItem>>(xml);
    }
    #endregion
}