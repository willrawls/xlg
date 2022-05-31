using System.Collections.Generic;
using MetX.Five;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Fiver;

[TestClass]
public class ComboTests
{
    [TestMethod]
    public void TESTNAME_Simple()
    {
        var data = new ValidCombo<TestActor>(ArgumentVerb.Test, ArgumentNoun.Script);
        
        ArgumentSettings settings = new ArgumentSettings
        {
            Verb = ArgumentVerb.Test ,
            Noun = ArgumentNoun.Script,
            Name = "Fred",
            AdditionalArguments = new List<string>
            {
                "George",
            }
        };

        Assert.IsTrue(data.ReadyToAct(settings, out var reason));
        Assert.IsNull(reason);
        var actual = data.Act(settings);
    }
}

public class TestActor : IAct
{
    public TestActor()
    {
    }

    public bool FakeReadyToActResult { get; set; }
    public bool ReadyToActWasCalled { get; set; }
    public string FakeReason { get; set; }
    public ProcessorResult ResultFromAct { get; set; } = new();

    public bool ActWasCalled { get; set; }

    public bool ReadyToAct(ArgumentSettings settings, out string reason)
    {
        reason = FakeReason;
        ReadyToActWasCalled = true;
        return FakeReadyToActResult;
    }

    public ProcessorResult Act(ArgumentSettings settings)
    {
        ActWasCalled = true;
        return ResultFromAct;
    }
}