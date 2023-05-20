using MetX._471.Strings.Interfaces;
using System;
using System.Xml.Serialization;

namespace MetX._471.Strings.Generics;

public class AssocTimelineItem<TAxis, TItem> : BasicAssocItem
    where TAxis : class, IAssocItem
    where TItem : class, IAssocItem, new()
{
    [XmlAttribute] public DateTime At { get; set; } = DateTime.Now;
    public AssocCubeOfT2<TAxis, TItem> FirstAxis { get; set; } = new();
}