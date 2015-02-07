namespace XLG.Pipeliner
{
    partial class ClipScriptEditor
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClipScriptEditor));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SaveClipScript = new System.Windows.Forms.ToolStripButton();
            this.AddClipScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.RunClipScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ClipScriptList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.DestinationList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.ClipScriptInput = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveClipScript,
            this.AddClipScript,
            this.toolStripSeparator2,
            this.RunClipScript,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.ClipScriptList,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.DestinationList,
            this.toolStripSeparator8});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(832, 31);
            this.toolStrip1.TabIndex = 14;
            // 
            // SaveClipScript
            // 
            this.SaveClipScript.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SaveClipScript.Image = global::XLG.Pipeliner.Properties.Resources.floppy_disk_48;
            this.SaveClipScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveClipScript.Name = "SaveClipScript";
            this.SaveClipScript.Size = new System.Drawing.Size(59, 28);
            this.SaveClipScript.Text = "&Save";
            this.SaveClipScript.ToolTipText = "Save the current ClipScript";
            // 
            // AddClipScript
            // 
            this.AddClipScript.Image = global::XLG.Pipeliner.Properties.Resources.add_48;
            this.AddClipScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddClipScript.Name = "AddClipScript";
            this.AddClipScript.Size = new System.Drawing.Size(92, 28);
            this.AddClipScript.Text = "&New Script";
            this.AddClipScript.ToolTipText = "Create a new ClipScript";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // RunClipScript
            // 
            this.RunClipScript.Image = global::XLG.Pipeliner.Properties.Resources.arrow_down_48;
            this.RunClipScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RunClipScript.Name = "RunClipScript";
            this.RunClipScript.Size = new System.Drawing.Size(56, 28);
            this.RunClipScript.Text = "&Run";
            this.RunClipScript.ToolTipText = "Runs the current ClipScript processing the clipboard";
            this.RunClipScript.Click += new System.EventHandler(this.RunClipScript_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(46, 28);
            this.toolStripLabel1.Text = "Source:";
            // 
            // ClipScriptList
            // 
            this.ClipScriptList.Name = "ClipScriptList";
            this.ClipScriptList.Size = new System.Drawing.Size(200, 31);
            this.ClipScriptList.SelectedIndexChanged += new System.EventHandler(this.ClipScriptList_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(70, 28);
            this.toolStripLabel2.Text = "&Destination:";
            // 
            // DestinationList
            // 
            this.DestinationList.Items.AddRange(new object[] {
            "Text box",
            "Clipboard",
            "Notepad"});
            this.DestinationList.Name = "DestinationList";
            this.DestinationList.Size = new System.Drawing.Size(121, 31);
            this.DestinationList.Text = "Text box";
            this.DestinationList.ToolTipText = "When run, where should the ClipScript send the output?";
            this.DestinationList.SelectedIndexChanged += new System.EventHandler(this.DestinationList_SelectedIndexChanged);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 31);
            // 
            // ClipScriptInput
            // 
            this.ClipScriptInput.AcceptsReturn = true;
            this.ClipScriptInput.AcceptsTab = true;
            this.ClipScriptInput.AllowDrop = true;
            this.ClipScriptInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClipScriptInput.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClipScriptInput.Location = new System.Drawing.Point(0, 31);
            this.ClipScriptInput.Multiline = true;
            this.ClipScriptInput.Name = "ClipScriptInput";
            this.ClipScriptInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ClipScriptInput.Size = new System.Drawing.Size(832, 511);
            this.ClipScriptInput.TabIndex = 17;
            this.ClipScriptInput.Text = resources.GetString("ClipScriptInput.Text");
            this.ClipScriptInput.WordWrap = false;
            // 
            // ClipScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 542);
            this.Controls.Add(this.ClipScriptInput);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ClipScriptEditor";
            this.Text = "Clip Script Editor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton SaveClipScript;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton RunClipScript;
        private System.Windows.Forms.ToolStripButton AddClipScript;
        private System.Windows.Forms.TextBox ClipScriptInput;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox ClipScriptList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox DestinationList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}