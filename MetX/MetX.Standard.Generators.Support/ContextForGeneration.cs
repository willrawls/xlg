using Microsoft.CodeAnalysis;

namespace MetX.Standard.Generators.Support
{
    public abstract class ContextForGeneration : IAmAContextForGeneration
    {
        //public ISourceGenerator Parent { get; set; }
        public bool Succeeded { get; set; }

        public abstract void Execute(GeneratorExecutionContext context);
        public abstract void Initialize(GeneratorInitializationContext context);
    }
}