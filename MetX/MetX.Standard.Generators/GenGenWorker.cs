using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using MetX.Standard.Generation;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Standard.Generators.GenGen;
using MetX.Standard.Library;

namespace MetX.Standard.Generators
{
    public class GenGenWorker
    {
        public GenGenOptions Options;
        public List<string> Errors = new List<string>();
        
        public AspectsCsProjGenerator AspectsProjectGenerator;
        public GeneratorsCsProjGenerator GeneratorsProjectGenerator;
        public ClientCsProjGenerator ClientProjectGenerator;
        
        public void Go(GenGenOptions options)
        {
            Options = options;
            
            switch (options.Operation)
            {
                case Operation.Create:
                    Directory.CreateDirectory(options.RootFolder);
                    AspectsProjectGenerator = new AspectsCsProjGenerator(options.ToCsProjGeneratorOptions(GenFramework.Standard20));
                    GeneratorsProjectGenerator = new GeneratorsCsProjGenerator(options.ToCsProjGeneratorOptions(GenFramework.Standard20));
                    ClientProjectGenerator = new ClientCsProjGenerator(options.ToCsProjGeneratorOptions(GenFramework.Net50));

                    AspectsProjectGenerator.Setup().Generate().Save();
                    GeneratorsProjectGenerator.Setup().Generate().Save();
                    ClientProjectGenerator.Setup().Generate().Save();
                    break;

                case Operation.GenGen:
                    break;
                case Operation.Client:
                    break;
                case Operation.Update:
                    break;
                case Operation.Inject:
                    break;
                case Operation.Check:
                    break;
                case Operation.Remove:
                    break;
                case Operation.Clean:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
      
        }

        public static void AddGeneratorAsAnalyzer(GenGenOptions options)
        {

        }

        //  Generates generator in current folder
        //  MetX.Standard.Generators.exe -generate Namespace.GeneratorName 
        //  When sub folders are missing or are empty
        //      Creates 2 sub folders
        //          Namespace
        //              Namespace.csproj
        //              GeneratorName.cs
        //          Namespace.Aspects
        //              Namespace.Aspects.csproj
        //              GeneratorNameAttribute.cs
        //  When sub folder already exist
        //      Add or overwrite
        //          Namespace
        //              Namespace.csproj
        //                  Create NextGeneratorName.cs
        //                  Add NextGeneratorName.cs to .csproj
        //              Namespace.NextGeneratorName.cs
        //          Namespace.Aspects
        //              Namespace.Name.Aspects.csproj
        //              NextGeneratorNameAttribute.cs
        //  MetX.Standard.Generators.exe -addto "PathTo.csproj the generator should be added as an analyzer"
        //      
        //  MetX.Standard.Generators.exe -just Namespace.GeneratorName -addto "PathTo.csproj the generator should be added as an analyzer"
        //      Find and modify/update the csproj of the client
    }
}