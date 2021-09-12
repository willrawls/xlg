using MetX.Standard.Generation;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    public partial class ConsoleClientCsProjGeneratorTests
    {
        [TestMethod][Ignore("Never finished line of code that will not be used but who's code may be re-purposed")]
        public void FromScratchXmlIsAsExpected()
        {
            var generator = TestHelpers.SetupGenerator<ClientCsProjGenerator>(GenFramework.Net50Windows);
            Assert.IsNotNull(generator);
            generator.Generate();
            var actual = generator.Document.OuterXml.AsFormattedXml();
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.Contains(CsProjGeneratorOptions.Delimiter), actual.TokenAt(2, CsProjGeneratorOptions.Delimiter));
            Assert.IsTrue(actual.Contains("net-5.0windows"));
            Assert.IsTrue(actual.Contains("Analyzer"));
            Assert.IsTrue(actual.Contains("ReferenceOutputAssembly"));
        }
    }
}