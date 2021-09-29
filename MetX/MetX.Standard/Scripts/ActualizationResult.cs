using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
        public string DestinationExecutableFilePath { get; set; }

        public bool ActualizationSuccessful => ActualizeErrorText.IsEmpty();
        public bool CompileSuccessful => ActualizationSuccessful 
                                         && CompileErrorText.IsEmpty()
                                         && OutputText.AsString().Contains(" 0 Error(s)")
                                         && File.Exists(DestinationExecutableFilePath);

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

            var csprojFilePath = Directory.GetFiles(Settings.OutputFolder).FirstOrDefault(f => f.EndsWith(".csproj"));
            OutputText = FileSystem.GatherOutputAndErrors("dotnet", $"build \"{csprojFilePath}\"", out var errorOutput, Settings.OutputFolder, 60, ProcessWindowStyle.Hidden);
            CompileErrorText = errorOutput;

            return CompileSuccessful;
        }

        public BaseLineProcessor AsBaseLineProcessor()
        {
            throw new NotSupportedException();

            if (!CompileSuccessful || !File.Exists(this.DestinationExecutableFilePath))
                return null;

            var assembly = Assembly.LoadFile(this.DestinationExecutableFilePath);
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

        public string FinalDetails()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("-----[ Output Folder ]-----");
            sb.AppendLine($"{Settings.OutputFolder}");

            if (CompileErrorText.IsEmpty() && !OutputText.Contains(": error "))
            {
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("-----[ SUCCESS! ]-----");
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine();
                sb.AppendLine("-----[ Compilation failure ]-----");
                sb.AppendLine();
                sb.AppendLine(CompileErrorText);
            }   
            
            sb.AppendLine();
            sb.AppendLine("-----[ Output from dotnet.exe ]-----");
            sb.AppendLine();
            var nonWarningLines = OutputText.LineList().Where(l => !l.ToLower().Contains("warning")).ToArray();
            sb.AppendLine(string.Join("\n", nonWarningLines));
            sb.AppendLine();
            return sb.ToString();
        }
    }
}