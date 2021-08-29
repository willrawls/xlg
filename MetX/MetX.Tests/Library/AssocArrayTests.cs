using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Library
{
    [TestClass]
    public class AssocArrayTests
    {
        [TestMethod]
        public void AssocArray_Simple()
        {
            var data = new AssocArray(null, "Mary")
            {
                ["Fred"] = "George"
            };
            Assert.AreEqual("George", data["Fred"]);
        }

        public class Fred
        {
            public int Tiny;
            public Guid Id = Guid.NewGuid();
        }

        [TestMethod]
        public void AssocArrayOfT_Simple()
        {
            var item = new Fred();
            var data = new AssocArray<Fred>
            {
                ["Fred"] = item,
            };
            Assert.AreEqual(item.Id, data["Fred"].Id);
        }

        [TestMethod]
        public void AssocArrayList_Simple()
        {
            var data = new AssocArrayList("Beth");
            Assert.AreEqual(data.Name, "Beth");

            data["Mary"]["Fred"] = "George";
            Assert.AreEqual("George", data["Mary"]["Fred"]);

            data["Mary"]["Frank"] = "Tim";
            Assert.AreEqual("Tim", data["Mary"]["Frank"]);
            Assert.AreEqual("George", data["Mary"]["Fred"]);
        }

        
    }
}

