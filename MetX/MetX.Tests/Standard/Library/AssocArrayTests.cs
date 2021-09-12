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
        public void AssocArray_ValuesArray_Simple()
        {
            var data = new AssocArray {["Fred"] = {Value = "Henry"}, ["George"] = {Value = "Mary"}};
            CollectionAssert.AreEqual(new string[] { "Henry", "Mary"}, data.Values);
        }

        [TestMethod]
        public void AssocArray_KeysArray_Simple()
        {
            var data = new AssocArray {["Fred"] = {Value = "Henry"}, ["George"] = {Value = "Mary"}};
            Assert.IsNotNull(data.Keys);
            CollectionAssert.AreEqual(new string[] { "Fred", "George"}, data.Keys);
        }

        [TestMethod]
        public void AssocArray_NamesArray_Simple()
        {
            var data = new AssocArray {["Fred"] = {Name = "Henry"}, ["George"] = {Name = "Mary"}};
            Assert.IsNotNull(data.Names);
            CollectionAssert.AreEqual(new string[] { "Henry", "George"}, data.Names);
        }

        [TestMethod]
        public void AssocArray_IDsArray_Simple()
        {
            var data = new AssocArray {["Fred"] = {Name = "Henry"}, ["George"] = {Name = "Mary"}};
            Assert.IsNotNull(data.IDs);
            Assert.AreEqual(2, data.IDs.Length);
            Assert.AreEqual(data.IDs[0], data["Fred"].Id);
            Assert.AreEqual(data.IDs[1], data["George"].Id);
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

