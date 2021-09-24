using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using MetX.Standard.IO;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Scripts
{
    public class ActualizationResult
    {
        public ActualizationSettings Settings { get; set; }

        public string ActualizeErrorText { get; set; }
        public string CompileErrorText { get; set; }
        public string OutputText { get; set; }
        public AssocArray OutputFiles { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
        public string CompiledAssemblyFilePath { get; set; }

        public bool ActualizationSuccessful => ActualizeErrorText.IsEmpty();
        public bool CompileSuccessful => ActualizationSuccessful 
                                         && CompileErrorText.IsEmpty()
                                         && !OutputText.AsString().ToLower().Contains("error")
                                         && File.Exists(CompiledAssemblyFilePath);

        public ActualizationResult(ActualizationSettings settings)
        {
            Settings = settings;
        }

        public bool Compile()
        {
            if (!ActualizationSuccessful)
                return false;

            CompiledAssemblyFilePath = Path.Combine(Settings.OutputFolder, Settings.ProjectName.AsFilename() + ".exe");

            if (!FileSystem.SafeDelete(CompiledAssemblyFilePath))
            {
                CompileErrorText = $"Couldn't delete: {CompiledAssemblyFilePath}";
                return false;
            }

            OutputText = FileSystem.GatherOutputAndErrors("dotnet", "build", out var errorOutput, Settings.OutputFolder);
            CompileErrorText = errorOutput;

            return CompileSuccessful;
        }

        public BaseLineProcessor AsBaseLineProcessor()
        {
            if (!CompileSuccessful)
                return null;

            var assembly = Assembly.LoadFile(this.CompiledAssemblyFilePath);
            var typeOfBaseLineProcessorToCreate = assembly.ExportedTypes.FirstOrDefault(t => t.Name == this.FullClassNameWithNamespace());
            if (typeOfBaseLineProcessorToCreate == null)
                return null;
            
            var instanceObject = Activator.CreateInstance(typeOfBaseLineProcessorToCreate);

            var instance = instanceObject as BaseLineProcessor;
            if (instance == null)
                throw new Exception($"Cast to Actual failed: {this.FullClassNameWithNamespace()}");

            return instance;
        }

        public string FullClassNameWithNamespace()
        {
            return "QuickScriptProcessor";
        }
    }
}