using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;

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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ScriptRunningWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(185, 168);
            this.Name = "ScriptRunningWindow";
            this.ResumeLayout(false);

        }
    }
}