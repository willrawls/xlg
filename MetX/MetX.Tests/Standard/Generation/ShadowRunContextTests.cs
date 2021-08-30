using System.Reflection;
using MetX.Standard.Generators;
using MetX.Standard.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation
{
    [TestClass]
    public class ShadowRunContextTests
    {
        [TestMethod]
        public void Initialize_AddStaticCodeActual()
        {
            var data = new AddStaticCode();
            var actual = data.InitializeShadowRunContext();
            Assert.IsTrue(actual);
            
            Assert.IsTrue(data.FullNameOfActual.IsNotEmpty());
            Assert.IsNotNull(data.ShadowRunContext);
            Assert.IsTrue(data.PathToActual.IsNotEmpty());
            Assert.IsTrue(data.ShadowDomainName.IsNotEmpty());
        }
    }
}