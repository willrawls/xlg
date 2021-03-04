using System.IO;
using System.Threading.Tasks;
using MetX.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Scripts
{
    public static class Sources
    {
        // // //
        public static readonly string Source_WriteStaticLine = BuildMain(
            @"
        System.Console.WriteLine(""Build_Exe_Simple"");
");

        // // //
        public static string WrapWithNamespace(this string source, string namespaceName = "Tests")
        {
            return $@"
namespace {namespaceName} 
{{
    {source}
}}";
        }

        public static string WrapWithVoidMain(this string source)
        {
            return WrapWithNamespace(
                WrapWithClass("Program",
                    $@"
        // [STAThread]
        public static void Main() 
        {{
        {source}
        }}"));
        }

        public static string WrapWithClass(string className, string code)
        {
            return $@"
    public class {className}
    {{
        {code}
    }}
";
        }

        public static string BuildMain(this string sourceInsideVoidMain)
        {
            return WrapWithVoidMain(sourceInsideVoidMain);
        }
    }

    [TestClass]
    public class InMemoryCompilerTests
    {

        [TestMethod]
        public void Build_Exe_Simple()
        {
            var source = Sources.Source_WriteStaticLine;
            var outputFilePath = "fake.exe";

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
    }
}