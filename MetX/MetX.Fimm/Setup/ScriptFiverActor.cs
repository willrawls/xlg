using MetX.Fimm.Scripts;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Fimm.Setup;

public class ScriptFimmActor : FimmActorBase
{
    public ScriptFimmActor(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }

    public ScriptFimmActor()
    {
    }

    public override ProcessorResult Run(ArgumentSettings settings)
    {
        var context = new ConsoleContext();
        IGenerationHost host = new ConsoleGenerationHost(context);
        var wallaby = new Wallaby(host);
        var scriptToRun = wallaby.FindScript(settings.Name);

        return new ProcessorResult
        {
            ActualizationResult = wallaby.RunQuickScript(scriptToRun),
        };
    }
}