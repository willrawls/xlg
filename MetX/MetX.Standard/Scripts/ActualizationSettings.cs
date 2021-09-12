using System;
using System.IO;
using System.Reflection;
using MetX.Standard.Library;

namespace MetX.Standard.Scripts
{
    public class ActualizationSettings
    {
        public string ProjectName { get; set; }
        public bool Simulate { get; set; }
        public XlgQuickScriptTemplate QuickScriptTemplate { get; set; }

        public AssocArray Answers { get; set; } = new ();
        public string Id { get; set; }= Guid.NewGuid().ToString("N");

        public ActualizationSettings(XlgQuickScriptTemplate quickScriptTemplate, string outputFolder, string projectName, bool simulate)
        {
            if (outputFolder == null) throw new ArgumentNullException(nameof(outputFolder));
            QuickScriptTemplate = quickScriptTemplate ?? throw new ArgumentNullException(nameof(quickScriptTemplate));
            ProjectName = projectName ?? throw new ArgumentNullException(nameof(projectName));
            Simulate = simulate;

            if (QuickScriptTemplate.TemplatePath.Contains(":")
                || QuickScriptTemplate.TemplatePath.Contains(@"\\")
                || QuickScriptTemplate.TemplatePath.StartsWith(".")) {
                // Default to current directory
                OutputFolder = Path.Combine(Environment.CurrentDirectory, "QuickScripter", Id ?? "Code", QuickScriptTemplate.Name);
            }
            else
            {
                if (string.Equals(Environment.CurrentDirectory, Assembly.GetCallingAssembly().Location, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Default to Temp
                    OutputFolder = Path.Combine(Environment.GetEnvironmentVariable("TEMP") ?? @"C:\Windows\Temp", "QuickScripter", Id ?? "Code", QuickScriptTemplate.Name);
                }
                else
                {
                    // Default to current directory
                    OutputFolder = Path.Combine(Environment.CurrentDirectory, "QuickScripter", Id ?? "Code", QuickScriptTemplate.Name);
                }
            }
        }

        public string OutputFolder { get; set; }
    }
}