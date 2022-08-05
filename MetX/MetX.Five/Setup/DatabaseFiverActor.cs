using System;

namespace MetX.Five.Setup;

public class DatabaseFiverActor : FiverActorBase
{
    public new bool ReadyToAct(ArgumentSettings settings, out string reason)
    {
        throw new NotImplementedException();
    }

    public DatabaseFiverActor()
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

    public DatabaseFiverActor(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }
}