﻿using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MetX.Five.Setup;

public class ValidCombo<T> : FiverActorBase where T : new()
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
}