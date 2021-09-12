using System;
using System.Diagnostics;
using MetX.Standard.Interfaces;
using MetX.Standard.Scripts;

namespace MetX.Standard
{
    public class ContextBase
    {
        public static ContextBase Default { get; set; }

        public XlgQuickScriptFile Scripts;
        public XlgQuickScriptTemplateList Templates = new(Environment.CurrentDirectory);
        public IGenerationHost Host;
        
        public void SetPropertiesFocus(string propertyName)
        {
            // TODO
        }
    }
}