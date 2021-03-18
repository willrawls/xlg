using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace MetX.Generators
{
    [Generator]
    public class AddClassSupportFromTemplate : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) {}

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("Fred.exampleOne.cs", SourceText.From(@"
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
}", Encoding.UTF8));
        }
    }
}