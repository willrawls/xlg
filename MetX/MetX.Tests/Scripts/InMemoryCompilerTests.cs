using System;
using System.IO;
using MetX.Standard.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            
            var result = XlgQuickScript.CompileSource(source, true, null, null, outputFilePath);
            Assert.IsTrue(result.CompiledSuccessfully, source);
            Assert.IsNotNull(result.CompiledAssembly, source);
            var entryPoint = result.CompiledAssembly.EntryPoint;
            Assert.IsNotNull(entryPoint);
            
            entryPoint.Invoke(null, null); // new object[] { new string[] { "arg1", "arg2", "etc" } } );
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
            
            var result = XlgQuickScript.CompileSource(source, true, null, null, outputFilePath);
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

            var result = XlgQuickScript.CompileSource(source, true, null, null, outputFilePath);
            Assert.IsTrue(result.CompiledSuccessfully);
            Assert.IsNotNull(result.CompiledAssembly);
        }
    }
}