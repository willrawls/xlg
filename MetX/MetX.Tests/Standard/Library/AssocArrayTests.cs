using System.Collections;
using System.Collections.Generic;
using MetX.Standard.Library;
using MetX.Standard.Library.Generics;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Frameworks;

namespace MetX.Tests.Standard.Library
{
    [TestClass]
    public class AssocArrayTests
    {
        [TestMethod]
        public void AssocArray_Simple()
        {
            var data = new AssocArray(null, "Mary");
            data["Fred"].Value = "George";
            Assert.AreEqual("George", data["Fred"].Value);
        }

        [TestMethod]
        public void AssocArray_IterableList()
        {
            var assocArray = new AssocArray(null, "Mary")
            {
                ["Fred"] = {Value = "George"}, 
                ["Mary"] = {Value = "Beth"},
            };
            assocArray["Henry"].Value = "Greg";

            Assert.AreEqual(3, assocArray.Count);
            Assert.AreEqual("George", assocArray[0].Value);
            Assert.AreEqual("Beth", assocArray[1].Value);
            Assert.AreEqual("Greg", assocArray[2].Value);
        }
    }
}

