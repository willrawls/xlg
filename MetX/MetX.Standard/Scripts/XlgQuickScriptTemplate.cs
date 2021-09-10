using System;
using System.IO;
using MetX.Standard.Library;

namespace MetX.Standard.Scripts
{
    [Serializable]
    public class XlgQuickScriptTemplate
    {
        public string Name;
        public string TemplatePath;
        public AssocArray Assets = new();

        public XlgQuickScriptTemplate(string templatePath)
        {
            TemplatePath = templatePath;
            Name = TemplatePath.LastPathToken();
            if (!Directory.Exists(TemplatePath)) return;

            foreach (var file in Directory.GetFiles(TemplatePath))
            {
                var name = file.LastPathToken();
                Assets[name].Value = File.ReadAllText(file);
            }
        }

        public void Actualize(Actualization actual)
        {
            Directory.CreateDirectory(actual.OutputFolder);
            foreach (var asset in Assets)
            {

            }
        }
    }

    public class Actualization
    {

    }
}