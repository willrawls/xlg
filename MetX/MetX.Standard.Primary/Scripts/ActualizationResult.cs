using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Primary.IO;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

namespace MetX.Standard.Primary.Scripts
{
    public class ActualizationResult
    {
        public ActualizationSettings Settings { get; set; }

        public string ActualizeErrorText { get; set; }
        public string CompileErrorText { get; set; }
        public string OutputText { get; set; }
        public AssocArray OutputFiles { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
        public string DestinationExecutableFilePath { get; set; }

        public bool ActualizationSuccessful => ActualizeErrorText.IsEmpty();
        public bool CompileSuccessful => Settings.Simulate || 
                                         (ActualizationSuccessful 
                                          && CompileErrorText.IsEmpty()
                                          && OutputText.AsString().Contains(" 0 Error(s)")
                                          && File.Exists(DestinationExecutableFilePath));

        public ActualizationResult(ActualizationSettings settings)
        {
            Settings = settings;
        }

        public bool Compile()
        {
            if (!ActualizationSuccessful)
                return false;

            if (!FileSystem.SafelyDeleteFile(DestinationExecutableFilePath))
            {
                CompileErrorText = $"Couldn't delete: {DestinationExecutableFilePath}";
                return false;
            }

            var csprojFilePath = Directory.GetFiles(Settings.ProjectFolder).FirstOrDefault(f => f.EndsWith(".csproj"));
            OutputText = FileSystem.GatherOutputAndErrors("dotnet", $"build \"{csprojFilePath}\"", out var errorOutput, Settings.ProjectFolder, 60, ProcessWindowStyle.Hidden);
            CompileErrorText = errorOutput;

            return CompileSuccessful;
        }

        public string FullClassNameWithNamespace()
        {
            return "QuickScriptProcessor";
        }

        public string FinalDetails(out List<int> keyLines)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("-----[ Output Folder ]-----");
            sb.AppendLine($"{Settings.ProjectFolder}");

            if (CompileErrorText.IsEmpty() && !OutputText.Contains(": error "))
            {
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("-----[ SUCCESS! ]-----");
                sb.AppendLine();
            }
            else if(CompileErrorText.IsNotEmpty())
            {
                sb.AppendLine();
                sb.AppendLine("-----[ Compilation failure ]-----");
                sb.AppendLine();
                sb.AppendLine(CompileErrorText);
            }   
            
            sb.AppendLine();
            sb.AppendLine("-----[ Output from dotnet.exe ]-----");
            sb.AppendLine();

            var projectFolder = Settings.ProjectFolder;
            if (projectFolder != @"\")
                projectFolder += @"\";
            var massagedOutputText = OutputText
                    .Replace($"[{this.Settings.ProjectFilePath}]", "")
                    .Replace(projectFolder, "...")
                    ;

            var nonWarningLines = massagedOutputText.LineList().Where(l => !l.ToLower().Contains("warning")).ToArray();
            foreach(var nonWarningLine in nonWarningLines)
            {
                if (!nonWarningLine.Contains("Microsoft (R) Build Engine") &&
                    !nonWarningLine.Contains("Copyright (C) Microsoft Corporation") &&
                    !nonWarningLine.Contains("Determining projects to") &&
                    !nonWarningLine.StartsWith("  Restored "))
                {
                    if (nonWarningLine.Contains("Error(s)")
                        || nonWarningLine.StartsWith("..."))
                        sb.AppendLine();
                    
                    sb.AppendLine(nonWarningLine);
                }
                else
                {
                    sb.AppendLine();
                }
            }
            sb.AppendLine();

            keyLines = new List<int>();
            var finalDetails = sb.ToString();
            var finalDetailLines = finalDetails.Lines();
            foreach (var finalDetailLine in finalDetailLines)
            {
                if (!finalDetailLine.Contains("QuickScriptProcessor.cs")) continue;

                var lineNumberText = finalDetailLine.TokenBetween("(", ")");
                if (lineNumberText.Contains(","))
                    lineNumberText = lineNumberText.FirstToken(",");
                if (!int.TryParse(lineNumberText, out var lineNumber)) continue;

                if(keyLines.All(x => x != lineNumber))
                    keyLines.Add(lineNumber);
            }

            return finalDetails;
        }
    }
}