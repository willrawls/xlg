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
            var data = new AssocArrayList("Beth", "Dolly");
            Assert.AreEqual(data.Key, "Beth");
            Assert.AreEqual(data.Name, "Dolly");

            data["Mary"]["Fred"].Value = "George";
            Assert.AreEqual("George", data["Mary"]["Fred"].Value);

            data["Mary"]["Frank"].Value = "Tim";
            Assert.AreEqual("Tim", data["Mary"]["Frank"].Value);
        }

        [TestMethod]
        public void AssocArrayList_Simple2()
        {
            var data = new AssocArrayList("Beth", "Dolly");
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