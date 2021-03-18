using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace MetX.Generators
{
    [Generator]
    public class AddStaticCode : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) {}

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("myGeneratedFile.cs", SourceText.From(@"
namespace MetX.Generated
{
    public static class GClass
    {
        public static void GMethod()
        {
            // generated code
            System.Console.WriteLine(""Ding 1"");
        }
    }
}", Encoding.UTF8));
        }
    }
}