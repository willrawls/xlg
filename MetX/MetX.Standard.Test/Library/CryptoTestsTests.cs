using MetX.Standard.Library.Encryption;

namespace MetX.Standard.Test.Library;

[TestClass]
public class CryptoTestsTests
{
    [TestMethod]
    public void CryptoTestsTest_Simple()
    {
        var expected = "Freddy";
        var data = Crypt.ToBase64(expected);
        var actual = Crypt.FromBase64(data);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }
}