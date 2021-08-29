using System.Collections.Generic;
using System.Diagnostics;
using MetX.Standard.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Library
{
    [TestClass]
    public class AssocArrayListTests
    {
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

        [TestMethod]
        public void AssocArrayList_Simple2()
        {
            var data = new AssocArrayList("Beth");
            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 100; i++)
            {
                var assocArray = data[$"key{i++} array"];
                for (int j = 0; j < 100; j++)
                {
                    var key1 = $"key{i++}.{j++}";
                    assocArray[$"{key1} item"] = $"{key1} value";
                }
            }
            watch.Stop();
            Assert.IsTrue(watch.ElapsedMilliseconds < 1000);
        }
    }
}