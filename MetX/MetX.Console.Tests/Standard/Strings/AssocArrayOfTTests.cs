using MetX.Console.Tests.Standard.Strings.TestingClasses;
using MetX.Standard.Strings.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.Strings;

[TestClass]
public class AssocArrayOfTTests
{
    [TestMethod]
    public void AssocArrayOfT_Simple()
    {
        var item = new Fred();
        var data = new AssocArrayOfT<Fred>
        {
            ["Fred"] =
            {
                Item = item
            }
        };
        Assert.AreEqual(item.TestGuid, data["Fred"].Item.TestGuid);
    }

    [TestMethod]
    public void AssocArrayOfT_AssocTypeOfInt()
    {
        var assocType = new AssocType<int>(12);
        var data = new AssocArrayOfT<AssocType<int>>
        {
            ["Fred"] =
            {
                Item = assocType
            }
        };
        Assert.AreEqual(assocType.Target, data["Fred"].Item.Target);
    }
    [TestMethod]
    public void AssocArrayOfAssocArrayOfT_Simple()
    {
        var fred = new Fred();
        var data = new AssocArrayOfT<AssocArrayOfT<Fred>>();
        data["Fred"].Item = new AssocArrayOfT<Fred>();
        data["Fred"].Item["George1DArray"].Item = fred;
        Assert.AreEqual(fred.TestGuid, data["Fred"].Item["George1DArray"].Item.TestGuid);

        fred = new Fred();
        data = new AssocArrayOfT<AssocArrayOfT<Fred>>();
        data["Fred"].Item["George1DArray"].Item = fred;
        Assert.AreEqual(fred.TestGuid, data["Fred"].Item["George1DArray"].Item.TestGuid);
    }
}