using System;
using System.Xml.Serialization;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings.Generics;

public class AssocTimelineItem<TAxis, TItem> : BasicAssocItem
    where TAxis : class, IAssocItem, new()
    where TItem : class, IAssocItem, new()
{
    [XmlAttribute] public DateTime At { get; set; } = DateTime.Now;
    public AssocCubeOfT2<TAxis, TItem> FirstAxis { get; set; } = new();
}