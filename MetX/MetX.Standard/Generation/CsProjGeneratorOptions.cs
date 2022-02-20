using System;
using System.IO;
using MetX.Standard.IO;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Generation
{
    public class CsProjGeneratorOptions
    {
        public const string Delimiter = "~~";
            
        public string GenerationSet { get; set; }
        public string PathToTemplatesFolder { get; set; }
        public string OutputPath { get; set; }
        public string ClientName { get; set; }
        public string Namespace { get; set; }
        public string AspectsName { get; set; }
        public string GeneratorsName { get; set; }
        public string Filename { get; set; }
        public GenFramework TargetFramework { get; set; }
        public string PathToMetXStandardDll { get; set; }
        public string TargetTemplate { get; set; }
        public GenOutputType OutputType { get; set; } = GenOutputType.Library;
        public string Language { get; set; } = "CSharp";


        public static CsProjGeneratorOptions Defaults(GenFramework framework = GenFramework.Standard20)
        {
            CsProjGeneratorOptions generatorOptions = new()
            {
                PathToTemplatesFolder = @"Templates\",
                GenerationSet = "Default",
                ClientName = "Client",
                Namespace = "GenGen",
                AspectsName = "Aspects",
                GeneratorsName = "Generators",
                TargetFramework = framework,
                OutputPath = @".\Generated\",
                PathToMetXStandardDll = GetMetXPath(),
            };
            return generatorOptions;
        }

        public static string GetMetXPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public string PathResolved(string pathTemplate)
        {
            return JustResolve(pathTemplate, string.Empty);
        }

        public string ResolveContents(string contentsTemplate)
        {
            return JustResolve(contentsTemplate, "~~");
        }

        public string JustResolve(string contentsTemplate, string delimiter)
        {
            var resolved = contentsTemplate
                    .Replace(delimiter + "TargetFramework" + delimiter, TargetFramework.ToTargetFramework())
                    .Replace(delimiter + "GenerationSet" + delimiter, GenerationSet)
                    .Replace(delimiter + "OutputPath" + delimiter, OutputPath)
                    .Replace(delimiter + "OutputType" + delimiter, OutputType.ToString())
                    .Replace(delimiter + "PathToTemplatesFolder" + delimiter, PathToTemplatesFolder)
                    .Replace(delimiter + "ClientName" + delimiter, ClientName)
                    .Replace(delimiter + "Namespace" + delimiter, Namespace)
                    .Replace(delimiter + "AspectsName" + delimiter, AspectsName)
                    .Replace(delimiter + "GeneratorsName" + delimiter, GeneratorsName)
                    .Replace(delimiter + "Target" + delimiter, TargetTemplate)
                    .Replace(delimiter + "MetXPath" + delimiter, PathToMetXStandardDll)
                    .Replace(delimiter + "Framework" + delimiter, FrameworkValue())
                    .Replace("\r", "")
                ;
            return resolved;
        }

        public string FrameworkValue()
        {
            switch (TargetFramework)
            {
                case GenFramework.Net50:
                    return "net5.0";
                case GenFramework.Net50Windows:
                    return "net-5.0windows";
                case GenFramework.Core31:
                    return "netcoreapp3.1";
                case GenFramework.Standard20:
                    return "netstandard2.0";
                case GenFramework.Standard21:
                    return "netstandard2.1";
                case GenFramework.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string TemplateFilePath()
        {
            return Path.Combine(
                PathToTemplatesFolder,
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
                return OutputPath;

            return Path.Combine(OutputPath, TargetTemplateResolved());
        }

        public bool TryResolveAndSave()
        {
            if (!TryFullResolve(out var contents)) return false;

            var outputPath = OutputPathResolved();
            var outputFilePath = OutputFilePathResolved();

            Directory.CreateDirectory(outputPath);
            return FileSystem.TryWriteAllText(outputFilePath, contents);
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

        public CsProjGeneratorOptions WithOutputPath(string outputPath)
        {
            OutputPath = outputPath;
            return this;
        }

        public CsProjGeneratorOptions WithFilename(string filename)
        {
            Filename = filename;
            return this;
        }

        public CsProjGeneratorOptions WithPathToTemplatesFolder(string pathToTemplatesFolder)
        {
            PathToTemplatesFolder = pathToTemplatesFolder;
            return this;
        }

        public CsProjGeneratorOptions WithFramework(GenFramework targetFramework)
        {
            TargetFramework = targetFramework;
            return this;
        }
 
        public bool AssertValid()
        {
            GeneratorsName.ThrowIfEmpty("GeneratorsName");
            PathToTemplatesFolder.ThrowIfEmpty("PathToTemplatesFolder");
            ClientName.ThrowIfEmpty("ClientName");
            Namespace.ThrowIfEmpty("Namespace");
            AspectsName.ThrowIfEmpty("AspectsName");
            GeneratorsName.ThrowIfEmpty("GeneratorsName");
            PathToMetXStandardDll.ThrowIfEmpty("PathToMetXStandardDll");
            //TargetTemplate.ThrowIfEmpty("TargetTemplate");  << This will be null at this point but is immediately set elsewhere
            Language.ThrowIfEmpty("Language");

            OutputPath.ThrowIfEmpty("OutputPath");
            //Filename.ThrowIfEmpty("Filename");

            if (TargetFramework == GenFramework.Unknown)
                throw new ArgumentOutOfRangeException("TargetFramework");
            if(OutputType == GenOutputType.Unknown)
                throw new ArgumentOutOfRangeException("OutputType");

            if (!Directory.Exists(PathToTemplatesFolder))
                throw new ArgumentOutOfRangeException("PathToTemplatesFolder");
            if (!File.Exists(PathToMetXStandardDll))
            {
                var secondChance = Path.Combine(PathToMetXStandardDll, "MetX.Standard.dll");
                if(!File.Exists(secondChance))
                    throw new ArgumentOutOfRangeException("PathToMetXStandardDll");
                PathToMetXStandardDll = secondChance;
            }
            
            return true;
        }
   }
}