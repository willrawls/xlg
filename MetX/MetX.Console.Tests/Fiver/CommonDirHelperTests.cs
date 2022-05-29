using System.IO;
using MetX.Five;
using MetX.Standard.Primary.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Fiver;

[TestClass]
public class CommonDirHelperTests
{
    const string fred = "Fred";
    const string george = "George";
    const string harry = "Harry";
    const string paul = "Paul";

    [TestMethod]
    public void SettingsFile_WriteValue_NewCDM_WriteAnotherValue_NewCDM_BothValuesShouldBeRead()
    {
        var settingsFilePath = "SettingsFile_WriteValue_NewCDM_WriteAnotherValue_NewCDM_BothValuesShouldBeRead.txt";

        FileSystem.SafelyDeleteFile(settingsFilePath);
        Assert.IsFalse(File.Exists(settingsFilePath));

        Shared.InitializeDirs(settingsFilePath, true);

        Shared.Dirs.ToSettingsFile(fred, harry);
        Assert.IsTrue(File.Exists(settingsFilePath));

        Shared.InitializeDirs(settingsFilePath, false);
        Shared.Dirs.ToSettingsFile(george, paul);

        var actual = Shared.Dirs.FromSettingsFile(fred);
        Assert.IsNotNull(actual);
        Assert.AreEqual(harry, actual);

        actual = Shared.Dirs.FromSettingsFile(george);
        Assert.IsNotNull(actual);
        Assert.AreEqual(paul, actual);
    }
    
    [TestMethod]
    public void SettingsFile_RoundTrip()
    {
        var expected = "Fred";
        var settingsFilePath = "SettingsFile_RoundTrip.txt";

        FileSystem.SafelyDeleteFile(settingsFilePath);
        Assert.IsFalse(File.Exists(settingsFilePath));

        Shared.InitializeDirs(settingsFilePath, true);
        
        Shared.Dirs.ToSettingsFile("George", expected);
        Assert.IsTrue(File.Exists(settingsFilePath));

        var actual = Shared.Dirs.FromSettingsFile("George");
        Shared.Dirs.ResetSettingsFile();
        Assert.IsFalse(File.Exists(settingsFilePath));
        
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