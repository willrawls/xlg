namespace XLG.Pipeliner
{
    partial class QuickScriptEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickScriptEditor));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SaveScript = new System.Windows.Forms.ToolStripButton();
            this.AddScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.RunScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.QuickScriptList = new System.Windows.Forms.ToolStripComboBox();
            this.QuickScript = new System.Windows.Forms.TextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.ViewGeneratedCode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.Input = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.SliceAt = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.DiceAt = new System.Windows.Forms.ToolStripComboBox();
            this.FilePathStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.InputFilePath = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.DestinationFilePath = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.DestinationList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.FilePathStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveScript,
            this.AddScript,
            this.toolStripSeparator4,
            this.DeleteScript,
            this.toolStripSeparator2,
            this.RunScript,
            this.toolStripLabel1,
            this.QuickScriptList});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(794, 31);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // SaveScript
            // 
            this.SaveScript.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SaveScript.Image = ((System.Drawing.Image)(resources.GetObject("SaveScript.Image")));
            this.SaveScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveScript.Name = "SaveScript";
            this.SaveScript.Size = new System.Drawing.Size(59, 28);
            this.SaveScript.Text = "&Save";
            this.SaveScript.ToolTipText = "Save the current ClipScript";
            this.SaveScript.Click += new System.EventHandler(this.SaveQuickScript_Click);
            // 
            // AddScript
            // 
            this.AddScript.Image = ((System.Drawing.Image)(resources.GetObject("AddScript.Image")));
            this.AddScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddScript.Name = "AddScript";
            this.AddScript.Size = new System.Drawing.Size(59, 28);
            this.AddScript.Text = "&New";
            this.AddScript.ToolTipText = "Create a new ClipScript";
            this.AddScript.Click += new System.EventHandler(this.AddQuickScript_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // DeleteScript
            // 
            this.DeleteScript.Image = ((System.Drawing.Image)(resources.GetObject("DeleteScript.Image")));
            this.DeleteScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteScript.Name = "DeleteScript";
            this.DeleteScript.Size = new System.Drawing.Size(68, 28);
            this.DeleteScript.Text = "Delete";
            this.DeleteScript.Click += new System.EventHandler(this.DeleteScript_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // RunScript
            // 
            this.RunScript.Image = ((System.Drawing.Image)(resources.GetObject("RunScript.Image")));
            this.RunScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RunScript.Name = "RunScript";
            this.RunScript.Size = new System.Drawing.Size(56, 28);
            this.RunScript.Text = "&Run";
            this.RunScript.ToolTipText = "Runs the current ClipScript processing the clipboard";
            this.RunScript.Click += new System.EventHandler(this.RunQuickScript_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(40, 28);
            this.toolStripLabel1.Text = "Script:";
            // 
            // QuickScriptList
            // 
            this.QuickScriptList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.QuickScriptList.Name = "QuickScriptList";
            this.QuickScriptList.Size = new System.Drawing.Size(282, 31);
            this.QuickScriptList.SelectedIndexChanged += new System.EventHandler(this.QuickScriptList_SelectedIndexChanged);
            // 
            // QuickScript
            // 
            this.QuickScript.AcceptsReturn = true;
            this.QuickScript.AcceptsTab = true;
            this.QuickScript.AllowDrop = true;
            this.QuickScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuickScript.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuickScript.Location = new System.Drawing.Point(0, 81);
            this.QuickScript.Multiline = true;
            this.QuickScript.Name = "QuickScript";
            this.QuickScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.QuickScript.Size = new System.Drawing.Size(794, 323);
            this.QuickScript.TabIndex = 17;
            this.QuickScript.WordWrap = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewGeneratedCode,
            this.toolStripSeparator6,
            this.toolStripLabel5,
            this.Input,
            this.toolStripSeparator5,
            this.toolStripLabel3,
            this.SliceAt,
            this.toolStripSeparator7,
            this.toolStripLabel8,
            this.DestinationList,
            this.toolStripLabel4,
            this.DiceAt});
            this.toolStrip2.Location = new System.Drawing.Point(0, 31);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(794, 25);
            this.toolStrip2.TabIndex = 18;
            this.toolStrip2.Text = "toolStrip2";
            this.toolStrip2.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip2_ItemClicked);
            // 
            // ViewGeneratedCode
            // 
            this.ViewGeneratedCode.Image = ((System.Drawing.Image)(resources.GetObject("ViewGeneratedCode.Image")));
            this.ViewGeneratedCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewGeneratedCode.Name = "ViewGeneratedCode";
            this.ViewGeneratedCode.Size = new System.Drawing.Size(48, 22);
            this.ViewGeneratedCode.Text = "Gen";
            this.ViewGeneratedCode.Click += new System.EventHandler(this.ViewGeneratedCode_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(38, 22);
            this.toolStripLabel5.Text = "Input:";
            // 
            // Input
            // 
            this.Input.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Input.Items.AddRange(new object[] {
            "Clipboard",
            "File"});
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(146, 25);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(47, 22);
            this.toolStripLabel3.Text = "Slice at:";
            // 
            // SliceAt
            // 
            this.SliceAt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SliceAt.Items.AddRange(new object[] {
            "End of line",
            "Equal sign",
            "Tab",
            "Pipe",
            "Space"});
            this.SliceAt.Name = "SliceAt";
            this.SliceAt.Size = new System.Drawing.Size(84, 25);
            this.SliceAt.Tag = "";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Enabled = false;
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(46, 22);
            this.toolStripLabel4.Text = "Dice at:";
            this.toolStripLabel4.Visible = false;
            // 
            // DiceAt
            // 
            this.DiceAt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DiceAt.Enabled = false;
            this.DiceAt.Items.AddRange(new object[] {
            "Space",
            "Tab",
            "Equal sign",
            "Pipe",
            "End of line"});
            this.DiceAt.Name = "DiceAt";
            this.DiceAt.Size = new System.Drawing.Size(92, 25);
            this.DiceAt.Visible = false;
            // 
            // FilePathStrip
            // 
            this.FilePathStrip.BackColor = System.Drawing.Color.Transparent;
            this.FilePathStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.FilePathStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel6,
            this.InputFilePath,
            this.toolStripSeparator3,
            this.toolStripLabel7,
            this.DestinationFilePath});
            this.FilePathStrip.Location = new System.Drawing.Point(0, 56);
            this.FilePathStrip.Name = "FilePathStrip";
            this.FilePathStrip.Size = new System.Drawing.Size(794, 25);
            this.FilePathStrip.TabIndex = 19;
            this.FilePathStrip.Text = "toolStrip3";
            this.FilePathStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.FilePathStrip_ItemClicked);
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(92, 22);
            this.toolStripLabel6.Text = "Input File Path:  ";
            // 
            // InputFilePath
            // 
            this.InputFilePath.Name = "InputFilePath";
            this.InputFilePath.Size = new System.Drawing.Size(285, 25);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(70, 22);
            this.toolStripLabel7.Text = "     File Path:";
            // 
            // DestinationFilePath
            // 
            this.DestinationFilePath.Name = "DestinationFilePath";
            this.DestinationFilePath.Size = new System.Drawing.Size(310, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size(70, 22);
            this.toolStripLabel8.Text = "&Destination:";
            // 
            // DestinationList
            // 
            this.DestinationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DestinationList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DestinationList.Items.AddRange(new object[] {
            "Text Box",
            "Clipboard",
            "Notepad"});
            this.DestinationList.Name = "DestinationList";
            this.DestinationList.Size = new System.Drawing.Size(121, 25);
            // 
            // QuickScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(794, 404);
            this.Controls.Add(this.QuickScript);
            this.Controls.Add(this.FilePathStrip);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QuickScriptEditor";
            this.Text = "Quick Script Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QuickScriptEditor_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.FilePathStrip.ResumeLayout(false);
            this.FilePathStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton SaveScript;
        private System.Windows.Forms.ToolStripButton RunScript;
        private System.Windows.Forms.ToolStripButton AddScript;
        private System.Windows.Forms.TextBox QuickScript;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox QuickScriptList;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton ViewGeneratedCode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripComboBox Input;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox SliceAt;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripComboBox DiceAt;
        private System.Windows.Forms.ToolStripButton DeleteScript;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStrip FilePathStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripTextBox InputFilePath;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripTextBox DestinationFilePath;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel toolStripLabel8;
        private System.Windows.Forms.ToolStripComboBox DestinationList;
    }
}