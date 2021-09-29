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
        public XlgQuickScriptTemplateList Templates = new(Environment.CurrentDirectory, "Templates");
        public IGenerationHost Host;

        public ContextBase(IGenerationHost host)
        {
            Host = host;
        }

        public void SetPropertiesFocus(string propertyName)
        {
            // TODO
        }
    }
}