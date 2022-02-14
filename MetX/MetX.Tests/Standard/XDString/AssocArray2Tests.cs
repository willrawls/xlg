using System;
using MetX.Standard.XDimensionalString.Generics;
using MetX.Tests.Standard.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.XDString
{
    [TestClass()]
    public class AssocArray2Tests
    {
        [TestMethod()]
        public void AssocArray1D_Of_1D_Fred()
        {
            var fred = new AssocArray1D<AssocArray1D<Fred>>();
            Guid george = fred["A"].Item["B"].Item.TestGuid;
        }

        [TestMethod()]
        public void AssocArray2D_Fred()
        {
            var fred = new AssocArray2D<Fred>();
            Guid george = fred["A", "B"].TestGuid;
        }

        [TestMethod()]
        public void AssocArray4D_Of_Fred()
        {
            var mary = new AssocArray4D<Fred>();
            Guid henry = mary["A","B","C","D"].TestGuid;
            Assert.IsTrue(mary.ContainsKey("A","B","C","D"));
        }

    }

}