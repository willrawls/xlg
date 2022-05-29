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
        Dirs.Initialize();
        
        Dirs.SettingsFilePath = "SettingsFile_RoundTrip.txt";
        Dirs.ResetSettingsFile();

        Dirs.SettingsFilePath = "SettingsFile_RoundTrip.txt";

        Dirs.ToSettingsFile("George", expected);
        var actual = Dirs.FromSettingsFile("George");
        Dirs.ResetSettingsFile();

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void SettingsFile_RoundTripTwice()
    {
        var expected = "Fred";

        // Trip 1
        const string filePath = "SettingsFile_RoundTripTwice.txt";

        Dirs.SettingsFilePath = filePath;
        Dirs.ResetSettingsFile();
        Assert.IsFalse(File.Exists(filePath));

        Dirs.ToSettingsFile("George", expected);
        Assert.IsTrue(File.Exists(filePath));

        var actual = Dirs.FromSettingsFile("George");
        Dirs.ResetSettingsFile();
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);

        // Trip 2
        Dirs.SettingsFilePath = filePath;
        Dirs.ResetSettingsFile();
        Assert.IsFalse(File.Exists(filePath));

        Dirs.ToSettingsFile("George", expected);
        Assert.IsTrue(File.Exists(filePath));

        actual = Dirs.FromSettingsFile("George");
        Dirs.ResetSettingsFile();

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

}