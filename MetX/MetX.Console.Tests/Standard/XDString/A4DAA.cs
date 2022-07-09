using System.IO;
using MetX.Standard.XDString.Generics;

namespace MetX.Console.Tests.Standard.XDString;

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