using System.Diagnostics;
using MetX.Standard.Library;
using MetX.Standard.Library.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Library
{
    [TestClass]
    public class AssocArrayListOfTTests
    {
        [TestMethod]
        public void AssocArrayListOfT_Simple()
        {
            var fred = new Fred();
            var data = new AssocArrayList<Fred>();

            data["Mary"]["Fred"].Value = "Harry";
            Assert.AreEqual("Harry", data["Mary"]["Fred"].Value);

            data["Fred"]["George"].Item = fred;
            Assert.IsNotNull(data["Fred"]["George"].Item);
            Assert.AreEqual(fred.TestGuid, data["Fred"]["George"].Item.TestGuid);
        }

        [TestMethod]
        public void AssocArrayListOfT_Simple2()
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