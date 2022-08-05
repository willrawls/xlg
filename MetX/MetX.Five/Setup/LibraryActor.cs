namespace MetX.Five.Setup;

public class LibraryActor : FiverActorBase
{
    public LibraryActor()
    {

    }

    public LibraryActor(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }

    public override ProcessorResult Run(ArgumentSettings settings)
    {
        throw new System.NotImplementedException();
    }
}