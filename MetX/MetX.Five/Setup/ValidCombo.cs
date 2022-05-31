using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MetX.Five.Setup;

public class ValidCombo<T> : FiverActorBase where T : new()
{
    public ArgumentNoun Noun;
    public ArgumentVerb Verb;

    public ValidCombo(ArgumentVerb verb, ArgumentNoun noun)
    {
        Verb = verb;
        Noun = noun;
    }

    public T Factory()
    {
        var actor = new T();
        return actor;
    }

    public IAct TheAct()
    {
        return (IAct) this;
    }
}