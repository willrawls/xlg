using System;
using System.IO;
using MetX.Standard.IO;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetX.Controls;

namespace MetX.Tests.Scripts
{
    [TestClass]
    public class InMemoryCompilerTests
    {

        [TestMethod]
        public void Build_Exe_Simple()
        {
            var source = Sources.WriteStaticLine;
            var outputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test_Build_Exe_Simple.exe");

            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
            
            //var result = QuickScriptProcessorFactory.CompileSource(source, true, null, null, outputFilePath);
            var result = QuickScriptProcessorFactory.Actualize(source, true);
            Assert.IsTrue(result.CompiledSuccessfully, source);
            Assert.IsNotNull(result.CompiledAssembly, source);
            var entryPoint = result.CompiledAssembly.EntryPoint;
            Assert.IsNotNull(entryPoint);

            string workingFolder = result.CompiledAssembly.Location.TokensBeforeLast(@"\");
            string gatherResult = FileSystem.GatherOutputAndErrors(result.CompiledAssembly.Location, null, out var errorOutput, workingFolder);

            Console.WriteLine();
            Console.WriteLine(result.CompiledAssembly.Location);
            
            if(gatherResult.IsNotEmpty())
            {
                Console.WriteLine();
                Console.WriteLine("-----[ Output ]-----");
                Console.WriteLine(gatherResult);
            }

            if(errorOutput.IsNotEmpty())
            {
                Console.WriteLine();
                Console.WriteLine("-----[ Errors ]-----");
                Console.WriteLine(errorOutput);
            }            
            Assert.IsTrue(gatherResult.Contains("Simple_Build_Exe"));
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
            
            var result = QuickScriptProcessorFactory.CompileSource(source, true, null, null, outputFilePath);
            Assert.IsTrue(result.CompiledSuccessfully);
            Assert.IsNotNull(result.CompiledAssembly);
        }

        [TestMethod]
        public void Build_Exe_CalculateSomething()
        {
            var source = Sources.CalculateSomething;
            var outputFolder  = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tests", "Build_Exe_CalculateSomething");
            var outputFilePath = Path.Combine(outputFolder, "The.exe");
            Directory.CreateDirectory(outputFolder);

            if (File.Exists(outputFilePath)) 
                File.Delete(outputFilePath);

            var result = QuickScriptProcessorFactory.CompileSource(source, true, null, null, outputFilePath);
            Assert.IsTrue(result.CompiledSuccessfully);
            Assert.IsNotNull(result.CompiledAssembly);
        }
    }
}