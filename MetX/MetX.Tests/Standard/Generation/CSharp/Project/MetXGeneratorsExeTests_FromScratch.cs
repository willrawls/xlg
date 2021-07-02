using System;
using System.IO;
using MetX.Standard.Generators;
using MetX.Standard.Generators.GenGen;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests2.Standard.Generation.CSharp.Project
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

            var rootFolder = AppDomain.CurrentDomain.BaseDirectory;
            Assert.IsTrue(Directory.Exists($"{rootFolder}Net50windows.Generators.Aspects"));
            Assert.IsTrue(File.Exists($@"{rootFolder}Net50windows.Generators.Aspects\Net50windows.Generators.Aspects.csproj"));
            Assert.IsTrue(File.Exists($@"{rootFolder}Net50windows.Generators.Aspects\GenerateFromTemplate.cs"));
            
            Assert.IsTrue(Directory.Exists("Net50windows.Generators.Client"));
            Assert.IsTrue(File.Exists($@"{rootFolder}Net50windows.Generators.Client\Net50windows.Generators.Client.csproj"));
            
            Assert.IsTrue(Directory.Exists("Net50windows.Generators"));
            Assert.IsTrue(File.Exists($@"{rootFolder}Net50windows.Generators\Net50windows.Generators.csproj"));
            Assert.IsTrue(File.Exists($@"{rootFolder}Net50windows.Generators\FromTemplateGenerator.cs"));
        }
       
    }
}