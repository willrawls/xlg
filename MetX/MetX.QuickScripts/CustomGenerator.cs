namespace XLG.QuickScripts
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;
    using System.Linq;
    using System.Text;

    namespace XLG.QuickScripts
    {
        [Generator]
        public class CustomGenerator : IIncrementalGenerator
        {
            public void Initialize(IncrementalGeneratorInitializationContext context)
            {
                // Collect candidate syntax nodes
                var provider = context.SyntaxProvider
                    .CreateSyntaxProvider(
                        predicate: (s, _) => s is TypeDeclarationSyntax typeDecl &&
                                             typeDecl.AttributeLists.Any(a => a.Attributes
                                                 .Any(attr =>
                                                     attr.Name.ToString() == "MapTo" ||
                                                     attr.Name.ToString() == "MapToAttribute")),
                        transform: (ctx, _) => (TypeDeclarationSyntax)ctx.Node
                    )
                    .Where(static typeDecl => typeDecl is not null);

                context.RegisterSourceOutput(provider, GenerateCode);
            }

            private void GenerateCode(SourceProductionContext context, TypeDeclarationSyntax typeDeclaration)
            {
                var className = typeDeclaration.Identifier.Text;
                var generatedSource = $@"
namespace XLG.QuickScripts.Generated
{{
    public static class {className}Generated
    {{
        public static void GMethod()d
        {{
            // generated code
        }}
    }}
}}";
                context.AddSource($"{className}Generated.cs", SourceText.From(generatedSource, Encoding.UTF8));
            }
        }
    }
}