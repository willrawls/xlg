using System.Diagnostics;
using MetX.Console.Tests.Standard.Library;
using MetX.Standard.XDString;
using MetX.Standard.XDString.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.XDString
{
    [TestClass]
    public class AssocArrayListOfTTests
    {
        [TestMethod]
        public void AssocArrayListOfT_Simple()
        {
            var fred = new Fred();
            var data = new AssocArrayList<Fred>("Mike");
            data["Fred"]["George"].Item = fred;
            
            Assert.AreEqual("Fred", data["Fred"].Key);
            Assert.AreEqual("George", data["Fred"]["George"].Key);
            Assert.AreEqual(fred.TestGuid, data["Fred"]["George"].Item.TestGuid);

            data["Mary"]["Frank"].Value = "Tim";
            Assert.AreEqual("Tim", data["Mary"]["Frank"].Value);
            System.Console.WriteLine(data["Fred"].ToString());
        }

        [TestMethod]
        public void AssocArrayListOfT_Simple2()
        {
            var data = new AssocArrayList();
            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 100; i++)
            {
                var assocArray = data[$"key{i++} array"];
                for (int j = 0; j < 100; j++)
                {
                    var key1 = $"key{i++}.{j++}";
                    assocArray[$"{key1} item"].Value = $"{key1} value";
                }
            }
            watch.Stop();
            Assert.IsTrue(watch.ElapsedMilliseconds < 1000);
        }
    }
}