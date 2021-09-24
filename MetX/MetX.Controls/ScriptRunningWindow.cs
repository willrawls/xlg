using MetX.Standard.Scripts;
using MetX.Standard.Interfaces;
using MetX.Standard.Pipelines;

namespace MetX.Controls
{
    public class ScriptRunningWindow : ToolWindow, IRunQuickScript
    {
        public void RunQuickScript(ScriptRunningWindow caller, XlgQuickScript scriptToRun, IShowText targetOutput)
        {
            caller ??= this;
            Context.RunQuickScript(caller, scriptToRun, targetOutput, Host);
        }

        public ToolWindow Window { get => this; set { } }
        public void RunQuickScript(XlgQuickScript currentScript) { RunQuickScript(null, currentScript, null); }
        public virtual void Progress(int index = -1) { }
    }
}