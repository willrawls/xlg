using System;
using System.Diagnostics;
using System.Xml.Serialization;
using MetX.Standard.Library;
using MetX.Standard.XDString;
using MetX.Standard.XDString.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests
{
    [TestClass()]
    public class ListSerializesToXmlTests
    {
        public class FredItem
        {
            public string Name {get; set; }
        }

        public class Fred : ListSerializesToXml<Fred, FredItem>
        {

        }

        public class George : AssocArray1D<George>
        {
            [XmlAttribute]
            public string Name {get; set; }
        }

        [TestMethod()]
        public void FredFromXmlTest()
        {
            var fred = new Fred { new() { Name = "Mary" } };

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
            var george = new George();
            george.Items.Add(new AssocItem<George>
            {
                Name = "Mary",
                Value = "Something",
                Category = "Tim",
                Number = 4,
                ID = Guid.NewGuid(),
                Key = "SomeKey",
            });
            
            george.Name = "Henry";

            var xml = george.ToXml();
            Assert.IsNotNull(xml);
            Debug.WriteLine("XML=" + xml);

            Assert.IsTrue(xml.Contains("Henry"));
            var reGeorge = George.FromXml(xml);
            Assert.IsNotNull(reGeorge);

            var actual = reGeorge.ToXml();
            Debug.WriteLine("ReXML=" + actual);

            Assert.AreEqual("\n" + xml, "\n" + actual);
        }

        [TestMethod()]
        public void GeorgeToJsonTest()
        {
            var george = new George();
            var guid = Guid.NewGuid();
            george.Items.Add(new AssocItem<George>
            {
                Name = "Mary",
                Value = "Something",
                Category = "Tim",
                Number = 4,
                ID = guid,
                Key = "SomeKey",
            });
            george.Name = "Henry";

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
}