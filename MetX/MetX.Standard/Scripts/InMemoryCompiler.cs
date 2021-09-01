using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using MetX.Standard.Library;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit; //using System.DirectoryServices;
#pragma warning disable 8618
#pragma warning disable 8604

namespace MetX.Standard.Scripts
{
    public class InMemoryCompiler<TResultType> where TResultType : class
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string FrameworkFolder { get; set; }
        public string OutputFolder { get; set; }
        public string OutputFilename { get; set; }
        public string OutputFilePath =>
            OutputFolder.IsNotEmpty()
                ? Path.Combine(OutputFolder, OutputFilename)
                : OutputFilename ?? $"{Id:N}.{(AsExecutable ? "exe" : "dll")}";

        public string PathToSharedRoslynAsThisProcess { get; set; }

        public bool AsExecutable { get; set; }
        public List<Type> AdditionalFrameworkReferences { get; set; }
        public List<string> AdditionalCustomReferences { get; set; }
        public Assembly? CompiledAssembly { get; set; }
        public Type? CompiledType { get; set; }
        public CSharpCompilation? CSharpCompiler { get; set; }
        public BaseLineProcessor? Instance { get; set; }
        public object? InstanceObject { get; set; }
        public SyntaxTree SyntaxTree { get; set; }
        public Diagnostic[]? Failures { get; set; }

        public bool CompiledSuccessfully => Failures is not { Length: > 0 } && CompiledAssembly != null;

        public InMemoryCompiler(string code, bool asExecutable,
            string frameworkFolder,
            string outputFolder,
            string outputFilename,
            List<Type> additionalFrameworkReferences,
            List<string> additionalCustomReferences)
        {
            if (frameworkFolder.IsEmpty()) throw new ArgumentNullException(nameof(frameworkFolder));
            if (outputFolder.IsEmpty()) throw new ArgumentNullException(nameof(outputFolder));

            AsExecutable = asExecutable;
            FrameworkFolder = frameworkFolder;
            OutputFolder = outputFolder;
            OutputFilename = outputFilename;
            AdditionalFrameworkReferences = additionalFrameworkReferences;
            AdditionalCustomReferences = additionalCustomReferences;
            SyntaxTree = CSharpSyntaxTree.ParseText(code);
            BuildCompiledAssembly();
        }


        public void SetupCompiler()
        {
            Failures = null!;
            CSharpCompiler = null!;
            CompiledType = null!;
            CompiledAssembly = null!;

            string? assemblyName = Path.GetRandomFileName();

            var references = new List<MetadataReference>
            {
                GetFrameworkReference(FrameworkFolder, "netstandard"),
                GetFrameworkReference(FrameworkFolder, "System.Runtime"),
                GetFrameworkReference(FrameworkFolder, "System"),
                GetFrameworkReference(FrameworkFolder, "System.IO"),
                GetFrameworkReference(FrameworkFolder, "System.Data"),
                GetFrameworkReference(FrameworkFolder, "System.Threading.Tasks"),
                GetFrameworkReference(FrameworkFolder, "System.Threading.Thread"),
                GetFrameworkReference(FrameworkFolder, "System.Threading"),
                GetFrameworkReference(FrameworkFolder, "System.Text.Json"),
                GetFrameworkReference(FrameworkFolder, "System.Text.RegularExpressions"),
                GetFrameworkReference(FrameworkFolder, "System.Linq"),
                GetFrameworkReference(FrameworkFolder, "System.Linq.Expressions"),
                GetFrameworkReference(FrameworkFolder, "System.Linq.Queryable"),
                GetFrameworkReference(FrameworkFolder, "System.Collections"),
                GetFrameworkReference(FrameworkFolder, "System.Collections.Immutable"),
                GetFrameworkReference(FrameworkFolder, "System.Collections.NonGeneric"),
                GetFrameworkReference(FrameworkFolder, "System.Collections.Specialized"),
                GetFrameworkReference(FrameworkFolder, "System.Drawing.Primitives"),
                GetFrameworkReference(FrameworkFolder, "System.ComponentModel"),
                GetFrameworkReference(FrameworkFolder, "System.Windows"),
                GetFrameworkReference(FrameworkFolder, "System.Xml"),
                GetFrameworkReference(FrameworkFolder, "System.Xml.Serialization"),
                GetFrameworkReference(FrameworkFolder, "System.Xml.XPath"),
                CopyAssemblyAndGetCustomReference(OutputFolder, typeof(GenInstance)),
                CopyAssemblyAndGetCustomReference(OutputFolder, typeof(AssocArray)),
            };

            if (AdditionalFrameworkReferences?.Count > 0)
            {
                foreach (Type referenceType in AdditionalFrameworkReferences)
                    references.Add(CopyAssemblyAndGetCustomReference(OutputFolder, referenceType));
            }

            if (AdditionalCustomReferences?.Count > 0)
            {
                foreach (var filePath in AdditionalCustomReferences)
                    references.Add(GetFrameworkReference(OutputFilePath, filePath));
            }

            references = references.Distinct(new ReferenceEqualityComparer()).ToList();
            references.Sort((reference, metadataReference) => string.Compare(
                reference.Display,
                metadataReference.Display,
                StringComparison.OrdinalIgnoreCase));

            CSharpCompiler = CSharpCompilation.Create(
                assemblyName,
                new[] { SyntaxTree },
                references,
                new CSharpCompilationOptions(AsExecutable
                    ? OutputKind.ConsoleApplication
                    : OutputKind.DynamicallyLinkedLibrary));
        }

