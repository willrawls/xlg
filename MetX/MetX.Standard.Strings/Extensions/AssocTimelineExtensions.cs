using MetX.Standard.Strings.Generics;
using MetX.Standard.Strings.Interfaces;
using MetX.Standard.Strings.ML;

namespace MetX.Standard.Strings.Extensions;

public static class AssocTimelineExtensions
{
    /*
    #region One type

    public static string ToXml<T>(this AssocTimeline<T> AssocTimeline)
        where T : class, IAssocItem, new()
    {
        if (AssocTimeline == null || AssocTimeline.FirstAxis.Count == 0)
            return "";

        var xml = Xml.ToXml(AssocTimeline);
        return xml;
    }

    public static AssocTimeline<TAssocItem> FromXml<TAssocItem>(string xml)
        where TAssocItem : class, IAssocItem , new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocTimeline<TAssocItem>>(xml);
    }
    #endregion
    */

    #region One type for axis, one for item
    public static string ToXml<TAxis, TItem>(
        this AssocTimeline<TAxis, TItem> assocTimeline)
        where TAxis : class, IAssocItem, new() 
        where TItem : class, IAssocItem, new()
    {
        if (assocTimeline?.Timeline == null || assocTimeline.Timeline.Count == 0)
            return "";

        var xml = Xml.ToXml(assocTimeline);
        return xml;
    }

    public static AssocTimeline<TAxis, TItem> FromXml<TAxis, TItem>(string xml)
        where TAxis : class, IAssocItem , new()
        where TItem : class, IAssocItem, new()
    {
        return xml.IsEmpty() 
            ? new() 
            : Xml.FromXml<AssocTimeline<TAxis, TItem>>(xml);
    }
    #endregion
}