using System;
using System.Xml.Serialization;
using MetX.Console.Tests.Standard.XDString.TestingClasses;
using MetX.Standard.Primary.Extensions;
using MetX.Standard.XDString;
using MetX.Standard.XDString.Generics;
using MetX.Standard.XDString.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.XDString;

public class AssocVerse2 : AssocArray<AssocReality>
{
}

public class AssocReality : AssocArray<AssocDimension>
{
}

public class AssocDimension : AssocCube<AssocSpace>
{

}

public class AssocSpace : AssocSheet<AssocTime>, IAssocItem
{
    [XmlAttribute] public string Key { get; }
    [XmlAttribute] public string Value { get; set; }
    [XmlAttribute] public string Name { get; set; }
    [XmlAttribute] public Guid ID { get; set; }

    public AssocSpace() : base()
    {
    }
}

public class AssocTime : AssocCube<AssocGravity>
{
}

public class AssocGravity : AssocCube<AssocVector>
{
}

public class AssocVector : AssocItem
{
}

[TestClass]
public class AssocVerseTests
{
    [TestMethod, Ignore]
    public void GetByIDsSimplified()
    {
        AssocVerse2 mv = new AssocVerse2();
        var at = DateTime.Now;
        mv.Key = "Alpha";
        var r = mv["Beta"].Item;
        var d = r["Charlie"].Item;
        var s = d["Theta", "Charlie", "Wilco"];
        var t = s["+1.5287354", "+35.201"];
        AssocArray<AssocArray<AssocArray<AssocVector>>> g = t["0", "-90", "+Infinity"].FirstAxis;
        //var o = g["200", "45", "10.00043"].Item;

        //var l = mv[mv, r, point, t];


        var realityLongAssocType = new LongAssocType {Target = 1};
        var reality = new AssocReality();
        AssocSpacetime spacetime = new ();

        //reality[realityLongAssocType, spacetime] = new VectorAssocType{ Deformation = long.MaxValue, Arc = 0, Speed=0, Spin = long.MinValue};
        //var vectorAssocType = reality[realityLongAssocType, spacetime];
        //mv[realityLongAssocType.ID, reality.ID] = vectorAssocType;

        var space = new AssocCube<AssocItem>
        {
            ["2", "3", "4"] = new("fred")
        };

        var atDateTimeAssocType = AssocType<DateTime>.Wrap(at);
        
        var secondSpacetime = spacetime[atDateTimeAssocType.ID, space.ID];
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
