using System;
using System.IO;
using MetX.Aspects;
using MetX.Standard.Generation.CSharp.Project;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    public class TestHelpers
    {
        public static IGenerateCsProj SetupGenerator<T>() where T : class, IGenerateCsProj, new()
        {
            var options = CsProjGeneratorOptions
                .Defaults()
                .WithFramework(GenFramework.Standard20)
                .WithPathToTemplatesFolder(@"..\..\..\..\MetX.Generators\Templates")
                .WithOutputPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Product"))
                ;

            var generator = new T().WithOptions(options);
            return generator;
        }

    }
}