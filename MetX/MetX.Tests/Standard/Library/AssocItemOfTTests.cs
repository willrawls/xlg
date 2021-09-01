using System;
using MetX.Standard.Library;
using MetX.Standard.Library.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Library
{
    [TestClass]
    public class AssocItemOfTTests
    {
        [TestMethod]
        public void AssocItemOfT_Simple()
        {
            var testGuid = Guid.NewGuid();
            Fred fred = new Fred();
            Fred fredItem = new Fred();
            IAssocItem otherItem = new AssocItem<Fred>("Other", fred);

            var data = new AssocItem<Fred>("Fred", fredItem, "George", testGuid, "Mary", otherItem);
            
            Assert.AreEqual("Fred", data.Key);
            Assert.AreEqual("George", data.Value);
            Assert.AreEqual(testGuid, data.Id);
            Assert.AreEqual("Mary", data.Name);
            Assert.AreEqual(otherItem.Id, data.Parent.Id);
        }
    }
}