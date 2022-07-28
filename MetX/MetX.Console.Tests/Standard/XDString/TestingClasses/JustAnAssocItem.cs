using System;
using MetX.Standard.Library.Encryption;
using MetX.Standard.XDString;

namespace MetX.Console.Tests.Standard.XDString.TestingClasses;

public class JustAnAssocItem : TimeTrackingAssocItem
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