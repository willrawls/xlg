using System;
using MetX.Console.Tests.Standard.XDString.TestingClasses;
using MetX.Standard.Primary.Extensions;
using MetX.Standard.XDString;
using MetX.Standard.XDString.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.XDString;

public class AssocVerse2 : AssocArray
{
    public AssocReality Reality { get; set; }
}

public class AssocReality : AssocArray
{
    public AssocDimension Dimension { get; set; }
}

public class AssocDimension : AssocArray
{

}

[TestClass]
public class AssocVerseTests
{
    [TestMethod]
    public void GetByIDsSimplified()
    {
        var mv = new AssocVerse2();
        var at = DateTime.Now;
        mv.Key = "Alpha";
        var r = mv.Reality["Beta"];
        var d = r.Dimension["Charlie"];
        var s = d.Space["Theta", "Charlie", "Wilco"];
        var t = d.Time["+1.5287354", "+35.201"];
        var g = t.Gravity["0", "-90", "+Infinity"];
        var o = t.Vector["200", "45", "10.00043"];

        var l = mv[mv, r, point, t];


        var realityLongAssocType = new LongAssocType {Target = 1};
        var reality = new AssocReality();
        AssocSpacetime spacetime = new ();

        reality[realityLongAssocType, spacetime] = new VectorAssocType{ Deformation = long.MaxValue, Arc = 0, Speed=0, Spin = long.MinValue};

        var vectorAssocType = reality[realityLongAssocType, spacetime];

        mv[realityLongAssocType.ID, reality.ID] = vectorAssocType;

        var space = new AssocCube<AssocItem>
        {
            ["2", "3", "4"] = new("fred")
        };

        var atDateTimeAssocType = AssocType<DateTime>.Wrap(at);
        
        var secondSpacetime = spacetime[atDateTimeAssocType.ID, space.ID];
        var vector2 = reality[realityLongAssocType.ID, secondSpacetime.ID];
        var expected = vector.ID;
        mv[realityLongAssocType.ID, vector.ID].Spin = 1;


        VectorAssocType x = mv[Guid.NewGuid(), Guid.NewGuid()];
        
        var actual = vector.ID;

        System.Console.WriteLine(mv.ToXml(new[] { typeof(AssocVerse) }));

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

}
