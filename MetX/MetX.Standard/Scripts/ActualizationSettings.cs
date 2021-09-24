using System;
using System.Collections;
using System.IO;
using System.Reflection;
using MetX.Standard.IO;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Scripts
{
    public class ActualizationSettings
    {
        public ActualizationSettings(XlgQuickScriptTemplate quickScriptTemplate, bool simulate, XlgQuickScript scriptToRun)
        {
            QuickScriptTemplate = quickScriptTemplate ?? throw new ArgumentNullException(nameof(quickScriptTemplate));
            Simulate = simulate;
            Source = scriptToRun;
            TemplateNameAsLegalFilenameWithoutExtension = Source.Name.AsFilename(); // Source.Id.ToString("N");
            ProjectName = QuickScriptTemplate.Name ?? TemplateNameAsLegalFilenameWithoutExtension;

            var targetFolder = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "QuickScriptProcessors");
            OutputFolder = Path.Combine(targetFolder, TemplateNameAsLegalFilenameWithoutExtension);
            Directory.CreateDirectory(OutputFolder);

            FileSystem.CleanFolder(Path.Combine(targetFolder, "obj"));
            FileSystem.CleanFolder(Path.Combine(targetFolder, "bin"));

            GeneratedAreas = new GenInstance(scriptToRun, quickScriptTemplate, true);

            /*
            if (QuickScriptTemplate.TemplatePath.Contains(":")
                || QuickScriptTemplate.TemplatePath.Contains(@"\\")
                || QuickScriptTemplate.TemplatePath.StartsWith("."))
            {
                OutputFolder = Path.Combine(targetFolder, "QuickScripter", Id ?? "Code", QuickScriptTemplate.Name.AsFilename());
            }
            else
            {
                if (string.Equals(targetFolder, Assembly.GetCallingAssembly().Location,
                        StringComparison.InvariantCultureIgnoreCase))
                    // Default to Temp
                    OutputFolder = Path.Combine(Environment.GetEnvironmentVariable("TEMP") ?? @"C:\Windows\Temp",
                        "QuickScripter", Id ?? "Code", QuickScriptTemplate.Name);
                else
                    // Default to current directory
                    OutputFolder = Path.Combine(targetFolder, "QuickScripter", Id ?? "Code",
                        QuickScriptTemplate.Name);
            }
        */
        }

        public string ProjectName { get; set; }
        public bool Simulate { get; set; }
        public XlgQuickScriptTemplate QuickScriptTemplate { get; set; }

        public AssocArray Answers { get; set; } = new AssocArray();
        public string TemplateNameAsLegalFilenameWithoutExtension { get; set; } = Guid.NewGuid().ToString("N");

        public XlgQuickScript Source { get; set; }
        public string OutputFolder { get; set; }
        public GenInstance GeneratedAreas { get; set; }
    }
}