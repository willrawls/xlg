using System.IO;
using System.Text;
using System.Xml.Serialization;
using MetX.Five.Metadata;
using MetX.Standard.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Metadata
{
    [TestClass]
    public class xlgDocTests 
    {

        [TestMethod]
        public void XlgDocToAndFromXml_xlgDocWorksWithXmlSerializer()
        {
            var expected = "<xlgDoc></xlgDoc>";
            var serializer = new XmlSerializer(typeof(xlgDoc));
            
            var sb = new StringBuilder();
            using var reader = new StringReader(expected);
            var deserialized = (xlgDoc) serializer.Deserialize(reader);
            
            using(var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, deserialized);
            }

            var actual = sb.ToString();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Contains("<xlgDoc"));
            Assert.IsTrue(actual.Contains("/xlgDoc"));
        }
    }
}