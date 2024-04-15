using System;
using System.Collections.Generic;
using System.Linq;

namespace MetX.Fimm.Setup;

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

    public static List<FimmActorBase> SupportedActions => new()
    {

        new ScriptFimmActor(ArgumentVerb.Run, ArgumentNoun.Script),
        new ScriptFimmActor(ArgumentVerb.Build, ArgumentNoun.Script),
        new ScriptFimmActor(ArgumentVerb.Clone, ArgumentNoun.Script),
        new ScriptFimmActor(ArgumentVerb.Add, ArgumentNoun.Script),
        new ScriptFimmActor(ArgumentVerb.Delete, ArgumentNoun.Script),

        new DatabaseFimmActor(ArgumentVerb.Gen, ArgumentNoun.Database),
        new DatabaseFimmActor(ArgumentVerb.Regen, ArgumentNoun.Database),
        new DatabaseFimmActor(ArgumentVerb.Walk, ArgumentNoun.Database),
        new DatabaseFimmActor(ArgumentVerb.Add, ArgumentNoun.Database),
        new DatabaseFimmActor(ArgumentVerb.Remove, ArgumentNoun.Database),

        new FolderFimmActor(ArgumentVerb.Walk, ArgumentNoun.Folder),
        new FolderFimmActor(ArgumentVerb.Add, ArgumentNoun.Folder),
        new FolderFimmActor(ArgumentVerb.Remove, ArgumentNoun.Folder),
        new FolderFimmActor(ArgumentVerb.Delete, ArgumentNoun.Folder),

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
        new TemplateActor(ArgumentVerb.Clone, ArgumentNoun.Template),
        new TemplateActor(ArgumentVerb.Walk, ArgumentNoun.TemplateFolder),
        new TemplateActor(ArgumentVerb.Walk, ArgumentNoun.Database),
        new TemplateActor(ArgumentVerb.Walk, ArgumentNoun.Folder),
    };

    public static FimmActorBase GetActor(this ArgumentVerb verb, ArgumentNoun noun)
    {
        return SupportedActions
            .FirstOrDefault(a => a
                                 .Verb == verb 
                                 && a.Noun == noun);
    }
}