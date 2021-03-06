﻿#nullable enable
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
        public string? PathToSharedRoslyn { get; set; }
        public bool AsExecutable { get; set; }
        public List<Type> AdditionalReferenceTypes { get; set; }
        public List<string> AdditionalSharedReferences { get; set; }
        public Assembly? CompiledAssembly { get; set; }
        public Type? CompiledType { get; set; }
        public CSharpCompilation? CSharpCompiler { get; set; }
        public BaseLineProcessor? Instance { get; set; }
        public object? InstanceObject { get; set; }
        public SyntaxTree SyntaxTree { get; set; }
        public Diagnostic[]? Failures { get; set; }

        public bool CompiledSuccessfully
        {
            get
            {
                if (Failures != null && Failures.Length > 0)
                    return false;
                return CompiledAssembly != null;
            }
        }

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
            Failures = null!;
            CSharpCompiler = null!;
            CompiledType = null!;
            CompiledAssembly = null!;

            var assemblyName = Path.GetRandomFileName();
            var references = new List<MetadataReference>
            {
                GetReference(typeof(object)),
                GetReference(typeof(Enumerable)),
                GetReference(typeof(Console)),
                GetReference(typeof(GCSettings)),
                GetReference(typeof(InMemoryCompiler<TResultType>)),
                GetReference(typeof(System.Windows.Forms.Application)),
                GetSharedReference("System.Runtime"),
                //GetSharedReference("System.Windows.Forms"),
            };

            references.AddRange(AdditionalReferenceTypes.Select(GetReference));
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
                new CSharpCompilationOptions(AsExecutable
                    ? OutputKind.ConsoleApplication
                    : OutputKind.DynamicallyLinkedLibrary));
        }

        internal MetadataReference GetSharedReference(string name)
        {
            PathToSharedRoslyn ??= typeof(object).Assembly.Location.TokensBeforeLast(@"\");

            var systemRuntimeDll = Path.Combine(PathToSharedRoslyn, "System.Runtime.DLL");
            if (!File.Exists(systemRuntimeDll))
                throw new ArgumentException(nameof(name));
            
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

            if (CSharpCompiler == null)
                throw new InvalidOperationException();

            using var memoryStream = new MemoryStream();
            var emitResult = CSharpCompiler.Emit(memoryStream);

            if (!emitResult.Success)
            {
                Failures = emitResult.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error)
                    .ToArray();

                foreach (var diagnostic in Failures)
                    Console.WriteLine($"{diagnostic.Id}: {diagnostic.Location}: {diagnostic.GetMessage()}");

                if (Failures.Any()) return Failures.ToArray().Length;
            }
            else
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                CompiledAssembly = Assembly.Load(memoryStream.ToArray());
            }

            return 0;
        }

        public BaseLineProcessor? GetInstance(string fullClassNameWithNamespace)
        {
            if (BuildCompiledAssembly() > 0)
                // At least one failure
                return null;

            if (CompiledAssembly == null)
                return null!;

            CompiledType = CompiledAssembly.GetType(fullClassNameWithNamespace);
            if (CompiledType == null)
                throw new Exception("GetType failed");

            InstanceObject = Activator.CreateInstance(CompiledType);

            Instance = InstanceObject as BaseLineProcessor;
            if (Instance == null)
                throw new Exception("Cast to Actual failed");

            return Instance;
        }

        internal class ReferenceEqualityComparer : IEqualityComparer<MetadataReference>
        {
            public bool Equals(MetadataReference? x, MetadataReference? y)
            {
                if (y == null) throw new ArgumentNullException(nameof(y));
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                return x.GetType() == y.GetType() && string.Equals(x.Display, y.Display, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(MetadataReference obj)
            {
                return obj.Display != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Display) : 0;
            }
        }
    }
}