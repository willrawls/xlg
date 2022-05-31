namespace MetX.Windows.Controls
{
    partial class QuickScriptControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // textAreaPanel
            // 
            this.textAreaPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textAreaPanel.Size = new System.Drawing.Size(640, 522);
            // 
            // QuickScriptControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "QuickScriptControl";
            this.Size = new System.Drawing.Size(640, 522);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.QuickScriptControl_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.QuickScriptControl_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
