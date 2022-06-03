using System;

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

public abstract class FiveActorAbstractBase : IAct
{
    public ArgumentNoun Noun;
    public ArgumentVerb Verb;

    public ProcessorResult this[ArgumentVerb verb]
    {
        get
        {
            ProcessorResult result = null;

            switch (verb)
            {
                case ArgumentVerb.Unknown:
                    return null;

                case ArgumentVerb.Run:
                    result = Run(Settings);
                    break;

                case ArgumentVerb.Build:
                    result = Build(Settings);
                    break;
                case ArgumentVerb.Gen:
                    result = Gen(Settings);
                    break;
                case ArgumentVerb.Regen:
                    result = Regen(Settings);
                    break;
                case ArgumentVerb.Walk:
                    result = Walk(Settings);
                    break;
                case ArgumentVerb.Add:
                    result = Add(Settings);
                    break;
                case ArgumentVerb.Clone:
                    result = Clone(Settings);
                    break;
                case ArgumentVerb.Delete:
                    result = Delete(Settings);
                    break;
                case ArgumentVerb.Stage:
                    result = Stage(Settings);
                    break;
                case ArgumentVerb.Test:
                    result = Test(Settings);
                    break;
                case ArgumentVerb.Remove:
                    result = Remove(Settings);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }
    }

    private ProcessorResult Test(ArgumentSettings settings)
    {
        throw new NotImplementedException();
    }

    private ProcessorResult Stage(ArgumentSettings settings)
    {
        throw new NotImplementedException();
    }

    public virtual bool ReadyToAct(ArgumentSettings settings, out string reason)
    {
        Settings = settings;
        reason = null;
        return false;
    }

    public ArgumentSettings Settings { get; set; }

    public virtual ProcessorResult Run(ArgumentSettings settings)
    {
        return null;
    }

    public virtual ProcessorResult Build(ArgumentSettings settings)
    {
        return null;
    }

    public virtual ProcessorResult Add(ArgumentSettings settings)
    {
        return null;
    }

    public virtual ProcessorResult Remove(ArgumentSettings settings)
    {
        return null;
    }

    public virtual ProcessorResult Clone(ArgumentSettings settings)
    {
        return null;
    }

    public virtual ProcessorResult Delete(ArgumentSettings settings)
    {
        return null;
    }

    public virtual ProcessorResult Walk(ArgumentSettings settings)
    {
        return null;
    }

    public virtual ProcessorResult Gen(ArgumentSettings settings)
    {
        return null;
    }

    public virtual ProcessorResult Regen(ArgumentSettings settings)
    {
        return null;
    }
}