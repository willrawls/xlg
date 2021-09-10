using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MetX.Standard.Generators.QuickScripter
{
    [Generator]
    public class CustomGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) => context.RegisterForSyntaxNotifications(() => new MapToReceiver());

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("myGeneratedFile.cs", SourceText.From(@"
namespace XLG.QuickScripts.Generated
{
    public static class GClass
    {
        public static void GMethod()
        {
            // generated code
        }
    }
}", Encoding.UTF8));
        }
        
        
        public sealed class MapToReceiver : ISyntaxReceiver
        {
            public List<TypeDeclarationSyntax> Candidates { get; } =
                new List<TypeDeclarationSyntax>();
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if(syntaxNode is TypeDeclarationSyntax typeDeclarationSyntax)
                {
                    foreach (var attributeList in 
                        typeDeclarationSyntax.AttributeLists)
                    {
                        foreach (var attribute in attributeList.Attributes)
                        {
                            if(attribute.Name.ToString() == "MapTo" ||
                               attribute.Name.ToString() == "MapToAttribute")
                            {
                                this.Candidates.Add(typeDeclarationSyntax);
                            }
                        }
                    }
                }
            }
        }
    }
}