using System;
using System.Collections.Generic;
using System.Linq;

namespace MetX.Five.Setup;

public static class Ron
{
    public static ArgumentVerb DetermineVerb(string text)
    {
        if (Enum.TryParse(
                typeof(ArgumentVerb),
                text,
                true,
                out var result))
        {
            var verb 
                = (result as ArgumentVerb? 
                   ?? ArgumentVerb.Unknown);
            return verb;
        }

        return ArgumentVerb.Unknown;
    }

    public static ArgumentNoun DetermineNoun(string text)
    {
        if (Enum.TryParse(
                typeof(ArgumentNoun),
                text,
                true,
                out var result))
        {
            var verb 
                = (result as ArgumentNoun? 
                   ?? ArgumentNoun.Unknown);
            return verb;
        }

        return ArgumentNoun.Unknown;
    }

    public static List<FiverActorBase> SupportedFiverActions => new()
    {

        new ScriptFiverActor(ArgumentVerb.Run, ArgumentNoun.Script),
        new ScriptFiverActor(ArgumentVerb.Build, ArgumentNoun.Script),
        new ScriptFiverActor(ArgumentVerb.Clone, ArgumentNoun.Script),
        new ScriptFiverActor(ArgumentVerb.Add, ArgumentNoun.Script),
        new ScriptFiverActor(ArgumentVerb.Delete, ArgumentNoun.Script),

        new DatabaseFiverActor(ArgumentVerb.Gen, ArgumentNoun.Database),
        new DatabaseFiverActor(ArgumentVerb.Regen, ArgumentNoun.Database),
        new DatabaseFiverActor(ArgumentVerb.Walk, ArgumentNoun.Database),
        new DatabaseFiverActor(ArgumentVerb.Add, ArgumentNoun.Database),
        new DatabaseFiverActor(ArgumentVerb.Remove, ArgumentNoun.Database),

        new FolderFiverActor(ArgumentVerb.Walk, ArgumentNoun.Folder),
        new FolderFiverActor(ArgumentVerb.Add, ArgumentNoun.Folder),
        new FolderFiverActor(ArgumentVerb.Remove, ArgumentNoun.Folder),
        new FolderFiverActor(ArgumentVerb.Delete, ArgumentNoun.Folder),

        new FileActor(ArgumentVerb.Walk, ArgumentNoun.Xml),
        new FileActor(ArgumentVerb.Walk, ArgumentNoun.Xsd),
        new FileActor(ArgumentVerb.Walk, ArgumentNoun.Json),
        new FileActor(ArgumentVerb.Add, ArgumentNoun.Xml),
        new FileActor(ArgumentVerb.Add, ArgumentNoun.Xsd),
        new FileActor(ArgumentVerb.Add, ArgumentNoun.Json),
        new FileActor(ArgumentVerb.Remove, ArgumentNoun.Xml),
        new FileActor(ArgumentVerb.Remove, ArgumentNoun.Xsd),
        new FileActor(ArgumentVerb.Remove, ArgumentNoun.Json),

        new GenGenActor(ArgumentVerb.Build, ArgumentNoun.GenGen),
        new GenGenActor(ArgumentVerb.Add, ArgumentNoun.GenGen),
        new GenGenActor(ArgumentVerb.Remove, ArgumentNoun.GenGen),
        new GenGenActor(ArgumentVerb.Delete, ArgumentNoun.GenGen),

        new StageActor(ArgumentVerb.Stage, ArgumentNoun.Project),
        new StageActor(ArgumentVerb.Add, ArgumentNoun.Project),
        new StageActor(ArgumentVerb.Remove, ArgumentNoun.Project),

        new LibraryActor(ArgumentVerb.Add, ArgumentNoun.Library),
        new LibraryActor(ArgumentVerb.Remove, ArgumentNoun.Library),

        new TemplateActor(ArgumentVerb.Add, ArgumentNoun.Template),
        new TemplateActor(ArgumentVerb.Remove, ArgumentNoun.Template),
        new TemplateActor(ArgumentVerb.Clone, ArgumentNoun.Template)
    };

    public static FiverActorBase GetActor(this ArgumentVerb verb, ArgumentNoun noun)
    {
        return SupportedFiverActions
            .FirstOrDefault(a => a
                                 .Verb == verb 
                                 && a.Noun == noun);
    }
}