using System;
using MetX.Console.Tests.Standard.Strings.TestingClasses;
using MetX.Standard.Strings.Generics;
using MetX.Standard.Strings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.Strings;

[TestClass]
public class AssocItemOfTTests
{
    [TestMethod]
    public void AssocItemOfT_Simple()
    {
        var testGuid = Guid.NewGuid();
        var fred = new Fred();
        var fredItem = new Fred();
        var otherItem = new AssocItemOfT<Fred>("Other", fred);

        var data = new AssocRelativeItem<Fred>("Fred", fredItem, "George1DArray", testGuid, "Mary", otherItem.ID);

        Assert.AreEqual("Fred", data.Key);
        Assert.AreEqual("George1DArray", data.Value);
        Assert.AreEqual(testGuid, data.ID);
        Assert.AreEqual("Mary", data.Name);
        Assert.AreEqual(otherItem.ID, data.Parent);
    }
}

public class OwenAssocItem : IAssocItem
{
    public string OwenItemName {get; set; }
    public Guid OwenItemTestGuid { get; set; } = Guid.NewGuid();

    public string Key { get; set; }
    public string Value { get; set; }
    public string Name { get; set; }
    public Guid ID { get; set; }
    public int Number { get; set; }
    public string Category { get; set; }
}