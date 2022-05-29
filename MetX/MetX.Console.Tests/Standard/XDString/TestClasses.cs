using System;
using System.IO;
using System.Xml.Serialization;
using MetX.Standard.Library.ML;
using MetX.Standard.XDString.Generics;
using MetX.Standard.XDString.Support;

namespace MetX.Console.Tests.Standard.XDString;

public class JustAClass
{
    public string JustAClassName { get; set; }
}

public class A4DAA : AssocArray4D<A4DAA, George, GeorgeItem, Fred, FredItem>
{
    public string A4DAAName { get; set; }

    public new static A4DAA FromXml(string xml)
    {
        /*var name = xml.TokenBetween("<", ">").FirstToken();
        if (name != ActualName)
            xml = xml
                    .Replace($"<{name}", $"<AssocArray Name=\"{name}\"")
                    .Replace($"</{name}", $"</AssocArray ")
                ;*/

        using var sr = new StringReader(xml);
        var xmlSerializer = GetSerializer(typeof(A4DAA), ExtraTypes());

        return xmlSerializer.Deserialize(sr) as A4DAA;
    }
}

public class FredItem
{
    public string FredItemName {get; set; }
    public Guid FredItemTestGuid { get; set; } = Guid.NewGuid();
}

public class Fred : ListSerializesToXml<Fred, FredItem>
{
    public Guid TestGuid { get; set; } = Guid.NewGuid();
}

public class George : AssocArray1D<George, GeorgeItem>
{
    [XmlAttribute]
    public string GeorgeName {get; set; }
        
}

public class GeorgeItem
{
    [XmlAttribute]
    public string ItemName {get; set; }
}


