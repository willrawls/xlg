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
        public CaseFreeDictionary Views = new();

        public XlgQuickScriptTemplate(string templatePath)
        {
            TemplatePath = templatePath;
            Name = TemplatePath.LastPathToken();
            if (Directory.Exists(TemplatePath))
            {
                foreach (var file in Directory.GetFiles(TemplatePath, "*.cs"))
                {
                    Views.Add(file.LastPathToken().ToLower().TokensBeforeLast(".cs"), File.ReadAllText(file));
                }
            }
        }
    }
}