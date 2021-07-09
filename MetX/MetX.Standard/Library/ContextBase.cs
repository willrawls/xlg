using MetX.Standard.Interfaces;
using MetX.Standard.Pipelines;
using MetX.Standard.Scripts;

namespace MetX.Standard.Library
{
    public class ContextBase
    {
        public static ContextBase Default { get; set; }

        public XlgQuickScriptFile Scripts;
        public XlgQuickScriptTemplateList Templates = new();
        public IGenerationHost Host;
        
        public void SetPropertiesFocus(string propertyName)
        {
            // TODO
        }
    }
}