using System;
using System.Collections.Generic;
using MetX.Five.Setup;
using MetX.Standard.Primary.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Fiver;

[TestClass]
public class ComboTests
{
    [TestMethod]
    public void TestScript_Simple()
    {
        var validCombo = new ValidCombo<TestActor>(ArgumentVerb.Test, ArgumentNoun.Script);
        
        var testActor = validCombo.Factory();
        testActor.FakeReadyToActResult = true;

        var quickScriptTemplate = new XlgQuickScriptTemplate(@"TestTemplates\TestExe");
        quickScriptTemplate.Name = "George";
        XlgQuickScript quickScript = new XlgQuickScript("Freddy", "a = b;");


        testActor.ResultFromAct = new ProcessorResult
        {
            ActualizationResult = new ActualizationResult
                (new ActualizationSettings(
                    quickScriptTemplate, true, 
                    quickScript, false, null)),
        };

        var settings = new ArgumentSettings
        {
            Verb = ArgumentVerb.Test,
            Noun = ArgumentNoun.Script,
            Name = "Fred",
            AdditionalArguments = new List<string>
            {
                "George"
            }
        };

        Assert.IsTrue(testActor.ReadyToAct(settings, out var reason));
        Assert.IsNull(reason);
        var actual = testActor[settings](settings);
        Assert.IsNotNull(actual);
    }
}