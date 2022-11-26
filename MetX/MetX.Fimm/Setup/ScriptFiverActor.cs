using MetX.Fimm.Scripts;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings;

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
        XlgQuickScript scriptToRun = null;

        var context = new ConsoleContext();
        IGenerationHost host = new ConsoleGenerationHost(context);
        var wallaby = new Wallaby(host);
        if(settings.AdditionalArguments.IsNotEmpty())
        {
            scriptToRun = wallaby.FindScript(settings.Name, settings.AdditionalArguments[0]);
        }
        else
        {
            scriptToRun = wallaby.FindScript(settings.Name);
        }

        return new ProcessorResult
        {
            ActualizationResult = wallaby.RunQuickScript(scriptToRun),
        };
    }
}