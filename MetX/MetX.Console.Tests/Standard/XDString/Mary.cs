using System;
using MetX.Standard.Library.ML;

namespace MetX.Console.Tests.Standard.XDString;

public class Mary : ListSerializesToXml<Mary, MaryItem>
{
    public Guid TestGuid { get; set; } = Guid.NewGuid();
}