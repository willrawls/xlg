using System;
using MetX.Standard.Strings;

namespace MetX.Console.Tests.Standard.Strings.TestingClasses;

public class FredAssocItem : TimeTrackingAssocItem
{
    public string FredAssocItemName {get; set; }
    public Guid FredAssocItemTestGuid { get; set; } = Guid.NewGuid();

    public FredAssocItem()
    {
        Key = "Fred";
    }
}