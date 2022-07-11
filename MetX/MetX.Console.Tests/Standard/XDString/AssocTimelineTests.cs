using System;
using MetX.Console.Tests.Standard.XDString.TestingClasses;
using MetX.Standard.Primary.Extensions;
using MetX.Standard.XDString.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.XDString;

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
        data[fred.Key, george.Key, adam.Key, at] = mary;

        var actual = data[fred.Key, george.Key, adam.Key, at];

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
        data[fred.ID, george.ID, adam.ID, at] = mary;

        JustAnAssocItem actual = data[fred.ID, george.ID, adam.ID, at];

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
        data[fred, george, adam, at] = mary;

        var actual = data[fred, george, adam, at];

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

        data[fred.Key, george.Key, adam.Key, at] = mary;

        var actual = data.ToXml();

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Contains("</AssocTimelineOfFredAssocItemJustAnAssocItem>"), actual);
    }

    [TestMethod]
    public void AxiTheSameDifferentItem_FromXml_Simple()
    {
        var at = DateTime.Now;
        var data = new AssocTimeline<FredAssocItem, JustAnAssocItem>();
        var fred = new FredAssocItem();
        var george = new FredAssocItem(); var adam = new FredAssocItem();
        var mary = new JustAnAssocItem();

        data[fred.Key, george.Key, adam.Key, at] = mary;
        
        var expected = data.ToXml();
        var actual = AssocTimelineExtensions.FromXml<FredAssocItem, JustAnAssocItem>(expected);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual.ToXml());
    }
    #endregion

}