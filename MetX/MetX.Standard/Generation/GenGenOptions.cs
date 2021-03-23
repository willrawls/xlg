using System;
using System.Diagnostics;
using System.IO;
using MetX.Standard.Library;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MetX.Aspects
{
    public class GenGenOptions
    {
        public string GenerationSet { get; set; }
        public string TemplatesRootPath { get; set; }
        public string BaseOutputPath { get; set; }
        public string ClientName { get; set; }
        public string Namespace { get; set; }
        public string AspectsName { get; set; }
        public string GeneratorsName { get; set; }
        public string Filename { get; set; }
        public GenFramework Framework { get; set; }

        public static GenGenOptions Defaults => new()
            {
                TemplatesRootPath = @"Templates\",
                GenerationSet = "Default",
                ClientName = "Client",
                Namespace = "GenGen",
                AspectsName = "Aspects",
                GeneratorsName = "Generators",
                Framework = GenFramework.Net50Windows,
                BaseOutputPath = @".\",
            };

        public string TargetTemplate { get; set; }
        public string Language { get; set; } = "CSharp";

        public string PathResolved(string pathTemplate)
        {
            var resolved = pathTemplate
                    .Replace("GenerationSet", GenerationSet)
                    .Replace("BasePath", BaseOutputPath)
                    .Replace("TemplatesRootPath", TemplatesRootPath)
                    .Replace("ClientName", ClientName)
                    .Replace("Namespace", Namespace)
                    .Replace("AspectsName", AspectsName)
                    .Replace("GeneratorsName", GeneratorsName)
                    .Replace("Target", TargetTemplate)
                    .Replace("Framework", FrameworkValue())
                    .Replace("\r", "")
                ;
            return resolved;
        }
        
        public string ResolveContents(string contentsTemplate)
        {
            var resolved = contentsTemplate
                    .Replace("~~GenerationSet~~", GenerationSet)
                    .Replace("~~BasePath~~", BaseOutputPath)
                    .Replace("~~TemplatesRootPath~~", TemplatesRootPath)
                    .Replace("~~ClientName~~", ClientName)
                    .Replace("~~Namespace~~", Namespace)
                    .Replace("~~AspectsName~~", AspectsName)
                    .Replace("~~GeneratorsName~~", GeneratorsName)
                    .Replace("~~Target~~", TargetTemplate)
                    .Replace("~~Framework~~", FrameworkValue())
                    .Replace("\r", "")
                ;
            //Filename = Path.Combine(BaseOutputPath, TargetTemplate, TargetTemplate + ".csproj");
            //resolved = resolved.TokensAfterFirst("\n");
            return resolved;
        }

        public string FrameworkValue()
        {
            switch(Framework)
            {
                case GenFramework.Net50:
                    return "net5.0";
                case GenFramework.Net50Windows:
                    return "net-5.0windows";
                case GenFramework.Core31:
                    return "netcoreapp3.1";
                case GenFramework.Standard20:
                    return "netstandard2.0";
                case GenFramework.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string TemplateFilePath()
        {
            return Path.Combine(
                TemplatesRootPath,
                    Language,
                        GenerationSet,
                            TargetTemplate,
                                TargetTemplate + ".csproj");
        }

        public string OutputFilePathResolved()
        {
            var resolved = PathResolved(TargetTemplate);
            return Path.Combine(
                OutputPathResolved(),
                    resolved,
                        resolved + ".csproj");
        }

        public string TargetTemplateResolved()
        {
            return PathResolved(TargetTemplate);
        }
        
        public string OutputPathResolved()
        {
            if (TargetTemplate.IsEmpty())
                return BaseOutputPath;
            
            return Path.Combine(BaseOutputPath, TargetTemplateResolved());
        }

        public bool TryResolveAndSave()
        {
            if (!TryFullResolve(out var contents))
            {
                return false;
            }

            var outputPath = OutputPathResolved();
            var outputFilePath = OutputFilePathResolved();
            
            Directory.CreateDirectory(outputPath);
            return Standard.IO.FileSystem.TryWriteAllText(outputFilePath, contents);
        }

        public bool TryFullResolve(out string contents)
        {
            contents = null;
            var templateFilePath = TemplateFilePath();
            if (!File.Exists(templateFilePath))
                throw new FileNotFoundException(templateFilePath);

            var template = File.ReadAllText(templateFilePath);
            contents = ResolveContents(template);
            return true;
        }
    }
}