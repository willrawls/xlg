using System.Diagnostics;
using MetX.Standard.XDString;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.XDString;

[TestClass]
public class AssocArrayListTests
{
    [TestMethod]
    public void AssocArrayList_Simple()
    {
        var data = new AssocArrayList();

        var item = data["Mary"]["Fred"];
        item.Value = "George";
        Assert.AreEqual("George", item.Value);

        data["Mary"]["Frank"].Value = "Tim";
        Assert.AreEqual("Tim", data["Mary"]["Frank"].Value);

        var id = item.ID;
        Assert.AreEqual(id, item.ID);
    }

    [TestMethod]
    public void AssocArrayList_Simple2()
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