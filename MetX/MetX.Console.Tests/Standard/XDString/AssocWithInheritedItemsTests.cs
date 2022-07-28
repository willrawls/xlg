using System;
using MetX.Console.Tests.Standard.XDString.TestingClasses;
using MetX.Standard.XDString.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.XDString;

[TestClass]
public class AssocWithInheritedItemsTests
{
    [TestMethod]
    public void AssocItem_Simple()
    {
        var array = new AssocWithInheritedItems<OwenAssocItem>();
        var testGuid = Guid.NewGuid();
        var owen = new Fred();
        var fredItem = new Fred();
        var otherItem = new OwenAssocItem();

        var data = new AssocRelativeItem<Fred>("Fred", fredItem, "George1DArray", testGuid, "Mary", otherItem.ID);

        Assert.AreEqual("Fred", data.Key);
        Assert.AreEqual("George1DArray", data.Value);
        Assert.AreEqual(testGuid, data.ID);
        Assert.AreEqual("Mary", data.Name);
        Assert.AreEqual(otherItem.ID, data.Parent);
    }
}