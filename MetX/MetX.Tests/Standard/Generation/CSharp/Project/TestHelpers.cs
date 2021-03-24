using System;
using System.IO;
using MetX.Aspects;
using MetX.Standard.Generation.CSharp.Project;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    public class TestHelpers
    {
        public static IGenerateCsProj Setup<T>() where T : class, IGenerateCsProj, new()
        {
            var genGenOptions = CsProjGeneratorOptions.Defaults(GenFramework.Standard20);
            genGenOptions.OutputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Product");
            genGenOptions.PathToTemplatesFolder = @"..\..\..\..\MetX.Generators\Templates";

            var generator = new T()
                .WithOptions(genGenOptions);
            return generator;
        }

    }
}