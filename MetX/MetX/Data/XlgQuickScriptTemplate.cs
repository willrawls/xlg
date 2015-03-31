using System;
using System.IO;
using MetX.Library;

namespace MetX.Data
{
    [Serializable]
    public class XlgQuickScriptTemplate
    {
        public string Name;
        public string TemplatePath;
        public CaseFreeDictionary Views = new CaseFreeDictionary();

        public XlgQuickScriptTemplate(string templatePath)
        {
            TemplatePath = templatePath;
            Name = TemplatePath.LastPathToken();
            if (Directory.Exists(TemplatePath))
            {
                foreach (string file in Directory.GetFiles(TemplatePath, "*.cs"))
                {
                    Views.Add(file.LastPathToken().ToLower().TokensBeforeLast(".cs"), File.ReadAllText(file));
                }
            }
        }
    }
}