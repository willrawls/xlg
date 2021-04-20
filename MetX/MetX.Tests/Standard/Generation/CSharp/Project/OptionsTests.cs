using MetX.Standard.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    [TestClass]
    public class CsProjGeneratorOptionsTests
    {
        [TestMethod]
        public void AssertValid_OptionsAreValid()
        {
            var data = CsProjGeneratorOptions.Defaults();
            var actual = data.AssertValid();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
        }

    }
}