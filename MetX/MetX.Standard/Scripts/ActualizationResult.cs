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
        public AssocArray OutputFiles { get; set; } = new();
        public List<string> Warnings { get; set; } = new();

        public string ExecutableFilePath { get; set; }

        public ActualizationResult(ActualizationSettings settings)
        {
            Settings = settings;
        }

        public bool Compile()
        {
            ExecutableFilePath = Path.Combine(Settings.OutputFolder, Settings.ProjectName.AsFilename() + ".exe");

            if (!FileSystem.SafeDelete(ExecutableFilePath))
            {
                CompileErrorText = $"Couldn't delete: {ExecutableFilePath}";
                return false;
            }

            var output = FileSystem.GatherOutput("dotnet", "build", Settings.OutputFolder);
            if (output.IsNotEmpty()
                && !output.ToLower().Contains("error")
                && !File.Exists(ExecutableFilePath))
                return true;

            CompileErrorText = output;
            return false;
        }
    }
}