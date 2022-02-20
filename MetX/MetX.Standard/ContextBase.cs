using System;
using MetX.Standard.Interfaces;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Scripts;

namespace MetX.Standard
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

            Host = host;
            LoadTemplates(pathToTemplates);
        }

        public void LoadTemplates(string pathToTemplates = null)
        {
            if(pathToTemplates.IsEmpty())
            {
                if (Templates.TemplatesPath.IsEmpty())
                    return;
                pathToTemplates = Templates.TemplatesPath;
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