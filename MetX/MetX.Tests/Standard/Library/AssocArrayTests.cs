using MetX.Standard.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Library
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
    }
}

