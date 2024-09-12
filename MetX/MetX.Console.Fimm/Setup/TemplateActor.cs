using System;
using MetX.Fimm.Scripts;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

namespace MetX.Fimm.Setup;

public class TemplateActor : FimmActorBase
{
    public TemplateActor()
    {

    }

    public TemplateActor(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }

    public override ProcessorResult Walk(ArgumentSettings settings)
    {
        throw new Exception("Start here");
        // <<<<< Start here
        return base.Walk(settings);
    }

    public override ProcessorResult Run(ArgumentSettings settings)
    {
        if(settings.Host == null)
        {
            var context = new ConsoleContext();
            settings.Host = new ConsoleGenerationHost(context);
        }   

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

        var wallaby = new Wallaby(settings.Host);
        return new ProcessorResult
        {
            ActualizationResult = wallaby.BuildActualizeAndCompileQuickScript(scriptToRun)
        };
   
    }
}