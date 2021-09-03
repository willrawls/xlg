using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using MetX.Standard.Library;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit; //using System.DirectoryServices;
#pragma warning disable 8601
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
        public List<string> AdditionalFrameworkAssemblyNames { get; set; }
        public List<Type> AdditionalCustomTypeReferences { get; set; }
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
            List<string> additionalFrameworkAssemblyNames,
            List<Type> additionalCustomTypeReferences)
        {
            if (frameworkFolder.IsEmpty()) throw new ArgumentNullException(nameof(frameworkFolder));
            if (outputFolder.IsEmpty()) throw new ArgumentNullException(nameof(outputFolder));

            AsExecutable = asExecutable;
            FrameworkFolder = frameworkFolder;
            OutputFolder = outputFolder;
            OutputFilename = outputFilename;
            AdditionalFrameworkAssemblyNames = additionalFrameworkAssemblyNames;
            AdditionalCustomTypeReferences = additionalCustomTypeReferences;
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

            var references = new List<MetadataReference>();

            references.AddRange(new MetadataReference[]
            {
                // The loaded framework (.net standard 2.0)
                GetReference(typeof(object)),
                GetReference(typeof(Enumerable)),
                GetReference(typeof(Console)),
                GetReference(typeof(GCSettings)),
                GetReference(typeof(StreamBuilder)),
                GetReference(typeof(InMemoryCompiler<TResultType>)),
                GetReference(typeof(System.IO.File)),
                GetReference(typeof(System.Diagnostics.Process)),
                GetReference(typeof(System.ComponentModel.Component)),
            });

            references.AddRange(new MetadataReference[]
            {
                GetFrameworkReference("System.Private.CoreLib"),
                GetFrameworkReference("mscorlib"),
                GetFrameworkReference("netstandard"),
                GetFrameworkReference("System.Runtime"),
                GetFrameworkReference("System"),
                GetFrameworkReference("System.IO"),
                GetFrameworkReference("System.Data"),
                GetFrameworkReference("System.Threading.Tasks"),
                GetFrameworkReference("System.Threading.Thread"),
                GetFrameworkReference("System.Threading"),
                GetFrameworkReference("System.Text.Json"),
                GetFrameworkReference("System.Text.RegularExpressions"),
                GetFrameworkReference("System.Linq"),
                GetFrameworkReference("System.Linq.Expressions"),
                GetFrameworkReference("System.Linq.Queryable"),
                GetFrameworkReference("System.Collections"),
                GetFrameworkReference("System.Collections.Immutable"),
                GetFrameworkReference("System.Collections.NonGeneric"),
                GetFrameworkReference("System.Collections.Specialized"),
                GetFrameworkReference("System.Drawing.Primitives"),
                GetFrameworkReference("System.ComponentModel"),
                GetFrameworkReference("System.Windows"),
                GetFrameworkReference("System.Xml"),
                GetFrameworkReference("System.Xml.Serialization"),
                GetFrameworkReference("System.Xml.XPath"),
            });

            references.AddRange(new MetadataReference[]
            {
                CopyAssemblyAndGetCustomReference(OutputFolder, typeof(GenInstance)),
                CopyAssemblyAndGetCustomReference(OutputFolder, typeof(AssocArray)),
            });

            references.AddRange(DefaultCustomTypesForCompiler());

            if (AdditionalFrameworkAssemblyNames?.Count > 0)
            {
                foreach (var referenceType in AdditionalFrameworkAssemblyNames)
                    GetFrameworkReference(assemblyName);
            }

            if (AdditionalCustomTypeReferences?.Count > 0)
            {
                foreach (Type customType in AdditionalCustomTypeReferences)
                    references.Add(CopyAssemblyAndGetCustomReference(OutputFilePath, customType));
            }

            references = references.Where(r => r != null).Distinct(new ReferenceEqualityComparer()).ToList();
            references.Sort((reference, metadataReference) => string.Compare(
                reference.Display,
                metadataReference.Display,
                StringComparison.OrdinalIgnoreCase));

            var cSharpCompilationOptions = new CSharpCompilationOptions(AsExecutable
                ? OutputKind.ConsoleApplication
                : OutputKind.DynamicallyLinkedLibrary);

            
            CSharpCompiler = CSharpCompilation.Create(
                assemblyName,
                new[] { SyntaxTree },
                references,
                cSharpCompilationOptions);

            File.WriteAllText(
                Path.ChangeExtension(OutputFilePath, "runtimeconfig.json"),
                GenerateRuntimeConfig()
            );
        }

        public static List<MetadataReference> DefaultCustomTypesForCompiler()
        {
            var assemblies = new List<MetadataReference>
            {
                GetReference(typeof(InMemoryCache<>)), // MetX.Library
                GetReference(typeof(Microsoft.CSharp.CSharpCodeProvider)),
                GetReference(typeof(MetX.Standard.Library.BaseLineProcessor)),
            };
            return assemblies;
        }


        public static MetadataReference GetReference(Type type)
        {
            Console.WriteLine($"++{type.Assembly.FullName}  @  {type.Assembly.Location}");

            var reference = MetadataReference.CreateFromFile(type.Assembly.Location);
            return reference;
        }


        public static MetadataReference CopyAssemblyAndGetCustomReference(string outputFolder, Type customType)
        {
            var destinationCustomAssemblyFilename = customType.Assembly.Location.LastPathToken();
            var customAssemblyDestinationPath = Path.Combine(outputFolder, destinationCustomAssemblyFilename);
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            if (File.Exists(customAssemblyDestinationPath))
            {
                return null;
            }

            File.Copy(customType.Assembly.Location, customAssemblyDestinationPath);
            Console.WriteLine($"+Assembly {destinationCustomAssemblyFilename}");
            var reference = MetadataReference.CreateFromFile(destinationCustomAssemblyFilename);

            var customPdbSourcePath = Path.Combine(customType.Assembly.Location.TokensBeforeLast(".") + ".pdb");
            if (File.Exists(customPdbSourcePath))
            {
                var customPdbDestinationPath = Path.Combine(outputFolder,
                    destinationCustomAssemblyFilename.TokensBeforeLast(".") + ".pdb");
                if (!File.Exists(customPdbDestinationPath))
                {
                    File.Copy(customPdbSourcePath, customPdbDestinationPath);
                }
            }

            return reference;

        }

        public MetadataReference? GetFrameworkReference(string assemblyName)
        {
            var filename = OfficialFrameworkPath.GetFrameworkAssemblyPath(assemblyName);
            if (filename.IsEmpty())
            {
                Console.WriteLine($"Can't find framework assembly named {assemblyName}");
                return null;
            }

            Console.WriteLine($"+Framework: {filename}");
            var reference = MetadataReference.CreateFromFile(filename);
            return reference;
        }

        public string GenerateRuntimeConfig()
        {
            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions()
                {
                    Indented = true
                }
            ))
            {
                writer.WriteStartObject();
                    writer.WriteStartObject("runtimeOptions");
                        writer.WriteStartObject("framework");
                        writer.WriteString("name", "Microsoft.NETCore.App");
                        writer.WriteString("version", OfficialFrameworkPath.LatestVersion);
                        writer.WriteEndObject();
                    writer.WriteEndObject();
                writer.WriteEndObject();
            }

            return Encoding.UTF8.GetString(stream.ToArray());
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