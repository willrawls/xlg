﻿using MetX.Standard.Library.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.Library;

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