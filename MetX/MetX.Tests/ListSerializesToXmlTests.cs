using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetX.Standard.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MetX.Standard.Library.Tests
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

        public class George : AssocArray2<George>
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
            george.Items.Add(new AssocItem
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
    }
}