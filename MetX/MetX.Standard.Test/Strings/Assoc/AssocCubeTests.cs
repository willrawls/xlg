using MetX.Standard.Strings.Generics;
using MetX.Standard.Strings.Generics.V3;
using MetX.Standard.Test.TestingClasses;

namespace MetX.Standard.Test.Strings.Assoc;

[TestClass]
public class AssocCubeTests
{
    #region FourDifferentTypes
    [TestMethod]
    public void FourDifferentTypes_GetByBothKeysSimplified()
    {
        var data = new AssocCubeOf<FredAssocItem, GeorgeAssocItem, MaryAssocItem, JustAnAssocItem>();
        var fred = new FredAssocItem();
        var george = new GeorgeAssocItem();
        var mary = new MaryAssocItem();
        var adam = new JustAnAssocItem();

        var expected = adam.JustAGuid;
        data[fred.Key, george.Key, mary.Key] = adam;

        var actual = data[fred.Key, george.Key, mary.Key];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void FourDifferentTypes_GetByBothIDsSimplified()
    {
        var data = new AssocCubeOf<FredAssocItem, GeorgeAssocItem, MaryAssocItem, JustAnAssocItem>();
        var fred = new FredAssocItem();
        var george = new GeorgeAssocItem();
        var mary = new MaryAssocItem();
        var adam = new JustAnAssocItem();

        var expected = adam.JustAGuid;
        data[fred.ID, george.ID, mary.ID] = adam;

        var actual = data[fred.ID, george.ID, mary.ID];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void FourDifferentTypes_GetByBothObjectsSimplified()
    {
        var data = new AssocCubeOf<FredAssocItem, GeorgeAssocItem, MaryAssocItem, JustAnAssocItem>();
        var fred = new FredAssocItem();
        var george = new GeorgeAssocItem();
        var mary = new MaryAssocItem();
        var adam = new JustAnAssocItem();

        var expected = adam.JustAGuid;
        data[fred, george, mary] = adam;

        var actual = data[fred, george, mary];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);

        actual = data[fred.ID, george.ID, mary.ID];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void FourDifferentTypes_ToXml_Simple()
    {
        var data = new AssocCubeOf<FredAssocItem, GeorgeAssocItem, MaryAssocItem, JustAnAssocItem>();
        var fred = new FredAssocItem();
        var george = new GeorgeAssocItem();
        var mary = new MaryAssocItem();
        var adam = new JustAnAssocItem();

        data[fred.Key, george.Key, mary.Key] = adam;

        var actual = data.ToXml();

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Contains("</AssocCubeOfOfFredAssocItemGeorgeAssocItemMaryAssocItemJustAnAssocItem>"), actual);
    }

    [TestMethod]
    public void FourDifferentTypes_FromXml_Simple()
    {
        var data = new AssocCubeOf<FredAssocItem, GeorgeAssocItem, MaryAssocItem, JustAnAssocItem>();
        var fred = new FredAssocItem();
        var george = new GeorgeAssocItem();
        var mary = new MaryAssocItem();
        var adam = new JustAnAssocItem();

        data[fred.Key, george.Key, mary.Key] = adam;

        var expected = data.ToXml();
        var actual = AssocCube.From<FredAssocItem, GeorgeAssocItem, MaryAssocItem, JustAnAssocItem>(expected);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.ToXml());
    }
    #endregion

