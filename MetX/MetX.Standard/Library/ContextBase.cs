using MetX.Scripts;

namespace MetX.Library
{
    public class ContextBase
    {
        public static ContextBase Default { get; set; }

        public XlgQuickScriptFile Scripts;
        public XlgQuickScriptTemplateList Templates = new();

        public void SetPropertiesFocus(string propertyName)
        {
            // TODO
        }
    }
}