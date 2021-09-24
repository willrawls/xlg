using MetX.Standard.Scripts;

namespace MetX.Tests.Scripts
{
    public static class Sources
    {
        // // //
        public static readonly string WriteStaticLine = BuildMain(
            @"
        System.Console.WriteLine(""Simple_Build_Exe"");
");

        public static readonly string FirstScript = @"
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MetX.Standard;
using MetX.Standard.IO;
using MetX.Standard.Data;
using MetX.Standard.Scripts;
using MetX.Standard.Library;
namespace MetX.Scripts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(""Ding FirstScript"");
        }
    }

    public class QuickScriptProcessor : BaseLineProcessor
    {
        public QuickScriptProcessor(MetX.Standard.Interfaces.IGenerationHost host) : base(host)
        {
        }

        public override bool? ReadInput(string inputType)
        {
            return base.ReadInput(inputType);
        }
        public override bool Start()
        {
            return true;
        }
        public override bool ProcessLine(string line, int number)
        {
            if (string.IsNullOrEmpty(line) && number > -1) return true;

            if(line.Length < 20)
	            Output.AppendLine(line);
            else
	            Output.AppendLine(line.Substring(0, 20));

            return true;    // true = keep going
        }
        public override bool Finish()
        {
            Output.Finish();
            return true;
        }
    }
}";

        public static string CalculateSomething = @"
    using System;

    namespace Foo
    {
        public sealed class Bar 
        {
            public static void Main()
            {
                Console.WriteLine($""Square root of 42 is {CalculateSomething()}"");
            }

            public static int CalculateSomething()
            {
                return (int) Math.Sqrt(42);
            }
        }
    }";

        public static XlgQuickScript FirstScriptScript()
        {
            return new ("First script", FirstScript);

        }

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

        public static XlgQuickScript WriteStaticLineScript()
        {
            return new ("Write static line", WriteStaticLine);
        }

    }
}