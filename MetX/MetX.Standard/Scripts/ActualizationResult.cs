using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
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
        public string ExecutableFilePath { get; set; }

        public bool ActualizationSuccessful => ActualizeErrorText.IsEmpty();
        public bool CompileSuccessful => ActualizationSuccessful 
                                         && CompileErrorText.IsEmpty()
                                         && !OutputText.AsString().ToLower().Contains("error")
                                         && File.Exists(ExecutableFilePath);

        public ActualizationResult(ActualizationSettings settings)
        {
            Settings = settings;
        }

        public bool Compile()
        {
            if (!ActualizationSuccessful)
                return false;

            ExecutableFilePath = Path.Combine(Settings.OutputFolder, Settings.ProjectName.AsFilename() + ".exe");

            if (!FileSystem.SafeDelete(ExecutableFilePath))
            {
                CompileErrorText = $"Couldn't delete: {ExecutableFilePath}";
                return false;
            }

            OutputText = FileSystem.GatherOutputAndErrors("dotnet", "build", out var errorOutput, Settings.OutputFolder);
            CompileErrorText = errorOutput;

            return CompileSuccessful;
        }
    }
}