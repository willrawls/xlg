using System;
using System.Collections.Generic;

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

        new ValidCombo<ScriptFiverActor>(ArgumentVerb.Run, ArgumentNoun.Script),
        new ValidCombo<ScriptFiverActor>(ArgumentVerb.Build, ArgumentNoun.Script),
        new ValidCombo<ScriptFiverActor>(ArgumentVerb.Clone, ArgumentNoun.Script),
        new ValidCombo<ScriptFiverActor>(ArgumentVerb.Add, ArgumentNoun.Script),
        new ValidCombo<ScriptFiverActor>(ArgumentVerb.Delete, ArgumentNoun.Script),

        new ValidCombo<DatabaseFiverActor>(ArgumentVerb.Gen, ArgumentNoun.Database),
        new ValidCombo<DatabaseFiverActor>(ArgumentVerb.Regen, ArgumentNoun.Database),
        new ValidCombo<DatabaseFiverActor>(ArgumentVerb.Walk, ArgumentNoun.Database),
        new ValidCombo<DatabaseFiverActor>(ArgumentVerb.Add, ArgumentNoun.Database),
        new ValidCombo<DatabaseFiverActor>(ArgumentVerb.Remove, ArgumentNoun.Database),

        new ValidCombo<FolderFiverActor>(ArgumentVerb.Walk, ArgumentNoun.Folder),
        new ValidCombo<FolderFiverActor>(ArgumentVerb.Add, ArgumentNoun.Folder),
        new ValidCombo<FolderFiverActor>(ArgumentVerb.Remove, ArgumentNoun.Folder),
        new ValidCombo<FolderFiverActor>(ArgumentVerb.Delete, ArgumentNoun.Folder),

        new ValidCombo<FileActor>(ArgumentVerb.Walk, ArgumentNoun.Xml),
        new ValidCombo<FileActor>(ArgumentVerb.Walk, ArgumentNoun.Xsd),
        new ValidCombo<FileActor>(ArgumentVerb.Walk, ArgumentNoun.Json),
        new ValidCombo<FileActor>(ArgumentVerb.Add, ArgumentNoun.Xml),
        new ValidCombo<FileActor>(ArgumentVerb.Add, ArgumentNoun.Xsd),
        new ValidCombo<FileActor>(ArgumentVerb.Add, ArgumentNoun.Json),
        new ValidCombo<FileActor>(ArgumentVerb.Remove, ArgumentNoun.Xml),
        new ValidCombo<FileActor>(ArgumentVerb.Remove, ArgumentNoun.Xsd),
        new ValidCombo<FileActor>(ArgumentVerb.Remove, ArgumentNoun.Json),

        new ValidCombo<GenGenActor>(ArgumentVerb.Build, ArgumentNoun.GenGen),
        new ValidCombo<GenGenActor>(ArgumentVerb.Add, ArgumentNoun.GenGen),
        new ValidCombo<GenGenActor>(ArgumentVerb.Remove, ArgumentNoun.GenGen),
        new ValidCombo<GenGenActor>(ArgumentVerb.Delete, ArgumentNoun.GenGen),

        new ValidCombo<StageActor>(ArgumentVerb.Stage, ArgumentNoun.Project),
        new ValidCombo<StageActor>(ArgumentVerb.Add, ArgumentNoun.Project),
        new ValidCombo<StageActor>(ArgumentVerb.Remove, ArgumentNoun.Project),

        new ValidCombo<LibraryActor>(ArgumentVerb.Add, ArgumentNoun.Library),
        new ValidCombo<LibraryActor>(ArgumentVerb.Remove, ArgumentNoun.Library),

        new ValidCombo<TemplateActor>(ArgumentVerb.Add, ArgumentNoun.Template),
        new ValidCombo<TemplateActor>(ArgumentVerb.Remove, ArgumentNoun.Template),
        new ValidCombo<TemplateActor>(ArgumentVerb.Clone, ArgumentNoun.Template)
    };
}

public class TemplateActor : FiverActorBase
{
    public TemplateActor() {}
    public TemplateActor(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }
}

public class LibraryActor : FiverActorBase
{
    public LibraryActor(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }

    public LibraryActor()
    {
    }
}

public class StageActor : FiverActorBase
{
    public StageActor(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }

    public StageActor()
    {
    }
}

public class GenGenActor : FiverActorBase
{
    public GenGenActor(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }

    public GenGenActor()
    {
    }
}