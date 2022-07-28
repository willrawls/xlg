using System.Diagnostics;
using MetX.Five.Scripts;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Five.Setup;

public class ScriptFiverActor : FiverActorBase
{
    public ScriptFiverActor(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }

    public ScriptFiverActor()
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
            ActualizationResult = wallaby.FiverRunScript(scriptToRun),
        };
    }
}