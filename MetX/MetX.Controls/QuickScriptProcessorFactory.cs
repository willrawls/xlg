using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using MetX.Standard;
using MetX.Standard.Interfaces;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Pipelines;
using MetX.Standard.Scripts;
using MetX.Windows.Library;
using Microsoft.CSharp;

namespace MetX.Controls
{
    public static class QuickScriptProcessorFactory
    {
        public static BaseLineProcessor GenerateAssemblyFromNative(IGenerationHost host, ContextBase @base, XlgQuickScript scriptToRun)
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

        private static bool BuildQuickScriptProcessor(
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

        private static InMemoryCompiler<string> CompileSource(string source,
            bool asExecutable,
            List<Type> additionalReferences,
            List<string> additionalSharedReferences, 
            string filePathForAssembly)
        {
            var compiler = new InMemoryCompiler<string>(source, asExecutable, additionalReferences, additionalSharedReferences, filePathForAssembly);
            return compiler;
        }

        public static ActualizationResult Actualize(XlgQuickScript scriptToRun, bool asExecutable, IGenerationHost host)
        {
            ActualizationResult result = ActualizeCode(scriptToRun, asExecutable, false, host);
            if (result == null) 
                return null;

            if (result.ActualizationSuccessful)
            {
                var compileResult = result.Compile();
                if (compileResult)
                {
                    return result;
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("-----[ Output Folder ]-----");
            sb.AppendLine($"{result.Settings.OutputFolder}");

            sb.AppendLine();
            sb.AppendLine("-----[ Compilation failure ]-----");
            sb.AppendLine();
            sb.AppendLine(result.CompileErrorText);
            sb.AppendLine();
            sb.AppendLine("-----[ Output from dotnet.exe ]-----");
            sb.AppendLine();
            sb.AppendLine(result.OutputText);
            sb.AppendLine();

            var answer = host.MessageBox.Show(sb.ToString(), "OPEN OUTPUT FOLDER?", MessageBoxChoices.YesNo);
            if (answer == MessageBoxResult.Yes)
            {
                QuickScriptWorker.ViewFolder(host, result.Settings.OutputFolder);
            }
            return result;
        }

        public static ActualizationResult ActualizeCode(XlgQuickScript script, bool forExecutable, bool simulate, IGenerationHost host)
        {
            var settings = ActualizationSettingsFactory(script, forExecutable, simulate, host);
            var result = settings.QuickScriptTemplate.Actualize(settings);
            return result;
        }

        public static ActualizationSettings ActualizationSettingsFactory(XlgQuickScript script, bool forExecutable, bool simulate, IGenerationHost host)
        {
            if (script == null)
                return null;

            var templateName = forExecutable
                ? script.ExeTemplateName
                : script.NativeTemplateName;

            var template = host.Context.Templates[templateName];
            var settings = new ActualizationSettings(template, simulate, script);
            return settings;
        }
    }
}