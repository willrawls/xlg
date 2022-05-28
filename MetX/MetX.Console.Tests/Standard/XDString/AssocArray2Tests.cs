using System;
using System.Diagnostics;
using MetX.Console.Tests.Standard.Library;
using MetX.Standard.XDString.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.XDString
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
            mary.Name = "Mary";
            Debug.WriteLine(mary.ToXml());
            Debug.WriteLine(mary.ToJson());
            Debug.WriteLine(mary.ToString());
        }

        [TestMethod()]
        public void AssocArray4D_Of_Fred_FromXml()
        {
            var mary = new AssocArray4D<Fred>();
            Guid henry = mary["A","B","C","D"].TestGuid;
            Assert.IsTrue(mary.ContainsKey("A","B","C","D"));
            Assert.IsFalse(mary.ContainsKey("A","B","C","E"));

            mary.Name = "Mary";
            var actual = AssocArray4D<Fred>.FromXml(mary.ToXml());
            Assert.AreEqual("Mary", actual.Name);
            Debug.WriteLine(actual.ToXml());
        }

    }

}