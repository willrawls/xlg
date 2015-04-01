using MetX.Data;
using MetX.Interfaces;

namespace MetX.Controls
{
    public class ScriptRunningWindow : ToolWindow, IRunQuickScript
    {
        public void RunQuickScript(ScriptRunningWindow caller, XlgQuickScript scriptToRun, IShowText targetOutput)
        {
            if (caller == null)
            {
                caller = this;
            }
            Context.RunQuickScript(caller, scriptToRun, targetOutput);
        }

        public ToolWindow Window { get { return this; } set { } }

        public void RunQuickScript(XlgQuickScript currentScript) { RunQuickScript(null, currentScript, null); }
    }
}