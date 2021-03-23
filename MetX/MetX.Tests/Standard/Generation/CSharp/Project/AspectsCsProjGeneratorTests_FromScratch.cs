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
        public void ClientFromScratchXmlIsAsExpected()
        {
            var genGenOptions = GenGenOptions.Defaults;
            genGenOptions.BaseOutputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Product");
            genGenOptions.TemplatesRootPath = @"..\..\..\..\MetX.Generators\Templates";
            
            var clientCsProjGenerator = new ClientCsProjGenerator(genGenOptions);

            Assert.IsNotNull(clientCsProjGenerator);
            var actual = clientCsProjGenerator.Document.OuterXml;
            Assert.IsFalse(actual.Contains("~~"));
            Assert.IsTrue(actual.Contains("net-5.0windows"));
            Assert.IsTrue(actual.Contains("Analyzer"));
        }
    }
}