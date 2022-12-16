using System;

namespace MetX.Fimm.Setup;

public class ValidCombo<T> : FimmActorBase where T : new()
{
    public T Factory()
    {
        var actor = new T();
        return actor;
    }

    public IAct TheAct()
    {
        return (IAct) this;
    }

    public ValidCombo(ArgumentVerb verb, ArgumentNoun noun) : base(verb, noun)
    {
    }

    public override ProcessorResult Run(ArgumentSettings settings)
    {
        throw new NotImplementedException();
    }
}