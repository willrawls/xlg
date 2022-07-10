using System;
using MetX.Standard.Library.ML;

namespace MetX.Console.Tests.Standard.XDString.TestingClasses;

public class Fred : ListSerializesToXml<Fred, FredItem>
{
    public Guid TestGuid { get; set; } = Guid.NewGuid();
}