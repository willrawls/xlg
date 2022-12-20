using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MetX.Fimm;
using MetX.Fimm.Setup;
using MetX.Standard.Primary.IO;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.Mime.MediaTypeNames;

namespace MetX.Console.Tests.Fimm;

[TestClass]
public class FimmRunQuickScriptTests
{
    public XlgQuickScript FimDoesNotExist = new("fimm1");
    public XlgQuickScript TestScript1 = new("TestScript1", @"
~~Finish:
  ~~:Ding
");

    public static string TempPath => Environment.GetEnvironmentVariable("TEMP");

    public static string RandomizedTempScriptFilename(string name) =>
        Path.Combine(TempPath, name + Guid.NewGuid().ToString("N") + ".test.fimm");

    public static void CleanUpTestFimmFilesInTemp()
    {
        var files = Directory.GetFiles(Environment.GetEnvironmentVariable("TEMP"), ".test.fimm");
        foreach (var file in files)
            FileSystem.SafelyDeleteFile(file);
    }
    
    private static ArgumentSettings ArgumentSettingsFactory(string scriptName, string scriptPath = "",
        params string[] additionalArguments)
    {
        var settings = new ArgumentSettings
        {
            Verb = Ron.DetermineVerb("Run"),
            Noun = Ron.DetermineNoun("Script"),
            Name = scriptName,
            AdditionalArguments = new List<string>(additionalArguments)
        };
        if (scriptPath.IsNotEmpty())
            settings.Path = scriptPath;
        return settings;
    }

    [TestMethod]
    public void FindScript_JustLikeFimmProgram()
    {
        var settings = ArgumentSettingsFactory("TestScripts", "", "TestScript1");
        var actor = new TestFimmActor(ArgumentVerb.Run, ArgumentNoun.Script, TestScript1);
        var actual = actor.Run(settings);
        Assert.IsNotNull(actual.ActualizationResult);

        Assert.IsTrue(actual.ActualizationResult.ActualizationSuccessful, actual.ActualizationResult.Errors);
    }

    [TestMethod]
    public void SingleFimmScript_DoesNotExist()
    {
        var args = new[]
        {
            "run", "script", "scriptthatdoesnotexist"
        };
        var results = TestHarnessAction(args, out var stringBuilder);
        Assert.IsTrue(results.Contains("Error"));
    }

    [TestMethod]
    public void SingleFimmScript_Exists_Outputs_Ding()
    {
        var fimmOutputDing = "fimmOutputDing";
        var testFimm = SetupTestFimmAndGetFilename(fimmOutputDing, TestScript1.Script);
        var args = new[]
        {
            "run", "script", testFimm 
        };
        Assert.IsTrue(testFimm.IsNotEmpty());
        var results = TestHarnessAction(args, out var stringBuilder);

       FileSystem.SafelyDeleteFile(testFimm);
       Assert.IsFalse(results.Contains("Error"), results );
    }

    public string SetupTestFimmAndGetFilename(string name, string script)
    {
        CleanUpTestFimmFilesInTemp();
        var filename = RandomizedTempScriptFilename(name);
        var xlgScript = XlgQuickScriptFile.FimmFileFormatScriptFactory(script);
        File.WriteAllText(filename, xlgScript);
        return filename;
    }

    public static string TestHarnessAction(string[] args, out StringBuilder stringBuilder)
    {
        stringBuilder = new StringBuilder();
        using TextWriter textWriter = new StringWriter(stringBuilder);
        Harness.ActOn(args, textWriter);
        var results = stringBuilder.ToString();
        return results;
    }
}