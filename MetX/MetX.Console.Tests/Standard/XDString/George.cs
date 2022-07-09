using System.Xml.Serialization;
using MetX.Standard.XDString.Generics;

namespace MetX.Console.Tests.Standard.XDString;

public class George : AssocArray1D<George, GeorgeItem>
{
    [XmlAttribute]
    public string GeorgeName {get; set; }
        
}