    #region SameTypesForAll
    [TestMethod]
    public void SameTypesForAll_GetByBothKeysSimplified()
    {
        var data = new AssocCubeOf<JustAnAssocItem>();

        var fred = new JustAnAssocItem();
        var george = new JustAnAssocItem(); var adam = new JustAnAssocItem();
        var mary = new JustAnAssocItem();

        var expected = mary.JustAGuid;
        data[fred.Key, george.Key, adam.Key] = mary;

        var actual = data[fred.Key, george.Key, adam.Key];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void SameTypesForAll_GetByBothIDsSimplified()
    {
        var data = new AssocCubeOf<JustAnAssocItem>();

        var fred = new JustAnAssocItem();
        var george = new JustAnAssocItem(); var adam = new JustAnAssocItem();
        var mary = new JustAnAssocItem();

        var expected = mary.JustAGuid;
        data[fred.ID, george.ID, adam.ID] = mary;

        var actual = data[fred.ID, george.ID, adam.ID];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void SameTypesForAll_GetByBothObjectsSimplified()
    {
        var data = new AssocCubeOf<JustAnAssocItem>();

        var fred = new JustAnAssocItem();
        var george = new JustAnAssocItem(); var adam = new JustAnAssocItem();
        var mary = new JustAnAssocItem();

        var expected = mary.JustAGuid;
        data[fred, george, adam] = mary;

        var actual = data[fred, george, adam];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void SameTypesForAll_ToXml_Simple()
    {
        var data = new AssocCubeOf<JustAnAssocItem>();

        var fred = new JustAnAssocItem();
        var george = new JustAnAssocItem(); var adam = new JustAnAssocItem();
        var mary = new JustAnAssocItem();

        data[fred.Key, george.Key, adam.Key] = mary;

        var actual = data.ToXml();

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Contains("</AssocCubeOfOfJustAnAssocItem>"), actual);
    }

    [TestMethod]
    public void SameTypesForAll_FromXml_Simple()
    {
        var data = new AssocCubeOf<JustAnAssocItem>();

        var fred = new JustAnAssocItem();
        var george = new JustAnAssocItem(); var adam = new JustAnAssocItem();
        var mary = new JustAnAssocItem();

        data[fred.Key, george.Key, adam.Key] = mary;

        var expected = data.ToXml();
        var actual = AssocCubeExtensions.FromXml<JustAnAssocItem>(expected);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.ToXml());
    }
    #endregion

    #region AxiTheSameDifferentItem
    [TestMethod]
    public void AxiTheSameDifferentItem_GetByBothKeysSimplified()
    {
        var data = new AssocCubeOfT2<FredAssocItem, JustAnAssocItem>();

        var fred = new FredAssocItem();
        var george = new FredAssocItem(); var adam = new FredAssocItem();
        var mary = new JustAnAssocItem();

        var expected = mary.JustAGuid;
        data[fred.Key, george.Key, adam.Key] = mary;

        var actual = data[fred.Key, george.Key, adam.Key];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void AxiTheSameDifferentItem_GetByBothIDsSimplified()
    {
        var data = new AssocCubeOfT2<FredAssocItem, JustAnAssocItem>();

        var fred = new FredAssocItem();
        var george = new FredAssocItem(); var adam = new FredAssocItem();
        var mary = new JustAnAssocItem();

        Guid expected = mary.JustAGuid;
        data[fred.ID, george.ID, adam.ID] = mary;

        JustAnAssocItem actual = data[fred.ID, george.ID, adam.ID];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void AxiTheSameDifferentItem_GetByBothObjectsSimplified()
    {
        var data = new AssocCubeOfT2<FredAssocItem, JustAnAssocItem>();

        var fred = new FredAssocItem();
        var george = new FredAssocItem(); var adam = new FredAssocItem();
        var mary = new JustAnAssocItem();

        var expected = mary.JustAGuid;
        data[fred, george, adam] = mary;

        var actual = data[fred, george, adam];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void AxiTheSameDifferentItem_ToXml_Simple()
    {
        var data = new AssocCubeOfT2<FredAssocItem, JustAnAssocItem>();

        var fred = new FredAssocItem();
        var george = new FredAssocItem(); var adam = new FredAssocItem();
        var mary = new JustAnAssocItem();

        data[fred.Key, george.Key, adam.Key] = mary;

        var actual = data.ToXml();

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Contains("</AssocItemOfTOfAssocArrayOfTOfAssocArrayOfTOfJustAnAssocItem>"), actual);
    }

    [TestMethod]
    public void AxiTheSameDifferentItem_FromXml_Simple()
    {
        var data = new AssocCubeOfT2<FredAssocItem, JustAnAssocItem>();

        var fred = new FredAssocItem();
        var george = new FredAssocItem(); var adam = new FredAssocItem();
        var mary = new JustAnAssocItem();

        data[fred.Key, george.Key, adam.Key] = mary;

        var expected = data.ToXml();
        var actual = AssocCubeExtensions.FromXml<FredAssocItem, JustAnAssocItem>(expected);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.ToXml());
    }
    #endregion

}