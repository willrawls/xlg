﻿using System;
using MetX.Standard.Strings;

namespace MetX.Console.Tests.Standard.Strings.TestingClasses;

public class MaryAssocItem : TimeTrackingAssocItem
{
    public string MaryAssocItemName {get; set; }
    public Guid MaryAssocItemTestGuid { get; set; } = Guid.NewGuid();

    public MaryAssocItem()
    {
        Key = "Mary";
    }
}