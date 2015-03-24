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
            this.DeleteScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.QuickScriptList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.SliceAt = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.DiceAt = new System.Windows.Forms.ToolStripComboBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.RunScript = new System.Windows.Forms.ToolStripButton();
            this.ViewGeneratedCode = new System.Windows.Forms.ToolStripButton();
            this.ViewIndependectGeneratedCode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.InputList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.InputParam = new System.Windows.Forms.ToolStripTextBox();
            this.EditInputFilePath = new System.Windows.Forms.ToolStripButton();
            this.BrowseInputFilePath = new System.Windows.Forms.ToolStripButton();
            this.FilePathStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.TemplateList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.DestinationList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.DestinationParam = new System.Windows.Forms.ToolStripTextBox();
            this.EditDestinationFilePath = new System.Windows.Forms.ToolStripButton();
            this.BrowseDestinationFilePath = new System.Windows.Forms.ToolStripButton();
            this.OpenInputFilePathDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveDestinationFilePathDialog = new System.Windows.Forms.SaveFileDialog();
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
            this.DeleteScript,
            this.toolStripSeparator4,
            this.toolStripLabel1,
            this.QuickScriptList,
            this.toolStripSeparator7,
            this.toolStripLabel3,
            this.SliceAt,
            this.toolStripLabel4,
            this.DiceAt});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.toolStrip1.Size = new System.Drawing.Size(875, 31);
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
            // DeleteScript
            // 
            this.DeleteScript.Image = ((System.Drawing.Image)(resources.GetObject("DeleteScript.Image")));
            this.DeleteScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteScript.Name = "DeleteScript";
            this.DeleteScript.Size = new System.Drawing.Size(71, 28);
            this.DeleteScript.Text = "Delete ";
            this.DeleteScript.ToolTipText = "Delete the current quick script";
            this.DeleteScript.Click += new System.EventHandler(this.DeleteScript_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(46, 28);
            this.toolStripLabel1.Text = "Script:  ";
            // 
            // QuickScriptList
            // 
            this.QuickScriptList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.QuickScriptList.Name = "QuickScriptList";
            this.QuickScriptList.Size = new System.Drawing.Size(319, 31);
            this.QuickScriptList.SelectedIndexChanged += new System.EventHandler(this.QuickScriptList_SelectedIndexChanged);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(47, 28);
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
            this.SliceAt.Size = new System.Drawing.Size(84, 31);
            this.SliceAt.Tag = "";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(46, 28);
            this.toolStripLabel4.Text = "Dice at:";
            // 
            // DiceAt
            // 
            this.DiceAt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DiceAt.Items.AddRange(new object[] {
            "Space",
            "Tab",
            "Equal sign",
            "Pipe",
            "End of line"});
            this.DiceAt.Name = "DiceAt";
            this.DiceAt.Size = new System.Drawing.Size(92, 31);
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStrip2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunScript,
            this.ViewGeneratedCode,
            this.ViewIndependectGeneratedCode,
            this.toolStripSeparator6,
            this.toolStripLabel5,
            this.InputList,
            this.toolStripLabel6,
            this.InputParam,
            this.EditInputFilePath,
            this.BrowseInputFilePath});
            this.toolStrip2.Location = new System.Drawing.Point(0, 31);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.toolStrip2.Size = new System.Drawing.Size(875, 31);
            this.toolStrip2.TabIndex = 18;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // RunScript
            // 
            this.RunScript.Image = ((System.Drawing.Image)(resources.GetObject("RunScript.Image")));
            this.RunScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RunScript.Name = "RunScript";
            this.RunScript.Size = new System.Drawing.Size(56, 28);
            this.RunScript.Text = "&Run";
            this.RunScript.ToolTipText = "Runs the current quick script processing the Input and displaying the results in " +
                "Output. If compilation errors are found, they will be display in notepad integra" +
                "ted at the end of the appropriate line.";
            this.RunScript.Click += new System.EventHandler(this.RunQuickScript_Click);
            // 
            // ViewGeneratedCode
            // 
            this.ViewGeneratedCode.Image = ((System.Drawing.Image)(resources.GetObject("ViewGeneratedCode.Image")));
            this.ViewGeneratedCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewGeneratedCode.Name = "ViewGeneratedCode";
            this.ViewGeneratedCode.Size = new System.Drawing.Size(56, 28);
            this.ViewGeneratedCode.Text = "&Gen";
            this.ViewGeneratedCode.ToolTipText = "Generate the quick script now and open the result in notepad. Compilation errors " +
                "will not be shown.";
            this.ViewGeneratedCode.Click += new System.EventHandler(this.ViewGeneratedCode_Click);
            // 
            // ViewIndependectGeneratedCode
            // 
            this.ViewIndependectGeneratedCode.Image = ((System.Drawing.Image)(resources.GetObject("ViewIndependectGeneratedCode.Image")));
            this.ViewIndependectGeneratedCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewIndependectGeneratedCode.Name = "ViewIndependectGeneratedCode";
            this.ViewIndependectGeneratedCode.Size = new System.Drawing.Size(76, 28);
            this.ViewIndependectGeneratedCode.Text = "Gen &Exe";
            this.ViewIndependectGeneratedCode.ToolTipText = "Generates and complies an independent executable that can be run from the command" +
                " line or explorer. When run, it optionally accepts an input and output file as p" +
                "arameters.";
            this.ViewIndependectGeneratedCode.Click += new System.EventHandler(this.ViewIndependectGeneratedCode_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(47, 28);
            this.toolStripLabel5.Text = "Input:   ";
            // 
            // InputList
            // 
            this.InputList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InputList.Items.AddRange(new object[] {
            "File",
            "Clipboard",
            "Database Query",
            "Web Address",
            "None"});
            this.InputList.Name = "InputList";
            this.InputList.Size = new System.Drawing.Size(147, 31);
            this.InputList.SelectedIndexChanged += new System.EventHandler(this.InputList_SelectedIndexChanged);
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(34, 28);
            this.toolStripLabel6.Text = "Path:";
            // 
            // InputParam
            // 
            this.InputParam.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InputParam.Name = "InputParam";
            this.InputParam.Size = new System.Drawing.Size(365, 31);
            this.InputParam.ToolTipText = "When Input is File, this is the file that will be processed.";
            // 
            // EditInputFilePath
            // 
            this.EditInputFilePath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.EditInputFilePath.Image = ((System.Drawing.Image)(resources.GetObject("EditInputFilePath.Image")));
            this.EditInputFilePath.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditInputFilePath.Name = "EditInputFilePath";
            this.EditInputFilePath.Size = new System.Drawing.Size(31, 28);
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
            this.BrowseInputFilePath.Size = new System.Drawing.Size(23, 28);
            this.BrowseInputFilePath.Text = "...";
            this.BrowseInputFilePath.ToolTipText = "Click to browse for an input file";
            this.BrowseInputFilePath.Click += new System.EventHandler(this.BrowseInputFilePath_Click);
            // 
            // FilePathStrip
            // 
            this.FilePathStrip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.FilePathStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.FilePathStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.FilePathStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.FilePathStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.TemplateList,
            this.toolStripSeparator3,
            this.toolStripLabel8,
            this.DestinationList,
            this.toolStripLabel7,
            this.DestinationParam,
            this.EditDestinationFilePath,
            this.BrowseDestinationFilePath});
            this.FilePathStrip.Location = new System.Drawing.Point(0, 62);
            this.FilePathStrip.Name = "FilePathStrip";
            this.FilePathStrip.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.FilePathStrip.Size = new System.Drawing.Size(875, 25);
            this.FilePathStrip.TabIndex = 19;
            this.FilePathStrip.Text = "toolStrip3";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(60, 22);
            this.toolStripLabel2.Text = "&Template:";
            // 
            // TemplateList
            // 
            this.TemplateList.Items.AddRange(new object[] {
            "Single file input",
            "Single folder input",
            "Single folder recursive"});
            this.TemplateList.Name = "TemplateList";
            this.TemplateList.Size = new System.Drawing.Size(126, 25);
            this.TemplateList.SelectedIndexChanged += new System.EventHandler(this.TemplateList_SelectedIndexChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabel8.Text = "&Output:";
            // 
            // DestinationList
            // 
            this.DestinationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DestinationList.Items.AddRange(new object[] {
            "File",
            "Clipboard",
            "Text Box",
            "Notepad"});
            this.DestinationList.Name = "DestinationList";
            this.DestinationList.Size = new System.Drawing.Size(146, 25);
            this.DestinationList.SelectedIndexChanged += new System.EventHandler(this.DestinationList_SelectedIndexChanged);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(34, 22);
            this.toolStripLabel7.Text = "Path:";
            // 
            // DestinationParam
            // 
            this.DestinationParam.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DestinationParam.Name = "DestinationParam";
            this.DestinationParam.Size = new System.Drawing.Size(365, 25);
            this.DestinationParam.ToolTipText = "When Destination is File, this is the path where output will be (over) written.";
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
            // ScriptEditor
            // 
            this.ScriptEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ScriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScriptEditor.IsIconBarVisible = true;
            this.ScriptEditor.IsReadOnly = false;
            this.ScriptEditor.Location = new System.Drawing.Point(0, 87);
            this.ScriptEditor.Name = "ScriptEditor";
            this.ScriptEditor.Size = new System.Drawing.Size(875, 547);
            this.ScriptEditor.TabIndex = 20;
            // 
            // QuickScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(875, 634);
            this.Controls.Add(this.ScriptEditor);
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
        private System.Windows.Forms.ToolStripButton AddScript;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox QuickScriptList;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton ViewGeneratedCode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStrip FilePathStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripTextBox DestinationParam;
        private System.Windows.Forms.ToolStripButton EditDestinationFilePath;
        private System.Windows.Forms.ToolStripButton BrowseDestinationFilePath;
        private System.Windows.Forms.OpenFileDialog OpenInputFilePathDialog;
        private System.Windows.Forms.SaveFileDialog SaveDestinationFilePathDialog;
        private System.Windows.Forms.ToolStripButton ViewIndependectGeneratedCode;
        private ICSharpCode.TextEditor.TextEditorControl ScriptEditor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox SliceAt;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripComboBox DiceAt;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripComboBox InputList;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripTextBox InputParam;
        private System.Windows.Forms.ToolStripButton EditInputFilePath;
        private System.Windows.Forms.ToolStripButton BrowseInputFilePath;
        private System.Windows.Forms.ToolStripLabel toolStripLabel8;
        private System.Windows.Forms.ToolStripComboBox DestinationList;
        private System.Windows.Forms.ToolStripButton DeleteScript;
        private System.Windows.Forms.ToolStripButton RunScript;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox TemplateList;
    }
}