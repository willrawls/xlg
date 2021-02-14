using System.Threading.Tasks;
using MetX.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Scripts
{
    public static class Sources
    {
        // // //
        public static readonly string Source_WriteStaticLine = BuildMain(@"System.Console.WriteLine(""Build_Exe_Simple"")");

        // // //
        public static string WrapWithNamespace(this string source, string namespaceName = "Tests")
        {
            return $"namespace {namespaceName} {{\n{source}\n}}";
        }

        public static string WrapWithVoidMain(this string source)
        {
            return WrapWithNamespace($"public void main() {{ \n{source}\n}}");
        }

        public static string BuildMain(this string sourceInsideVoidMain)
        {
            return WrapWithNamespace(WrapWithVoidMain(sourceInsideVoidMain));
        }
    }

    [TestClass]
    public class InMemoryCompilerTests
    {

        [TestMethod]
        public void Build_Exe_Simple()
        {

            InMemoryCompiler<string> result = XlgQuickScript.CompileSource(Sources.Source_WriteStaticLine, true, null, null);
            Assert.IsTrue(result.CompiledSuccessfully);

        }
    }
}