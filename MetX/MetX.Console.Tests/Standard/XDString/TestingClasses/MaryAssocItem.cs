using System;
using MetX.Standard.XDString;

namespace MetX.Console.Tests.Standard.XDString.TestingClasses;

public class MaryAssocItem : TimeTrackingAssocItem
{
    public string MaryAssocItemName {get; set; }
    public Guid MaryAssocItemTestGuid { get; set; } = Guid.NewGuid();

    public MaryAssocItem()
    {
        Key = "Mary";
    }
}