using System;
using System.IO;
using MetX.Standard.Library;

namespace MetX.Standard.Scripts
{
    public class ActualizationSettings
    {
        public XlgQuickScriptTemplate QuickScriptTemplate { get; set; }
        public AssocArray Answers { get; set; } = new AssocArray();
        public string ProjectName { get; set; }
        public bool Simulate { get; set; }

        public ActualizationSettings(XlgQuickScriptTemplate quickScriptTemplate, string projectName, bool simulate)
        {
            QuickScriptTemplate = quickScriptTemplate;
            ProjectName = projectName;
            Simulate = simulate;
        }

        public string OutputFolder
        {
            get
            {
                if (!QuickScriptTemplate.TemplatePath.Contains(":") && !QuickScriptTemplate.TemplatePath.Contains(@"\\") && !QuickScriptTemplate.TemplatePath.StartsWith("."))
                    return Path.Combine(Environment.CurrentDirectory, QuickScriptTemplate.TemplatePath, QuickScriptTemplate.Name);
                return Path.Combine(QuickScriptTemplate.TemplatePath, QuickScriptTemplate.Name);
            }
        }
    }
}