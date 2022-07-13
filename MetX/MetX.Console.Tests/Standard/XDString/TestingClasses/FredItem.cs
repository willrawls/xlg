using System;

namespace MetX.Console.Tests.Standard.XDString.TestingClasses;

public class FredItem
{
    public string FredItemName {get; set; }
    public Guid FredItemTestGuid { get; set; } = Guid.NewGuid();
}