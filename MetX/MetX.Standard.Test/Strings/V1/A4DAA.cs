using System.Xml.Serialization;
using MetX.Standard.Strings.Generics.V1;
using MetX.Standard.Test.TestingClasses;

namespace MetX.Standard.Test.Strings.V1;

[Serializable]
public class A4DAA : AssocArray4D<A4DAA, George1DArray, GeorgeItem, Fred, FredItem>
{
    [XmlAttribute] public string A4DAAName { get; set; }

    /*
    public new static A4DAA FromXml(string xml)
    {
        using var sr = new StringReader(xml);
        var xmlSerializer = Xml.Serializer(typeof(A4DAA), ExtraTypes());

        return xmlSerializer.Deserialize(sr) as A4DAA;
    }*/
}