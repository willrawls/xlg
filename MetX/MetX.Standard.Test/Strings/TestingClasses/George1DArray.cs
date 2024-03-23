using System;
using System.Xml.Serialization;
using MetX.Standard.Strings.Generics.V1;

namespace MetX.Console.Tests.Standard.Strings.TestingClasses;

[Serializable]
public class George1DArray : AssocArray1D<George1DArray, GeorgeItem>
{
    [XmlAttribute]
    public string GeorgeName {get; set; }
        
}