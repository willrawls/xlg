using MetX.Standard.Library.Encryption;
using MetX.Standard.Primary.Extensions;
using MetX.Standard.XDString.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.XDString;

[TestClass]
public class AssocArray2dRethinkTests
{
#region ThreeDifferentAxisTypes
    [TestMethod]
    public void ThreeDifferentAxisTypes_GetByBothKeysSimplified()
    {
        var data = new TwoDimensionalAssocArray<FredAssocItem, GeorgeAssocItem, MaryAssocItem>();

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
    public void ThreeDifferentAxisTypes_GetByBothIDsSimplified()
    {
        var data = new TwoDimensionalAssocArray<FredAssocItem, GeorgeAssocItem, MaryAssocItem>();

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
    public void ThreeDifferentAxisTypes_GetByBothObjectsSimplified()
    {
        var data = new TwoDimensionalAssocArray<FredAssocItem, GeorgeAssocItem, MaryAssocItem>();

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
    public void ThreeDifferentAxisTypes_ToXml_Simple()
    {
        var data = new TwoDimensionalAssocArray<FredAssocItem, GeorgeAssocItem, MaryAssocItem>();

        var fred = new FredAssocItem();
        var george = new GeorgeAssocItem();;
        var mary = new MaryAssocItem();

        data[fred.Key, george.Key] = mary;

        var actual = data.ToXml();

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Contains("</TwoDimensionalAssocArrayOfFredAssocItemGeorgeAssocItemMaryAssocItem>"), actual);
    }

    [TestMethod]
    public void ThreeDifferentAxisTypes_FromXml_Simple()
    {
        var data = new TwoDimensionalAssocArray<FredAssocItem, GeorgeAssocItem, MaryAssocItem>();

        var fred = new FredAssocItem();
        var george = new GeorgeAssocItem();;
        var mary = new MaryAssocItem();

        data[fred.Key, george.Key] = mary;

        var expected = data.ToXml();
        var actual = XDStringExtensions.FromXml<FredAssocItem, GeorgeAssocItem, MaryAssocItem>(expected);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.ToXml());
    }
    #endregion

    #region SameForAllThree
    [TestMethod]
    public void SameForAllThree_GetByBothKeysSimplified()
    {
        var data = new TwoDimensionalAssocArray<JustAnAssocItem>();

        var fred = new JustAnAssocItem();
        var george = new JustAnAssocItem();
        var mary = new JustAnAssocItem();

        var expected = mary.JustAGuid;
        data[fred.Key, george.Key] = mary;

        var actual = data[fred.Key, george.Key];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void SameForAllThree_GetByBothIDsSimplified()
    {
        var data = new TwoDimensionalAssocArray<JustAnAssocItem>();

        var fred = new JustAnAssocItem();
        var george = new JustAnAssocItem();
        var mary = new JustAnAssocItem();

        var expected = mary.JustAGuid;
        data[fred.ID, george.ID] = mary;

        var actual = data[fred.ID, george.ID];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void SameForAllThree_GetByBothObjectsSimplified()
    {
        var data = new TwoDimensionalAssocArray<JustAnAssocItem>();

        var fred = new JustAnAssocItem();
        var george = new JustAnAssocItem();
        var mary = new JustAnAssocItem();

        var expected = mary.JustAGuid;
        data[fred, george] = mary;

        var actual = data[fred, george];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void SameForAllThree_ToXml_Simple()
    {
        var data = new TwoDimensionalAssocArray<JustAnAssocItem>();

        var fred = new JustAnAssocItem();
        var george = new JustAnAssocItem();
        var mary = new JustAnAssocItem();

        data[fred.Key, george.Key] = mary;

        var actual = data.ToXml();

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Contains("</TwoDimensionalAssocArrayOfJustAnAssocItem>"), actual);
    }

    [TestMethod]
    public void SameForAllThree_FromXml_Simple()
    {
        var data = new TwoDimensionalAssocArray<JustAnAssocItem>();

        var fred = new JustAnAssocItem();
        var george = new JustAnAssocItem();
        var mary = new JustAnAssocItem();

        data[fred.Key, george.Key] = mary;

        var expected = data.ToXml();
        var actual = XDStringExtensions.FromXml<JustAnAssocItem>(expected);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.ToXml());
    }
    #endregion

}