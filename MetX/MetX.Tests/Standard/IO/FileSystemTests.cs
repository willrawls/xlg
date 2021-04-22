using MetX.Standard.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests
{
    [TestClass]
    public class FileSystemTests
    {
        [TestMethod]
        public void FindExecutable_Simple()
        {
            var expected = @"c:\windows\system32\xcopy.exe";
            var actual = FileSystem.FindExecutableAlongPath(@"D:\A\B\xcopy.exe");

            Assert.IsNotNull(actual);
            actual = actual.ToLower();
            Assert.AreEqual(expected, actual);
        }
    }
}