namespace MetX.Fimm.Setup;

public class GenGenActor : FimmActorBase
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