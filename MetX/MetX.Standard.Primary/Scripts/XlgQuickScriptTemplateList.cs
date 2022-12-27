using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MetX.Standard.Primary.Scripts
{
    [Serializable]
    [XmlType(Namespace = "", AnonymousType = true)]
    public class XlgQuickScriptTemplateList : List<XlgQuickScriptTemplate>
    {
        public string TemplatesFolderPath;
        public bool Initialized;

        public XlgQuickScriptTemplateList(string templatesFolderPath)
        {
            TemplatesFolderPath = templatesFolderPath;
            if (!Directory.Exists(TemplatesFolderPath)) return;

            foreach (var directory in Directory.GetDirectories(TemplatesFolderPath))
            {
                Add(new XlgQuickScriptTemplate(TemplatesFolderPath, directory));
            }

            Initialized = true;
        }

        public bool Contains(string name)
        {
            return this[name] != null;
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