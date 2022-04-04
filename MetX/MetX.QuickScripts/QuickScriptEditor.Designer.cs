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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickScriptEditor));
            this.OpenInputFilePathDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveDestinationFilePathDialog = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.RunningLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ScriptEditor = new MetX.Controls.QuickScriptControl();
            this.TopPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BrowseDestinationFilePath = new System.Windows.Forms.Button();
            this.EditDestinationFilePath = new System.Windows.Forms.Button();
            this.DestinationParam = new System.Windows.Forms.TextBox();
            this.DestinationList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.InputList = new System.Windows.Forms.ComboBox();
            this.InputParam = new System.Windows.Forms.TextBox();
            this.EditInputFilePath = new System.Windows.Forms.Button();
            this.BrowseInputFilePath = new System.Windows.Forms.Button();
            this.BrowseTemplateFolderPathButton = new System.Windows.Forms.Button();
            this.TemplateFolderPath = new System.Windows.Forms.TextBox();
            this.CloneTemplateButton = new System.Windows.Forms.Button();
            this.SliceAt = new System.Windows.Forms.ComboBox();
            this.DiceAt = new System.Windows.Forms.ComboBox();
            this.QuickScriptName = new System.Windows.Forms.TextBox();
            this.CloneScriptButton = new System.Windows.Forms.Button();
            this.DeleteScriptButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.GitHubButton = new System.Windows.Forms.Button();
            this.FeedbackButton = new System.Windows.Forms.Button();
            this.ScriptEditorHelpButton = new System.Windows.Forms.Button();
            this.LeftPanel = new System.Windows.Forms.TableLayoutPanel();
            this.QuickScriptList = new System.Windows.Forms.ListBox();
            this.ActionPanel = new System.Windows.Forms.TableLayoutPanel();
            this.OpenLink = new System.Windows.Forms.LinkLabel();
            this.SaveAsLink = new System.Windows.Forms.LinkLabel();
            this.SaveLink = new System.Windows.Forms.LinkLabel();
            this.ReplaceLink = new System.Windows.Forms.LinkLabel();
            this.FindLink = new System.Windows.Forms.LinkLabel();
            this.ViewCodeLink = new System.Windows.Forms.LinkLabel();
            this.PostBuildActionsLiink = new System.Windows.Forms.LinkLabel();
            this.BuildExeLink = new System.Windows.Forms.LinkLabel();
            this.RunQuickScriptLink = new System.Windows.Forms.LinkLabel();
            this.NewScriptLink = new System.Windows.Forms.LinkLabel();
            this.NewFileLink = new System.Windows.Forms.LinkLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.ScriptNameLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ScriptNameColumn = new System.Windows.Forms.ColumnHeader();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            this.ActionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenInputFilePathDialog
            // 
            this.OpenInputFilePathDialog.FileName = "openFileDialog1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunningLabel,
            this.toolStripStatusLabel2,
            this.ProgressLabel});
            this.statusStrip1.Location = new System.Drawing.Point(426, 754);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(818, 22);
            this.statusStrip1.TabIndex = 23;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // RunningLabel
            // 
            this.RunningLabel.Name = "RunningLabel";
            this.RunningLabel.Size = new System.Drawing.Size(72, 17);
            this.RunningLabel.Text = "Not running";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel2.Text = " | ";
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(13, 17);
            this.ProgressLabel.Text = "0";
            // 
            // ScriptEditor
            // 
            this.ScriptEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ScriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScriptEditor.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ScriptEditor.IsIconBarVisible = true;
            this.ScriptEditor.IsReadOnly = false;
            this.ScriptEditor.Location = new System.Drawing.Point(426, 254);
            this.ScriptEditor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ScriptEditor.Name = "ScriptEditor";
            this.ScriptEditor.Size = new System.Drawing.Size(818, 500);
            this.ScriptEditor.TabIndex = 0;
            // 
            // TopPanel
            // 
            this.TopPanel.ColumnCount = 6;
            this.TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.TopPanel.Controls.Add(this.label10, 0, 2);
            this.TopPanel.Controls.Add(this.label14, 0, 7);
            this.TopPanel.Controls.Add(this.label2, 0, 6);
            this.TopPanel.Controls.Add(this.label5, 0, 5);
            this.TopPanel.Controls.Add(this.BrowseDestinationFilePath, 4, 4);
            this.TopPanel.Controls.Add(this.EditDestinationFilePath, 3, 4);
            this.TopPanel.Controls.Add(this.DestinationParam, 2, 4);
            this.TopPanel.Controls.Add(this.DestinationList, 1, 4);
            this.TopPanel.Controls.Add(this.label3, 0, 4);
            this.TopPanel.Controls.Add(this.label1, 0, 3);
            this.TopPanel.Controls.Add(this.InputList, 1, 3);
            this.TopPanel.Controls.Add(this.InputParam, 2, 3);
            this.TopPanel.Controls.Add(this.EditInputFilePath, 3, 3);
            this.TopPanel.Controls.Add(this.BrowseInputFilePath, 4, 3);
            this.TopPanel.Controls.Add(this.BrowseTemplateFolderPathButton, 4, 5);
            this.TopPanel.Controls.Add(this.TemplateFolderPath, 1, 5);
            this.TopPanel.Controls.Add(this.CloneTemplateButton, 3, 5);
            this.TopPanel.Controls.Add(this.SliceAt, 1, 6);
            this.TopPanel.Controls.Add(this.DiceAt, 1, 7);
            this.TopPanel.Controls.Add(this.QuickScriptName, 1, 2);
            this.TopPanel.Controls.Add(this.CloneScriptButton, 3, 2);
            this.TopPanel.Controls.Add(this.DeleteScriptButton, 4, 2);
            this.TopPanel.Controls.Add(this.label6, 0, 1);
            this.TopPanel.Controls.Add(this.label7, 0, 8);
            this.TopPanel.Controls.Add(this.GitHubButton, 4, 7);
            this.TopPanel.Controls.Add(this.FeedbackButton, 3, 7);
            this.TopPanel.Controls.Add(this.ScriptEditorHelpButton, 4, 8);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.TopPanel.Location = new System.Drawing.Point(426, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.RowCount = 9;
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TopPanel.Size = new System.Drawing.Size(818, 250);
            this.TopPanel.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(3, 34);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 18);
            this.label10.TabIndex = 41;
            this.label10.Text = "Name:";
            // 
            // label14
            // 
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label14.Location = new System.Drawing.Point(3, 184);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(79, 30);
            this.label14.TabIndex = 32;
            this.label14.Text = "Dice at:";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(3, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 30);
            this.label2.TabIndex = 27;
            this.label2.Text = "Slice at:";
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(3, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 30);
            this.label5.TabIndex = 14;
            this.label5.Text = "Template:";
            // 
            // BrowseDestinationFilePath
            // 
            this.BrowseDestinationFilePath.BackColor = System.Drawing.Color.PaleTurquoise;
            this.BrowseDestinationFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowseDestinationFilePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseDestinationFilePath.Image = global::XLG.QuickScripts.Properties.Resources.file_search_fill;
            this.BrowseDestinationFilePath.Location = new System.Drawing.Point(771, 97);
            this.BrowseDestinationFilePath.Name = "BrowseDestinationFilePath";
            this.BrowseDestinationFilePath.Size = new System.Drawing.Size(40, 24);
            this.BrowseDestinationFilePath.TabIndex = 11;
            this.toolTip1.SetToolTip(this.BrowseDestinationFilePath, "Browse for output filename");
            this.BrowseDestinationFilePath.UseVisualStyleBackColor = false;
            this.BrowseDestinationFilePath.Click += new System.EventHandler(this.BrowseDestinationFilePath_Click);
            // 
            // EditDestinationFilePath
            // 
            this.EditDestinationFilePath.BackColor = System.Drawing.Color.PaleTurquoise;
            this.EditDestinationFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditDestinationFilePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EditDestinationFilePath.ForeColor = System.Drawing.Color.Black;
            this.EditDestinationFilePath.Location = new System.Drawing.Point(720, 97);
            this.EditDestinationFilePath.Name = "EditDestinationFilePath";
            this.EditDestinationFilePath.Size = new System.Drawing.Size(45, 24);
            this.EditDestinationFilePath.TabIndex = 10;
            this.EditDestinationFilePath.Text = "Edit";
            this.toolTip1.SetToolTip(this.EditDestinationFilePath, "Edit the selected output file in notepad");
            this.EditDestinationFilePath.UseVisualStyleBackColor = false;
            this.EditDestinationFilePath.Click += new System.EventHandler(this.EditDestinationFilePath_Click);
            // 
            // DestinationParam
            // 
            this.DestinationParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationParam.Location = new System.Drawing.Point(196, 97);
            this.DestinationParam.Name = "DestinationParam";
            this.DestinationParam.Size = new System.Drawing.Size(518, 20);
            this.DestinationParam.TabIndex = 9;
            this.DestinationParam.Enter += new System.EventHandler(this.DestinationParam_Enter);
            // 
            // DestinationList
            // 
            this.DestinationList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DestinationList.FormattingEnabled = true;
            this.DestinationList.Items.AddRange(new object[] {
            "File",
            "Clipboard",
            "Text Box",
            "Notepad"});
            this.DestinationList.Location = new System.Drawing.Point(88, 97);
            this.DestinationList.Name = "DestinationList";
            this.DestinationList.Size = new System.Drawing.Size(102, 21);
            this.DestinationList.TabIndex = 8;
            this.DestinationList.SelectedIndexChanged += new System.EventHandler(this.DestinationList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(3, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 30);
            this.label3.TabIndex = 5;
            this.label3.Text = "Output:";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input:";
            // 
            // InputList
            // 
            this.InputList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InputList.FormattingEnabled = true;
            this.InputList.Items.AddRange(new object[] {
            "File",
            "Clipboard",
            "Database Query",
            "Web Address",
            "None"});
            this.InputList.Location = new System.Drawing.Point(88, 67);
            this.InputList.Name = "InputList";
            this.InputList.Size = new System.Drawing.Size(102, 21);
            this.InputList.TabIndex = 4;
            this.InputList.SelectedIndexChanged += new System.EventHandler(this.InputList_SelectedIndexChanged);
            // 
            // InputParam
            // 
            this.InputParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputParam.Location = new System.Drawing.Point(196, 67);
            this.InputParam.Name = "InputParam";
            this.InputParam.Size = new System.Drawing.Size(518, 20);
            this.InputParam.TabIndex = 5;
            this.InputParam.Enter += new System.EventHandler(this.InputParam_Enter);
            this.InputParam.Leave += new System.EventHandler(this.InputParam_Leave);
            // 
            // EditInputFilePath
            // 
            this.EditInputFilePath.BackColor = System.Drawing.Color.PaleTurquoise;
            this.EditInputFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditInputFilePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EditInputFilePath.ForeColor = System.Drawing.Color.Black;
            this.EditInputFilePath.Location = new System.Drawing.Point(720, 67);
            this.EditInputFilePath.Name = "EditInputFilePath";
            this.EditInputFilePath.Size = new System.Drawing.Size(45, 24);
            this.EditInputFilePath.TabIndex = 6;
            this.EditInputFilePath.Text = "Edit";
            this.toolTip1.SetToolTip(this.EditInputFilePath, "Edit the selected input file in notepad");
            this.EditInputFilePath.UseVisualStyleBackColor = false;
            this.EditInputFilePath.Click += new System.EventHandler(this.EditInputFilePath_Click);
            // 
            // BrowseInputFilePath
            // 
            this.BrowseInputFilePath.BackColor = System.Drawing.Color.PaleTurquoise;
            this.BrowseInputFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowseInputFilePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseInputFilePath.Image = global::XLG.QuickScripts.Properties.Resources.file_search_fill;
            this.BrowseInputFilePath.Location = new System.Drawing.Point(771, 67);
            this.BrowseInputFilePath.Name = "BrowseInputFilePath";
            this.BrowseInputFilePath.Size = new System.Drawing.Size(40, 24);
            this.BrowseInputFilePath.TabIndex = 7;
            this.toolTip1.SetToolTip(this.BrowseInputFilePath, "Browse for input filename");
            this.BrowseInputFilePath.UseVisualStyleBackColor = false;
            this.BrowseInputFilePath.Click += new System.EventHandler(this.BrowseInputFilePath_Click);
            // 
            // BrowseTemplateFolderPathButton
            // 
            this.BrowseTemplateFolderPathButton.BackColor = System.Drawing.Color.PaleTurquoise;
            this.BrowseTemplateFolderPathButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowseTemplateFolderPathButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseTemplateFolderPathButton.Image = global::XLG.QuickScripts.Properties.Resources.folder_4_fill;
            this.BrowseTemplateFolderPathButton.Location = new System.Drawing.Point(771, 127);
            this.BrowseTemplateFolderPathButton.Name = "BrowseTemplateFolderPathButton";
            this.BrowseTemplateFolderPathButton.Size = new System.Drawing.Size(40, 24);
            this.BrowseTemplateFolderPathButton.TabIndex = 14;
            this.toolTip1.SetToolTip(this.BrowseTemplateFolderPathButton, "Browse for the folder containing the template you want to use with this script (o" +
        "r set text box to \'Exe\' for the default template)");
            this.BrowseTemplateFolderPathButton.UseVisualStyleBackColor = false;
            this.BrowseTemplateFolderPathButton.Click += new System.EventHandler(this.BrowseTemplateFolderPathButton_Click);
            // 
            // TemplateFolderPath
            // 
            this.TopPanel.SetColumnSpan(this.TemplateFolderPath, 2);
            this.TemplateFolderPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TemplateFolderPath.Location = new System.Drawing.Point(88, 127);
            this.TemplateFolderPath.Name = "TemplateFolderPath";
            this.TemplateFolderPath.Size = new System.Drawing.Size(626, 20);
            this.TemplateFolderPath.TabIndex = 12;
            // 
            // CloneTemplateButton
            // 
            this.CloneTemplateButton.BackColor = System.Drawing.Color.PaleTurquoise;
            this.CloneTemplateButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.CloneTemplateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloneTemplateButton.ForeColor = System.Drawing.Color.Black;
            this.CloneTemplateButton.Location = new System.Drawing.Point(720, 127);
            this.CloneTemplateButton.Name = "CloneTemplateButton";
            this.CloneTemplateButton.Size = new System.Drawing.Size(45, 23);
            this.CloneTemplateButton.TabIndex = 13;
            this.CloneTemplateButton.Text = "Clone";
            this.toolTip1.SetToolTip(this.CloneTemplateButton, "Copy the current template folder to a new folder and set the current script\'s tem" +
        "plate to the new folder");
            this.CloneTemplateButton.UseVisualStyleBackColor = false;
            this.CloneTemplateButton.Click += new System.EventHandler(this.CloneTemplateButton_Click);
            // 
            // SliceAt
            // 
            this.SliceAt.FormattingEnabled = true;
            this.SliceAt.Items.AddRange(new object[] {
            "End of line",
            "Equal sign",
            "Tab",
            "Pipe",
            "Space"});
            this.SliceAt.Location = new System.Drawing.Point(88, 157);
            this.SliceAt.Name = "SliceAt";
            this.SliceAt.Size = new System.Drawing.Size(102, 21);
            this.SliceAt.TabIndex = 15;
            this.SliceAt.SelectedIndexChanged += new System.EventHandler(this.SliceAt_SelectedIndexChanged);
            // 
            // DiceAt
            // 
            this.DiceAt.FormattingEnabled = true;
            this.DiceAt.Items.AddRange(new object[] {
            "Space",
            "Tab",
            "Equal sign",
            "Pipe",
            "End of line"});
            this.DiceAt.Location = new System.Drawing.Point(88, 187);
            this.DiceAt.Name = "DiceAt";
            this.DiceAt.Size = new System.Drawing.Size(102, 21);
            this.DiceAt.TabIndex = 16;
            this.DiceAt.SelectedIndexChanged += new System.EventHandler(this.DiceAt_SelectedIndexChanged);
            // 
            // QuickScriptName
            // 
            this.QuickScriptName.BackColor = System.Drawing.SystemColors.Control;
            this.TopPanel.SetColumnSpan(this.QuickScriptName, 2);
            this.QuickScriptName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuickScriptName.Location = new System.Drawing.Point(88, 37);
            this.QuickScriptName.Name = "QuickScriptName";
            this.QuickScriptName.Size = new System.Drawing.Size(626, 20);
            this.QuickScriptName.TabIndex = 2;
            // 
            // CloneScriptButton
            // 
            this.CloneScriptButton.BackColor = System.Drawing.Color.PaleTurquoise;
            this.CloneScriptButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CloneScriptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloneScriptButton.ForeColor = System.Drawing.Color.Black;
            this.CloneScriptButton.Location = new System.Drawing.Point(720, 37);
            this.CloneScriptButton.Name = "CloneScriptButton";
            this.CloneScriptButton.Size = new System.Drawing.Size(45, 24);
            this.CloneScriptButton.TabIndex = 3;
            this.CloneScriptButton.Text = "Clone";
            this.toolTip1.SetToolTip(this.CloneScriptButton, "Copy the current script to a new script with a new name");
            this.CloneScriptButton.UseVisualStyleBackColor = false;
            this.CloneScriptButton.Click += new System.EventHandler(this.CloneScriptButton_Click);
            // 
            // DeleteScriptButton
            // 
            this.DeleteScriptButton.BackColor = System.Drawing.Color.Pink;
            this.DeleteScriptButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeleteScriptButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.DeleteScriptButton.Image = global::XLG.QuickScripts.Properties.Resources.delete_bin_2_fill;
            this.DeleteScriptButton.Location = new System.Drawing.Point(770, 36);
            this.DeleteScriptButton.Margin = new System.Windows.Forms.Padding(2);
            this.DeleteScriptButton.Name = "DeleteScriptButton";
            this.DeleteScriptButton.Size = new System.Drawing.Size(42, 26);
            this.DeleteScriptButton.TabIndex = 9999999;
            this.DeleteScriptButton.TabStop = false;
            this.toolTip1.SetToolTip(this.DeleteScriptButton, "Delete this script");
            this.DeleteScriptButton.UseVisualStyleBackColor = false;
            this.DeleteScriptButton.Click += new System.EventHandler(this.DeleteScript_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.TopPanel.SetColumnSpan(this.label6, 3);
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(0, 4);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(190, 26);
            this.label6.TabIndex = 2;
            this.label6.Text = "Script Properties";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.TopPanel.SetColumnSpan(this.label7, 4);
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(0, 224);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 26);
            this.label7.TabIndex = 2;
            this.label7.Text = "Script Editor";
            // 
            // GitHubButton
            // 
            this.GitHubButton.BackColor = System.Drawing.Color.Lavender;
            this.GitHubButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GitHubButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GitHubButton.ForeColor = System.Drawing.Color.White;
            this.GitHubButton.Image = global::XLG.QuickScripts.Properties.Resources.github_fill;
            this.GitHubButton.Location = new System.Drawing.Point(769, 185);
            this.GitHubButton.Margin = new System.Windows.Forms.Padding(1);
            this.GitHubButton.Name = "GitHubButton";
            this.GitHubButton.Size = new System.Drawing.Size(44, 28);
            this.GitHubButton.TabIndex = 45;
            this.GitHubButton.TabStop = false;
            this.GitHubButton.UseVisualStyleBackColor = false;
            this.GitHubButton.Click += new System.EventHandler(this.GitHubButton_Click);
            // 
            // FeedbackButton
            // 
            this.FeedbackButton.BackColor = System.Drawing.Color.Lavender;
            this.FeedbackButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FeedbackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FeedbackButton.ForeColor = System.Drawing.Color.White;
            this.FeedbackButton.Image = global::XLG.QuickScripts.Properties.Resources.feedback_fill;
            this.FeedbackButton.Location = new System.Drawing.Point(718, 185);
            this.FeedbackButton.Margin = new System.Windows.Forms.Padding(1);
            this.FeedbackButton.Name = "FeedbackButton";
            this.FeedbackButton.Size = new System.Drawing.Size(49, 28);
            this.FeedbackButton.TabIndex = 46;
            this.FeedbackButton.TabStop = false;
            this.FeedbackButton.UseVisualStyleBackColor = false;
            this.FeedbackButton.Click += new System.EventHandler(this.FeedbackButton_Click);
            // 
            // ScriptEditorHelpButton
            // 
            this.ScriptEditorHelpButton.BackColor = System.Drawing.Color.Lavender;
            this.ScriptEditorHelpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScriptEditorHelpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScriptEditorHelpButton.ForeColor = System.Drawing.Color.White;
            this.ScriptEditorHelpButton.Image = global::XLG.QuickScripts.Properties.Resources.question_fill;
            this.ScriptEditorHelpButton.Location = new System.Drawing.Point(769, 215);
            this.ScriptEditorHelpButton.Margin = new System.Windows.Forms.Padding(1);
            this.ScriptEditorHelpButton.Name = "ScriptEditorHelpButton";
            this.ScriptEditorHelpButton.Size = new System.Drawing.Size(44, 34);
            this.ScriptEditorHelpButton.TabIndex = 17;
            this.ScriptEditorHelpButton.UseVisualStyleBackColor = false;
            this.ScriptEditorHelpButton.Click += new System.EventHandler(this.ScriptEditorHelpButton_Click);
            // 
            // LeftPanel
            // 
            this.LeftPanel.ColumnCount = 4;
            this.LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.49161F));
            this.LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.50839F));
            this.LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.LeftPanel.Controls.Add(this.QuickScriptList, 2, 2);
            this.LeftPanel.Controls.Add(this.ActionPanel, 1, 2);
            this.LeftPanel.Controls.Add(this.ScriptNameLabel, 2, 1);
            this.LeftPanel.Controls.Add(this.label4, 1, 1);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.RowCount = 4;
            this.LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.LeftPanel.Size = new System.Drawing.Size(426, 776);
            this.LeftPanel.TabIndex = 26;
            // 
            // QuickScriptList
            // 
            this.QuickScriptList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(134)))));
            this.QuickScriptList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.QuickScriptList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuickScriptList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.QuickScriptList.ForeColor = System.Drawing.Color.White;
            this.QuickScriptList.ItemHeight = 20;
            this.QuickScriptList.Items.AddRange(new object[] {
            "<Empty this should never be seen>"});
            this.QuickScriptList.Location = new System.Drawing.Point(158, 35);
            this.QuickScriptList.Margin = new System.Windows.Forms.Padding(6);
            this.QuickScriptList.Name = "QuickScriptList";
            this.QuickScriptList.Size = new System.Drawing.Size(257, 710);
            this.QuickScriptList.TabIndex = 1;
            this.QuickScriptList.SelectedIndexChanged += new System.EventHandler(this.QuickScriptList_SelectedIndexChanged);
            // 
            // ActionPanel
            // 
            this.ActionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(70)))));
            this.ActionPanel.ColumnCount = 1;
            this.ActionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ActionPanel.Controls.Add(this.OpenLink, 0, 12);
            this.ActionPanel.Controls.Add(this.SaveAsLink, 0, 11);
            this.ActionPanel.Controls.Add(this.SaveLink, 0, 10);
            this.ActionPanel.Controls.Add(this.ReplaceLink, 0, 8);
            this.ActionPanel.Controls.Add(this.FindLink, 0, 7);
            this.ActionPanel.Controls.Add(this.ViewCodeLink, 0, 5);
            this.ActionPanel.Controls.Add(this.PostBuildActionsLiink, 0, 4);
            this.ActionPanel.Controls.Add(this.BuildExeLink, 0, 3);
            this.ActionPanel.Controls.Add(this.RunQuickScriptLink, 0, 2);
            this.ActionPanel.Controls.Add(this.NewScriptLink, 0, 1);
            this.ActionPanel.Controls.Add(this.NewFileLink, 0, 13);
            this.ActionPanel.Controls.Add(this.button1, 0, 14);
            this.ActionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionPanel.Location = new System.Drawing.Point(7, 32);
            this.ActionPanel.Name = "ActionPanel";
            this.ActionPanel.RowCount = 15;
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.Size = new System.Drawing.Size(142, 716);
            this.ActionPanel.TabIndex = 1;
            this.ActionPanel.Click += new System.EventHandler(this.ActionPanel_Click);
            // 
            // OpenLink
            // 
            this.OpenLink.AutoSize = true;
            this.OpenLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.OpenLink.LinkColor = System.Drawing.Color.Aqua;
            this.OpenLink.Location = new System.Drawing.Point(3, 320);
            this.OpenLink.Name = "OpenLink";
            this.OpenLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.OpenLink.Size = new System.Drawing.Size(71, 24);
            this.OpenLink.TabIndex = 7;
            this.OpenLink.TabStop = true;
            this.OpenLink.Text = "Open file";
            this.toolTip1.SetToolTip(this.OpenLink, "Ctrl+O");
            this.OpenLink.Click += new System.EventHandler(this.OpenScriptFile_Click);
            // 
            // SaveAsLink
            // 
            this.SaveAsLink.AutoSize = true;
            this.SaveAsLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SaveAsLink.LinkColor = System.Drawing.Color.Aqua;
            this.SaveAsLink.Location = new System.Drawing.Point(3, 290);
            this.SaveAsLink.Name = "SaveAsLink";
            this.SaveAsLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.SaveAsLink.Size = new System.Drawing.Size(66, 24);
            this.SaveAsLink.TabIndex = 7;
            this.SaveAsLink.TabStop = true;
            this.SaveAsLink.Text = "Save as";
            this.toolTip1.SetToolTip(this.SaveAsLink, "Ctrl+A");
            this.SaveAsLink.Click += new System.EventHandler(this.SaveAs_Click);
            // 
            // SaveLink
            // 
            this.SaveLink.AutoSize = true;
            this.SaveLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SaveLink.LinkColor = System.Drawing.Color.Aqua;
            this.SaveLink.Location = new System.Drawing.Point(3, 260);
            this.SaveLink.Name = "SaveLink";
            this.SaveLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.SaveLink.Size = new System.Drawing.Size(108, 24);
            this.SaveLink.TabIndex = 7;
            this.SaveLink.TabStop = true;
            this.SaveLink.Text = "Save file (xlgq)";
            this.toolTip1.SetToolTip(this.SaveLink, "Ctrl-S");
            this.SaveLink.Click += new System.EventHandler(this.SaveQuickScriptFile_Click);
            // 
            // ReplaceLink
            // 
            this.ReplaceLink.AutoSize = true;
            this.ReplaceLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ReplaceLink.LinkColor = System.Drawing.Color.White;
            this.ReplaceLink.Location = new System.Drawing.Point(3, 210);
            this.ReplaceLink.Name = "ReplaceLink";
            this.ReplaceLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.ReplaceLink.Size = new System.Drawing.Size(67, 24);
            this.ReplaceLink.TabIndex = 7;
            this.ReplaceLink.TabStop = true;
            this.ReplaceLink.Text = "Replace";
            this.toolTip1.SetToolTip(this.ReplaceLink, "Ctrl+R\r\nReplace string in current script");
            this.ReplaceLink.Click += new System.EventHandler(this.ReplaceMenuItem_Click);
            // 
            // FindLink
            // 
            this.FindLink.AutoSize = true;
            this.FindLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FindLink.LinkColor = System.Drawing.Color.White;
            this.FindLink.Location = new System.Drawing.Point(3, 180);
            this.FindLink.Name = "FindLink";
            this.FindLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.FindLink.Size = new System.Drawing.Size(106, 24);
            this.FindLink.TabIndex = 7;
            this.FindLink.TabStop = true;
            this.FindLink.Text = "Find / highlight";
            this.toolTip1.SetToolTip(this.FindLink, "Ctrl+F\r\nFind string in current script \r\nAnd optionally highlight all occurences");
            this.FindLink.Click += new System.EventHandler(this.FindMenuItem_Click);
            // 
            // ViewCodeLink
            // 
            this.ViewCodeLink.AutoSize = true;
            this.ViewCodeLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ViewCodeLink.ForeColor = System.Drawing.Color.White;
            this.ViewCodeLink.LinkColor = System.Drawing.Color.Yellow;
            this.ViewCodeLink.Location = new System.Drawing.Point(3, 130);
            this.ViewCodeLink.Name = "ViewCodeLink";
            this.ViewCodeLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.ViewCodeLink.Size = new System.Drawing.Size(81, 24);
            this.ViewCodeLink.TabIndex = 6;
            this.ViewCodeLink.TabStop = true;
            this.ViewCodeLink.Text = "View code";
            this.ViewCodeLink.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.ViewCodeLink, "Ctrl+G");
            this.ViewCodeLink.Click += new System.EventHandler(this.ViewGeneratedCode_Click);
            // 
            // PostBuildActionsLiink
            // 
            this.PostBuildActionsLiink.AutoSize = true;
            this.PostBuildActionsLiink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PostBuildActionsLiink.ForeColor = System.Drawing.Color.White;
            this.PostBuildActionsLiink.LinkColor = System.Drawing.Color.Yellow;
            this.PostBuildActionsLiink.Location = new System.Drawing.Point(3, 100);
            this.PostBuildActionsLiink.Name = "PostBuildActionsLiink";
            this.PostBuildActionsLiink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.PostBuildActionsLiink.Size = new System.Drawing.Size(130, 24);
            this.PostBuildActionsLiink.TabIndex = 3;
            this.PostBuildActionsLiink.TabStop = true;
            this.PostBuildActionsLiink.Text = "Post build actions";
            this.toolTip1.SetToolTip(this.PostBuildActionsLiink, "F12");
            this.PostBuildActionsLiink.Click += new System.EventHandler(this.PostBuild_Click);
            // 
            // BuildExeLink
            // 
            this.BuildExeLink.AutoSize = true;
            this.BuildExeLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BuildExeLink.ForeColor = System.Drawing.Color.White;
            this.BuildExeLink.LinkColor = System.Drawing.Color.Yellow;
            this.BuildExeLink.Location = new System.Drawing.Point(3, 70);
            this.BuildExeLink.Name = "BuildExeLink";
            this.BuildExeLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.BuildExeLink.Size = new System.Drawing.Size(131, 24);
            this.BuildExeLink.TabIndex = 2;
            this.BuildExeLink.TabStop = true;
            this.BuildExeLink.Text = "Build cmd line exe";
            this.toolTip1.SetToolTip(this.BuildExeLink, "F6");
            this.BuildExeLink.Click += new System.EventHandler(this.BuildExe_Click);
            // 
            // RunQuickScriptLink
            // 
            this.RunQuickScriptLink.AutoSize = true;
            this.RunQuickScriptLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RunQuickScriptLink.ForeColor = System.Drawing.Color.White;
            this.RunQuickScriptLink.LinkColor = System.Drawing.Color.Yellow;
            this.RunQuickScriptLink.Location = new System.Drawing.Point(3, 40);
            this.RunQuickScriptLink.Name = "RunQuickScriptLink";
            this.RunQuickScriptLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.RunQuickScriptLink.Size = new System.Drawing.Size(130, 24);
            this.RunQuickScriptLink.TabIndex = 1;
            this.RunQuickScriptLink.TabStop = true;
            this.RunQuickScriptLink.Text = "Run current script";
            this.toolTip1.SetToolTip(this.RunQuickScriptLink, "F5");
            this.RunQuickScriptLink.Click += new System.EventHandler(this.RunQuickScript_Click);
            // 
            // NewScriptLink
            // 
            this.NewScriptLink.AutoSize = true;
            this.NewScriptLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NewScriptLink.ForeColor = System.Drawing.Color.White;
            this.NewScriptLink.LinkColor = System.Drawing.Color.Yellow;
            this.NewScriptLink.Location = new System.Drawing.Point(3, 10);
            this.NewScriptLink.Name = "NewScriptLink";
            this.NewScriptLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.NewScriptLink.Size = new System.Drawing.Size(78, 24);
            this.NewScriptLink.TabIndex = 0;
            this.NewScriptLink.TabStop = true;
            this.NewScriptLink.Text = "Add script";
            this.toolTip1.SetToolTip(this.NewScriptLink, "Ctrl+N");
            this.NewScriptLink.Click += new System.EventHandler(this.NewQuickScript_Click);
            // 
            // NewFileLink
            // 
            this.NewFileLink.AutoSize = true;
            this.NewFileLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NewFileLink.LinkColor = System.Drawing.Color.Aqua;
            this.NewFileLink.Location = new System.Drawing.Point(3, 350);
            this.NewFileLink.Name = "NewFileLink";
            this.NewFileLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.NewFileLink.Size = new System.Drawing.Size(65, 24);
            this.NewFileLink.TabIndex = 7;
            this.NewFileLink.TabStop = true;
            this.NewFileLink.Text = "New file";
            this.NewFileLink.Click += new System.EventHandler(this.NewScriptFile_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 383);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // ScriptNameLabel
            // 
            this.ScriptNameLabel.AutoSize = true;
            this.ScriptNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ScriptNameLabel.Location = new System.Drawing.Point(152, 4);
            this.ScriptNameLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ScriptNameLabel.Name = "ScriptNameLabel";
            this.ScriptNameLabel.Size = new System.Drawing.Size(86, 25);
            this.ScriptNameLabel.TabIndex = 2;
            this.ScriptNameLabel.Text = "Scripts";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(4, 4);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 25);
            this.label4.TabIndex = 2;
            this.label4.Text = "Actions";
            // 
            // ScriptNameColumn
            // 
            this.ScriptNameColumn.Text = "Script Name";
            this.ScriptNameColumn.Width = 500;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(426, 250);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(818, 4);
            this.panel1.TabIndex = 27;
            // 
            // FolderBrowserDialog
            // 
            this.FolderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyDocuments;
            this.FolderBrowserDialog.SelectedPath = "XLG\\Scripts\\";
            this.FolderBrowserDialog.UseDescriptionForTitle = true;
            // 
            // QuickScriptEditor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1244, 776);
            this.Controls.Add(this.ScriptEditor);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.LeftPanel);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1060, 600);
            this.Name = "QuickScriptEditor";
            this.Text = "Qk Scrptr";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QuickScriptEditor_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.QuickScriptEditor_ResizeEnd);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.QuickScriptEditor_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.LeftPanel.ResumeLayout(false);
            this.LeftPanel.PerformLayout();
            this.ActionPanel.ResumeLayout(false);
            this.ActionPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog OpenInputFilePathDialog;
        private System.Windows.Forms.SaveFileDialog SaveDestinationFilePathDialog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel RunningLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel ProgressLabel;
        private QuickScriptControl ScriptEditor;
        private System.Windows.Forms.TableLayoutPanel TopPanel;
        private System.Windows.Forms.Button BrowseDestinationFilePath;
        private System.Windows.Forms.Button EditDestinationFilePath;
        private System.Windows.Forms.TextBox DestinationParam;
        private System.Windows.Forms.ComboBox DestinationList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox InputList;
        private System.Windows.Forms.TextBox InputParam;
        private System.Windows.Forms.Button EditInputFilePath;
        private System.Windows.Forms.Button BrowseInputFilePath;
        private System.Windows.Forms.TableLayoutPanel LeftPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BrowseTemplateFolderPathButton;
        private System.Windows.Forms.TextBox TemplateFolderPath;
        private System.Windows.Forms.Button CloneTemplateButton;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox SliceAt;
        private System.Windows.Forms.ComboBox DiceAt;
        private System.Windows.Forms.TableLayoutPanel ActionPanel;
        private System.Windows.Forms.LinkLabel RunQuickScriptLink;
        private System.Windows.Forms.LinkLabel NewScriptLink;
        private System.Windows.Forms.LinkLabel ReplaceLink;
        private System.Windows.Forms.LinkLabel BuildExeLink;
        private System.Windows.Forms.LinkLabel PostBuildActionsLiink;
        private System.Windows.Forms.LinkLabel ViewCodeLink;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel FindLink;
        private System.Windows.Forms.LinkLabel SaveLink;
        private System.Windows.Forms.LinkLabel SaveAsLink;
        private System.Windows.Forms.LinkLabel OpenLink;
        private System.Windows.Forms.LinkLabel NewFileLink;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox QuickScriptName;
        private System.Windows.Forms.Button CloneScriptButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox QuickScriptList;
        private System.Windows.Forms.ColumnHeader ScriptNameColumn;
        private System.Windows.Forms.Button DeleteScriptButton;
        private System.Windows.Forms.Label ScriptNameLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button GitHubButton;
        private System.Windows.Forms.Button FeedbackButton;
        private System.Windows.Forms.Button ScriptEditorHelpButton;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
        private System.Windows.Forms.Button button1;
    }
}