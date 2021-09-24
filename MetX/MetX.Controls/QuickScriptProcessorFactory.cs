using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using MetX.Standard;
using MetX.Standard.Interfaces;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Scripts;
using MetX.Windows.Library;
using Microsoft.CSharp;

namespace MetX.Controls
{
    public static class QuickScriptProcessorFactory
    {
        public static BaseLineProcessor GenerateQuickScriptLineProcessor(IGenerationHost host, ContextBase @base, XlgQuickScript scriptToRun)
        { 
            var source = scriptToRun.ToCSharp(false, ContextBase.Default.Templates["Native"]);
            var assemblies = DefaultTypesForCompiler();
            var shared = new List<string>();
            var compiler = CompileSource(source, false, assemblies, shared, null);

            if (compiler == null)
            {
                host?.MessageBox.Show("Failed to create and compile (internal not due to script). This should never happen. Call Will");
                return null;
            }
            
            if (BuildQuickScriptProcessor(host, scriptToRun, compiler, out var generatedQuickScriptLineProcessor)) 
                return generatedQuickScriptLineProcessor;

            var forDisplay = compiler.Failures.ForDisplay(source.Lines());
            QuickScriptWorker.ViewText(@base.Host, source, true);
            QuickScriptWorker.ViewText(@base.Host, forDisplay, true);
            return null;
        }

        public static bool BuildQuickScriptProcessor(
            IGenerationHost host, 
            XlgQuickScript scriptToRun,
            InMemoryCompiler<string> compiler,
            out BaseLineProcessor generateQuickScriptLineProcessor)
        {
            if (compiler.CompiledSuccessfully)
            {
                if (!ReferenceEquals(compiler.CompiledAssembly, null))
                {
                    var arguments = new object[] { host };
                    var processor = compiler.CompiledAssembly
                            .CreateInstance("MetX.Scripts.QuickScriptProcessor", false, BindingFlags.CreateInstance, null, arguments, null, null)
                        as BaseLineProcessor;

                    if (processor == null)
                    {
                        generateQuickScriptLineProcessor = null;
                        return true;
                    }

                    processor.InputFilePath = scriptToRun.InputFilePath;
                    processor.DestinationFilePath = scriptToRun.DestinationFilePath;

                    generateQuickScriptLineProcessor = processor;
                    return true;
                }
            }
            
            generateQuickScriptLineProcessor = null;
            return false;
        }

        public static List<Type> DefaultTypesForCompiler()
        {
            var assemblies = new List<Type>
            {
                typeof(Application),                             // System.Windows.Forms
                typeof(CSharpCodeProvider),
                typeof(BaseLineProcessor),                       // MetX.Standard
                typeof(InMemoryCache<>),                         // MetX.Library
                typeof(Context),                                 // MetX.Controls
                typeof(ChooseOrderDialog),
                typeof(AskForStringDialog),                      // MetX.Windows
            };
            return assemblies;
        }

        public static InMemoryCompiler<string> CompileSource(string source,
            bool asExecutable,
            List<Type> additionalReferences,
            List<string> additionalSharedReferences, 
            string filePathForAssembly)
        {
            var compiler = new InMemoryCompiler<string>(source, asExecutable, additionalReferences, additionalSharedReferences, filePathForAssembly);
            return compiler;
        }
    }
}