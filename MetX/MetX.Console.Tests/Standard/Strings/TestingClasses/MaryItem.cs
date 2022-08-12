using System;

namespace MetX.Console.Tests.Standard.Strings.TestingClasses;

public class MaryItem
{
    public string MaryItemName {get; set; }
    public Guid MaryItemTestGuid { get; set; } = Guid.NewGuid();
}