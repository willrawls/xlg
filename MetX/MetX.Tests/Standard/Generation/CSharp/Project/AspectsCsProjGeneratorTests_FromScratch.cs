using MetX.Aspects;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Standard.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    [TestClass]
    public partial class AspectsCsProjGeneratorTests
    {
        [TestMethod]
        public void FromScratchXmlIsAsExpected()
        {
            var generator = TestHelpers
                .SetupGenerator<AspectsCsProjGenerator>();

            Assert.IsNotNull(generator);

            generator.Generate();
            var actual = generator.Document.OuterXml.AsFormattedXml();
            Assert.IsFalse(actual.Contains(CsProjGeneratorOptions.Delimiter), actual);
            Assert.IsFalse(actual.Contains("Analyzer"), actual);
            Assert.IsTrue(actual.Contains(GenFramework.Standard20.ToTargetFramework()), actual);
            Assert.IsTrue(actual.Contains(generator.Options.AspectsName), actual);
        }
    }
}