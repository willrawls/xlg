using System;
using MetX.Standard.Library.Encryption;
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

public class JustAnAssocItem : AssocItem
{
    public string JustAName {get; set; }
    public Guid JustAGuid { get; set; } = Guid.NewGuid();

    public JustAnAssocItem(string key) : base(key)
    {
        JustAName = SuperRandom.NextHexString(8);
    }

    public JustAnAssocItem() : base(SuperRandom.NextHexString(8))
    {
        JustAName = SuperRandom.NextHexString(8);
    }
}