using System;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings;

namespace MetX.Standard.Primary
{
    public class ContextBase
    {
        public static ContextBase Default { get; set; }

        public XlgQuickScriptFile Scripts;
        public XlgQuickScriptTemplateList Templates;
        public IGenerationHost Host;

        public ContextBase(string pathToTemplates, IGenerationHost host)
        {
            if (pathToTemplates.IsEmpty()) throw new ArgumentException(nameof(pathToTemplates));

            Host = host ?? new DoNothingGenerationHost("", this);;
            LoadTemplates(pathToTemplates);
        }

        public void LoadTemplates(string pathToTemplates = null)
        {
            if(pathToTemplates.IsEmpty())
            {
                if (Templates.TemplatesFolderPath.IsEmpty())
                    return;
                pathToTemplates = Templates.TemplatesFolderPath;
            }

            Templates = pathToTemplates.IsNotEmpty()
                ? new XlgQuickScriptTemplateList(pathToTemplates)
                : null;
        }

        public void SetPropertiesFocus(string propertyName)
        {
            // TODO
        }
    }
}