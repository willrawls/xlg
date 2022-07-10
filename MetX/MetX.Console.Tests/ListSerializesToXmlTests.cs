using System;
using System.Diagnostics;
using System.Xml.Serialization;
using MetX.Console.Tests.Standard.XDString;
using MetX.Console.Tests.Standard.XDString.TestingClasses;
using MetX.Standard.Library.ML;
using MetX.Standard.XDString.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests;

[TestClass()]
public class ListSerializesToXmlTests
{
    [TestMethod()]
    public void FredFromXmlTest()
    {
        var fred = new Fred { new() { FredItemName = "Mary" } };

        var xml = fred.ToXml();
        Assert.IsNotNull(xml);

        var reFred = Fred.FromXml(xml);
        Assert.IsNotNull(reFred);

        var actual = reFred.ToXml();

        Assert.AreEqual(xml, actual);
        Debug.WriteLine(actual);
    }

    [TestMethod()]
    public void GeorgeFromXmlTest()
    {
        George george = new George();
        george.Items.Add(new AssocItem<GeorgeItem>
        {
            Name = "Mary",
            Value = "Something",
            Category = "Tim",
            Number = 4,
            ID = Guid.NewGuid(),
            Key = "SomeKey",
        });
            
        george.GeorgeName = "Henry";

        var expected = george.ToXml();
        Assert.IsNotNull(expected);
        Debug.WriteLine(expected);

        Assert.IsTrue(expected.Contains("Henry"));
        var reGeorge = George.FromXml(expected);
        Assert.IsNotNull(reGeorge);
        Assert.AreEqual("Henry", reGeorge.GeorgeName);

        var actual = reGeorge.ToXml();
        Debug.WriteLine(actual);

        Assert.AreEqual("\n" + expected, "\n" + actual);
    }

    [TestMethod, Ignore]
    public void GeorgeToJsonTest()
    {
        George george = new George();
        var guid = Guid.NewGuid();
        george.Items.Add(new AssocItem<GeorgeItem>
        {
            Name = "Mary",
            Value = "Something",
            Category = "Tim",
            Number = 4,
            ID = guid,
            Key = "SomeKey",
        });
        george.GeorgeName = "Henry";

        var json = george.ToJson();
        Assert.IsNotNull(json);
        Debug.WriteLine("JSON=" + json);

        Assert.IsTrue(json.Contains("Henry"));
        Assert.IsTrue(json.Contains("Mary"));
        Assert.IsTrue(json.Contains("Tim"));
        Assert.IsTrue(json.Contains("Something"));
        Assert.IsTrue(json.Contains("SomeKey"));
        Assert.IsTrue(json.Contains(guid.ToString("D")));
    }
}