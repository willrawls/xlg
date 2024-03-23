using System.Diagnostics;
using MetX.Standard.Strings.Generics;
using MetX.Standard.Test.TestingClasses;
using AssocArrayList = MetX.Standard.Strings.AssocArrayList;

namespace MetX.Standard.Test.Strings;

[TestClass]
public class AssocArrayListOfTTests
{
    [TestMethod]
    public void AssocArrayListOfT_Simple()
    {
        var fred = new FredItem();
        var data = new AssocArrayOfT<FredItem>();
        data["Mike"].Key = "Fred";
        AssocItemOfT<FredItem>? fredObject = data["Fred"];
        var georgeObject = data["George"];
        georgeObject.Name = "Mary";
        var guid = Guid.NewGuid();
        fred.FredItemTestGuid = guid;

        Assert.AreEqual("Fred", data["Fred"].Key);
        Assert.AreEqual("George", data["George"].Key);
        Assert.AreEqual(fred.FredItemTestGuid, guid);

    }

    [TestMethod]
    public void AssocArrayListOfT_Simple2()
    {
        var fred = new FredItem();
        var data = new AssocArrayOfT<FredItem>();

        var watch = new Stopwatch();
        watch.Start();
        for (int i = 0; i < 100; i++)
        {
            var assocArray = data[$"key{i++} array"];
            for (int j = 0; j < 100; j++)
            {
                var key1 = $"key{i++}.{j++}";
                data[$"item {key1}"].Value = $"{key1} value";
            }
        }
        watch.Stop();
        Assert.IsTrue(watch.ElapsedMilliseconds < 1000);
    }
}