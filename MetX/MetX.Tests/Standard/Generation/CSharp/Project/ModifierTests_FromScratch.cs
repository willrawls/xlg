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
            Assert.IsFalse(modifier.Document.OuterXml.Contains("~~"));
        }
        
    }
}