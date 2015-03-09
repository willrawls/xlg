namespace XLG.QuickScripts
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
            this.ViewIndependectGeneratedCode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.InputList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.DestinationList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.SliceAt = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.DiceAt = new System.Windows.Forms.ToolStripComboBox();
            this.FilePathStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.InputFilePath = new System.Windows.Forms.ToolStripTextBox();
            this.EditInputFilePath = new System.Windows.Forms.ToolStripButton();
            this.BrowseInputFilePath = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.DestinationFilePath = new System.Windows.Forms.ToolStripTextBox();
            this.EditDestinationFilePath = new System.Windows.Forms.ToolStripButton();
            this.BrowseDestinationFilePath = new System.Windows.Forms.ToolStripButton();
            this.OpenInputFilePathDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveDestinationFilePathDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.ScriptEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.FilePathStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveScript,
            this.AddScript,
            this.toolStripSeparator4,
            this.DeleteScript,
            this.toolStripSeparator2,
            this.RunScript,
            this.toolStripLabel1,
            this.QuickScriptList,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.toolStrip1.Size = new System.Drawing.Size(769, 31);
            this.toolStrip1.TabIndex = 14;
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
            this.AddScript.ToolTipText = "Create a new quick script with the option to clone the current script.";
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
            this.DeleteScript.ToolTipText = "Delete the current quick script";
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
            this.RunScript.ToolTipText = "Runs the current quick script processing the clipboard";
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
            this.QuickScriptList.Size = new System.Drawing.Size(319, 31);
            this.QuickScriptList.SelectedIndexChanged += new System.EventHandler(this.QuickScriptList_SelectedIndexChanged);
            // 
            // QuickScript
            // 
            this.QuickScript.AcceptsReturn = true;
            this.QuickScript.AcceptsTab = true;
            this.QuickScript.AllowDrop = true;
            this.QuickScript.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuickScript.Location = new System.Drawing.Point(625, 223);
            this.QuickScript.Multiline = true;
            this.QuickScript.Name = "QuickScript";
            this.QuickScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.QuickScript.Size = new System.Drawing.Size(132, 177);
            this.QuickScript.TabIndex = 17;
            this.QuickScript.WordWrap = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStrip2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewGeneratedCode,
            this.ViewIndependectGeneratedCode,
            this.toolStripSeparator6,
            this.toolStripLabel5,
            this.InputList,
            this.toolStripSeparator5,
            this.toolStripLabel8,
            this.DestinationList,
            this.toolStripLabel3,
            this.SliceAt,
            this.toolStripSeparator7,
            this.toolStripLabel4,
            this.DiceAt});
            this.toolStrip2.Location = new System.Drawing.Point(0, 31);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.toolStrip2.Size = new System.Drawing.Size(769, 25);
            this.toolStrip2.TabIndex = 18;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // ViewGeneratedCode
            // 
            this.ViewGeneratedCode.Image = ((System.Drawing.Image)(resources.GetObject("ViewGeneratedCode.Image")));
            this.ViewGeneratedCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewGeneratedCode.Name = "ViewGeneratedCode";
            this.ViewGeneratedCode.Size = new System.Drawing.Size(48, 22);
            this.ViewGeneratedCode.Text = "Gen";
            this.ViewGeneratedCode.ToolTipText = "Generate the quick script now and open the result in notepad. Errors will not be " +
    "shown.";
            this.ViewGeneratedCode.Click += new System.EventHandler(this.ViewGeneratedCode_Click);
            // 
            // ViewIndependectGeneratedCode
            // 
            this.ViewIndependectGeneratedCode.Image = ((System.Drawing.Image)(resources.GetObject("ViewIndependectGeneratedCode.Image")));
            this.ViewIndependectGeneratedCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewIndependectGeneratedCode.Name = "ViewIndependectGeneratedCode";
            this.ViewIndependectGeneratedCode.Size = new System.Drawing.Size(68, 22);
            this.ViewIndependectGeneratedCode.Text = "Gen &Exe";
            this.ViewIndependectGeneratedCode.Click += new System.EventHandler(this.ViewIndependectGeneratedCode_Click);
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
            // InputList
            // 
            this.InputList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InputList.Items.AddRange(new object[] {
            "Clipboard",
            "File"});
            this.InputList.Name = "InputList";
            this.InputList.Size = new System.Drawing.Size(146, 25);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size(45, 22);
            this.toolStripLabel8.Text = "&Output";
            // 
            // DestinationList
            // 
            this.DestinationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DestinationList.Items.AddRange(new object[] {
            "Text Box",
            "Clipboard",
            "Notepad",
            "File"});
            this.DestinationList.Name = "DestinationList";
            this.DestinationList.Size = new System.Drawing.Size(121, 25);
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
            this.SliceAt.Enabled = false;
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
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
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
            this.FilePathStrip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.FilePathStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.FilePathStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.FilePathStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel6,
            this.InputFilePath,
            this.EditInputFilePath,
            this.BrowseInputFilePath,
            this.toolStripSeparator3,
            this.toolStripLabel7,
            this.DestinationFilePath,
            this.EditDestinationFilePath,
            this.BrowseDestinationFilePath});
            this.FilePathStrip.Location = new System.Drawing.Point(0, 56);
            this.FilePathStrip.Name = "FilePathStrip";
            this.FilePathStrip.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.FilePathStrip.Size = new System.Drawing.Size(769, 25);
            this.FilePathStrip.TabIndex = 19;
            this.FilePathStrip.Text = "toolStrip3";
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(92, 22);
            this.toolStripLabel6.Text = "Input File Path:  ";
            // 
            // InputFilePath
            // 
            this.InputFilePath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InputFilePath.Name = "InputFilePath";
            this.InputFilePath.Size = new System.Drawing.Size(231, 25);
            this.InputFilePath.ToolTipText = "When Input is File, this is the file that will be processed.";
            // 
            // EditInputFilePath
            // 
            this.EditInputFilePath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.EditInputFilePath.Image = ((System.Drawing.Image)(resources.GetObject("EditInputFilePath.Image")));
            this.EditInputFilePath.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditInputFilePath.Name = "EditInputFilePath";
            this.EditInputFilePath.Size = new System.Drawing.Size(31, 22);
            this.EditInputFilePath.Text = "Edit";
            this.EditInputFilePath.ToolTipText = "Click to open the input file in notepad";
            this.EditInputFilePath.Click += new System.EventHandler(this.EditInputFilePath_Click);
            // 
            // BrowseInputFilePath
            // 
            this.BrowseInputFilePath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BrowseInputFilePath.Image = ((System.Drawing.Image)(resources.GetObject("BrowseInputFilePath.Image")));
            this.BrowseInputFilePath.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BrowseInputFilePath.Name = "BrowseInputFilePath";
            this.BrowseInputFilePath.Size = new System.Drawing.Size(23, 22);
            this.BrowseInputFilePath.Text = "...";
            this.BrowseInputFilePath.ToolTipText = "Click to browse for an input file";
            this.BrowseInputFilePath.Click += new System.EventHandler(this.BrowseInputFilePath_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(70, 22);
            this.toolStripLabel7.Text = "     File Path:";
            // 
            // DestinationFilePath
            // 
            this.DestinationFilePath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DestinationFilePath.Name = "DestinationFilePath";
            this.DestinationFilePath.Size = new System.Drawing.Size(250, 25);
            this.DestinationFilePath.ToolTipText = "When Destination is File, this is the path where output will be (over) written.";
            // 
            // EditDestinationFilePath
            // 
            this.EditDestinationFilePath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.EditDestinationFilePath.Image = ((System.Drawing.Image)(resources.GetObject("EditDestinationFilePath.Image")));
            this.EditDestinationFilePath.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditDestinationFilePath.Name = "EditDestinationFilePath";
            this.EditDestinationFilePath.Size = new System.Drawing.Size(31, 22);
            this.EditDestinationFilePath.Text = "Edit";
            this.EditDestinationFilePath.ToolTipText = "Click to open the destination file in notepad";
            this.EditDestinationFilePath.Click += new System.EventHandler(this.EditDestinationFilePath_Click);
            // 
            // BrowseDestinationFilePath
            // 
            this.BrowseDestinationFilePath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BrowseDestinationFilePath.Image = ((System.Drawing.Image)(resources.GetObject("BrowseDestinationFilePath.Image")));
            this.BrowseDestinationFilePath.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BrowseDestinationFilePath.Name = "BrowseDestinationFilePath";
            this.BrowseDestinationFilePath.Size = new System.Drawing.Size(23, 22);
            this.BrowseDestinationFilePath.Text = "...";
            this.BrowseDestinationFilePath.ToolTipText = "Click to browse for an output file path (destination).";
            this.BrowseDestinationFilePath.Click += new System.EventHandler(this.BrowseDestinationFilePath_Click);
            // 
            // OpenInputFilePathDialog
            // 
            this.OpenInputFilePathDialog.FileName = "openFileDialog1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // ScriptEditor
            // 
            this.ScriptEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ScriptEditor.IsIconBarVisible = true;
            this.ScriptEditor.IsReadOnly = false;
            this.ScriptEditor.Location = new System.Drawing.Point(0, 84);
            this.ScriptEditor.Name = "ScriptEditor";
            this.ScriptEditor.Size = new System.Drawing.Size(619, 324);
            this.ScriptEditor.TabIndex = 20;
            // 
            // QuickScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(769, 412);
            this.Controls.Add(this.ScriptEditor);
            this.Controls.Add(this.QuickScript);
            this.Controls.Add(this.FilePathStrip);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(785, 450);
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
        private System.Windows.Forms.ToolStripComboBox InputList;
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
        private System.Windows.Forms.ToolStripButton EditInputFilePath;
        private System.Windows.Forms.ToolStripButton BrowseInputFilePath;
        private System.Windows.Forms.ToolStripButton EditDestinationFilePath;
        private System.Windows.Forms.ToolStripButton BrowseDestinationFilePath;
        private System.Windows.Forms.OpenFileDialog OpenInputFilePathDialog;
        private System.Windows.Forms.SaveFileDialog SaveDestinationFilePathDialog;
        private System.Windows.Forms.ToolStripButton ViewIndependectGeneratedCode;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private ICSharpCode.TextEditor.TextEditorControl ScriptEditor;
    }
}