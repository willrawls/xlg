using System;
using System.IO;
using MetX.Standard.Primary.Generation;
using MetX.Standard.Primary.Generation.CSharp.Project;

namespace MetX.Console.Tests.Standard.Generation.CSharp.Project;

public class TestHelpers
{
    public static IGenerateCsProj SetupGenerator<T>(GenFramework genFramework) where T : class, IGenerateCsProj, new()
    {
        var options = CsProjGeneratorOptions
                .Defaults()
                .WithFramework(genFramework)
                .WithPathToTemplatesFolder(@"..\..\..\..\MetX.Standard.Generators\Templates")
                .WithOutputPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Product"))
            ;

        var generator = new T()
            .WithOptions(options)
            .Setup();
        return generator;
    }

}