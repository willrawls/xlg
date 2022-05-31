using System;

namespace MetX.Five.Setup;

public class FiverActorBase : FiveActorAbstractBase
{

}

public abstract class FiveActorAbstractBase : IAct
{
    public virtual bool ReadyToAct(ArgumentSettings settings, out string reason)
    {
        reason = null;
        return false;
    }

    public virtual ProcessorResult Run(ArgumentSettings settings) => null;
    public virtual ProcessorResult Build(ArgumentSettings settings) => null;
    public virtual ProcessorResult Add(ArgumentSettings settings) => null;
    public virtual ProcessorResult Remove(ArgumentSettings settings) => null;
    public virtual ProcessorResult Clone(ArgumentSettings settings) => null;
    public virtual ProcessorResult Delete(ArgumentSettings settings) => null;
    public virtual ProcessorResult Walk(ArgumentSettings settings) => null;
    public virtual ProcessorResult Gen(ArgumentSettings settings) => null;
    public virtual ProcessorResult Regen(ArgumentSettings settings) => null;
}