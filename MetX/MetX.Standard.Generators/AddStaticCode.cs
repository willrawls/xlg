#pragma warning disable RS1036

using MetX.Standard.Generators.Support;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Reflection;

namespace MetX.Standard.Generators
{
    // TODO Do not use until fixed
    [Generator]
    public class AddStaticCode : BaseRoslynCodeGenerator, IIncrementalGenerator
    {
        public string AssemblyPath = @"MetX.Standard.Generators.Actual.dll";
        public string GeneratorClassName = "MetX.Standard.Generators.Actual.AddStaticCodeActual";

        public AddStaticCode() : base(
            @"..\..\..\MetX.Standard.Generators.Actual\bin\Debug\netstandard2.1\MetX.Standard.Generators.Actual.dll",
            @"MetX.Standard.Generators.Actual.AddStaticCodeActual")
        {
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            InitializeContextIfNeeded();
            ShadowRunContext?.Initialize(context);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            InitializeContextIfNeeded();
            ShadowRunContext?.Execute(context);
            Cleanup();
        }

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Register generator execution
            context.RegisterPostInitializationOutput(LoadGenerator);
        }

        private void LoadGenerator(IncrementalGeneratorPostInitializationContext context)
        {
            try
            {
                string baseDir = AppContext.BaseDirectory;
                string assemblyFullPath = Path.Combine(baseDir, AssemblyPath);

                if (!File.Exists(assemblyFullPath))
                {
                    /*context.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor("GEN001", "Missing Assembly",
                            $"Assembly not found: {assemblyFullPath}", "Error",
                            DiagnosticSeverity.Error, true),
                        Location.None));*/
                    return;
                }

                Assembly assembly = Assembly.LoadFrom(assemblyFullPath);
                Type generatorType = assembly.GetType(GeneratorClassName);

                if (generatorType == null)
                {
                    /*context.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor("GEN002", "Missing Generator Class",
                            $"Class {GeneratorClassName} not found in {assemblyFullPath}", "Error",
                            DiagnosticSeverity.Error, true),
                        Location.None));*/
                    return;
                }

                IAmAContextForGeneration generatorInstance =
                    Activator.CreateInstance(generatorType) as IAmAContextForGeneration;

                // TODO: generatorInstance?.Initialize()
            }
            catch (Exception ex)
            {
                /*context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor("GEN999", "Generator Error",
                        $"Exception: {ex.Message}", "Error",
                        DiagnosticSeverity.Error, true),
                    Location.None));*/
            }
        }
    }
}
