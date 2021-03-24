using System;
using System.IO;
using MetX.Aspects;
using MetX.Standard.Generation.CSharp.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    [TestClass]
    public class AspectsCsProjGeneratorTests_FromScratch
    {
        [TestMethod]
        public void FromScratchXmlIsAsExpected()
        {
            var genGenOptions = CsProjGeneratorOptions.Defaults(GenFramework.Standard20);
            genGenOptions.OutputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Product");
            genGenOptions.PathToTemplatesFolder = @"..\..\..\..\MetX.Generators\Templates";
            
            var generator = new AspectsCsProjGenerator(genGenOptions);

            Assert.IsNotNull(generator);
            var actual = generator.Document.OuterXml;
            Assert.IsFalse(actual.Contains(CsProjGeneratorOptions.Delimiter));
            
            Assert.IsTrue(actual.Contains(GenFramework.Standard20.ToTargetFramework()));
            Assert.IsFalse(actual.Contains("Analyzer"));
        }
    }
}