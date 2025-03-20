using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace MetX.Standard.Generators
{
    [Generator]
    public class AddClassSupportFromTemplate : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Register the code generation process
            context.RegisterPostInitializationOutput(ctx =>
            {
                const string generatedCode = @"
namespace MetX.Generated
{
    public static class FredExampleOne
    {
        public static void Go()
        {
            // generated code
            System.Console.WriteLine(""Ding 2"");
        }
    }
}";
                ctx.AddSource("Fred.exampleOne.g.cs", SourceText.From(generatedCode, Encoding.UTF8));
            });
        }
    }
}