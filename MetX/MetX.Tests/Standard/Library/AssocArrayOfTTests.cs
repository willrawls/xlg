using MetX.Standard.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Library
{
    [TestClass]
    public class AssocArrayOfTTests
    {
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
    }
}