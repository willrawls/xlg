﻿using System;
using MetX.Standard.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.Strings;

[TestClass]
public class AssocRelativeItemTests
{
    [TestMethod]
    public void AssocItem_Simple()
    {
        var testGuid = Guid.NewGuid();
        var otherItem = new TimeTrackingAssocItem("Other");
        var data = new AssocRelativeItem("Fred", "George1DArray", testGuid, "Mary", otherItem.ID);
            
        Assert.AreEqual("Fred", data.Key);
        Assert.AreEqual("George1DArray", data.Value);
        Assert.AreEqual(testGuid, data.ID);
        Assert.AreEqual("Mary", data.Name);
        Assert.AreEqual(otherItem.ID, data.Parent);
    }
}