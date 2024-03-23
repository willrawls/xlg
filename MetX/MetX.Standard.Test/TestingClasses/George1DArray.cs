using System.Xml.Serialization;
using MetX.Standard.Strings.Generics.V1;

namespace MetX.Standard.Test.TestingClasses;

[Serializable]
public class George1DArray : AssocArray1D<George1DArray, GeorgeItem>
{
    [XmlAttribute]
    public string GeorgeName { get; set; }

}