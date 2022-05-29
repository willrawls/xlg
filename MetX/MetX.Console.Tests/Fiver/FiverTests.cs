using System.IO;
using MetX.Five;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Fiver;

[TestClass]
public class FiverTests
{
    [TestMethod]
    public void SettingsFile_RoundTrip()
    {
        var expected = "Fred";
        
        Shared.InitializeDirs("SettingsFile_RoundTrip.txt", true);
        
        Shared.Dirs.ToSettingsFile("George", expected);
        var actual = Shared.Dirs.FromSettingsFile("George");
        Shared.Dirs.ResetSettingsFile();

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void SettingsFile_RoundTripTwice()
    {
        var expected = "Fred";

        // Trip 1
        const string filePath = "SettingsFile_RoundTripTwice.txt";

        Shared.Dirs.SettingsFilePath = filePath;
        Shared.Dirs.ResetSettingsFile();
        Assert.IsFalse(File.Exists(filePath));

        Shared.Dirs.ToSettingsFile("George", expected);
        Assert.IsTrue(File.Exists(filePath));

        var actual = Shared.Dirs.FromSettingsFile("George");
        Shared.Dirs.ResetSettingsFile();
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);

        // Trip 2
        Shared.Dirs.SettingsFilePath = filePath;
        Shared.Dirs.ResetSettingsFile();
        Assert.IsFalse(File.Exists(filePath));

        Shared.Dirs.ToSettingsFile("George", expected);
        Assert.IsTrue(File.Exists(filePath));

        actual = Shared.Dirs.FromSettingsFile("George");
        Shared.Dirs.ResetSettingsFile();

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

}