using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Techniques.Tests
{
    [TestClass]
    public class PatternWorksTests
    {
        [TestMethod]
        public void ToXmlTest()
        {
            var target = new PatternWorks
            {
                Techniques = new ParticleList<Technique>
                {
                    new(),
                },
                Connections = new ParticleList<Connection>()
            };
            var actual = target.ToXml();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Contains("</PatternWorks>"));
        }
    }
}