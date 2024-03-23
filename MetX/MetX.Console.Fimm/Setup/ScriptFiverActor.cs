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
        XlgQuickScript scriptToRun; // Run script FilenameWithOptionalPath (only the first script runs)
        
        if (settings.AdditionalArguments.IsNotEmpty())
        {
            scriptToRun = Wallaby.FindScript(settings.Name, settings.AdditionalArguments[0]);
        }
        else if (settings.Path.IsNotEmpty())
        {
            scriptToRun = Wallaby.FindScript(settings.Name, settings.Path);
        }
        else if(settings.Name.Contains("\\"))
        {
            var settingsName = settings.Name.LastToken("\\");
            var path = settings.Name.TokensBeforeLast("\\");
            scriptToRun = Wallaby.FindScript(path, settingsName);
        }
        else
        {
            scriptToRun = Wallaby.FindScript(settings.Name);
        }

        var wallaby = new Wallaby(host);
        return new ProcessorResult
        {
            ActualizationResult = wallaby.BuildActualizeAndCompileQuickScript(scriptToRun)
        };
    }
}