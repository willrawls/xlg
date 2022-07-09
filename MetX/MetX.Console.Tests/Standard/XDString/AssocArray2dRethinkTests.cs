using MetX.Standard.Library.Encryption;
using MetX.Standard.XDString.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.XDString;

[TestClass]
public class AssocArray2dRethinkTests
{
    [TestMethod]
    public void GetByBothKeysSimplified()
    {
        var data = new AssocArray2dRethink<FredAssocItem, GeorgeAssocItem, MaryAssocItem>();

        var fred = new FredAssocItem();
        var george = new GeorgeAssocItem();;
        var mary = new MaryAssocItem();
        var expected = mary.MaryAssocItemTestGuid;
        data[fred.Key, george.Key] = mary;

        var actual = data[fred.Key, george.Key];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.MaryAssocItemTestGuid);
    }

}