namespace MetX.Five.Setup;

public class GenGenActor : FiverActorBase
{
    public GenGenActor()
    {

    }

    public GenGenActor(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }

    public override ProcessorResult Run(ArgumentSettings settings)
    {
        throw new System.NotImplementedException();
    }
}