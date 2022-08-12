using MetX.Fimm.Setup;

namespace MetX.Console.Tests.Fimm;

public class TestActor : FimmActorBase
{
    public bool FakeReadyToActResult { get; set; }
    public bool ReadyToActWasCalled { get; set; }
    public string FakeReason { get; set; }

    public ProcessorResult ResultFromAct { get; set; } = new();
    public ProcessorResult ResultFromTest { get; set; } = new();

    public bool ActWasCalled { get; set; }

    public override bool ReadyToAct(ArgumentSettings settings, out string reason)
    {
        reason = FakeReason;
        ReadyToActWasCalled = true;
        return FakeReadyToActResult;
    }

    public override ProcessorResult Run(ArgumentSettings settings)
    {
        ActWasCalled = true;
        return ResultFromAct;
    }

    public override ProcessorResult Test(ArgumentSettings settings)
    {
        ActWasCalled = true;
        return ResultFromTest;
    }

}