using MetX.Standard.Strings.Generics;
using MetX.Standard.Test.TestingClasses;

namespace MetX.Standard.Test.Strings.Assoc;

[TestClass]
public class AssocTimelineTests
{
    #region AxiTheSameDifferentItem
    [TestMethod]
    public void AxiTheSameDifferentItem_GetByBothKeysSimplified()
    {
        var at = DateTime.Now;
        var data = new AssocTimeline<FredAssocItem, JustAnAssocItem>();
        var fred = new FredAssocItem();
        var george = new FredAssocItem(); var adam = new FredAssocItem();
        var mary = new JustAnAssocItem();

        var expected = mary.JustAGuid;
        data[at, fred.Key, george.Key, adam.Key] = mary;

        var actual = data[at, fred.Key, george.Key, adam.Key];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void AxiTheSameDifferentItem_GetByBothIDsSimplified()
    {
        var at = DateTime.Now;
        var data = new AssocTimeline<FredAssocItem, JustAnAssocItem>();
        var fred = new FredAssocItem();
        var george = new FredAssocItem(); var adam = new FredAssocItem();
        var mary = new JustAnAssocItem();

        Guid expected = mary.JustAGuid;
        data[at, fred.ID, george.ID, adam.ID] = mary;

        JustAnAssocItem actual = data[at, fred.ID, george.ID, adam.ID];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void AxiTheSameDifferentItem_GetByBothObjectsSimplified()
    {
        var data = new AssocTimeline<FredAssocItem, JustAnAssocItem>();

        var at = DateTime.Now;
        var fred = new FredAssocItem();
        var george = new FredAssocItem(); var adam = new FredAssocItem();
        var mary = new JustAnAssocItem();

        var expected = mary.JustAGuid;
        data[at, fred, george, adam] = mary;

        var actual = data[at, fred, george, adam];

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.JustAGuid);
    }

    [TestMethod]
    public void AxiTheSameDifferentItem_ToXml_Simple()
    {
        var data = new AssocTimeline<FredAssocItem, JustAnAssocItem>();

        var at = DateTime.Now;
        var fred = new FredAssocItem();
        var george = new FredAssocItem(); var adam = new FredAssocItem();
        var mary = new JustAnAssocItem();

        data[at, fred.Key, george.Key, adam.Key] = mary;

        var actual = data.ToXml();

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Contains("</AssocTimelineOfFredAssocItemJustAnAssocItem>"), actual);
        Console.Write(actual);
    }

    [TestMethod]
    public void AxiTheSameDifferentItem_FromXml_Simple()
    {
        var at = DateTime.Now;
        var data = new AssocTimeline<FredAssocItem, JustAnAssocItem>();
        var fred = new FredAssocItem();
        var george = new FredAssocItem(); var adam = new FredAssocItem();
        var mary = new JustAnAssocItem();
        var mary2 = new JustAnAssocItem();

        data[at, fred.Key, george.Key, adam.Key] = mary;
        data[at.AddDays(1), fred.Key, george.Key, adam.Key] = mary2;

        var expected = data.ToXml();
        var actual = AssocTimelineExtensions.FromXml<FredAssocItem, JustAnAssocItem>(expected);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.ToXml());
        Console.Write(actual);
    }
    #endregion

}