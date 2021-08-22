using MetX.Five;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests2
{
    [TestClass]
    public class AreaTests
    {
        [TestMethod]
        public void New_TemplateInstructionWith2Arguments()
        {
            var processingAlreadyBeganPreviously = false;
            var area = new Area("Template", "Table ?", ref processingAlreadyBeganPreviously);
            Assert.AreEqual(InstructionType.Template, area.Instruction);
            Assert.AreEqual(2, area.InstructionArguments.Count);
        }
    }
}
