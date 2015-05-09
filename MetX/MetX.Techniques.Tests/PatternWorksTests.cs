using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Techniques.Tests
{
    [TestClass()]
    public class PatternWorksTests
    {
        [TestMethod()]
        public void ToXmlTest()
        {
            PatternWorks target = new PatternWorks
            {
                Techniques = new ParticleList<Technique>
                {
                    new Technique
                    {
                    },
                },
                Connections = new ParticleList<Connection>
                {
                }
            };
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ToXml();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}