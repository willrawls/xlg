using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetX.Standard.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetX.Standard.Scripts.Tests
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