using System;
using MetX.Standard.Library.ML;

namespace MetX.Console.Tests.Standard.Strings.TestingClasses;

public class Fred : ListSerializesToXml<Fred, FredItem>
{
    public Guid TestGuid { get; set; } = Guid.NewGuid();
}