        public static MetadataReference CopyAssemblyAndGetCustomReference(string outputFolder, Type customType)
        {
            string destinationCustomAssemblyFilename = customType.Assembly.Location.LastPathToken();
            string customAssemblyDestinationPath = Path.Combine(outputFolder, destinationCustomAssemblyFilename);
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            if (!File.Exists(customAssemblyDestinationPath))
            {
                File.Copy(customType.Assembly.Location, customAssemblyDestinationPath);
            }

            Console.WriteLine($"Custom assembly copied to {destinationCustomAssemblyFilename} from {customType.Assembly.Location}");
            PortableExecutableReference reference = MetadataReference.CreateFromFile(destinationCustomAssemblyFilename);

            string customPdbSourcePath = Path.Combine(customType.Assembly.Location.TokensBeforeLast(".") + ".pdb");
            if (File.Exists(customPdbSourcePath))
            {
                string customPdbDestinationPath = Path.Combine(outputFolder,
                    destinationCustomAssemblyFilename.TokensBeforeLast(".") + ".pdb");
                if (!File.Exists(customPdbDestinationPath))
                {
                    File.Copy(customPdbSourcePath, customPdbDestinationPath);
                }
            }

            return reference;

        }

        internal MetadataReference GetSharedReference(string name)
        {
            PathToSharedRoslynAsThisProcess ??= typeof(object).Assembly.Location.TokensBeforeLast(@"\");

            var systemRuntimeDll = Path.Combine(PathToSharedRoslynAsThisProcess, "System.Runtime.DLL");
            if (!File.Exists(systemRuntimeDll))
                throw new ArgumentException(nameof(name));

            var fullAssemblyPath = Path.Combine(PathToSharedRoslynAsThisProcess, name);
            if (!fullAssemblyPath.EndsWith(".dll"))
                fullAssemblyPath += ".dll";

            Console.WriteLine($"Shared: {name}  @  {fullAssemblyPath}");

            var reference = MetadataReference.CreateFromFile(fullAssemblyPath);
            return reference;
        }

        public MetadataReference GetReference(Type type)
        {
            Console.WriteLine($"{type.Assembly.FullName}  @  {type.Assembly.Location}");

            var reference = MetadataReference.CreateFromFile(type.Assembly.Location);
            return reference;
        }

        public MetadataReference? GetFrameworkReference(string targetFrameworkFolder, string assemblyFilePath)
        {
            if (!assemblyFilePath.ToLower().EndsWith(".dll") && !assemblyFilePath.ToLower().EndsWith(".exe"))
            {
                assemblyFilePath += ".dll";
            }
            var filename = Path.Combine(targetFrameworkFolder, assemblyFilePath);

            if (!File.Exists(filename))
            {
                Console.WriteLine($"Can't find framework assembly at {filename}");
                return null;
            }

            Console.WriteLine($"Adding reference to framework assembly at {filename}");
            var reference = MetadataReference.CreateFromFile(filename);
            return reference;
        }

        public int BuildCompiledAssembly()
        {
            if (CSharpCompiler == null)
                SetupCompiler();

            if (CSharpCompiler == null)
                throw new InvalidOperationException();

            using var memoryStream = new MemoryStream();
            var emitOptions = new EmitOptions(outputNameOverride: Path.GetFileName(OutputFilePath));
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
                if (OutputFilePath.IsNotEmpty())
                {
                    var directoryName = Path.GetDirectoryName(OutputFilePath);
                    if (directoryName.IsNotEmpty() && !Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName!);
                    }

                    if (File.Exists(OutputFilePath))
                    {
                        File.SetAttributes(OutputFilePath, FileAttributes.Normal);
                        File.Delete(OutputFilePath);
                    }
                    using var fileStream = File.OpenWrite(OutputFilePath);
                    {
                        fileStream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                        fileStream.Flush();
                        fileStream.Close();
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    CompiledAssembly = Assembly.LoadFile(OutputFilePath);
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