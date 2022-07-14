using System;
using MetX.Console.Tests.Standard.XDString.TestingClasses;
using MetX.Standard.Primary.Extensions;
using MetX.Standard.XDString;
using MetX.Standard.XDString.Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.XDString;

[TestClass]
public class AssocVerseTests
{
    [TestMethod]
    public void AxiTheSameDifferentItem_GetByIDsSimplified()
    {
        var at = DateTime.Now;
        var verse = new AssocVerse();
        var realityLongAssocType = new LongAssocType {Target = 1};
        var reality = new AssocReality();
        AssocSpacetime spacetime = new ();

        reality[realityLongAssocType, spacetime] = new VectorAssocType{ Deformation = long.MaxValue, Arc = 0, Speed=0, Spin = long.MinValue};

        var vectorAssocType = reality[realityLongAssocType, spacetime];

        verse[realityLongAssocType.ID, reality.ID] = vectorAssocType;

        var space = new AssocCube<AssocItem>
        {
            ["2", "3", "4"] = new("fred")
        };

        var atDateTimeAssocType = AssocType<DateTime>.Wrap(at);
        
        var secondSpacetime = spacetime[atDateTimeAssocType.ID, space.ID];
        var vector = reality[realityLongAssocType.ID, secondSpacetime.ID];
        var expected = vector.ID;
        verse[realityLongAssocType.ID, vector.ID].Spin = 1;

        var actual = vector.ID;

        System.Console.WriteLine(verse.ToXml(new[] { typeof(AssocVerse) }));

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

}
