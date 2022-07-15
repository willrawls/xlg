using System;
using System.IO;
using System.Xml.Serialization;
using MetX.Console.Tests.Standard.XDString.TestingClasses;
using MetX.Standard.XDString.Generics.V1;

namespace MetX.Console.Tests.Standard.XDString.V1;

[Serializable]
public class A4DAA : AssocArray4D<A4DAA, George1DArray, GeorgeItem, Fred, FredItem>
{
    [XmlAttribute] public string A4DAAName { get; set; }

    /*
    public new static A4DAA FromXml(string xml)
    {
        using var sr = new StringReader(xml);
        var xmlSerializer = GetSerializer(typeof(A4DAA), ExtraTypes());

        return xmlSerializer.Deserialize(sr) as A4DAA;
    }*/
}