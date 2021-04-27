using System.IO;
using System.Text;
using System.Xml.Serialization;
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

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, deserialized);
            }

            var actual = sb.ToString();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Contains("<xlgDoc"));
            Assert.IsTrue(actual.Contains("/xlgDoc"));
        }

        [TestMethod]
        public void ReadXlgDocFromString_Simple()
        {
            var actual = xlgDoc.FromXml(FredsXml);
            TestAgainstFredXml(actual);
        }

        [TestMethod]
        public void RoundTripXlgDocFromString_Simple()
        {
            // xml > object > xml
            var document = xlgDoc.FromXml(FredsXml).ToXml();

            // xml > object
            var actual = xlgDoc.FromXml(document);

            // xmlDoc contents as expected?
            TestAgainstFredXml(actual);
        }

        private static void TestAgainstFredXml(xlgDoc actual)
        {
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Tables);
            Assert.AreEqual(1, actual.Tables.Count);
            var actualTable = actual.Tables[0];
            Assert.IsNotNull(actualTable.Columns);
            Assert.AreEqual(1, actualTable.Columns.Count);
            Assert.AreEqual("Fred", actualTable.TableName);
            Assert.AreEqual("George", actualTable.Columns[0].ColumnName);

            Assert.IsNotNull(actualTable.Keys);
            Assert.AreEqual(1, actualTable.Keys.Count);
            Assert.AreEqual("Mary", actualTable.Keys[0].Name);
            Assert.AreEqual("Frank", actualTable.Keys[0].Columns[0].Column);

            Assert.IsNotNull(actualTable.Indexes);
            Assert.AreEqual(1, actualTable.Indexes.Count);
            Assert.AreEqual("Harry", actualTable.Indexes[0].IndexName);
            Assert.AreEqual("Aby", actualTable.Indexes[0].IndexColumns[0].IndexColumnName);
        }

        string FredsXml = @"
<xlgDoc>
  <Tables>
    <Table TableName=""Fred"">
        <Columns>
            <Column ColumnName=""George"" />
        </Columns>
        <Keys>
            <Key Name=""Mary"">
                <Columns>
                    <Column Column=""Frank"" />
                </Columns>
            </Key>
        </Keys>
        <Indexes>
            <Index IndexName=""Harry"">
                <IndexColumns>
                    <IndexColumn IndexColumnName=""Aby"" />
                </IndexColumns>
            </Index>
        </Indexes>
    </Table>
  </Tables>
</xlgDoc>
";


    }
}