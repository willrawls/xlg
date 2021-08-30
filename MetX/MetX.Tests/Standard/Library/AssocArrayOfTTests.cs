using MetX.Standard.Library;
using MetX.Standard.Library.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Library
{
    [TestClass]
    public class AssocArrayOfTTests
    {
        [TestMethod]
        public void Simple()
        {
            var item = new Fred();
            var data = new AssocArray<Fred>
            {
                ["Fred"] =
                {
                    Item = item
                }
            };
            Assert.AreEqual(item.TestGuid, data["Fred"].Item.TestGuid);
        }

        [TestMethod]
        public void Simple_AssocTypeInt()
        {
            var assocType = new AssocType<int>(12);
            var data = new AssocArray<AssocType<int>>
            {
                ["Fred"] =
                {
                    Item = assocType
                }
            };
            Assert.AreEqual(assocType.Target, data["Fred"].Item.Target);
        }
    }
}