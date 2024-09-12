using System;
using System.Xml.Serialization;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Generics.V3;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings.Generics;

public class AssocCubeOfT2<TAxis, TItem> : AssocCubeOf<TAxis, TAxis,TAxis, TItem>
    where TAxis : class, IAssocItem, new()
    where TItem : class, IAssocItem, new()
{
}