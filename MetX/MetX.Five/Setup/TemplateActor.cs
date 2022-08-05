namespace MetX.Five.Setup;

public class TemplateActor : FiverActorBase
{
    public TemplateActor()
    {

    }

    public TemplateActor(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }

    public override ProcessorResult Run(ArgumentSettings settings)
    {
        throw new System.NotImplementedException();
    }
}