#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit; //using System.DirectoryServices;

namespace MetX.Standard.Scripts
{
    public class InMemoryCompiler<TResultType> where TResultType : class
    {
        private string FilePathForAssembly;
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

        public InMemoryCompiler(string code, bool asExecutable, List<Type> additionalReferenceTypes,
            List<string> additionalSharedReferences, string filePathForAssembly = null!)
        {
            AsExecutable = asExecutable;
            FilePathForAssembly = filePathForAssembly;
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
                GetReference(typeof(StreamBuilder)),
                //GetReference(typeof(System.Windows.Forms.Application)),
                GetReference(typeof(System.IO.File)),
                GetReference(typeof(System.Diagnostics.Process)),
                GetReference(typeof(System.ComponentModel.Component)),
                
                GetSharedReference("System.Runtime"),
                GetSharedReference("System.Private.CoreLib"),
                GetSharedReference("System.Drawing.Primitives"),
                GetSharedReference("System.Windows"),
                GetSharedReference("netstandard"),
            };

            if(AdditionalReferenceTypes?.Count > 0)
                references.AddRange(AdditionalReferenceTypes.Select(GetReference));
            
            if(AdditionalSharedReferences?.Count > 0)
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
            var emitOptions = new EmitOptions(outputNameOverride:Path.GetFileName(FilePathForAssembly));
            var emitResult = CSharpCompiler.Emit(memoryStream, options: emitOptions);

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
                if (FilePathForAssembly.IsNotEmpty())
                {
                    var directoryName = Path.GetDirectoryName(FilePathForAssembly);
                    if(directoryName.IsNotEmpty() && !Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName!);
                    }
                    
                    if (File.Exists(FilePathForAssembly))
                    {
                        File.SetAttributes(FilePathForAssembly, FileAttributes.Normal);
                        File.Delete(FilePathForAssembly);
                    }
                    using var fileStream = File.OpenWrite(FilePathForAssembly);
                    {
                        fileStream.Write(memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
                        fileStream.Flush();
                        fileStream.Close();
                    }
                
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    CompiledAssembly = Assembly.LoadFile(FilePathForAssembly);
                }
                else
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