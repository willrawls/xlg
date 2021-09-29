using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using ICSharpCode.TextEditor.Actions;
using MetX.Standard.IO;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetX.Controls;
using MetX.Standard;
using MetX.Standard.Generation;
using MetX.Standard.Interfaces;
using MetX.Standard.Pipelines;

namespace MetX.Tests.Scripts
{
    [TestClass]
    public class InMemoryCompilerTests
    {
        [TestMethod]
        public void Build_Exe_Simple()
        {
            var source = Sources.WriteStaticLine;
            var script = Sources.WriteStaticLineScript();

            var outputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestBuildExeSimple");
            FileSystem.SafelyDeleteDirectory(outputFilePath);
            
            var host = new DoNothingGenerationHost();
            ContextBase context = new Context(host);
            host.Context = context;

            host.Context.Templates = new XlgQuickScriptTemplateList(AppDomain.CurrentDomain.BaseDirectory, "TestTemplates");
            Assert.IsTrue(host.Context.Templates.Any(t => t.Name == "TestExe"));
            
            var settings = script.BuildSettings(true, false, host);
            settings.OutputFolder = outputFilePath;

            ActualizationResult result = settings.ActualizeAndCompile();
            Assert.IsTrue(result.ActualizationSuccessful);
            Assert.IsTrue(result.CompileSuccessful);

            BaseLineProcessor actual = result.AsBaseLineProcessor();
            Assert.IsNotNull(actual, source);

            MethodInfo entryPoint = actual.GetType().Assembly.EntryPoint;
            Assert.IsNotNull(entryPoint);

            AssertConsoleExecutableOutputsProperly(result, "Simple_Build_Exe");
        }

        private static void AssertConsoleExecutableOutputsProperly(ActualizationResult result, string expected)
        {
            var gatherResult = FileSystem.GatherOutputAndErrors(result.DestinationExecutableFilePath, null, out var errorOutput, result.Settings.OutputFolder, 15, ProcessWindowStyle.Hidden);

            Console.WriteLine();
            Console.WriteLine(result.DestinationExecutableFilePath);

            if (gatherResult.IsNotEmpty())
            {
                Console.WriteLine();
                Console.WriteLine("-----[ Output ]-----");
                Console.WriteLine(gatherResult);
            }

            if (errorOutput.IsNotEmpty())
            {
                Console.WriteLine();
                Console.WriteLine("-----[ Errors ]-----");
                Console.WriteLine(errorOutput);
            }

            Assert.IsTrue(gatherResult.Contains(expected));
        }

        [TestMethod]
        public void Build_Exe_FromFirstScript()
        {
            var source = Sources.FirstScript;
            var outputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test_Build_Exe_FirstScript.exe");

            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            var settings = QuickScriptProcessorFactory.BuildSettings(Sources.FirstScriptScript(), true, true, new DoNothingGenerationHost());
            var result = settings.ActualizeAndCompile();
            Assert.IsTrue(result.CompileSuccessful);
            Assert.IsNotNull(result.DestinationExecutableFilePath);
            Assert.IsTrue(File.Exists(result.DestinationExecutableFilePath));

            AssertConsoleExecutableOutputsProperly(result, "First");
        }

        [TestMethod]
        public void Build_Exe_CalculateSomething()
        {
            var source = Sources.CalculateSomething;
            var outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tests", "Build_Exe_CalculateSomething");
            var outputFilePath = Path.Combine(outputFolder, "The.exe");
            Directory.CreateDirectory(outputFolder);

            if (File.Exists(outputFilePath))
                File.Delete(outputFilePath);

            var settings = QuickScriptProcessorFactory.BuildSettings(
                new XlgQuickScript("Calculate Something", Sources.CalculateSomething), true, true,
                new DoNothingGenerationHost());
            var result = settings.ActualizeAndCompile();
            Assert.IsTrue(result.CompileSuccessful);
            Assert.IsTrue(File.Exists(result.DestinationExecutableFilePath));
            AssertConsoleExecutableOutputsProperly(result, "Something");
        }
    }
}