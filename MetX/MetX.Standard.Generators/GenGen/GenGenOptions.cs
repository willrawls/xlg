using System;
using System.IO;
using CommandLine;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.Strings;
using MetX.Standard.Primary.Generation;

namespace MetX.Standard.Generators.GenGen
{
    public class GenGenOptions
    {
        public const string HelpText = @"
    NOTE: When this parameter is missing, 'create' is assumed,

        create (generate new generator, attribute, and client projects)
        gengen (generate the generator and attribute projects only)
        client (generate only a new client project)

        update (Update a previously created generator as needed),
            NOTE: Say when you've renamed classes, moved files, 
                  directories, or whenever something breaks 
                  the changes normally made

        inject (Add an existing generator to a project),
            NOTE: This will only modify an existing csproj

        check (Double check the setup of an existing generator and client),
            NOTE: Nothing changes here, just a report on the state / usability

        clean (remove any generated files in the client project folder)
            NOTE: Only from the 'Generated' folder this program 
                  adds to the client project

        remove (Delete the generator and aspect projects. Remove reference from client)
            NOTE: Will not delete the folders 
                  or any files that would not have been created
";

        [Option('o', "operation", Required = false, HelpText = HelpText)]
        public GenGen.Operation Operation { get; set; } = Operation.Create;
        
        [Option('b', "build", Required = true, HelpText = "Build generator and client when done (Default is true)")]
        public bool Build { get; set; } = true;

        [Option('s', "namespace", Required = true, HelpText = "Namespace of generator (Current folder name plus '.Generators')")]
        public string Namespace { get; set; } = DetermineNamspaceOfGenerator();

        private static string DetermineNamspaceOfGenerator()
        {
            var directory = Environment.CurrentDirectory;
            var namespacePart1 = directory.LastToken(@"\");
            var namespacePart2 = namespacePart1
                                     .Replace("-", "")
                                     .Replace(".", "")
                                     .Replace(" ", "") 
                                     .ProperCase()
                                 + ".Generators";
            return namespacePart2;
        }

        [Option('c', "class", Required = true, HelpText = "Class name of generator (Default is 'FromTemplateGenerator')")]
        public string GeneratorName { get; set; } = "FromTemplateGenerator";

        [Option('a', "attribute", Required = true, HelpText = "Name of generate attribute (Default is 'GenerateFromTemplate')")]
        public string AttributeName { get; set; } = "GenerateFromTemplate";

        [Option('f', "folder", Required = false,
            HelpText = "Root folder for generation (default is the current directory)")]
        public string RootFolder { get; set; } = Environment.CurrentDirectory;

        [Option('t', "templates", Required = false, HelpText = "Path to the set of templates to use (Default is the 'Templates' folder in the folder with GenGen.exe)")]
        public string TemplatesPath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");

        /*
        [Option('x', "client", Required = false, HelpText = "Path to a template for the client project (default is to use the built in template)")]
        public string ClientTemplate { get; set; }

        [Option('y', "generator", Required = false, HelpText = "Path to a template (or template folder) for the generator project (default is to use the built in template)")]
        public string GeneratorTemplate { get; set; }

        [Option('z', "aspects", Required = false, HelpText = "Path to a template for the aspects (attribute) project (default is to use the built in template)")]
        public string AspectsTemplate { get; set; }

        [Option('p', "client project", Required = false, HelpText = "Path to csproj the generator should be added as an analyzer (Default is the csproj of the same name as the folder parameter)")]
        public string AddTo { get; set; }
        */

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; } = true;

        public CsProjGeneratorOptions ToCsProjGeneratorOptions(GenFramework targetFramework)
        {
            var options = new CsProjGeneratorOptions
            {
                Namespace = Namespace,
                Language = "CSharp",
                OutputPath = RootFolder,
                TargetFramework = targetFramework,
                PathToTemplatesFolder = TemplatesPath,
                OutputType = GenOutputType.Library,
                PathToMetXStandardDll = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MetX.Standard.dll"),
                TargetTemplate = "Default",
                AspectsName = "Aspects",
                ClientName = "Client",
                GeneratorsName = "Generators",
                Filename = "DefaultFilename",        // ??
                GenerationSet = "Default",   // ??
            };
            return options;
        }
    }
}