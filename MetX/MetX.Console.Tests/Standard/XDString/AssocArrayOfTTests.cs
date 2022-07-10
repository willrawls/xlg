using MetX.Console.Tests.Standard.Library;
using MetX.Console.Tests.Standard.XDString.TestingClasses;
using MetX.Standard.XDString.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.XDString;

[TestClass]
public class AssocArrayOfTTests
{
    [TestMethod]
    public void AssocArrayOfT_Simple()
    {
        var item = new Fred();
        var data = new AssocArray<Fred>
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
        var data = new AssocArray<AssocType<int>>
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
        var data = new AssocArray<AssocArray<Fred>>();
        data["Fred"].Item = new AssocArray<Fred>();
        data["Fred"].Item["George"].Item = fred;
        Assert.AreEqual(fred.TestGuid, data["Fred"].Item["George"].Item.TestGuid);
    }
}