using System;
using System.Diagnostics;
using MetX.Console.Tests.Standard.Strings.TestingClasses;
using MetX.Standard.Strings.Generics.V1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.Strings.V1;

[TestClass()]
public class AssocArray2Tests
{
    [TestMethod()]
    public void AssocArray1D_Of_1D_Fred()
    {
        AssocArray1D<Fred, FredItem> fred = new AssocArray1D<Fred, FredItem>();
        Guid george = fred["A"].Item.FredItemTestGuid;
        Assert.AreNotEqual(george, Guid.Empty);
    }

    [TestMethod()]
    public void AssocArray2D_Fred()
    {
        var fred = new AssocArray2D<GeorgeItem, Fred, FredItem>();
        Guid george = fred["A", "B"].FredItemTestGuid;
        Assert.AreNotEqual(george, Guid.Empty);
    }

    [TestMethod()]
    public void AssocArray4D_Of_Fred()
    {
        var mary = new AssocArray4D<JustAClass, George1DArray, GeorgeItem, Fred, FredItem>();
        Guid henry = mary["A","B","C","D"].FredItemTestGuid;
        Assert.IsTrue(mary.ContainsKey("A","B","C","D"));
        mary.Name = "Mary";
        Debug.WriteLine(mary.ToXml());
        Debug.WriteLine(mary.ToJson());
        Debug.WriteLine(mary.ToString());
    }

    [TestMethod(), Ignore("maybe try to get this working later")]
    public void AssocArray4D_Of_Fred_FromXml()
    {
        var mary = new A4DAA();
        Guid henry = mary["A","B","C","D"].FredItemTestGuid;
        Assert.AreNotEqual(henry, Guid.Empty);

        Assert.IsTrue(mary.ContainsKey("A","B","C","D"));
        Assert.IsFalse(mary.ContainsKey("A","B","C","E"));

        mary.Name = "Mary";
        mary.A4DAAName = "A4DAAName";
        Assert.AreEqual("A4DAAName", mary.A4DAAName);
        var xml = mary.ToXml();
        System.Console.WriteLine(xml);

        A4DAA actual = A4DAA.FromTypedXml<A4DAA>(xml);
        Assert.AreEqual("A4DAAName", actual.A4DAAName);
        Debug.WriteLine(actual.ToXml());
    }

}