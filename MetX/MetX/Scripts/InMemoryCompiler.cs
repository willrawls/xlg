using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using MetX.Library;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MetX.Scripts
{
    public class InMemoryCompiler<TResultType> where TResultType : class
    {
        public bool AsExecutable { get; }
        public List<Type> AdditionalReferenceTypes { get; }
        public List<string> AdditionalSharedReferences { get; }
        public Assembly CompiledAssembly;
        public Type? CompiledType;
        public CSharpCompilation CSharpCompiler;
        public BaseLineProcessor Instance;
        public object? InstanceObject;

        public string PathToSharedRoslyn;
        public SyntaxTree SyntaxTree;
        public Diagnostic[] Failures;

        public bool CompiledSuccessfully =>
            (Failures != null && Failures.Length > 0) == false
             || CompiledType == null
             || CompiledAssembly == null;
        
        public InMemoryCompiler(string code, bool asExecutable, List<Type> additionalReferenceTypes, List<string> additionalSharedReferences)
        {
            AsExecutable = asExecutable;
            AdditionalReferenceTypes = additionalReferenceTypes;
            AdditionalSharedReferences = additionalSharedReferences;
            SyntaxTree = CSharpSyntaxTree.ParseText(code);
            BuildCompiledAssembly();
        }

        public void SetupCompiler()
        {
            Failures = null;
            CSharpCompiler = null;
            CompiledType = null;
            CompiledAssembly = null;
            
            var assemblyName = Path.GetRandomFileName();
            var references = new List<MetadataReference>
            {
                GetReference(typeof(object)),
                GetReference(typeof(Enumerable)),
                GetReference(typeof(Console)),
                GetReference(typeof(GCSettings)),
                GetReference(typeof(InMemoryCompiler<TResultType>)),
                GetSharedReference("System.Runtime")
            };

            if (AdditionalReferenceTypes != null) 
                references.AddRange(AdditionalReferenceTypes.Select(GetReference));

            if (AdditionalSharedReferences != null)
                references.AddRange(AdditionalSharedReferences.Select(GetSharedReference));

            references = references.Distinct(new ReferenceEqualityComparer()).ToList();

            references.Sort((reference, metadataReference) => string.Compare(
                reference.Display,
                metadataReference.Display,
                StringComparison.OrdinalIgnoreCase));

            CSharpCompiler = CSharpCompilation.Create(
                assemblyName,
                new[] {SyntaxTree},
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }

        internal class ReferenceEqualityComparer : IEqualityComparer<MetadataReference>
        {
            public bool Equals(MetadataReference x, MetadataReference y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Display, y.Display, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(MetadataReference obj)
            {
                return (obj.Display != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Display) : 0);
            }
        }
        
        internal MetadataReference GetSharedReference(string name)
        {
            PathToSharedRoslyn ??= typeof(object).Assembly.Location.TokensBeforeLast(@"\");

            var fullAssemblyPath = Path.Combine(PathToSharedRoslyn, name);
            if (!fullAssemblyPath.EndsWith(".dll"))
                fullAssemblyPath += ".dll";

            Console.WriteLine($"Shared: {name}  @  {fullAssemblyPath}");

            var reference = MetadataReference.CreateFromFile(fullAssemblyPath);
            return reference;
        }

        internal MetadataReference GetReference(Type type)
        {
            Console.WriteLine($"{type.Assembly.FullName}  @  {type.Assembly.Location}");

            var reference = MetadataReference.CreateFromFile(type.Assembly.Location);
            return reference;
        }

        public int BuildCompiledAssembly()
        {
            if (CSharpCompiler == null)
                SetupCompiler();

            using var ms = new MemoryStream();
            var result = CSharpCompiler.Emit(ms);

            if (!result.Success)
            {
                Failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error)
                    .ToArray();

                foreach (var diagnostic in Failures)
                {
                    Console.WriteLine($"{diagnostic.Id}: {diagnostic.Location}: {diagnostic.GetMessage()}");
                }

                if (Failures.Any()) return Failures.ToArray().Length;
            }
            else
            {
                ms.Seek(0, SeekOrigin.Begin);
                CompiledAssembly = Assembly.Load(ms.ToArray());
            }

            return 0;
        }
        
        public BaseLineProcessor GetInstance(string fullClassNameWithNamespace)
        {
            if (BuildCompiledAssembly() > 0)
                // At least one failure
                return null;

            if (CompiledAssembly == null)
                return null;

            CompiledType = CompiledAssembly.GetType(fullClassNameWithNamespace);
            if (CompiledType == null)
                throw new Exception("GetType failed");

            InstanceObject = Activator.CreateInstance(CompiledType);

            Instance = InstanceObject as BaseLineProcessor;
            if (Instance == null)
                throw new Exception("Cast to Actual failed");

            return Instance;
        }
    }
}