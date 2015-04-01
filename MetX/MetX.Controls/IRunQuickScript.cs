using System.Windows.Forms;
using MetX.Data;
using MetX.Interfaces;

namespace MetX.Controls
{
    public interface IRunQuickScript
    {
        void RunQuickScript(ScriptRunningWindow caller, XlgQuickScript scriptToRun, IShowText targetOutput);
        ToolWindow Window { get; set; }
    }
}