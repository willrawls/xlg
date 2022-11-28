using MetX.Standard.Library.Extensions;
using MetX.Standard.Primary.Generation;
using MetX.Standard.Primary.Generation.CSharp.Project;
using MetX.Standard.Strings;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.Generation.CSharp.Project;

[TestClass]
public partial class AspectsCsProjGeneratorTests
{
    [TestMethod] [Ignore("Never finished line of code that will not be used but who's code may be re-purposed")]
    public void FromScratchXmlIsAsExpected()
    {
        var generator = TestHelpers
            .SetupGenerator<AspectsCsProjGenerator>(GenFramework.Standard20);
        Assert.IsNotNull(generator);
        generator.Generate();
        var actual = generator.Document.OuterXml.AsStringFromFormattedXml();
            
        Assert.IsFalse(actual.Contains(CsProjGeneratorOptions.Delimiter), actual);
        Assert.IsFalse(actual.Contains("Analyzer"), actual);
        Assert.IsTrue(actual.Contains(GenFramework.Standard20.ToTargetFramework()), actual);
        Assert.IsFalse(actual.Contains(generator.Options.AspectsName), actual);
    }
}