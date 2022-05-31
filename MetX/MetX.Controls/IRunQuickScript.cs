using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;

namespace MetX.Windows.Controls
{
    /// <summary>
    /// Object that runs scripts targeted at a given control
    /// </summary>
    public interface IRunQuickScript
    {
        /// <summary>
        /// Executes the supplied quick script in the provided window
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="scriptToRun"></param>
        /// <param name="targetOutput"></param>
        void RunQuickScript(ScriptRunningWindow caller, XlgQuickScript scriptToRun, IShowText targetOutput);

        /// <summary>
        /// The provided window for output
        /// </summary>
        ToolWindow Window { get; set; }
    }
}