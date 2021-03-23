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
}