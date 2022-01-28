using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MetX.Standard.Scripts
{
    [Serializable]
    [XmlType(Namespace = "", AnonymousType = true)]
    public class XlgQuickScriptTemplateList : List<XlgQuickScriptTemplate>
    {
        public string TemplatesPath;

        public XlgQuickScriptTemplateList(string pathToTemplates)
        {
            TemplatesPath = pathToTemplates;
            if (!Directory.Exists(TemplatesPath)) return;

            foreach (var directory in Directory.GetDirectories(TemplatesPath))
            {
                Add(new XlgQuickScriptTemplate(directory));
            }
        }

        public XlgQuickScriptTemplate this[string name]
        {
            get
            {
                name = name.ToLower();
                foreach (var template in this)
                {
                    if (template.Name.ToLower() == name) return template;
                }
                return null;
            }
        }
    }
}