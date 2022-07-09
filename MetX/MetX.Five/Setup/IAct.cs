namespace MetX.Five.Setup;

public interface IAct
{
    bool ReadyToAct(ArgumentSettings settings, out string reason);

    ProcessorResult Run(ArgumentSettings settings) => null;
    ProcessorResult Build(ArgumentSettings settings) => null;
    ProcessorResult Add(ArgumentSettings settings) => null;
    ProcessorResult Remove(ArgumentSettings settings) => null;
    ProcessorResult Clone(ArgumentSettings settings) => null;
    ProcessorResult Delete(ArgumentSettings settings) => null;
    ProcessorResult Walk(ArgumentSettings settings) => null;
    ProcessorResult Gen(ArgumentSettings settings) => null;
    ProcessorResult Regen(ArgumentSettings settings) => null;
    ProcessorResult Test(ArgumentSettings settings) => null;
}