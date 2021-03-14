using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using CommandLine;
using MetX.Standard.Library;
using Microsoft.CodeAnalysis;

namespace MetX.Generators
{
    public partial class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<GenGenCommandLineOptions>(args)
                .WithParsed(options =>
                {
                    
                    if (options.AddTo.IsNotEmpty())
                    {
                        //  MetX.Generators.exe -addto "PathTo.csproj the generator should be added as an analyzer"
                        //  No generators generated, just adding the reference
                        if (File.Exists(options.AddTo)) AddGeneratorAsAnalyzer(options);
                    }
                    else
                    {
                        if (options.RootFolder.IsEmpty()) options.RootFolder = Environment.CurrentDirectory;
                    }
                });
        }

        public static void AddGeneratorAsAnalyzer(GenGenCommandLineOptions options)
        {

        }
        //  Generates generator in current folder
        //  MetX.Generators.exe -generate Namespace.GeneratorName 
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
        //  MetX.Generators.exe -addto "PathTo.csproj the generator should be added as an analyzer"
        //      
        //  MetX.Generators.exe -just Namespace.GeneratorName -addto "PathTo.csproj the generator should be added as an analyzer"
        //      Find and modify/update the csproj of the client
    }
}