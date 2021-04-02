using System;
using System.IO;
using MetX.Aspects;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Standard.Generators;
using MetX.Standard.Generators.GenGen;
using MetX.Standard.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    [TestClass]
    public class MetXGeneratorsExeTests
    {
        [TestMethod]
        public void DefaultParametersGenerateWithDefaultTemplatesNamesAndLocations()
        {
            Assert.IsTrue(Directory.Exists("Templates"));

            var worker = new GenGenWorker();
            var workerOptions = new GenGenOptions();
            worker.Go(workerOptions);
            Assert.IsTrue(Directory.Exists("netstandard2_0.Aspects"));
            Assert.IsTrue(File.Exists(@"netstandard2_0.Aspects\netstandard2_0.Aspects.csproj"));
            Assert.IsTrue(File.Exists(@"netstandard2_0.Aspects\GenerateFromTemplate.cs"));
            
            Assert.IsTrue(Directory.Exists("netstandard2_0.Client"));
            Assert.IsTrue(File.Exists(@"netstandard2_0.Client\netstandard2_0.Client.csproj"));
            
            Assert.IsTrue(Directory.Exists("netstandard2_0.Generators"));
            Assert.IsTrue(File.Exists(@"netstandard2_0.Generators\netstandard2_0.Generators.csproj"));
            Assert.IsTrue(File.Exists(@"netstandard2_0.Generators\FromTemplateGenerator.cs"));
        }
        
        [TestMethod]
        public void xDefaultParametersGenerateWithDefaultTemplatesNamesAndLocations()
        {
            var worker = new GenGenWorker();
            var workerOptions = new GenGenOptions
            {
                GeneratorName = "Fred",
                AttributeName = "George", 
                Namespace = "Mary.Kay",
                RootFolder = Path.Combine(".", Guid.NewGuid().AsString()),
                Build = false,
                Operation = Operation.Create,
                TemplatesPath = "Templates",
                Verbose = true,
            };
            worker.Go(workerOptions);
            
        }
    }
}