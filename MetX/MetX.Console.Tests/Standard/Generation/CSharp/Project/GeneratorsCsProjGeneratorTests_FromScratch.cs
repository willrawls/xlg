using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.Strings;
using MetX.Standard.Primary.Generation;
using MetX.Standard.Primary.Generation.CSharp.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.Generation.CSharp.Project;

[TestClass]
public class GeneratorsCsProjGeneratorTests
{
    [TestMethod][Ignore("Never finished line of code that will not be used but who's code may be re-purposed")]
    public void FromScratchXmlIsAsExpected()
    {
        var generator = TestHelpers.SetupGenerator<GeneratorsCsProjGenerator>(GenFramework.Standard20);
        Assert.IsNotNull(generator);
        generator.Generate();
            
        var actual = generator.Document.OuterXml.AsFormattedXml();
        Assert.IsNotNull(actual);
            
        Assert.IsFalse(actual.Contains(CsProjGeneratorOptions.Delimiter), actual.TokenAt(2, CsProjGeneratorOptions.Delimiter));
        Assert.IsTrue(actual.Contains(GenFramework.Standard20.ToTargetFramework()), actual);
        Assert.IsTrue(actual.Contains("Analyzer"), actual);
        Assert.IsTrue(actual.Contains("Microsoft.CodeAnalysis.Common"), actual);
        Assert.IsTrue(actual.Contains("Microsoft.CodeAnalysis.Analyzers"), actual);
        Assert.IsTrue(actual.Contains("Microsoft.CodeAnalysis.CSharp.Workspaces"), actual);
        Assert.IsTrue(actual.Contains("GenGen.Aspects"), actual);
        Assert.IsTrue(actual.Contains("MetX.Standard.Primary"), actual);
        Assert.IsTrue(actual.Contains("MetX.Standard.Generators"), actual);
        Assert.IsTrue(actual.Contains("Library"), actual);
    }
}