using System;
using System.Xml.Serialization;
using MetX.Standard.XDString.Interfaces;

namespace MetX.Standard.XDString.Generics;

public class AssocTimelineItem<TAxis, TItem> : AssocItem
    where TAxis : class, IAssocItem
    where TItem : class, IAssocItem, new()
{
    [XmlAttribute] public DateTime At { get; set; } = DateTime.Now;
    public AssocCube<TAxis, TItem> FirstAxis { get; set; } = new();
}