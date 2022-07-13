namespace MetX.Five.Setup;

public class FiverActorBase : FiveActorAbstractBase
{
    public FiverActorBase()
    {
    }

    public FiverActorBase(ArgumentVerb verb, ArgumentNoun noun)
    {
        Verb = verb;
        Noun = noun;
    }
}