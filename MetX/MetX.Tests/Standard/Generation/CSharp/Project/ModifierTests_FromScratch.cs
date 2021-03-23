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
        public void ClientFromScratchXmlIsAsExpected()
        {
            ClientCsProjGenerator clientCsProjGenerator = ClientCsProjGenerator.FromScratch(GenGenOptions.Defaults);

            Assert.IsNotNull(clientCsProjGenerator);
            var actual = clientCsProjGenerator.Document.OuterXml;
            Assert.IsFalse(actual.Contains("~~"));
            Assert.IsTrue(actual.Contains("net-5.0windows"));
        }

        [TestMethod]
        public void TESTNAME_Simple()
        {
            var data = new ClientCsProjGenerator(options)
            {
                
            };

            var expected = "EXPECTED";
            var actual = data.METHODNAME();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

    }
}