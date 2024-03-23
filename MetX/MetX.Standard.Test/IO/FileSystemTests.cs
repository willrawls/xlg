using MetX.Standard.Primary.IO;

namespace MetX.Standard.Test.IO;

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

    /*
    [TestMethod]
    public void DeepContents_PathShouldNotHaveTheFilenameInIt()
    {
        var actual = FileSystem.DeepContents(@"Standard\Generation\CSharp\Project\Pieces\");
        Assert.IsFalse(actual.Files[0].Path.Contains(actual.Files[0].Name));
    }
    */

    [TestMethod]
    public void FindAscendantDirectory_Simple()
    {
        var actual = FileSystem.FindAscendantDirectory(AppDomain.CurrentDomain.BaseDirectory, "bin", 5);

        Assert.IsNotNull(actual);
        
        var message = "\n" + actual + "\n" + AppDomain.CurrentDomain.BaseDirectory + "\n";
        Assert.IsTrue(
            actual.EndsWith(@"Test\bin"), 
            message);
    }

}