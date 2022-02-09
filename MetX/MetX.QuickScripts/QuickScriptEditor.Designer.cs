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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Fred");
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
            this.LeftPanel = new System.Windows.Forms.TableLayoutPanel();
            this.QuickScriptList = new System.Windows.Forms.ListView();
            this.ScriptNameColumn = new System.Windows.Forms.ColumnHeader();
            this.ActionPanel = new System.Windows.Forms.TableLayoutPanel();
            this.OpenLink = new System.Windows.Forms.LinkLabel();
            this.SaveAsLink = new System.Windows.Forms.LinkLabel();
            this.SaveLink = new System.Windows.Forms.LinkLabel();
            this.DeleteScriptLink = new System.Windows.Forms.LinkLabel();
            this.ReplaceLink = new System.Windows.Forms.LinkLabel();
            this.FindLink = new System.Windows.Forms.LinkLabel();
            this.ViewCodeLink = new System.Windows.Forms.LinkLabel();
            this.PostBuildActionsLiink = new System.Windows.Forms.LinkLabel();
            this.BuildExeLink = new System.Windows.Forms.LinkLabel();
            this.RunQuickScriptLink = new System.Windows.Forms.LinkLabel();
            this.NewScriptLink = new System.Windows.Forms.LinkLabel();
            this.NewFileLink = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
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
            this.statusStrip1.Location = new System.Drawing.Point(426, 751);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(818, 25);
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
            this.ScriptEditor.Location = new System.Drawing.Point(426, 197);
            this.ScriptEditor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ScriptEditor.Name = "ScriptEditor";
            this.ScriptEditor.Size = new System.Drawing.Size(818, 554);
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
            this.TopPanel.Controls.Add(this.label10, 0, 1);
            this.TopPanel.Controls.Add(this.label14, 0, 6);
            this.TopPanel.Controls.Add(this.label2, 0, 5);
            this.TopPanel.Controls.Add(this.label5, 0, 4);
            this.TopPanel.Controls.Add(this.BrowseDestinationFilePath, 4, 3);
            this.TopPanel.Controls.Add(this.EditDestinationFilePath, 3, 3);
            this.TopPanel.Controls.Add(this.DestinationParam, 2, 3);
            this.TopPanel.Controls.Add(this.DestinationList, 1, 3);
            this.TopPanel.Controls.Add(this.label3, 0, 3);
            this.TopPanel.Controls.Add(this.label1, 0, 2);
            this.TopPanel.Controls.Add(this.InputList, 1, 2);
            this.TopPanel.Controls.Add(this.InputParam, 2, 2);
            this.TopPanel.Controls.Add(this.EditInputFilePath, 3, 2);
            this.TopPanel.Controls.Add(this.BrowseInputFilePath, 4, 2);
            this.TopPanel.Controls.Add(this.BrowseTemplateFolderPathButton, 4, 4);
            this.TopPanel.Controls.Add(this.TemplateFolderPath, 1, 4);
            this.TopPanel.Controls.Add(this.CloneTemplateButton, 3, 4);
            this.TopPanel.Controls.Add(this.SliceAt, 1, 5);
            this.TopPanel.Controls.Add(this.DiceAt, 1, 6);
            this.TopPanel.Controls.Add(this.QuickScriptName, 1, 1);
            this.TopPanel.Controls.Add(this.CloneScriptButton, 3, 1);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.TopPanel.Location = new System.Drawing.Point(426, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.RowCount = 8;
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TopPanel.Size = new System.Drawing.Size(818, 193);
            this.TopPanel.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(3, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 18);
            this.label10.TabIndex = 41;
            this.label10.Text = "Name:";
            // 
            // label14
            // 
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label14.Location = new System.Drawing.Point(3, 160);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(79, 30);
            this.label14.TabIndex = 32;
            this.label14.Text = "Dice at:";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(3, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 30);
            this.label2.TabIndex = 27;
            this.label2.Text = "Slice at:";
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(3, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 30);
            this.label5.TabIndex = 14;
            this.label5.Text = "Template:";
            // 
            // BrowseDestinationFilePath
            // 
            this.BrowseDestinationFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowseDestinationFilePath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BrowseDestinationFilePath.Location = new System.Drawing.Point(771, 73);
            this.BrowseDestinationFilePath.Name = "BrowseDestinationFilePath";
            this.BrowseDestinationFilePath.Size = new System.Drawing.Size(40, 24);
            this.BrowseDestinationFilePath.TabIndex = 10;
            this.BrowseDestinationFilePath.Text = "...";
            this.BrowseDestinationFilePath.UseVisualStyleBackColor = true;
            this.BrowseDestinationFilePath.Click += new System.EventHandler(this.BrowseDestinationFilePath_Click);
            // 
            // EditDestinationFilePath
            // 
            this.EditDestinationFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditDestinationFilePath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.EditDestinationFilePath.Location = new System.Drawing.Point(720, 73);
            this.EditDestinationFilePath.Name = "EditDestinationFilePath";
            this.EditDestinationFilePath.Size = new System.Drawing.Size(45, 24);
            this.EditDestinationFilePath.TabIndex = 9;
            this.EditDestinationFilePath.Text = "Edit";
            this.EditDestinationFilePath.UseVisualStyleBackColor = true;
            this.EditDestinationFilePath.Click += new System.EventHandler(this.EditDestinationFilePath_Click);
            // 
            // DestinationParam
            // 
            this.DestinationParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationParam.Location = new System.Drawing.Point(196, 73);
            this.DestinationParam.Name = "DestinationParam";
            this.DestinationParam.Size = new System.Drawing.Size(518, 20);
            this.DestinationParam.TabIndex = 8;
            this.DestinationParam.Enter += new System.EventHandler(this.DestinationParam_Enter);
            this.DestinationParam.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DestinationParam_MouseUp);
            // 
            // DestinationList
            // 
            this.DestinationList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationList.FormattingEnabled = true;
            this.DestinationList.Items.AddRange(new object[] {
            "File",
            "Clipboard",
            "Text Box",
            "Notepad"});
            this.DestinationList.Location = new System.Drawing.Point(88, 73);
            this.DestinationList.Name = "DestinationList";
            this.DestinationList.Size = new System.Drawing.Size(102, 21);
            this.DestinationList.TabIndex = 6;
            this.DestinationList.SelectedIndexChanged += new System.EventHandler(this.DestinationList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(3, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 30);
            this.label3.TabIndex = 5;
            this.label3.Text = "Output:";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input:";
            // 
            // InputList
            // 
            this.InputList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputList.FormattingEnabled = true;
            this.InputList.Items.AddRange(new object[] {
            "File",
            "Clipboard",
            "Database Query",
            "Web Address",
            "None"});
            this.InputList.Location = new System.Drawing.Point(88, 43);
            this.InputList.Name = "InputList";
            this.InputList.Size = new System.Drawing.Size(102, 21);
            this.InputList.TabIndex = 1;
            this.InputList.SelectedIndexChanged += new System.EventHandler(this.InputList_SelectedIndexChanged);
            // 
            // InputParam
            // 
            this.InputParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputParam.Location = new System.Drawing.Point(196, 43);
            this.InputParam.Name = "InputParam";
            this.InputParam.Size = new System.Drawing.Size(518, 20);
            this.InputParam.TabIndex = 3;
            this.InputParam.Enter += new System.EventHandler(this.InputParam_Enter);
            this.InputParam.Leave += new System.EventHandler(this.InputParam_Leave);
            this.InputParam.MouseUp += new System.Windows.Forms.MouseEventHandler(this.InputParam_MouseUp);
            // 
            // EditInputFilePath
            // 
            this.EditInputFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditInputFilePath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.EditInputFilePath.Location = new System.Drawing.Point(720, 43);
            this.EditInputFilePath.Name = "EditInputFilePath";
            this.EditInputFilePath.Size = new System.Drawing.Size(45, 24);
            this.EditInputFilePath.TabIndex = 4;
            this.EditInputFilePath.Text = "Edit";
            this.EditInputFilePath.UseVisualStyleBackColor = true;
            this.EditInputFilePath.Click += new System.EventHandler(this.EditInputFilePath_Click);
            // 
            // BrowseInputFilePath
            // 
            this.BrowseInputFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowseInputFilePath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BrowseInputFilePath.Location = new System.Drawing.Point(771, 43);
            this.BrowseInputFilePath.Name = "BrowseInputFilePath";
            this.BrowseInputFilePath.Size = new System.Drawing.Size(40, 24);
            this.BrowseInputFilePath.TabIndex = 4;
            this.BrowseInputFilePath.Text = "...";
            this.BrowseInputFilePath.UseVisualStyleBackColor = true;
            this.BrowseInputFilePath.Click += new System.EventHandler(this.BrowseInputFilePath_Click);
            // 
            // BrowseTemplateFolderPathButton
            // 
            this.BrowseTemplateFolderPathButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowseTemplateFolderPathButton.Location = new System.Drawing.Point(771, 103);
            this.BrowseTemplateFolderPathButton.Name = "BrowseTemplateFolderPathButton";
            this.BrowseTemplateFolderPathButton.Size = new System.Drawing.Size(40, 24);
            this.BrowseTemplateFolderPathButton.TabIndex = 17;
            this.BrowseTemplateFolderPathButton.Text = ",,,";
            this.BrowseTemplateFolderPathButton.UseVisualStyleBackColor = true;
            // 
            // TemplateFolderPath
            // 
            this.TopPanel.SetColumnSpan(this.TemplateFolderPath, 2);
            this.TemplateFolderPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TemplateFolderPath.Location = new System.Drawing.Point(88, 103);
            this.TemplateFolderPath.Name = "TemplateFolderPath";
            this.TemplateFolderPath.Size = new System.Drawing.Size(626, 20);
            this.TemplateFolderPath.TabIndex = 18;
            // 
            // CloneTemplateButton
            // 
            this.CloneTemplateButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.CloneTemplateButton.ForeColor = System.Drawing.Color.Black;
            this.CloneTemplateButton.Location = new System.Drawing.Point(720, 103);
            this.CloneTemplateButton.Name = "CloneTemplateButton";
            this.CloneTemplateButton.Size = new System.Drawing.Size(45, 23);
            this.CloneTemplateButton.TabIndex = 19;
            this.CloneTemplateButton.Text = "Clone";
            this.CloneTemplateButton.UseVisualStyleBackColor = true;
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
            this.SliceAt.Location = new System.Drawing.Point(88, 133);
            this.SliceAt.Name = "SliceAt";
            this.SliceAt.Size = new System.Drawing.Size(102, 21);
            this.SliceAt.TabIndex = 33;
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
            this.DiceAt.Location = new System.Drawing.Point(88, 163);
            this.DiceAt.Name = "DiceAt";
            this.DiceAt.Size = new System.Drawing.Size(102, 21);
            this.DiceAt.TabIndex = 34;
            // 
            // QuickScriptName
            // 
            this.QuickScriptName.BackColor = System.Drawing.SystemColors.Control;
            this.TopPanel.SetColumnSpan(this.QuickScriptName, 2);
            this.QuickScriptName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuickScriptName.Location = new System.Drawing.Point(88, 13);
            this.QuickScriptName.Name = "QuickScriptName";
            this.QuickScriptName.Size = new System.Drawing.Size(626, 20);
            this.QuickScriptName.TabIndex = 42;
            // 
            // CloneScriptButton
            // 
            this.CloneScriptButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CloneScriptButton.ForeColor = System.Drawing.Color.Black;
            this.CloneScriptButton.Location = new System.Drawing.Point(720, 13);
            this.CloneScriptButton.Name = "CloneScriptButton";
            this.CloneScriptButton.Size = new System.Drawing.Size(45, 24);
            this.CloneScriptButton.TabIndex = 43;
            this.CloneScriptButton.Text = "Clone";
            this.CloneScriptButton.UseVisualStyleBackColor = true;
            // 
            // LeftPanel
            // 
            this.LeftPanel.ColumnCount = 4;
            this.LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.49161F));
            this.LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.50839F));
            this.LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.LeftPanel.Controls.Add(this.QuickScriptList, 2, 1);
            this.LeftPanel.Controls.Add(this.ActionPanel, 1, 1);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.RowCount = 3;
            this.LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.LeftPanel.Size = new System.Drawing.Size(426, 776);
            this.LeftPanel.TabIndex = 26;
            // 
            // QuickScriptList
            // 
            this.QuickScriptList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(134)))));
            this.QuickScriptList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.QuickScriptList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ScriptNameColumn});
            this.QuickScriptList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuickScriptList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.QuickScriptList.ForeColor = System.Drawing.Color.White;
            this.QuickScriptList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.QuickScriptList.HideSelection = false;
            this.QuickScriptList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.QuickScriptList.Location = new System.Drawing.Point(155, 7);
            this.QuickScriptList.MultiSelect = false;
            this.QuickScriptList.Name = "QuickScriptList";
            this.QuickScriptList.Size = new System.Drawing.Size(263, 741);
            this.QuickScriptList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.QuickScriptList.TabIndex = 0;
            this.QuickScriptList.UseCompatibleStateImageBehavior = false;
            this.QuickScriptList.View = System.Windows.Forms.View.Details;
            this.QuickScriptList.SelectedIndexChanged += new System.EventHandler(this.QuickScriptList_SelectedIndexChanged);
            // 
            // ScriptNameColumn
            // 
            this.ScriptNameColumn.Text = "Script Name";
            this.ScriptNameColumn.Width = 500;
            // 
            // ActionPanel
            // 
            this.ActionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(80)))));
            this.ActionPanel.ColumnCount = 1;
            this.ActionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ActionPanel.Controls.Add(this.OpenLink, 0, 14);
            this.ActionPanel.Controls.Add(this.SaveAsLink, 0, 13);
            this.ActionPanel.Controls.Add(this.SaveLink, 0, 12);
            this.ActionPanel.Controls.Add(this.DeleteScriptLink, 0, 10);
            this.ActionPanel.Controls.Add(this.ReplaceLink, 0, 8);
            this.ActionPanel.Controls.Add(this.FindLink, 0, 7);
            this.ActionPanel.Controls.Add(this.ViewCodeLink, 0, 5);
            this.ActionPanel.Controls.Add(this.PostBuildActionsLiink, 0, 4);
            this.ActionPanel.Controls.Add(this.BuildExeLink, 0, 3);
            this.ActionPanel.Controls.Add(this.RunQuickScriptLink, 0, 2);
            this.ActionPanel.Controls.Add(this.NewScriptLink, 0, 1);
            this.ActionPanel.Controls.Add(this.NewFileLink, 0, 15);
            this.ActionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionPanel.Location = new System.Drawing.Point(7, 7);
            this.ActionPanel.Name = "ActionPanel";
            this.ActionPanel.RowCount = 17;
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ActionPanel.Size = new System.Drawing.Size(142, 741);
            this.ActionPanel.TabIndex = 1;
            this.ActionPanel.Click += new System.EventHandler(this.ActionPanel_Click);
            // 
            // OpenLink
            // 
            this.OpenLink.AutoSize = true;
            this.OpenLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.OpenLink.LinkColor = System.Drawing.Color.Yellow;
            this.OpenLink.Location = new System.Drawing.Point(3, 394);
            this.OpenLink.Name = "OpenLink";
            this.OpenLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.OpenLink.Size = new System.Drawing.Size(71, 24);
            this.OpenLink.TabIndex = 7;
            this.OpenLink.TabStop = true;
            this.OpenLink.Text = "Open file";
            this.toolTip1.SetToolTip(this.OpenLink, "Ctrl+O");
            this.OpenLink.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // SaveAsLink
            // 
            this.SaveAsLink.AutoSize = true;
            this.SaveAsLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SaveAsLink.LinkColor = System.Drawing.Color.Yellow;
            this.SaveAsLink.Location = new System.Drawing.Point(3, 364);
            this.SaveAsLink.Name = "SaveAsLink";
            this.SaveAsLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.SaveAsLink.Size = new System.Drawing.Size(66, 24);
            this.SaveAsLink.TabIndex = 7;
            this.SaveAsLink.TabStop = true;
            this.SaveAsLink.Text = "Save as";
            this.SaveAsLink.Click += new System.EventHandler(this.SaveAs_Click);
            // 
            // SaveLink
            // 
            this.SaveLink.AutoSize = true;
            this.SaveLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SaveLink.LinkColor = System.Drawing.Color.Yellow;
            this.SaveLink.Location = new System.Drawing.Point(3, 334);
            this.SaveLink.Name = "SaveLink";
            this.SaveLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.SaveLink.Size = new System.Drawing.Size(68, 24);
            this.SaveLink.TabIndex = 7;
            this.SaveLink.TabStop = true;
            this.SaveLink.Text = "Save file";
            this.toolTip1.SetToolTip(this.SaveLink, "Ctrl-S");
            this.SaveLink.Click += new System.EventHandler(this.SaveQuickScript_Click);
            // 
            // DeleteScriptLink
            // 
            this.DeleteScriptLink.AutoSize = true;
            this.DeleteScriptLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DeleteScriptLink.LinkColor = System.Drawing.Color.Yellow;
            this.DeleteScriptLink.Location = new System.Drawing.Point(3, 274);
            this.DeleteScriptLink.Name = "DeleteScriptLink";
            this.DeleteScriptLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.DeleteScriptLink.Size = new System.Drawing.Size(95, 24);
            this.DeleteScriptLink.TabIndex = 5;
            this.DeleteScriptLink.TabStop = true;
            this.DeleteScriptLink.Text = "Delete script";
            this.toolTip1.SetToolTip(this.DeleteScriptLink, "Ctrl+Alt+Shift+D");
            this.DeleteScriptLink.Click += new System.EventHandler(this.DeleteScript_Click);
            // 
            // ReplaceLink
            // 
            this.ReplaceLink.AutoSize = true;
            this.ReplaceLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ReplaceLink.LinkColor = System.Drawing.Color.Yellow;
            this.ReplaceLink.Location = new System.Drawing.Point(3, 214);
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
            this.FindLink.LinkColor = System.Drawing.Color.Yellow;
            this.FindLink.Location = new System.Drawing.Point(3, 184);
            this.FindLink.Name = "FindLink";
            this.FindLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.FindLink.Size = new System.Drawing.Size(41, 24);
            this.FindLink.TabIndex = 7;
            this.FindLink.TabStop = true;
            this.FindLink.Text = "Find";
            this.toolTip1.SetToolTip(this.FindLink, "Ctrl+F\r\nFind string in current script \r\nAnd optionally highlight all occurences");
            this.FindLink.Click += new System.EventHandler(this.FindMenuItem_Click);
            // 
            // ViewCodeLink
            // 
            this.ViewCodeLink.AutoSize = true;
            this.ViewCodeLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ViewCodeLink.ForeColor = System.Drawing.Color.Yellow;
            this.ViewCodeLink.LinkColor = System.Drawing.Color.Yellow;
            this.ViewCodeLink.Location = new System.Drawing.Point(3, 124);
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
            this.PostBuildActionsLiink.ForeColor = System.Drawing.Color.Yellow;
            this.PostBuildActionsLiink.LinkColor = System.Drawing.Color.Yellow;
            this.PostBuildActionsLiink.Location = new System.Drawing.Point(3, 94);
            this.PostBuildActionsLiink.Name = "PostBuildActionsLiink";
            this.PostBuildActionsLiink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.PostBuildActionsLiink.Size = new System.Drawing.Size(130, 24);
            this.PostBuildActionsLiink.TabIndex = 3;
            this.PostBuildActionsLiink.TabStop = true;
            this.PostBuildActionsLiink.Text = "Post build actions";
            this.toolTip1.SetToolTip(this.PostBuildActionsLiink, "F12");
            this.PostBuildActionsLiink.Click += new System.EventHandler(this.postToolStripMenuItem_Click);
            // 
            // BuildExeLink
            // 
            this.BuildExeLink.AutoSize = true;
            this.BuildExeLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BuildExeLink.ForeColor = System.Drawing.Color.Yellow;
            this.BuildExeLink.LinkColor = System.Drawing.Color.Yellow;
            this.BuildExeLink.Location = new System.Drawing.Point(3, 64);
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
            this.RunQuickScriptLink.ForeColor = System.Drawing.Color.Yellow;
            this.RunQuickScriptLink.LinkColor = System.Drawing.Color.Yellow;
            this.RunQuickScriptLink.Location = new System.Drawing.Point(3, 34);
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
            this.NewScriptLink.ForeColor = System.Drawing.Color.Yellow;
            this.NewScriptLink.LinkColor = System.Drawing.Color.Aqua;
            this.NewScriptLink.Location = new System.Drawing.Point(3, 4);
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
            this.NewFileLink.LinkColor = System.Drawing.Color.Yellow;
            this.NewFileLink.Location = new System.Drawing.Point(3, 424);
            this.NewFileLink.Name = "NewFileLink";
            this.NewFileLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            this.NewFileLink.Size = new System.Drawing.Size(65, 24);
            this.NewFileLink.TabIndex = 7;
            this.NewFileLink.TabStop = true;
            this.NewFileLink.Text = "New file";
            this.NewFileLink.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(426, 193);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(818, 4);
            this.panel1.TabIndex = 27;
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
            this.MinimumSize = new System.Drawing.Size(1060, 600);
            this.Name = "QuickScriptEditor";
            this.Text = "Qk Scrptr";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QuickScriptEditor_FormClosing);
            this.Load += new System.EventHandler(this.QuickScriptEditor_Load);
            this.ResizeEnd += new System.EventHandler(this.QuickScriptEditor_ResizeEnd);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.LeftPanel.ResumeLayout(false);
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
        private System.Windows.Forms.ListView QuickScriptList;
        private System.Windows.Forms.ColumnHeader ScriptNameColumn;
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
        private System.Windows.Forms.LinkLabel DeleteScriptLink;
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
    }
}