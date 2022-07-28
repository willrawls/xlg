using System;
using System.Xml.Serialization;
using MetX.Standard.XDString.Interfaces;

namespace MetX.Standard.XDString.Generics;

public class AssocSpacetime : AssocSheet<DateTimeAssocType, AssocCube<BasicAssocItem>, VectorAssocType>, IAssocItem
{
}