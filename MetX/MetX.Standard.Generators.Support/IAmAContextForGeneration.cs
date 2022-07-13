using Microsoft.CodeAnalysis;

namespace MetX.Standard.Generators.Support
{
    public interface IAmAContextForGeneration
    {
        //ISourceGenerator Parent { get; set; }
        bool Succeeded { get; set; }

        void Initialize(GeneratorInitializationContext context);
        void Execute(GeneratorExecutionContext context);
    }
}
