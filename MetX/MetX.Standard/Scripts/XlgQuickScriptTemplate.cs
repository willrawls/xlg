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
            if (!Directory.Exists(TemplatePath)) return;

            foreach (var file in Directory.GetFiles(TemplatePath))
            {
                var name = file.LastPathToken().ToLower();
                if (name.EndsWith(".cs"))
                    name = name.TokensBeforeLast(".cs") + "__cs";

                Views.Add(name, File.ReadAllText(file));
            }
        }
    }
}