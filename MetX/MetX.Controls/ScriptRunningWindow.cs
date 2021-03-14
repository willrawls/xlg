using MetX.Standard.Scripts;
using MetX.Standard.Interfaces;

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
        public virtual void Progress(int index = -1) { }
    }
}