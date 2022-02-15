using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using MetX.Standard.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MetX.Console.Research
{
    public class Program
    {
        public static Assembly SystemRuntime = Assembly.Load(new AssemblyName("System.Runtime"));
 
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ExecuteInMemoryAssembly(Compilation compilation, int i)
        {
            var context = new CollectibleAssemblyLoadContext();

            using var ms = new MemoryStream();
            compilation.Emit(ms);
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = context.LoadFromStream(ms);
 
            var type = assembly.GetType("Greeter");
            if (type != null)
            {
                var greetMethod = type.GetMethod("Hello");
 
                var instance = Activator.CreateInstance(type);
                if (greetMethod != null)
                {
                    var result = greetMethod.Invoke(instance, new object[] { i });
                }
            }

            context.Unload();
        }

        public static void Main(string[] args)
        {
            var compilation = CSharpCompilation.Create("DynamicAssembly", new[] { CSharpSyntaxTree.ParseText(@"
        public class Greeter
        {
            public void Hello(int iteration)
            {
                System.Console.WriteLine($""Hello in memory {iteration}!"");
            }
        }") },
                new[]
                {
                    MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(System.Console).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(SystemRuntime.Location),
                },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
 
            for (var i = 0; i < 3000; i++)
            {
                ExecuteInMemoryAssembly(compilation, i);
            }
 
            GC.Collect();
            GC.WaitForPendingFinalizers();
 
            System.Console.ReadKey();
        }
    }
}