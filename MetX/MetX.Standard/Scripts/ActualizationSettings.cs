using System;
using System.Collections;
using System.IO;
using System.Reflection;
using MetX.Standard.Interfaces;
using MetX.Standard.IO;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Scripts
{
    public class ActualizationSettings
    {
        public ActualizationSettings(XlgQuickScriptTemplate quickScriptTemplate, bool simulate,
            XlgQuickScript scriptToRun, bool forExecutable, IGenerationHost host)
        {
            Host = host;
            QuickScriptTemplate = quickScriptTemplate ?? throw new ArgumentNullException(nameof(quickScriptTemplate));
            Simulate = simulate;
            Script = scriptToRun;
            ForExecutable = forExecutable;
            TemplateNameAsLegalFilenameWithoutExtension = Script.Name.AsFilename().Replace("-", " ");
            ProjectName = TemplateNameAsLegalFilenameWithoutExtension;

            var tempFolder = Environment.GetEnvironmentVariable("TEMP") ?? @"C:\Windows\Temp";
            var targetFolder = Path.Combine(tempFolder, "QuickScriptProcessors");
            OutputFolder = Path.Combine(targetFolder, TemplateNameAsLegalFilenameWithoutExtension);
            Directory.CreateDirectory(OutputFolder);

            FileSystem.CleanFolder(Path.Combine(targetFolder, "obj"));
            FileSystem.CleanFolder(Path.Combine(targetFolder, "bin"));

            GeneratedAreas = new GenInstance(scriptToRun, quickScriptTemplate, true);
            
        }

        public string ProjectName { get; set; }
        public bool Simulate { get; set; }
        public XlgQuickScriptTemplate QuickScriptTemplate { get; set; }

        public AssocArray Answers { get; set; } = new AssocArray();
        public string TemplateNameAsLegalFilenameWithoutExtension { get; set; } = Guid.NewGuid().ToString("N");

        public XlgQuickScript Script { get; set; }
        public bool ForExecutable { get; set; }
        public string OutputFolder { get; set; }
        public GenInstance GeneratedAreas { get; set; }
        public IGenerationHost Host { get; set; }
    }
}