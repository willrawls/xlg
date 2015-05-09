using MetX.Interfaces;
using MetX.Scripts;

namespace MetX.Controls
{
    public interface IRunQuickScript
    {
        void RunQuickScript(ScriptRunningWindow caller, XlgQuickScript scriptToRun, IShowText targetOutput);

        ToolWindow Window { get; set; }
    }
}