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

        //var l = mv[mv, r, point, t];


        //var realityLongAssocType = new LongAssocType {Target = 1};
        //var reality = new AssocReality();
        //AssocSpacetime spacetime = new ();

        //reality[realityLongAssocType, spacetime] = new VectorAssocType{ Deformation = long.MaxValue, Arc = 0, Speed=0, Spin = long.MinValue};
        //var vectorAssocType = reality[realityLongAssocType, spacetime];
        //mv[realityLongAssocType.ID, reality.ID] = vectorAssocType;

        /*
        var space = new AssocCube<AssocItem>
        {
            ["2", "3", "4"] = new("fred")
        };

        var atDateTimeAssocType = AssocType<DateTime>.Wrap(at);
        
        var secondSpacetime = spacetime[atDateTimeAssocType.ID, space.ID];
        */
        //var vector2 = reality[realityLongAssocType.ID, secondSpacetime.ID];
        //var expected = vector.ID;
        //mv[realityLongAssocType.ID, vector.ID].Spin = 1;


        //VectorAssocType x = mv[Guid.NewGuid(), Guid.NewGuid()];

        //var actual = vector.ID;

        //System.Console.WriteLine(mv.ToXml(new[] { typeof(AssocVerse) }));

        //Assert.IsNotNull(actual);
        //Assert.AreEqual(expected, actual);
    }

}
