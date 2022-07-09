using System;
using MetX.Standard.XDString;

namespace MetX.Console.Tests.Standard.XDString;

public class FredItem
{
    public string FredItemName {get; set; }
    public Guid FredItemTestGuid { get; set; } = Guid.NewGuid();
}

public class FredAssocItem : AssocItem
{
    public string FredAssocItemName {get; set; }
    public Guid FredAssocItemTestGuid { get; set; } = Guid.NewGuid();

    public FredAssocItem()
    {
        Key = "Fred";
    }
}