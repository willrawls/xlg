using MetX.Aspects;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Tests.Standard.Generation.CSharp.Project.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    [TestClass]
    public class ModifierTests_FromScratch
    {
        [TestMethod]
        public void FromScratchXmlIsAsExpected()
        {
            Modifier modifier = Modifier.FromScratch(CSharpProjectForGeneratorClientOptions.Defaults);

            Assert.IsNotNull(modifier);
            var actual = modifier.Document.OuterXml;
            Assert.IsFalse(actual.Contains("~~"));
            Assert.IsTrue(actual.Contains("net-5.0windows"));
        }
        
    }
}