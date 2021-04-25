using System;
using MetX.Standard.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Metadata
{
    [TestClass]
    public class XlgDocTestsTests
    {
        [TestMethod]
        public void ReadXlgDocFromFile_Simple()
        {
            var data = new MetX.Standard.Metadata.xlgDoc
            {
                Now = DateTime.Now.ToString("s"),
                Tables = new Tables[]
                {
                    new TablesTable
                }
            };
        }

        var expected = "";
            var actual = null;

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}