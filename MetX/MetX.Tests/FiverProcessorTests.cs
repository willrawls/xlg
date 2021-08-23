
using MetX.Five;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests
{
    [TestClass]
    public class FiverProcessorTests
    {
        [TestMethod]
        public void Parse_Simple()
        {
            var data = new FiverProcessor();
            data.Parse(@"
~~Template:
    string x = ""Ding"";
");
            Assert.AreEqual(1, data.Areas.Count);
            Assert.AreEqual("Default", data.Areas[0].Name);
            Assert.AreEqual(1, data.Areas.Count);
            Assert.AreEqual(1, data.Templates.Count);
            Assert.AreEqual(InstructionType.Template, data.Templates[0].InstructionType);
            Assert.AreEqual(1, data.Templates[0].Lines.Count);
            Assert.IsTrue(data.Templates[0].Lines[0].Contains(@"string x = ""Ding"";"));

        }

        
    }
}

