using System;
using MetX.Standard.XDString;

namespace MetX.Console.Tests.Standard.XDString.TestingClasses;

public class FredAssocItem : AssocItem
{
    public string FredAssocItemName {get; set; }
    public Guid FredAssocItemTestGuid { get; set; } = Guid.NewGuid();

    public FredAssocItem()
    {
        Key = "Fred";
    }
}