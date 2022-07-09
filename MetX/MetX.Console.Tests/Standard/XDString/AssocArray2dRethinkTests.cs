using MetX.Standard.Library.Encryption;
using MetX.Standard.Primary.Extensions;
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

    [TestMethod]
    public void GetByBothIDsSimplified()
    {
        var data = new AssocArray2dRethink<FredAssocItem, GeorgeAssocItem, MaryAssocItem>();

        var fred = new FredAssocItem();
        var george = new GeorgeAssocItem();;
        var mary = new MaryAssocItem();

        var expected = mary.MaryAssocItemTestGuid;
        data[fred.ID, george.ID] = mary;

        var actual = data[fred.ID, george.ID];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.MaryAssocItemTestGuid);
    }

    [TestMethod]
    public void GetByBothObjectsSimplified()
    {
        var data = new AssocArray2dRethink<FredAssocItem, GeorgeAssocItem, MaryAssocItem>();

        var fred = new FredAssocItem();
        var george = new GeorgeAssocItem();;
        var mary = new MaryAssocItem();
        var expected = mary.MaryAssocItemTestGuid;
        data[fred, george] = mary;

        var actual = data[fred, george];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.MaryAssocItemTestGuid);
    }

    [TestMethod]
    public void ToXml_Simple()
    {
        var data = new AssocArray2dRethink<FredAssocItem, GeorgeAssocItem, MaryAssocItem>();

        var fred = new FredAssocItem();
        var george = new GeorgeAssocItem();;
        var mary = new MaryAssocItem();

        data[fred.Key, george.Key] = mary;

        var expected = "Something";
        var actual = data.ToXml();

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Contains("</AssocArray2dRethinkOfFredAssocItemGeorgeAssocItemMaryAssocItem>"));
    }

    [TestMethod]
    public void FromXml_Simple()
    {
        var data = new AssocArray2dRethink<FredAssocItem, GeorgeAssocItem, MaryAssocItem>();

        var fred = new FredAssocItem();
        var george = new GeorgeAssocItem();;
        var mary = new MaryAssocItem();

        data[fred.Key, george.Key] = mary;

        var expected = data.ToXml();
        var actual = XDStringExtensions.FromXml<FredAssocItem, GeorgeAssocItem, MaryAssocItem>(expected);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.ToXml());
    }


}