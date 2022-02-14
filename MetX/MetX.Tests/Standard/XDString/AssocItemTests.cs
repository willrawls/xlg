using System;
using MetX.Standard.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.XDString
{
    [TestClass]
    public class AssocItemTests
    {
        [TestMethod]
        public void AssocItem_Simple()
        {
            var testGuid = Guid.NewGuid();
            IAssocItem otherItem = new AssocItem("Other");
            var data = new AssocItem("Fred", "George", testGuid, "Mary", otherItem);
            
            Assert.AreEqual("Fred", data.Key);
            Assert.AreEqual("George", data.Value);
            Assert.AreEqual(testGuid, data.ID);
            Assert.AreEqual("Mary", data.Name);
            Assert.AreEqual(otherItem.ID, data.Parent.ID);
        }
    }
}