using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Standard.Generators.GenGen;
using MetX.Standard.Library;

namespace MetX.Standard.Generators
{
    public class GenGenWorker
    {
        public List<string> Errors = new List<string>();
        
        public AspectsCsProjGenerator AspectsProjectGenerator;
        public GeneratorsCsProjGenerator GeneratorsProjectGenerator;
        public ClientCsProjGenerator ClientProjectGenerator;
        
        public void Go(GenGenOptions genGenOptions)
        {
            if (!FigureOutWhereEverythingIs())
            {
                Errors.Add("Couldn't figure out where everything is");
                return;
            }
            
            if(!FigureOutWhereEverythingWillGo())
            {
                Errors.Add("Couldn't figure out where everything should go");
                return;
            }
            
        }

        private bool FigureOutWhereEverythingWillGo()
        {
            return true;
        }

        private bool FigureOutWhereEverythingIs()
        {
            return true;
        }

        public static void AddGeneratorAsAnalyzer(GenGenOptions genGenOptions)
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