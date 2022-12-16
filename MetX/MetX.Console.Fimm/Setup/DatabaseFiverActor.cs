using System;

namespace MetX.Fimm.Setup;

public class DatabaseFimmActor : FimmActorBase
{
    public new bool ReadyToAct(ArgumentSettings settings, out string reason)
    {
        throw new NotImplementedException();
    }

    public DatabaseFimmActor()
    {
        
    }

    public override ProcessorResult Run(ArgumentSettings settings)
    {
        throw new NotImplementedException();
    }

    public ProcessorResult Act(ArgumentSettings settings)
    {
        throw new NotImplementedException();
    }

    public DatabaseFimmActor(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }
}