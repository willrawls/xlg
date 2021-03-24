using System;
using System.IO;
using MetX.Aspects;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Standard.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    [TestClass]
    public class GeneratorsCsProjGeneratorTests_FromScratch
    {
        [TestMethod]
        public void FromScratchXmlIsAsExpected()
        {
            var options = CsProjGeneratorOptions.Defaults(GenFramework.Standard20);
            options.OutputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Product");
            options.PathToTemplatesFolder = @"..\..\..\..\MetX.Generators\Templates";
            
            var generator = new GeneratorsCsProjGenerator(options);

            Assert.IsNotNull(generator);
            var actual = generator.Document.OuterXml;
            Assert.IsFalse(actual.Contains(CsProjGeneratorOptions.Delimiter), actual.TokenAt(2, "~~"));
            
            Assert.IsTrue(actual.Contains(GenFramework.Standard20.ToTargetFramework()), actual);
            Assert.IsTrue(actual.Contains("Analyzer"), actual);
        }
    }
}