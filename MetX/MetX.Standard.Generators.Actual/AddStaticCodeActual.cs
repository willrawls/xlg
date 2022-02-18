using System.Text;
using MetX.Standard.Generators.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace MetX.Standard.Generators.Actual
{
    // ReSharper disable once UnusedType.Global
    public class AddStaticCodeActual : ContextForGeneration
    {
        public override void Initialize(GeneratorInitializationContext context) {}

        public override void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("myGeneratedFile.cs", SourceText.From(@"
namespace MetX.Generated
{
    public static class GClass
    {
        public static void GMethod()
        {
            // generated code
            System.Console.WriteLine(""Ding 3"");
        }
    }
}", Encoding.UTF8));
        }
    }
}