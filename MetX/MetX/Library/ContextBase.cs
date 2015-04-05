using MetX.Data.Scripts;

namespace MetX.Library
{
    public class ContextBase
    {
        public static ContextBase Default { get; set; }

        public XlgQuickScriptFile Scripts;
        public XlgQuickScriptTemplateList Templates = new XlgQuickScriptTemplateList();

        public void SetPropertiesFocus(string propertyName)
        {
            // TODO
        }
    }
}