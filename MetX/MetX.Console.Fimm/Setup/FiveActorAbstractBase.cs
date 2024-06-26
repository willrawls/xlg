﻿using System;

namespace MetX.Fimm.Setup;

public abstract class FimmActorBase : IAct
{
    public ArgumentNoun Noun;
    public ArgumentVerb Verb;

    public FimmActorBase(ArgumentVerb verb, ArgumentNoun noun)
    {
        Verb = verb;
        Noun = noun;
    }

    protected FimmActorBase()
    {
    }

    public Func<ArgumentSettings, ProcessorResult> GetProcessingFunction(ArgumentSettings settings)
    {
        Func<ArgumentSettings, ProcessorResult> processor = null;

        switch (settings.Verb)
        {
            case ArgumentVerb.Unknown:
                return null;

            case ArgumentVerb.Run:
                processor = Run;
                break;
            case ArgumentVerb.Build:
                processor = Build;
                break;

            case ArgumentVerb.Gen:
                processor = Gen;
                break;
            case ArgumentVerb.Regen:
                processor = Regen;
                break;

            case ArgumentVerb.Walk:
                processor = Walk;
                break;

            case ArgumentVerb.Add:
                processor = Add;
                break;

            case ArgumentVerb.Clone:
                processor = Clone;
                break;

            case ArgumentVerb.Delete:
                processor = Delete;
                break;

            case ArgumentVerb.Stage:
                processor = Stage;
                break;

            case ArgumentVerb.Test:
                processor = Test;
                break;

            case ArgumentVerb.Remove:
                processor = Remove;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        return processor;
    }

    public virtual ProcessorResult Stage(ArgumentSettings settings) => throw new NotImplementedException();

    public virtual ProcessorResult Test(ArgumentSettings settings) => throw new NotImplementedException();

    public virtual bool ReadyToAct(ArgumentSettings settings, out string reason)
    {
        reason = null;
        return false;
    }

    public abstract ProcessorResult Run(ArgumentSettings settings);
    //public virtual ProcessorResult Run(ArgumentSettings settings) => null;

    public virtual ProcessorResult Build(ArgumentSettings settings) => null;

    public virtual ProcessorResult Add(ArgumentSettings settings) => null;

    public virtual ProcessorResult Remove(ArgumentSettings settings) => null;

    public virtual ProcessorResult Clone(ArgumentSettings settings) => null;

    public virtual ProcessorResult Delete(ArgumentSettings settings) => null;

    public virtual ProcessorResult Walk(ArgumentSettings settings) => null;

    public virtual ProcessorResult Gen(ArgumentSettings settings) => null;

    public virtual ProcessorResult Regen(ArgumentSettings settings) => null;
}