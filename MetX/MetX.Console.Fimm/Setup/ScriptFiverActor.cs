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
        var context = new ConsoleContext();
        IGenerationHost host = new ConsoleGenerationHost(context);
        var wallaby = new Wallaby(host);
        var scriptToRun = settings.AdditionalArguments.IsNotEmpty() 
            ? wallaby.FindScript(settings.Name, settings.AdditionalArguments[0]) // Run Script NameInFile FilenameWithOptionalPath
            : wallaby.FindScript(settings.Name); // Run script FilenameWithOptionalPath (only the first script runs)

        return new ProcessorResult
        {
            ActualizationResult = wallaby.RunQuickScript(scriptToRun),
        };
    }
}