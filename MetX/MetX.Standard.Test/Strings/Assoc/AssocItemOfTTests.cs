using MetX.Standard.Strings.Generics;
using MetX.Standard.Test.TestingClasses;

namespace MetX.Standard.Test.Strings.Assoc;

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