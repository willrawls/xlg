using MetX.Controls;

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
            this.InputOptions = new System.Windows.Forms.ToolStrip();
            this.InputLabel = new System.Windows.Forms.ToolStripLabel();
            this.InputList = new System.Windows.Forms.ToolStripComboBox();
            this.InputPathLabel = new System.Windows.Forms.ToolStripLabel();
            this.InputParam = new System.Windows.Forms.ToolStripTextBox();
            this.EditInputFilePath = new System.Windows.Forms.ToolStripButton();
            this.BrowseInputFilePath = new System.Windows.Forms.ToolStripButton();
            this.OutputOptions = new System.Windows.Forms.ToolStrip();
            this.OutputLabel = new System.Windows.Forms.ToolStripLabel();
            this.DestinationList = new System.Windows.Forms.ToolStripComboBox();
            this.OutputPathLabel = new System.Windows.Forms.ToolStripLabel();
            this.DestinationParam = new System.Windows.Forms.ToolStripTextBox();
            this.EditDestinationFilePath = new System.Windows.Forms.ToolStripButton();
            this.BrowseDestinationFilePath = new System.Windows.Forms.ToolStripButton();
            this.OpenInputFilePathDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveDestinationFilePathDialog = new System.Windows.Forms.SaveFileDialog();
            this.MainStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.QuickScriptList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.ShowInputOutputOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.TemplateList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.SliceAt = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.DiceAt = new System.Windows.Forms.ToolStripComboBox();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.NewScriptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewCodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RunScriptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BuildExeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteScriptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.testFuncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.RunningLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ScriptEditor = new MetX.Controls.QuickScriptControl();
            this.InputOptions.SuspendLayout();
            this.OutputOptions.SuspendLayout();
            this.MainStrip.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // InputOptions
            // 
            this.InputOptions.AutoSize = false;
            this.InputOptions.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.InputOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.InputOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.InputOptions.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.InputOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InputLabel,
            this.InputList,
            this.InputPathLabel,
            this.InputParam,
            this.EditInputFilePath,
            this.BrowseInputFilePath});
            this.InputOptions.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.InputOptions.Location = new System.Drawing.Point(0, 63);
            this.InputOptions.Name = "InputOptions";
            this.InputOptions.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.InputOptions.Size = new System.Drawing.Size(827, 27);
            this.InputOptions.TabIndex = 2;
            this.InputOptions.Text = "toolStrip2";
            // 
            // InputLabel
            // 
            this.InputLabel.AutoSize = false;
            this.InputLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.InputLabel.Name = "InputLabel";
            this.InputLabel.Size = new System.Drawing.Size(50, 22);
            this.InputLabel.Text = "&Input:";
            this.InputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InputList
            // 
            this.InputList.AutoSize = false;
            this.InputList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InputList.Items.AddRange(new object[] {
            "File",
            "Clipboard",
            "Database Query",
            "Web Address",
            "None"});
            this.InputList.Name = "InputList";
            this.InputList.Size = new System.Drawing.Size(126, 28);
            this.InputList.SelectedIndexChanged += new System.EventHandler(this.InputList_SelectedIndexChanged);
            // 
            // InputPathLabel
            // 
            this.InputPathLabel.AutoSize = false;
            this.InputPathLabel.Name = "InputPathLabel";
            this.InputPathLabel.Size = new System.Drawing.Size(47, 22);
            this.InputPathLabel.Text = "&Path:";
            this.InputPathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InputParam
            // 
            this.InputParam.AutoSize = false;
            this.InputParam.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InputParam.Name = "InputParam";
            this.InputParam.Size = new System.Drawing.Size(450, 25);
            this.InputParam.ToolTipText = "When Input is File, this is the file that will be processed.";
            this.InputParam.Enter += new System.EventHandler(this.InputParam_Enter);
            this.InputParam.Leave += new System.EventHandler(this.InputParam_Leave);
            this.InputParam.MouseUp += new System.Windows.Forms.MouseEventHandler(this.InputParam_MouseUp);
            // 
            // EditInputFilePath
            // 
            this.EditInputFilePath.AutoSize = false;
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
            this.BrowseInputFilePath.AutoSize = false;
            this.BrowseInputFilePath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BrowseInputFilePath.Image = ((System.Drawing.Image)(resources.GetObject("BrowseInputFilePath.Image")));
            this.BrowseInputFilePath.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BrowseInputFilePath.Name = "BrowseInputFilePath";
            this.BrowseInputFilePath.Size = new System.Drawing.Size(23, 22);
            this.BrowseInputFilePath.Text = "...";
            this.BrowseInputFilePath.ToolTipText = "Click to browse for an input file";
            this.BrowseInputFilePath.Click += new System.EventHandler(this.BrowseInputFilePath_Click);
            // 
            // OutputOutputs
            // 
            this.OutputOptions.AutoSize = false;
            this.OutputOptions.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.OutputOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.OutputOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.OutputOptions.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.OutputOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OutputLabel,
            this.DestinationList,
            this.OutputPathLabel,
            this.DestinationParam,
            this.EditDestinationFilePath,
            this.BrowseDestinationFilePath});
            this.OutputOptions.Location = new System.Drawing.Point(0, 90);
            this.OutputOptions.Name = "OutputOptions";
            this.OutputOptions.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.OutputOptions.Size = new System.Drawing.Size(827, 26);
            this.OutputOptions.TabIndex = 3;
            this.OutputOptions.Text = "toolStrip3";
            // 
            // OutputLabel
            // 
            this.OutputLabel.AutoSize = false;
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(50, 22);
            this.OutputLabel.Text = "&Output:";
            this.OutputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DestinationList
            // 
            this.DestinationList.AutoSize = false;
            this.DestinationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DestinationList.Items.AddRange(new object[] {
            "File",
            "Clipboard",
            "Text Box",
            "Notepad"});
            this.DestinationList.Name = "DestinationList";
            this.DestinationList.Size = new System.Drawing.Size(126, 28);
            this.DestinationList.SelectedIndexChanged += new System.EventHandler(this.DestinationList_SelectedIndexChanged);
            // 
            // OutputPathLabel
            // 
            this.OutputPathLabel.AutoSize = false;
            this.OutputPathLabel.Name = "OutputPathLabel";
            this.OutputPathLabel.Size = new System.Drawing.Size(47, 22);
            this.OutputPathLabel.Text = "P&ath:";
            this.OutputPathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DestinationParam
            // 
            this.DestinationParam.AutoSize = false;
            this.DestinationParam.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DestinationParam.Name = "DestinationParam";
            this.DestinationParam.Size = new System.Drawing.Size(450, 25);
            this.DestinationParam.ToolTipText = "When Destination is File, this is the path where output will be (over) written.";
            this.DestinationParam.Enter += new System.EventHandler(this.DestinationParam_Enter);
            this.DestinationParam.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DestinationParam_MouseUp);
            // 
            // EditDestinationFilePath
            // 
            this.EditDestinationFilePath.AutoSize = false;
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
            this.BrowseDestinationFilePath.AutoSize = false;
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
            // MainStrip
            // 
            this.MainStrip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.MainStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MainStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.QuickScriptList,
            this.toolStripDropDownButton1});
            this.MainStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.MainStrip.Location = new System.Drawing.Point(0, 32);
            this.MainStrip.Name = "MainStrip";
            this.MainStrip.Size = new System.Drawing.Size(827, 31);
            this.MainStrip.TabIndex = 1;
            this.MainStrip.Text = "toolStrip3";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.AutoSize = false;
            this.toolStripLabel1.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(50, 24);
            this.toolStripLabel1.Text = "&Script:";
            this.toolStripLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // QuickScriptList
            // 
            this.QuickScriptList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.QuickScriptList.Name = "QuickScriptList";
            this.QuickScriptList.Size = new System.Drawing.Size(300, 31);
            this.QuickScriptList.SelectedIndexChanged += new System.EventHandler(this.QuickScriptList_SelectedIndexChanged);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowInputOutputOptions,
            this.toolStripSeparator7,
            this.toolStripLabel2,
            this.TemplateList,
            this.toolStripSeparator10,
            this.toolStripLabel3,
            this.SliceAt,
            this.toolStripSeparator11,
            this.toolStripLabel4,
            this.DiceAt});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(37, 28);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Click += new System.EventHandler(this.toolStripDropDownButton1_Click);
            // 
            // ShowInputOutputOptions
            // 
            this.ShowInputOutputOptions.Checked = true;
            this.ShowInputOutputOptions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowInputOutputOptions.Name = "ShowInputOutputOptions";
            this.ShowInputOutputOptions.Size = new System.Drawing.Size(258, 24);
            this.ShowInputOutputOptions.Text = "Sho&w Input/Output options";
            this.ShowInputOutputOptions.Click += new System.EventHandler(this.ShowInputOutputOptions_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(255, 6);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabel2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(62, 24);
            this.toolStripLabel2.Text = "&TemplateName:";
            this.toolStripLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TemplateList
            // 
            this.TemplateList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TemplateList.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.TemplateList.Items.AddRange(new object[] {
            "Single file input",
            "Single folder input",
            "Single folder recursive"});
            this.TemplateList.Name = "TemplateList";
            this.TemplateList.Size = new System.Drawing.Size(130, 28);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(255, 6);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(60, 20);
            this.toolStripLabel3.Text = "&Slice at:";
            // 
            // SliceAt
            // 
            this.SliceAt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SliceAt.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.SliceAt.Items.AddRange(new object[] {
            "End of line",
            "Equal sign",
            "Tab",
            "Pipe",
            "Space"});
            this.SliceAt.Name = "SliceAt";
            this.SliceAt.Size = new System.Drawing.Size(130, 28);
            this.SliceAt.Tag = "";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(255, 6);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(59, 20);
            this.toolStripLabel4.Text = "&Dice at:";
            // 
            // DiceAt
            // 
            this.DiceAt.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.DiceAt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DiceAt.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.DiceAt.Items.AddRange(new object[] {
            "Space",
            "Tab",
            "Equal sign",
            "Pipe",
            "End of line"});
            this.DiceAt.Name = "DiceAt";
            this.DiceAt.Size = new System.Drawing.Size(130, 28);
            // 
            // MainMenu
            // 
            this.MainMenu.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.testFuncToolStripMenuItem,
            this.findMenuItem,
            this.replaceMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(827, 32);
            this.MainMenu.TabIndex = 22;
            this.MainMenu.Text = "menuStrip1";
            this.MainMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MainMenu_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 28);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.newToolStripMenuItem.Text = "&New scripts file";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.openToolStripMenuItem.Text = "&Open scripts file";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(235, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveQuickScript_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(235, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(238, 24);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewScriptMenuItem,
            this.ViewCodeMenuItem,
            this.RunScriptMenuItem,
            this.BuildExeMenuItem,
            this.toolStripSeparator8,
            this.DeleteScriptMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItem1.Size = new System.Drawing.Size(65, 28);
            this.toolStripMenuItem1.Text = "Scrip&ts";
            // 
            // NewScriptMenuItem
            // 
            this.NewScriptMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("NewScriptMenuItem.Image")));
            this.NewScriptMenuItem.Name = "NewScriptMenuItem";
            this.NewScriptMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.NewScriptMenuItem.Size = new System.Drawing.Size(334, 24);
            this.NewScriptMenuItem.Text = "Add &new script";
            this.NewScriptMenuItem.Click += new System.EventHandler(this.NewQuickScript_Click);
            // 
            // ViewCodeMenuItem
            // 
            this.ViewCodeMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ViewCodeMenuItem.Image")));
            this.ViewCodeMenuItem.Name = "ViewCodeMenuItem";
            this.ViewCodeMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.ViewCodeMenuItem.Size = new System.Drawing.Size(334, 24);
            this.ViewCodeMenuItem.Text = "&View generated code";
            this.ViewCodeMenuItem.Click += new System.EventHandler(this.ViewGeneratedCode_Click);
            // 
            // RunScriptMenuItem
            // 
            this.RunScriptMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("RunScriptMenuItem.Image")));
            this.RunScriptMenuItem.Name = "RunScriptMenuItem";
            this.RunScriptMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.RunScriptMenuItem.Size = new System.Drawing.Size(334, 24);
            this.RunScriptMenuItem.Text = "&Run current script";
            this.RunScriptMenuItem.Click += new System.EventHandler(this.RunQuickScript_Click);
            // 
            // BuildExeMenuItem
            // 
            this.BuildExeMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("BuildExeMenuItem.Image")));
            this.BuildExeMenuItem.Name = "BuildExeMenuItem";
            this.BuildExeMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.BuildExeMenuItem.Size = new System.Drawing.Size(334, 24);
            this.BuildExeMenuItem.Text = "Build command line &executable";
            this.BuildExeMenuItem.Click += new System.EventHandler(this.ViewIndependentGeneratedCode_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(331, 6);
            // 
            // DeleteScriptMenuItem
            // 
            this.DeleteScriptMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("DeleteScriptMenuItem.Image")));
            this.DeleteScriptMenuItem.Name = "DeleteScriptMenuItem";
            this.DeleteScriptMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.DeleteScriptMenuItem.Size = new System.Drawing.Size(334, 24);
            this.DeleteScriptMenuItem.Text = "Delete current script";
            this.DeleteScriptMenuItem.Click += new System.EventHandler(this.DeleteScript_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem2.Image")));
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(75, 28);
            this.toolStripMenuItem2.Text = "&New";
            this.toolStripMenuItem2.ToolTipText = "Add a new quick script (with the option to clone)";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.NewQuickScript_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem3.Image")));
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(71, 28);
            this.toolStripMenuItem3.Text = "&Gen";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.ViewGeneratedCode_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem4.Image")));
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(70, 28);
            this.toolStripMenuItem4.Text = "&Run";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.RunQuickScript_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem5.Image")));
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(79, 28);
            this.toolStripMenuItem5.Text = "&Build";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.ViewIndependentGeneratedCode_Click);
            // 
            // testFuncToolStripMenuItem
            // 
            this.testFuncToolStripMenuItem.Name = "testFuncToolStripMenuItem";
            this.testFuncToolStripMenuItem.Size = new System.Drawing.Size(79, 28);
            this.testFuncToolStripMenuItem.Text = "Test func";
            this.testFuncToolStripMenuItem.Visible = false;
            this.testFuncToolStripMenuItem.Click += new System.EventHandler(this.testFuncToolStripMenuItem_Click);
            // 
            // findMenuItem
            // 
            this.findMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("findMenuItem.Image")));
            this.findMenuItem.Name = "findMenuItem";
            this.findMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findMenuItem.Size = new System.Drawing.Size(73, 28);
            this.findMenuItem.Text = "Find";
            this.findMenuItem.Click += new System.EventHandler(this.findMenuItem_Click);
            // 
            // replaceMenuItem
            // 
            this.replaceMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("replaceMenuItem.Image")));
            this.replaceMenuItem.Name = "replaceMenuItem";
            this.replaceMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceMenuItem.Size = new System.Drawing.Size(98, 28);
            this.replaceMenuItem.Text = "Replace";
            this.replaceMenuItem.Click += new System.EventHandler(this.replaceMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunningLabel,
            this.toolStripStatusLabel2,
            this.ProgressLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 621);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(827, 25);
            this.statusStrip1.TabIndex = 23;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // RunningLabel
            // 
            this.RunningLabel.Name = "RunningLabel";
            this.RunningLabel.Size = new System.Drawing.Size(88, 20);
            this.RunningLabel.Text = "Not running";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(21, 20);
            this.toolStripStatusLabel2.Text = " | ";
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(17, 20);
            this.ProgressLabel.Text = "0";
            // 
            // ScriptEditor
            // 
            this.ScriptEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ScriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScriptEditor.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ScriptEditor.IsIconBarVisible = true;
            this.ScriptEditor.IsReadOnly = false;
            this.ScriptEditor.Location = new System.Drawing.Point(0, 116);
            this.ScriptEditor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ScriptEditor.Name = "ScriptEditor";
            this.ScriptEditor.Size = new System.Drawing.Size(827, 530);
            this.ScriptEditor.TabIndex = 0;
            // 
            // QuickScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(827, 646);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ScriptEditor);
            this.Controls.Add(this.OutputOptions);
            this.Controls.Add(this.InputOptions);
            this.Controls.Add(this.MainStrip);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "QuickScriptEditor";
            this.Text = "Quick Scriptr";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QuickScriptEditor_FormClosing);
            this.Load += new System.EventHandler(this.QuickScriptEditor_Load);
            this.ResizeEnd += new System.EventHandler(this.QuickScriptEditor_ResizeEnd);
            this.InputOptions.ResumeLayout(false);
            this.InputOptions.PerformLayout();
            this.OutputOptions.ResumeLayout(false);
            this.OutputOptions.PerformLayout();
            this.MainStrip.ResumeLayout(false);
            this.MainStrip.PerformLayout();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip InputOptions;
        private System.Windows.Forms.ToolStrip OutputOptions;
        private System.Windows.Forms.ToolStripLabel OutputPathLabel;
        private System.Windows.Forms.ToolStripTextBox DestinationParam;
        private System.Windows.Forms.ToolStripButton EditDestinationFilePath;
        private System.Windows.Forms.ToolStripButton BrowseDestinationFilePath;
        private System.Windows.Forms.OpenFileDialog OpenInputFilePathDialog;
        private System.Windows.Forms.SaveFileDialog SaveDestinationFilePathDialog;
        private System.Windows.Forms.ToolStripLabel InputLabel;
        private System.Windows.Forms.ToolStripComboBox InputList;
        private System.Windows.Forms.ToolStripLabel InputPathLabel;
        private System.Windows.Forms.ToolStripTextBox InputParam;
        private System.Windows.Forms.ToolStripButton EditInputFilePath;
        private System.Windows.Forms.ToolStripButton BrowseInputFilePath;
        private System.Windows.Forms.ToolStripLabel OutputLabel;
        private System.Windows.Forms.ToolStripComboBox DestinationList;
        private System.Windows.Forms.ToolStrip MainStrip;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox QuickScriptList;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox TemplateList;
        private System.Windows.Forms.ToolStripMenuItem ShowInputOutputOptions;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox SliceAt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripComboBox DiceAt;
        private System.Windows.Forms.ToolStripMenuItem NewScriptMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewCodeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RunScriptMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BuildExeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteScriptMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel RunningLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel ProgressLabel;
        private QuickScriptControl ScriptEditor;
        private System.Windows.Forms.ToolStripMenuItem testFuncToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItem;
    }
}