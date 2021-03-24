using System;
using System.IO;
using MetX.Aspects;
using MetX.Standard.Generation.CSharp.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    [TestClass]
    public class GeneratorsCsProjGeneratorTests_FromScratch
    {
        [TestMethod]
        public void FromScratchXmlIsAsExpected()
        {
            var genGenOptions = GenGenOptions.Defaults;
            genGenOptions.BaseOutputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Product");
            genGenOptions.TemplatesRootPath = @"..\..\..\..\MetX.Generators\Templates";
            
            var generator = new CsProjGenerator(genGenOptions, "Namespace.GeneratorsName");

            Assert.IsNotNull(generator);
            var actual = generator.Document.OuterXml;
            Assert.IsFalse(actual.Contains("~~"));
            
            Assert.IsTrue(actual.Contains(GenFramework.Standard20.ToTargetFramework()));
            Assert.IsFalse(actual.Contains("Analyzer"));
        }
    }
}