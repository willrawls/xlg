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
using Project = CsProjEditor.Project;

namespace MetX.Generators
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
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

        public static void AddGeneratorAsAnalyzer(Options options)
        {
// load csproj
            var project = Project.Load(options.AddTo);

// edit
            var document = project.Root.Document;
            XElement propertyGroup = document.XPathSelectElement("/Project/PropertyGroup");;
            if (propertyGroup == null)
            {
                propertyGroup = new XElement("PropertyGroup");
                document.Add(propertyGroup);
            }

            XElement node = document.XPathSelectElement("/Project/PropertyGroup/EmitCompilerGeneratedFiles");
            if (node == null)
            {
                propertyGroup.Add(new XElement("EmitCompilerGeneratedFiles", "true"));
                propertyGroup.Add(new XElement("EmitCompilerGeneratedFiles", "true"));
                
            }
            
            document.XPathEvaluate("/root/@a");  
            Console.WriteLine(att.Cast<XAttribute>().FirstOrDefault());  
            
            if (!project.ExistsNode(targetGroup, addSourceGeneratedFiles))
            {
                
                
                project.InsertGroup(targetGroup);
                project.InsertNode(targetGroup, name, addSourceGeneratedFiles);
                project.SetAttributeValue("Project", targetGroup, name, addSourceGeneratedFiles);
                project.SetAttributeValue("Project", targetGroup, afterTargets, "CoreCompile");
                project.InsertNode(targetGroup, "ItemGroup", null);
                project.InsertNode("ItemGroup");
                //project.InsertNode(targetGroup, afterTargets, "CoreCompile" );

            }
            if (!project.ExistsGroup(propertyGroup))
            {
                project.InsertGroup(propertyGroup);
            }
            
            project.InsertNode(propertyGroup, "EmitCompilerGeneratedFiles", "true");
            project.InsertNode(propertyGroup, "CompilerGeneratedFilesOutputPath", "true");
            project.InsertNode(propertyGroup, "OutputType", "Exe");
            project.InsertNode(propertyGroup, "LangVersion", "Latest");
            project.InsertNode(propertyGroup, "LangVersion", "Latest");

            project.InsertNode(propertyGroup, "PackageCertificateThumbprint", thumbprint);
            project.InsertNode(propertyGroup, "GenerateAppInstallerFile", "False");
            project.InsertNode(propertyGroup, "AppxAutoIncrementPackageRevision", "True");
            project.InsertNode(propertyGroup, "AppxSymbolPackageEnabled", "False");
            project.InsertNode(propertyGroup, "AppxBundle", "Always");
            project.InsertNode(propertyGroup, "AppxBundlePlatforms", "x86");
            project.InsertNode(propertyGroup, "AppInstallerUpdateFrequency", "1");
            project.InsertNode(propertyGroup, "AppInstallerCheckForUpdateFrequency", "OnApplicationRun");
            project.InsertAttribute("ItemGroup", "None", "Include", pfx, e => !e.HasAttributes);
            project.InsertAttribute("ItemGroup", "None", "Include", "Package.StoreAssociation.xml",
                e => !e.HasAttributes);

// save
            project.Save(path);
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

        public class Options
        {
            /*
            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }
            */

            [Option('n', "namespace", Required = true, HelpText = "Namespace of generator")]
            public string Namespace { get; set; }

            [Option('c', "generatorname", Required = true, HelpText = "Class name of generator")]
            public string GeneratorName { get; set; }

            [Option('f', "folder", Required = false,
                HelpText = "Root folder for generation (default is the current directory)")]
            public string RootFolder { get; set; }

            [Option('a', "addto", Required = false,
                HelpText = "Path to csproj the generator should be added as an analyzer")]
            public string AddTo { get; set; }

            [Option('j', "just", Required = false, HelpText = "Just add/update the Namespace.GeneratorName")]
            public string Just { get; set; }
        }
    }
}