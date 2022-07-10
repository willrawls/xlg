using System.Xml.Serialization;
using MetX.Standard.XDString.Generics.V1;

namespace MetX.Console.Tests.Standard.XDString.TestingClasses;

public class George : AssocArray1D<George, GeorgeItem>
{
    [XmlAttribute]
    public string GeorgeName {get; set; }
        
}