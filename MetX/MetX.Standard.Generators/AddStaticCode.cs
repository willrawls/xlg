using Microsoft.CodeAnalysis;

namespace MetX.Standard.Generators
{
    [Generator]
    public class AddStaticCode : BaseRoslynCodeGenerator, ISourceGenerator
    {
        public AddStaticCode() :  base(
            @"..\..\..\MetX.Standard.Generators.Actual\bin\Debug\netstandard2.1\MetX.Standard.Generators.Actual.dll", 
            @"MetX.Standard.Generators.Actual.AddStaticCodeActual")
        { }

        public void Initialize(GeneratorInitializationContext context)
        {
            InitializeContextIfNeeded();
            ShadowRunContext?.Initialize(context);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            InitializeContextIfNeeded();
            ShadowRunContext?.Execute(context);
            Cleanup();
        }
    }
}