using MetX.Data.Scripts;
using MetX.Interfaces;

namespace MetX.Controls
{
    public interface IRunQuickScript
    {
        void RunQuickScript(ScriptRunningWindow caller, XlgQuickScript scriptToRun, IShowText targetOutput);

        ToolWindow Window { get; set; }
    }
}