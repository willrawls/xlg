using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MetX.Data
{
    [Serializable]
    [XmlType(Namespace = "", AnonymousType = true)]
    public class XlgQuickScriptTemplateList : List<XlgQuickScriptTemplate>
    {
        public string TemplatesPath;

        public XlgQuickScriptTemplateList()
        {
            TemplatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            if (Directory.Exists(TemplatesPath))
            {
                foreach (string directory in Directory.GetDirectories(TemplatesPath))
                {
                    Add(new XlgQuickScriptTemplate(directory));
                }
            }
        }

        public XlgQuickScriptTemplate this[string name]
        {
            get
            {
                name = name.ToLower();
                foreach (XlgQuickScriptTemplate template in this)
                {
                    if (template.Name.ToLower() == name) return template;
                }
                return null;
            }
        }
    }
}