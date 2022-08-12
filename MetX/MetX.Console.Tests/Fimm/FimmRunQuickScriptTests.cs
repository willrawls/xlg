using System;
using System.Collections.Generic;
using MetX.Fimm.Setup;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Fimm;

[TestClass]
public class FimmRunQuickScriptTests
{
    private XlgQuickScript TestScript1 = new XlgQuickScript("TestScript1", @"
~~Finish:
  ~~:Ding
");

    [TestMethod]
    public void FindScript_JustLikeFimmProgram()
    {
        var settings = new ArgumentSettings
        {
            Verb = Ron.DetermineVerb("Run"),
            Noun = Ron.DetermineNoun("Script"),
            Name = "TestScripts",
            AdditionalArguments = new List<string>(new[] {"TestScript1"})
        };

        
        var actor = new TestFimmActor(ArgumentVerb.Run, ArgumentNoun.Script, TestScript1);

        var actual = actor.Run(settings);
        Assert.IsNotNull(actual.ActualizationResult);

        Assert.IsTrue(actual.ActualizationResult.ActualizationSuccessful, actual.ActualizationResult.Errors);
    }

    [TestMethod]
    public void RunScript_Simple()
    {

    }
}