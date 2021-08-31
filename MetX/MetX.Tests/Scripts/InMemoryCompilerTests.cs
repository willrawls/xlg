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
        public void Build_Exe_CalculateSomething()
        {
            var source = Sources.CalculateSomething;
            string exeName = "Test_Build_Exe_CalculateSomething.exe";
            BuildCommonToFreshFolder(exeName, source, false);
        }

        public static InMemoryCompiler<string> BuildCommonToFreshFolder(string exeName, string source, bool cleanupFolderAfter)
        {
            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            var outputFilePath = Path.Combine(outputPath, exeName);
            Console.WriteLine(outputFilePath);

            var result = XlgQuickScript.CompileSource(source, true, XlgQuickScript.OfficialFrameworkPath.LatestCore50(), outputPath, exeName, null, null);
        
            if(cleanupFolderAfter)
                Directory.Delete(outputPath, true);

            Assert.IsTrue(result.CompiledSuccessfully);
            Assert.IsNotNull(result.CompiledAssembly);
            return result;
        }

        [TestMethod]
        public void Build_Exe_FromFirstScript()
        {
            var source = Sources.FirstScript;
            string exeName = "Test_Build_Exe_FirstScript.exe";
            BuildCommonToFreshFolder(exeName, source, false);
        }

        [TestMethod]
        public void Build_Exe_Simple()
        {
            string source = Sources.WriteStaticLine;
            string exeName = "Test_Build_Exe_Simple.exe";
            InMemoryCompiler<string> result = BuildCommonToFreshFolder(exeName, source, false);

            Assert.IsTrue(result.CompiledSuccessfully, source);
            Assert.IsNotNull(result.CompiledAssembly, source);
            var entryPoint = result.CompiledAssembly.EntryPoint;
            Assert.IsNotNull(entryPoint);
            
            entryPoint.Invoke(null, null); // new object[] { new string[] { "arg1", "arg2", "etc" } } );
        }
    }
}