/*using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MetX.Standard.IO;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetX.Controls;
using MetX.Standard.Primary;
using MetX.Standard.Generation;

namespace MetX.Tests.Scripts
{
    [TestClass]
    public class InMemoryCompilerTests
    {
        [TestMethod, Ignore()]
        public void Build_Exe_Simple()
        {
            var source = Sources.WriteStaticLine;
            var script = Sources.WriteStaticLineScript();

            var outputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestBuildExeSimple");
            FileSystem.SafelyDeleteDirectory(outputFilePath);

            BuildDoNothingGenerationHost(out var pathToTestTemplates, out var host, out var context);
            Assert.IsTrue(host.Context.Templates.Any(t => t.Name == "TestExe"));
            
            var settings = script.BuildSettings(false, host);
            settings.ProjectFolder = outputFilePath;

            var result = settings.ActualizeAndCompile();
            Assert.IsTrue(result.ActualizationSuccessful);
            Assert.IsTrue(result.CompileSuccessful);

            var actual = result.AsBaseLineProcessor();
            Assert.IsNotNull(actual, source);

            var entryPoint = actual.GetType().Assembly.EntryPoint;
            Assert.IsNotNull(entryPoint);

            AssertConsoleExecutableOutputsProperly(result, "Simple_Build_Exe");
        }

        public static void BuildDoNothingGenerationHost(out string pathToTestTemplates,
            out DoNothingGenerationHost host, out ContextBase context)
        {
            pathToTestTemplates = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestTemplates");
            context = new ContextBase(pathToTestTemplates, null);
            host = new DoNothingGenerationHost("", context);
            context.Host = host;
            host.Context = context;
            host.Context.Templates = new XlgQuickScriptTemplateList(pathToTestTemplates);
        }

        private static void AssertConsoleExecutableOutputsProperly(ActualizationResult result, string expected)
        {
            var gatherResult = FileSystem.GatherOutputAndErrors(result.DestinationExecutableFilePath, null, out var errorOutput, result.Settings.ProjectFolder, 15, ProcessWindowStyle.Hidden);

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

        [TestMethod, Ignore()]
        public void Build_Exe_FromFirstScript()
        {
            var source = Sources.FirstScript;
            var outputFilePath = Path.Combine(Environment.GetEnvironmentVariable("TEMP")!, "Test_Build_Exe_FirstScript.exe");

            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            BuildDoNothingGenerationHost(out var pathToTestTemplates, out var host, out var context);
            var settings = Sources.ExampleFirstScript().BuildSettings(true, host);
            var result = settings.ActualizeAndCompile();
            Assert.IsTrue(result.CompileSuccessful);
            Assert.IsNotNull(result.DestinationExecutableFilePath);
            Assert.IsTrue(File.Exists(result.DestinationExecutableFilePath));

            AssertConsoleExecutableOutputsProperly(result, "First");
        }

        [TestMethod, Ignore()]
        public void Build_Exe_CalculateSomething()
        {
            var source = Sources.CalculateSomething;
            var outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tests", "Build_Exe_CalculateSomething");
            var outputFilePath = Path.Combine(outputFolder, "The.exe");
            Directory.CreateDirectory(outputFolder);

            if (File.Exists(outputFilePath))
                File.Delete(outputFilePath);

            var xlgQuickScript = new XlgQuickScript("Calculate Something", Sources.CalculateSomething) {TemplateName = "Exe"};

            BuildDoNothingGenerationHost(out var pathToTestTemplates, out var host, out var context);
            
            var template = new XlgQuickScriptTemplate("Fred", "Fred");
            context.Templates.Add(template);

            var settings = xlgQuickScript.BuildSettings(true, host);
            var result = settings.ActualizeAndCompile();
            Assert.IsTrue(result.CompileSuccessful);
            Assert.IsTrue(File.Exists(result.DestinationExecutableFilePath));
            AssertConsoleExecutableOutputsProperly(result, "Something");
        }
    }
}*/