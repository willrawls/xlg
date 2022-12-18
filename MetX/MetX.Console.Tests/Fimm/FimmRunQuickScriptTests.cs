using System;
using System.Collections.Generic;
using MetX.Fimm.Setup;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Fimm;

[TestClass]
public class FimmRunQuickScriptTests
{
    public XlgQuickScript TestScript1 = new("TestScript1", @"
~~Finish:
  ~~:Ding
");

    public XlgQuickScript FimDoesNotExist = new("fimm1");

    [TestMethod]
    public void FindScript_JustLikeFimmProgram()
    {
        var settings = BuildRunScriptWithArguments("TestScripts", "", new[] {"TestScript1"});
        var actor = new TestFimmActor(ArgumentVerb.Run, ArgumentNoun.Script, TestScript1);
        var actual = actor.Run(settings);
        Assert.IsNotNull(actual.ActualizationResult);

        Assert.IsTrue(actual.ActualizationResult.ActualizationSuccessful, actual.ActualizationResult.Errors);
    }

    [TestMethod]
    public void SingleFimmScript_DoesNotExist()
    {
        var settings = BuildRunScriptWithArguments("TestScripts", "", new[] {"TestScript1"});
        var actor = new TestFimmActor(ArgumentVerb.Run, ArgumentNoun.Script, FimDoesNotExist);
        var actual = actor.Run(settings);
        Assert.IsNotNull(actual.ActualizationResult);

        Assert.IsTrue(actual.ActualizationResult.ActualizationSuccessful, actual.ActualizationResult.Errors);
    }

    private static ArgumentSettings BuildRunScriptWithArguments(string scriptName, string scriptPath = "", params string[] additionalArguments)
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
    
}