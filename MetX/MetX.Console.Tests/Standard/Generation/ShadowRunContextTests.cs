/*using System.Reflection;
using MetX.Standard.Generators;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation
{
    [TestClass]
    public class ShadowRunContextTests
    {
        [TestMethod][Ignore("Never finished line of code that will not be used but who's code may be re-purposed")]
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
}*/