using MetX.Fimm.Scripts;
using MetX.Fimm.Setup;
using MetX.Standard.Primary.Scripts;

namespace MetX.Console.Tests.Fimm;

public class TestFimmActor : FimmActorBase
{
    public XlgQuickScript Target { get; set; }

    public TestFimmActor(ArgumentVerb verb, ArgumentNoun noun, XlgQuickScript target) : base(verb, noun)
    {
        Target = target;
    }

    public override ProcessorResult Run(ArgumentSettings settings)
    {
        var scriptToRun = Target; // wallaby.FindScript(settings.Name);
        var context = new TestContext();
        var wallaby = new Wallaby(context.Host);

        return new ProcessorResult
        {
            ActualizationResult = wallaby.BuildActualizeAndCompileQuickScript(scriptToRun),
        };
    }
}