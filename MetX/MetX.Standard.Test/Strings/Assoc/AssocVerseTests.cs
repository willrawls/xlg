namespace MetX.Standard.Test.Strings.Assoc;

[TestClass]
public class AssocVerseTests
{
    [TestMethod, Ignore]
    public void GetByIDsSimplified()
    {
        AssocVerse2 multiverse = new AssocVerse2();
        var at = DateTime.Now;
        multiverse.Key = "Alpha";
        var reality = multiverse["Beta"].Item;
        var dimension = reality["Charlie"].Item;
        var space = dimension["Theta", "Charlie", "Wilco"];
        var time = space["+1.5287354", "+35.201"];
        var gravity = time["0", "-90", "+Infinity"].FirstAxis;
        var o = gravity["200"].Item;
    }
}
