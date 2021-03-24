using System;
using System.IO;
using Accessibility;
using MetX.Aspects;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Standard.Library;
using MetX.Tests.Standard.Generation.CSharp.Project.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    public partial class ConsoleClientCsProjGeneratorTests
    {
        [TestMethod]
        public void FromScratchXmlIsAsExpected()
        {
            var genGenOptions = CsProjGeneratorOptions.Defaults(GenFramework.Net50Windows);
            genGenOptions.OutputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Product");
            genGenOptions.PathToTemplatesFolder = @"..\..\..\..\MetX.Generators\Templates";
            var clientCsProjGenerator = new ClientCsProjGenerator(genGenOptions);

            Assert.IsNotNull(clientCsProjGenerator);
            var actual = clientCsProjGenerator.Document.OuterXml;
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.Contains(CsProjGeneratorOptions.Delimiter), actual.TokenAt(2, CsProjGeneratorOptions.Delimiter));
            Assert.IsTrue(actual.Contains("net-5.0windows"));
            Assert.IsTrue(actual.Contains("Analyzer"));
            Assert.IsTrue(actual.Contains("ReferenceOutputAssembly"));
        }
    }
}