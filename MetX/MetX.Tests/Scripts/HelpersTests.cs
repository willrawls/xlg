using System;
using MetX.Standard.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Scripts
{
    [TestClass()]
    public class HelpersTests
    {
        [TestMethod()]
        public void ExpandScriptLineVariablesTest()
        {
            var data = "a%temp%b%%c";
            var actual = data.ExpandScriptLineVariables();
            var temp = Environment.GetEnvironmentVariable("TEMP");
            var expected = $"a{temp}bc";
            Assert.AreEqual(expected, actual);
        }
    }
}