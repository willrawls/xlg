using System;
using MetX.Console.Tests.Standard.Library;
using MetX.Standard.XDString;
using MetX.Standard.XDString.Generics;
using MetX.Standard.XDString.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.XDString
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
            Assert.AreEqual(testGuid, data.ID);
            Assert.AreEqual("Mary", data.Name);
            Assert.AreEqual(otherItem.ID, data.Parent.ID);
        }
    }
}