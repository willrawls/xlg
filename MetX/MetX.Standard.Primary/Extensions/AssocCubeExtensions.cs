using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.ML;
using MetX.Standard.XDString.Generics;
using MetX.Standard.XDString.Interfaces;

namespace MetX.Standard.Primary.Extensions;

public static class AssocCubeExtensions
{
    #region All different types

    public static string ToXml<TFirstAxis, TSecondAxis, TThirdAxis, TItem>(
        this AssocCube<TFirstAxis, TSecondAxis, TThirdAxis, TItem> AssocCube)
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

    public static AssocCube<TFirstAxis, TSecondAxis, TThirdAxis, TItem> 
        FromXml<TFirstAxis, TSecondAxis, TThirdAxis, TItem>(string xml)
        where TFirstAxis : class, IAssocItem 
        where TSecondAxis : class, IAssocItem
        where TThirdAxis : class, IAssocItem
        where TItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocCube<TFirstAxis, TSecondAxis, TThirdAxis, TItem>>(xml);
    }
    #endregion

    #region One type

    public static string ToXml<T>(this AssocCube<T> AssocCube)
        where T : class, IAssocItem, new()
    {
        if (AssocCube == null || AssocCube.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(AssocCube);
        return xml;
    }

    public static AssocCube<TAssocItem> FromXml<TAssocItem>(string xml)
        where TAssocItem : class, IAssocItem , new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocCube<TAssocItem>>(xml);
    }
    #endregion

    #region One type for axis, one for item
    public static string ToXml<TAxis, TItem>(
        this AssocCube<TAxis, TItem> AssocCube)
        where TAxis : class, IAssocItem 
        where TItem : class, IAssocItem, new()
    {
        if (AssocCube == null || AssocCube.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(AssocCube);
        return xml;
    }

    public static AssocCube<TAxis, TItem> FromXml<TAxis, TItem>(string xml)
        where TAxis : class, IAssocItem 
        where TItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocCube<TAxis, TItem>>(xml);
    }
    #endregion
}