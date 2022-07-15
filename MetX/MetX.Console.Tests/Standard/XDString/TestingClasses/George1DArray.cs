using System;
using System.Xml.Serialization;
using MetX.Standard.XDString.Generics.V1;

namespace MetX.Console.Tests.Standard.XDString.TestingClasses;

[Serializable]
public class George1DArray : AssocArray1D<George1DArray, GeorgeItem>
{
    [XmlAttribute]
    public string GeorgeName {get; set; }
        
}