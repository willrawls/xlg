using MetX.Aspects;
using MetX.Standard.Generation.CSharp.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    [TestClass]
    public partial class AspectsCsProjGeneratorTests
    {
        [TestMethod]
        public void FromScratchXmlIsAsExpected()
        {
            var generator = TestHelpers.SetupGenerator<AspectsCsProjGenerator>();

            Assert.IsNotNull(generator);
            var actual = generator.Document.OuterXml;
            Assert.IsFalse(actual.Contains(CsProjGeneratorOptions.Delimiter));
            Assert.IsFalse(actual.Contains("Analyzer"));
            Assert.IsTrue(actual.Contains(GenFramework.Standard20.ToTargetFramework()));
            Assert.IsTrue(actual.Contains(generator.Options.AspectsName));
        }
    }
}