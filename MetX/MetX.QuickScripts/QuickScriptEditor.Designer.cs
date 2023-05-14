using MetX.Windows.Controls;

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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickScriptEditor));
            OpenInputFilePathDialog = new System.Windows.Forms.OpenFileDialog();
            SaveDestinationFilePathDialog = new System.Windows.Forms.SaveFileDialog();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            RunningLabel = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            ProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ScriptEditor = new QuickScriptControl();
            TopPanel = new System.Windows.Forms.TableLayoutPanel();
            label10 = new System.Windows.Forms.Label();
            label14 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            BrowseDestinationFilePath = new System.Windows.Forms.Button();
            EditDestinationFilePath = new System.Windows.Forms.Button();
            DestinationParam = new System.Windows.Forms.TextBox();
            DestinationList = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            InputList = new System.Windows.Forms.ComboBox();
            InputParam = new System.Windows.Forms.TextBox();
            EditInputFilePath = new System.Windows.Forms.Button();
            BrowseInputFilePath = new System.Windows.Forms.Button();
            BrowseTemplateFolderPathButton = new System.Windows.Forms.Button();
            TemplateFolderPath = new System.Windows.Forms.TextBox();
            CloneTemplateButton = new System.Windows.Forms.Button();
            SliceAt = new System.Windows.Forms.ComboBox();
            DiceAt = new System.Windows.Forms.ComboBox();
            QuickScriptName = new System.Windows.Forms.TextBox();
            CloneScriptButton = new System.Windows.Forms.Button();
            DeleteScriptButton = new System.Windows.Forms.Button();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            GitHubButton = new System.Windows.Forms.Button();
            FeedbackButton = new System.Windows.Forms.Button();
            ScriptEditorHelpButton = new System.Windows.Forms.Button();
            LeftPanel = new System.Windows.Forms.TableLayoutPanel();
            QuickScriptList = new System.Windows.Forms.ListBox();
            ActionPanel = new System.Windows.Forms.TableLayoutPanel();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            RunQuickScriptLink = new System.Windows.Forms.LinkLabel();
            SaveLink = new System.Windows.Forms.LinkLabel();
            SaveAsLink = new System.Windows.Forms.LinkLabel();
            OpenLink = new System.Windows.Forms.LinkLabel();
            NewFileLink = new System.Windows.Forms.LinkLabel();
            FindLink = new System.Windows.Forms.LinkLabel();
            ReplaceLink = new System.Windows.Forms.LinkLabel();
            NewScriptLink = new System.Windows.Forms.LinkLabel();
            button1 = new System.Windows.Forms.Button();
            RestageTemplatesButton = new System.Windows.Forms.LinkLabel();
            PostBuildActionsLiink = new System.Windows.Forms.LinkLabel();
            BuildExeLink = new System.Windows.Forms.LinkLabel();
            ScriptNameLabel = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            ScriptNameColumn = new System.Windows.Forms.ColumnHeader();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            panel1 = new System.Windows.Forms.Panel();
            FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            statusStrip1.SuspendLayout();
            TopPanel.SuspendLayout();
            LeftPanel.SuspendLayout();
            ActionPanel.SuspendLayout();
            SuspendLayout();
            // 
            // OpenInputFilePathDialog
            // 
            OpenInputFilePathDialog.FileName = "openFileDialog1";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { RunningLabel, toolStripStatusLabel2, ProgressLabel });
            statusStrip1.Location = new System.Drawing.Point(426, 750);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(818, 26);
            statusStrip1.TabIndex = 23;
            statusStrip1.Text = "statusStrip1";
            // 
            // RunningLabel
            // 
            RunningLabel.Name = "RunningLabel";
            RunningLabel.Size = new System.Drawing.Size(95, 21);
            RunningLabel.Text = "Not running";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new System.Drawing.Size(22, 21);
            toolStripStatusLabel2.Text = " | ";
            // 
            // ProgressLabel
            // 
            ProgressLabel.Name = "ProgressLabel";
            ProgressLabel.Size = new System.Drawing.Size(19, 21);
            ProgressLabel.Text = "0";
            // 
            // ScriptEditor
            // 
            ScriptEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            ScriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            ScriptEditor.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ScriptEditor.IsIconBarVisible = true;
            ScriptEditor.IsReadOnly = false;
            ScriptEditor.Location = new System.Drawing.Point(426, 254);
            ScriptEditor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ScriptEditor.Name = "ScriptEditor";
            ScriptEditor.Size = new System.Drawing.Size(818, 496);
            ScriptEditor.TabIndex = 0;
            // 
            // TopPanel
            // 
            TopPanel.ColumnCount = 6;
            TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            TopPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            TopPanel.Controls.Add(label10, 0, 2);
            TopPanel.Controls.Add(label14, 0, 7);
            TopPanel.Controls.Add(label2, 0, 6);
            TopPanel.Controls.Add(label5, 0, 5);
            TopPanel.Controls.Add(BrowseDestinationFilePath, 4, 4);
            TopPanel.Controls.Add(EditDestinationFilePath, 3, 4);
            TopPanel.Controls.Add(DestinationParam, 2, 4);
            TopPanel.Controls.Add(DestinationList, 1, 4);
            TopPanel.Controls.Add(label3, 0, 4);
            TopPanel.Controls.Add(label1, 0, 3);
            TopPanel.Controls.Add(InputList, 1, 3);
            TopPanel.Controls.Add(InputParam, 2, 3);
            TopPanel.Controls.Add(EditInputFilePath, 3, 3);
            TopPanel.Controls.Add(BrowseInputFilePath, 4, 3);
            TopPanel.Controls.Add(BrowseTemplateFolderPathButton, 4, 5);
            TopPanel.Controls.Add(TemplateFolderPath, 1, 5);
            TopPanel.Controls.Add(CloneTemplateButton, 3, 5);
            TopPanel.Controls.Add(SliceAt, 1, 6);
            TopPanel.Controls.Add(DiceAt, 1, 7);
            TopPanel.Controls.Add(QuickScriptName, 1, 2);
            TopPanel.Controls.Add(CloneScriptButton, 3, 2);
            TopPanel.Controls.Add(DeleteScriptButton, 4, 2);
            TopPanel.Controls.Add(label6, 0, 1);
            TopPanel.Controls.Add(label7, 0, 8);
            TopPanel.Controls.Add(GitHubButton, 4, 7);
            TopPanel.Controls.Add(FeedbackButton, 3, 7);
            TopPanel.Controls.Add(ScriptEditorHelpButton, 4, 8);
            TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            TopPanel.ForeColor = System.Drawing.Color.WhiteSmoke;
            TopPanel.Location = new System.Drawing.Point(426, 0);
            TopPanel.Name = "TopPanel";
            TopPanel.RowCount = 9;
            TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            TopPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            TopPanel.Size = new System.Drawing.Size(818, 250);
            TopPanel.TabIndex = 25;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label10.Location = new System.Drawing.Point(3, 34);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(52, 18);
            label10.TabIndex = 41;
            label10.Text = "Name:";
            // 
            // label14
            // 
            label14.Dock = System.Windows.Forms.DockStyle.Fill;
            label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label14.Location = new System.Drawing.Point(3, 184);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(79, 30);
            label14.TabIndex = 32;
            label14.Text = "Dice at:";
            // 
            // label2
            // 
            label2.Dock = System.Windows.Forms.DockStyle.Fill;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(3, 154);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(79, 30);
            label2.TabIndex = 27;
            label2.Text = "Slice at:";
            // 
            // label5
            // 
            label5.Dock = System.Windows.Forms.DockStyle.Fill;
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label5.Location = new System.Drawing.Point(3, 124);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(79, 30);
            label5.TabIndex = 14;
            label5.Text = "Template:";
            // 
            // BrowseDestinationFilePath
            // 
            BrowseDestinationFilePath.BackColor = System.Drawing.Color.PaleTurquoise;
            BrowseDestinationFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            BrowseDestinationFilePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BrowseDestinationFilePath.Image = Properties.Resources.file_search_fill;
            BrowseDestinationFilePath.Location = new System.Drawing.Point(771, 97);
            BrowseDestinationFilePath.Name = "BrowseDestinationFilePath";
            BrowseDestinationFilePath.Size = new System.Drawing.Size(40, 24);
            BrowseDestinationFilePath.TabIndex = 11;
            toolTip1.SetToolTip(BrowseDestinationFilePath, "Browse for output filename");
            BrowseDestinationFilePath.UseVisualStyleBackColor = false;
            BrowseDestinationFilePath.Click += BrowseDestinationFilePath_Click;
            // 
            // EditDestinationFilePath
            // 
            EditDestinationFilePath.BackColor = System.Drawing.Color.PaleTurquoise;
            EditDestinationFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            EditDestinationFilePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            EditDestinationFilePath.ForeColor = System.Drawing.Color.Black;
            EditDestinationFilePath.Location = new System.Drawing.Point(720, 97);
            EditDestinationFilePath.Name = "EditDestinationFilePath";
            EditDestinationFilePath.Size = new System.Drawing.Size(45, 24);
            EditDestinationFilePath.TabIndex = 10;
            EditDestinationFilePath.Text = "Edit";
            toolTip1.SetToolTip(EditDestinationFilePath, "Edit the selected output file in notepad");
            EditDestinationFilePath.UseVisualStyleBackColor = false;
            EditDestinationFilePath.Click += EditDestinationFilePath_Click;
            // 
            // DestinationParam
            // 
            DestinationParam.Dock = System.Windows.Forms.DockStyle.Fill;
            DestinationParam.Location = new System.Drawing.Point(196, 97);
            DestinationParam.Name = "DestinationParam";
            DestinationParam.Size = new System.Drawing.Size(518, 20);
            DestinationParam.TabIndex = 9;
            DestinationParam.Enter += DestinationParam_Enter;
            // 
            // DestinationList
            // 
            DestinationList.Dock = System.Windows.Forms.DockStyle.Fill;
            DestinationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            DestinationList.FormattingEnabled = true;
            DestinationList.Items.AddRange(new object[] { "File", "Folder", "Clipboard", "Text Box", "Notepad" });
            DestinationList.Location = new System.Drawing.Point(88, 97);
            DestinationList.Name = "DestinationList";
            DestinationList.Size = new System.Drawing.Size(102, 21);
            DestinationList.TabIndex = 8;
            DestinationList.SelectedIndexChanged += DestinationList_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.Dock = System.Windows.Forms.DockStyle.Fill;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(3, 94);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(79, 30);
            label3.TabIndex = 5;
            label3.Text = "Output:";
            // 
            // label1
            // 
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(3, 64);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(79, 30);
            label1.TabIndex = 0;
            label1.Text = "Input:";
            // 
            // InputList
            // 
            InputList.Dock = System.Windows.Forms.DockStyle.Fill;
            InputList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            InputList.FormattingEnabled = true;
            InputList.Items.AddRange(new object[] { "File", "File pattern", "Folder", "Clipboard", "Database Query", "Web Address", "None" });
            InputList.Location = new System.Drawing.Point(88, 67);
            InputList.Name = "InputList";
            InputList.Size = new System.Drawing.Size(102, 21);
            InputList.TabIndex = 4;
            InputList.SelectedIndexChanged += InputList_SelectedIndexChanged;
            // 
            // InputParam
            // 
            InputParam.Dock = System.Windows.Forms.DockStyle.Fill;
            InputParam.Location = new System.Drawing.Point(196, 67);
            InputParam.Name = "InputParam";
            InputParam.Size = new System.Drawing.Size(518, 20);
            InputParam.TabIndex = 5;
            InputParam.Enter += InputParam_Enter;
            InputParam.Leave += InputParam_Leave;
            // 
            // EditInputFilePath
            // 
            EditInputFilePath.BackColor = System.Drawing.Color.PaleTurquoise;
            EditInputFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            EditInputFilePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            EditInputFilePath.ForeColor = System.Drawing.Color.Black;
            EditInputFilePath.Location = new System.Drawing.Point(720, 67);
            EditInputFilePath.Name = "EditInputFilePath";
            EditInputFilePath.Size = new System.Drawing.Size(45, 24);
            EditInputFilePath.TabIndex = 6;
            EditInputFilePath.Text = "Edit";
            toolTip1.SetToolTip(EditInputFilePath, "Edit the selected input file in notepad");
            EditInputFilePath.UseVisualStyleBackColor = false;
            EditInputFilePath.Click += EditInputFilePath_Click;
            // 
            // BrowseInputFilePath
            // 
            BrowseInputFilePath.BackColor = System.Drawing.Color.PaleTurquoise;
            BrowseInputFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            BrowseInputFilePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BrowseInputFilePath.Image = Properties.Resources.file_search_fill;
            BrowseInputFilePath.Location = new System.Drawing.Point(771, 67);
            BrowseInputFilePath.Name = "BrowseInputFilePath";
            BrowseInputFilePath.Size = new System.Drawing.Size(40, 24);
            BrowseInputFilePath.TabIndex = 7;
            toolTip1.SetToolTip(BrowseInputFilePath, "Browse for input filename");
            BrowseInputFilePath.UseVisualStyleBackColor = false;
            BrowseInputFilePath.Click += BrowseInputFilePath_Click;
            // 
            // BrowseTemplateFolderPathButton
            // 
            BrowseTemplateFolderPathButton.BackColor = System.Drawing.Color.PaleTurquoise;
            BrowseTemplateFolderPathButton.Dock = System.Windows.Forms.DockStyle.Fill;
            BrowseTemplateFolderPathButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BrowseTemplateFolderPathButton.Image = Properties.Resources.folder_4_fill;
            BrowseTemplateFolderPathButton.Location = new System.Drawing.Point(771, 127);
            BrowseTemplateFolderPathButton.Name = "BrowseTemplateFolderPathButton";
            BrowseTemplateFolderPathButton.Size = new System.Drawing.Size(40, 24);
            BrowseTemplateFolderPathButton.TabIndex = 14;
            toolTip1.SetToolTip(BrowseTemplateFolderPathButton, "Browse for the folder containing the template you want to use with this script (or set text box to 'Exe' for the default template)");
            BrowseTemplateFolderPathButton.UseVisualStyleBackColor = false;
            BrowseTemplateFolderPathButton.Click += BrowseTemplateFolderPathButton_Click;
            // 
            // TemplateFolderPath
            // 
            TopPanel.SetColumnSpan(TemplateFolderPath, 2);
            TemplateFolderPath.Dock = System.Windows.Forms.DockStyle.Fill;
            TemplateFolderPath.Location = new System.Drawing.Point(88, 127);
            TemplateFolderPath.Name = "TemplateFolderPath";
            TemplateFolderPath.Size = new System.Drawing.Size(626, 20);
            TemplateFolderPath.TabIndex = 12;
            // 
            // CloneTemplateButton
            // 
            CloneTemplateButton.BackColor = System.Drawing.Color.PaleTurquoise;
            CloneTemplateButton.Dock = System.Windows.Forms.DockStyle.Top;
            CloneTemplateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CloneTemplateButton.ForeColor = System.Drawing.Color.Black;
            CloneTemplateButton.Location = new System.Drawing.Point(720, 127);
            CloneTemplateButton.Name = "CloneTemplateButton";
            CloneTemplateButton.Size = new System.Drawing.Size(45, 23);
            CloneTemplateButton.TabIndex = 13;
            CloneTemplateButton.Text = "Clone";
            toolTip1.SetToolTip(CloneTemplateButton, "Copy the current template folder to a new folder and set the current script's template to the new folder");
            CloneTemplateButton.UseVisualStyleBackColor = false;
            CloneTemplateButton.Click += CloneTemplateButton_Click;
            // 
            // SliceAt
            // 
            SliceAt.FormattingEnabled = true;
            SliceAt.Items.AddRange(new object[] { "End of line", "Equal sign", "Tab", "Pipe", "Space" });
            SliceAt.Location = new System.Drawing.Point(88, 157);
            SliceAt.Name = "SliceAt";
            SliceAt.Size = new System.Drawing.Size(102, 21);
            SliceAt.TabIndex = 15;
            SliceAt.SelectedIndexChanged += SliceAt_SelectedIndexChanged;
            // 
            // DiceAt
            // 
            DiceAt.FormattingEnabled = true;
            DiceAt.Items.AddRange(new object[] { "Space", "Tab", "Equal sign", "Pipe", "End of line" });
            DiceAt.Location = new System.Drawing.Point(88, 187);
            DiceAt.Name = "DiceAt";
            DiceAt.Size = new System.Drawing.Size(102, 21);
            DiceAt.TabIndex = 16;
            DiceAt.SelectedIndexChanged += DiceAt_SelectedIndexChanged;
            // 
            // QuickScriptName
            // 
            QuickScriptName.BackColor = System.Drawing.SystemColors.Control;
            TopPanel.SetColumnSpan(QuickScriptName, 2);
            QuickScriptName.Dock = System.Windows.Forms.DockStyle.Fill;
            QuickScriptName.Location = new System.Drawing.Point(88, 37);
            QuickScriptName.Name = "QuickScriptName";
            QuickScriptName.Size = new System.Drawing.Size(626, 20);
            QuickScriptName.TabIndex = 2;
            // 
            // CloneScriptButton
            // 
            CloneScriptButton.BackColor = System.Drawing.Color.PaleTurquoise;
            CloneScriptButton.Dock = System.Windows.Forms.DockStyle.Fill;
            CloneScriptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CloneScriptButton.ForeColor = System.Drawing.Color.Black;
            CloneScriptButton.Location = new System.Drawing.Point(720, 37);
            CloneScriptButton.Name = "CloneScriptButton";
            CloneScriptButton.Size = new System.Drawing.Size(45, 24);
            CloneScriptButton.TabIndex = 3;
            CloneScriptButton.Text = "Clone";
            toolTip1.SetToolTip(CloneScriptButton, "Copy the current script to a new script with a new name");
            CloneScriptButton.UseVisualStyleBackColor = false;
            CloneScriptButton.Click += CloneScriptButton_Click;
            // 
            // DeleteScriptButton
            // 
            DeleteScriptButton.BackColor = System.Drawing.Color.Pink;
            DeleteScriptButton.Dock = System.Windows.Forms.DockStyle.Fill;
            DeleteScriptButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            DeleteScriptButton.Image = Properties.Resources.delete_bin_2_fill;
            DeleteScriptButton.Location = new System.Drawing.Point(770, 36);
            DeleteScriptButton.Margin = new System.Windows.Forms.Padding(2);
            DeleteScriptButton.Name = "DeleteScriptButton";
            DeleteScriptButton.Size = new System.Drawing.Size(42, 26);
            DeleteScriptButton.TabIndex = 9999999;
            DeleteScriptButton.TabStop = false;
            toolTip1.SetToolTip(DeleteScriptButton, "Delete this script");
            DeleteScriptButton.UseVisualStyleBackColor = false;
            DeleteScriptButton.Click += DeleteScript_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            TopPanel.SetColumnSpan(label6, 3);
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label6.Location = new System.Drawing.Point(0, 4);
            label6.Margin = new System.Windows.Forms.Padding(0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(190, 26);
            label6.TabIndex = 2;
            label6.Text = "Script Properties";
            // 
            // label7
            // 
            label7.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label7.AutoSize = true;
            TopPanel.SetColumnSpan(label7, 4);
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label7.Location = new System.Drawing.Point(0, 224);
            label7.Margin = new System.Windows.Forms.Padding(0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(144, 26);
            label7.TabIndex = 2;
            label7.Text = "Script Editor";
            // 
            // GitHubButton
            // 
            GitHubButton.BackColor = System.Drawing.Color.Lavender;
            GitHubButton.Dock = System.Windows.Forms.DockStyle.Fill;
            GitHubButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            GitHubButton.ForeColor = System.Drawing.Color.White;
            GitHubButton.Image = Properties.Resources.github_fill;
            GitHubButton.Location = new System.Drawing.Point(769, 185);
            GitHubButton.Margin = new System.Windows.Forms.Padding(1);
            GitHubButton.Name = "GitHubButton";
            GitHubButton.Size = new System.Drawing.Size(44, 28);
            GitHubButton.TabIndex = 45;
            GitHubButton.TabStop = false;
            GitHubButton.UseVisualStyleBackColor = false;
            GitHubButton.Click += GitHubButton_Click;
            // 
            // FeedbackButton
            // 
            FeedbackButton.BackColor = System.Drawing.Color.Lavender;
            FeedbackButton.Dock = System.Windows.Forms.DockStyle.Fill;
            FeedbackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            FeedbackButton.ForeColor = System.Drawing.Color.White;
            FeedbackButton.Image = Properties.Resources.feedback_fill;
            FeedbackButton.Location = new System.Drawing.Point(718, 185);
            FeedbackButton.Margin = new System.Windows.Forms.Padding(1);
            FeedbackButton.Name = "FeedbackButton";
            FeedbackButton.Size = new System.Drawing.Size(49, 28);
            FeedbackButton.TabIndex = 46;
            FeedbackButton.TabStop = false;
            FeedbackButton.UseVisualStyleBackColor = false;
            FeedbackButton.Click += FeedbackButton_Click;
            // 
            // ScriptEditorHelpButton
            // 
            ScriptEditorHelpButton.BackColor = System.Drawing.Color.Lavender;
            ScriptEditorHelpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            ScriptEditorHelpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ScriptEditorHelpButton.ForeColor = System.Drawing.Color.White;
            ScriptEditorHelpButton.Image = Properties.Resources.question_fill;
            ScriptEditorHelpButton.Location = new System.Drawing.Point(769, 215);
            ScriptEditorHelpButton.Margin = new System.Windows.Forms.Padding(1);
            ScriptEditorHelpButton.Name = "ScriptEditorHelpButton";
            ScriptEditorHelpButton.Size = new System.Drawing.Size(44, 34);
            ScriptEditorHelpButton.TabIndex = 17;
            ScriptEditorHelpButton.UseVisualStyleBackColor = false;
            ScriptEditorHelpButton.Click += ScriptEditorHelpButton_Click;
            // 
            // LeftPanel
            // 
            LeftPanel.ColumnCount = 4;
            LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.49161F));
            LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.50839F));
            LeftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            LeftPanel.Controls.Add(QuickScriptList, 2, 2);
            LeftPanel.Controls.Add(ActionPanel, 1, 2);
            LeftPanel.Controls.Add(ScriptNameLabel, 2, 1);
            LeftPanel.Controls.Add(label4, 1, 1);
            LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            LeftPanel.Location = new System.Drawing.Point(0, 0);
            LeftPanel.Name = "LeftPanel";
            LeftPanel.RowCount = 4;
            LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            LeftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            LeftPanel.Size = new System.Drawing.Size(426, 776);
            LeftPanel.TabIndex = 26;
            // 
            // QuickScriptList
            // 
            QuickScriptList.BackColor = System.Drawing.Color.FromArgb(0, 77, 134);
            QuickScriptList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            QuickScriptList.Dock = System.Windows.Forms.DockStyle.Fill;
            QuickScriptList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            QuickScriptList.ForeColor = System.Drawing.Color.White;
            QuickScriptList.ItemHeight = 20;
            QuickScriptList.Items.AddRange(new object[] { "<Empty this should never be seen>" });
            QuickScriptList.Location = new System.Drawing.Point(158, 35);
            QuickScriptList.Margin = new System.Windows.Forms.Padding(6);
            QuickScriptList.Name = "QuickScriptList";
            QuickScriptList.Size = new System.Drawing.Size(257, 710);
            QuickScriptList.TabIndex = 1;
            QuickScriptList.SelectedIndexChanged += QuickScriptList_SelectedIndexChanged;
            // 
            // ActionPanel
            // 
            ActionPanel.BackColor = System.Drawing.Color.FromArgb(0, 0, 70);
            ActionPanel.ColumnCount = 1;
            ActionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            ActionPanel.Controls.Add(linkLabel1, 0, 0);
            ActionPanel.Controls.Add(RunQuickScriptLink, 0, 1);
            ActionPanel.Controls.Add(SaveLink, 0, 11);
            ActionPanel.Controls.Add(SaveAsLink, 0, 12);
            ActionPanel.Controls.Add(OpenLink, 0, 13);
            ActionPanel.Controls.Add(NewFileLink, 0, 14);
            ActionPanel.Controls.Add(FindLink, 0, 6);
            ActionPanel.Controls.Add(ReplaceLink, 0, 7);
            ActionPanel.Controls.Add(NewScriptLink, 0, 9);
            ActionPanel.Controls.Add(button1, 0, 18);
            ActionPanel.Controls.Add(RestageTemplatesButton, 0, 16);
            ActionPanel.Controls.Add(PostBuildActionsLiink, 0, 2);
            ActionPanel.Controls.Add(BuildExeLink, 0, 3);
            ActionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            ActionPanel.Location = new System.Drawing.Point(7, 32);
            ActionPanel.Name = "ActionPanel";
            ActionPanel.RowCount = 19;
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            ActionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            ActionPanel.Size = new System.Drawing.Size(142, 716);
            ActionPanel.TabIndex = 1;
            ActionPanel.Click += ActionPanel_Click;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            linkLabel1.LinkColor = System.Drawing.Color.Aqua;
            linkLabel1.Location = new System.Drawing.Point(3, 0);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            linkLabel1.Size = new System.Drawing.Size(65, 10);
            linkLabel1.TabIndex = 9;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "New file";
            // 
            // RunQuickScriptLink
            // 
            RunQuickScriptLink.AutoSize = true;
            RunQuickScriptLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            RunQuickScriptLink.ForeColor = System.Drawing.Color.White;
            RunQuickScriptLink.LinkColor = System.Drawing.Color.Yellow;
            RunQuickScriptLink.Location = new System.Drawing.Point(3, 10);
            RunQuickScriptLink.Name = "RunQuickScriptLink";
            RunQuickScriptLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            RunQuickScriptLink.Size = new System.Drawing.Size(130, 24);
            RunQuickScriptLink.TabIndex = 1;
            RunQuickScriptLink.TabStop = true;
            RunQuickScriptLink.Text = "Run current script";
            toolTip1.SetToolTip(RunQuickScriptLink, "F5");
            RunQuickScriptLink.Click += RunQuickScript_Click;
            // 
            // SaveLink
            // 
            SaveLink.AutoSize = true;
            SaveLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            SaveLink.LinkColor = System.Drawing.Color.Aqua;
            SaveLink.Location = new System.Drawing.Point(3, 290);
            SaveLink.Name = "SaveLink";
            SaveLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            SaveLink.Size = new System.Drawing.Size(108, 24);
            SaveLink.TabIndex = 7;
            SaveLink.TabStop = true;
            SaveLink.Text = "Save file (xlgq)";
            toolTip1.SetToolTip(SaveLink, "Ctrl-S");
            SaveLink.Click += SaveQuickScriptFile_Click;
            // 
            // SaveAsLink
            // 
            SaveAsLink.AutoSize = true;
            SaveAsLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            SaveAsLink.LinkColor = System.Drawing.Color.Aqua;
            SaveAsLink.Location = new System.Drawing.Point(3, 320);
            SaveAsLink.Name = "SaveAsLink";
            SaveAsLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            SaveAsLink.Size = new System.Drawing.Size(66, 24);
            SaveAsLink.TabIndex = 7;
            SaveAsLink.TabStop = true;
            SaveAsLink.Text = "Save as";
            toolTip1.SetToolTip(SaveAsLink, "Ctrl+A");
            SaveAsLink.Click += SaveAs_Click;
            // 
            // OpenLink
            // 
            OpenLink.AutoSize = true;
            OpenLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            OpenLink.LinkColor = System.Drawing.Color.Aqua;
            OpenLink.Location = new System.Drawing.Point(3, 350);
            OpenLink.Name = "OpenLink";
            OpenLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            OpenLink.Size = new System.Drawing.Size(71, 24);
            OpenLink.TabIndex = 7;
            OpenLink.TabStop = true;
            OpenLink.Text = "Open file";
            toolTip1.SetToolTip(OpenLink, "Ctrl+O");
            OpenLink.Click += OpenScriptFile_Click;
            // 
            // NewFileLink
            // 
            NewFileLink.AutoSize = true;
            NewFileLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            NewFileLink.LinkColor = System.Drawing.Color.Aqua;
            NewFileLink.Location = new System.Drawing.Point(3, 380);
            NewFileLink.Name = "NewFileLink";
            NewFileLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            NewFileLink.Size = new System.Drawing.Size(65, 24);
            NewFileLink.TabIndex = 7;
            NewFileLink.TabStop = true;
            NewFileLink.Text = "New file";
            NewFileLink.Click += NewScriptFile_Click;
            // 
            // FindLink
            // 
            FindLink.AutoSize = true;
            FindLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FindLink.LinkColor = System.Drawing.Color.White;
            FindLink.Location = new System.Drawing.Point(3, 148);
            FindLink.Name = "FindLink";
            FindLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            FindLink.Size = new System.Drawing.Size(106, 24);
            FindLink.TabIndex = 7;
            FindLink.TabStop = true;
            FindLink.Text = "Find / highlight";
            toolTip1.SetToolTip(FindLink, "Ctrl+F\r\nFind string in current script \r\nAnd optionally highlight all occurences");
            FindLink.Click += FindMenuItem_Click;
            // 
            // ReplaceLink
            // 
            ReplaceLink.AutoSize = true;
            ReplaceLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ReplaceLink.LinkColor = System.Drawing.Color.White;
            ReplaceLink.Location = new System.Drawing.Point(3, 174);
            ReplaceLink.Name = "ReplaceLink";
            ReplaceLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            ReplaceLink.Size = new System.Drawing.Size(67, 24);
            ReplaceLink.TabIndex = 7;
            ReplaceLink.TabStop = true;
            ReplaceLink.Text = "Replace";
            toolTip1.SetToolTip(ReplaceLink, "Ctrl+R\r\nReplace string in current script");
            ReplaceLink.Click += ReplaceMenuItem_Click;
            // 
            // NewScriptLink
            // 
            NewScriptLink.AutoSize = true;
            NewScriptLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            NewScriptLink.ForeColor = System.Drawing.Color.White;
            NewScriptLink.LinkColor = System.Drawing.Color.Yellow;
            NewScriptLink.Location = new System.Drawing.Point(3, 230);
            NewScriptLink.Name = "NewScriptLink";
            NewScriptLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            NewScriptLink.Size = new System.Drawing.Size(78, 24);
            NewScriptLink.TabIndex = 0;
            NewScriptLink.TabStop = true;
            NewScriptLink.Text = "Add script";
            toolTip1.SetToolTip(NewScriptLink, "Ctrl+N");
            NewScriptLink.Click += NewQuickScript_Click;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(3, 521);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 8;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // RestageTemplatesButton
            // 
            RestageTemplatesButton.AutoSize = true;
            RestageTemplatesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            RestageTemplatesButton.LinkColor = System.Drawing.Color.Violet;
            RestageTemplatesButton.Location = new System.Drawing.Point(3, 430);
            RestageTemplatesButton.Name = "RestageTemplatesButton";
            RestageTemplatesButton.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            RestageTemplatesButton.Size = new System.Drawing.Size(136, 24);
            RestageTemplatesButton.TabIndex = 10;
            RestageTemplatesButton.TabStop = true;
            RestageTemplatesButton.Text = "Restage templates";
            RestageTemplatesButton.LinkClicked += RestageTemplatesButton_LinkClicked;
            // 
            // PostBuildActionsLiink
            // 
            PostBuildActionsLiink.AutoSize = true;
            PostBuildActionsLiink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            PostBuildActionsLiink.ForeColor = System.Drawing.Color.White;
            PostBuildActionsLiink.LinkColor = System.Drawing.Color.Yellow;
            PostBuildActionsLiink.Location = new System.Drawing.Point(3, 40);
            PostBuildActionsLiink.Name = "PostBuildActionsLiink";
            PostBuildActionsLiink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            PostBuildActionsLiink.Size = new System.Drawing.Size(130, 24);
            PostBuildActionsLiink.TabIndex = 3;
            PostBuildActionsLiink.TabStop = true;
            PostBuildActionsLiink.Text = "Post build actions";
            toolTip1.SetToolTip(PostBuildActionsLiink, "F12");
            PostBuildActionsLiink.Click += PostBuild_Click;
            // 
            // BuildExeLink
            // 
            BuildExeLink.AutoSize = true;
            BuildExeLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            BuildExeLink.ForeColor = System.Drawing.Color.White;
            BuildExeLink.LinkColor = System.Drawing.Color.Yellow;
            BuildExeLink.Location = new System.Drawing.Point(3, 70);
            BuildExeLink.Name = "BuildExeLink";
            BuildExeLink.Padding = new System.Windows.Forms.Padding(5, 6, 0, 0);
            BuildExeLink.Size = new System.Drawing.Size(131, 24);
            BuildExeLink.TabIndex = 2;
            BuildExeLink.TabStop = true;
            BuildExeLink.Text = "Build cmd line exe";
            toolTip1.SetToolTip(BuildExeLink, "F6");
            BuildExeLink.Click += BuildExe_Click;
            // 
            // ScriptNameLabel
            // 
            ScriptNameLabel.AutoSize = true;
            ScriptNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            ScriptNameLabel.Location = new System.Drawing.Point(152, 4);
            ScriptNameLabel.Margin = new System.Windows.Forms.Padding(0);
            ScriptNameLabel.Name = "ScriptNameLabel";
            ScriptNameLabel.Size = new System.Drawing.Size(86, 25);
            ScriptNameLabel.TabIndex = 2;
            ScriptNameLabel.Text = "Scripts";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label4.Location = new System.Drawing.Point(4, 4);
            label4.Margin = new System.Windows.Forms.Padding(0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(91, 25);
            label4.TabIndex = 2;
            label4.Text = "Actions";
            // 
            // ScriptNameColumn
            // 
            ScriptNameColumn.Text = "Script Name";
            ScriptNameColumn.Width = 500;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(426, 250);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(818, 4);
            panel1.TabIndex = 27;
            // 
            // FolderBrowserDialog
            // 
            FolderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyDocuments;
            FolderBrowserDialog.SelectedPath = "XLG\\Scripts\\";
            FolderBrowserDialog.UseDescriptionForTitle = true;
            // 
            // QuickScriptEditor
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.DimGray;
            ClientSize = new System.Drawing.Size(1244, 776);
            Controls.Add(ScriptEditor);
            Controls.Add(panel1);
            Controls.Add(TopPanel);
            Controls.Add(statusStrip1);
            Controls.Add(LeftPanel);
            ForeColor = System.Drawing.Color.WhiteSmoke;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MinimumSize = new System.Drawing.Size(1060, 600);
            Name = "QuickScriptEditor";
            Text = "Qk Scrptr";
            FormClosing += QuickScriptEditor_FormClosing;
            ResizeEnd += QuickScriptEditor_ResizeEnd;
            KeyUp += QuickScriptEditor_KeyUp;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            TopPanel.ResumeLayout(false);
            TopPanel.PerformLayout();
            LeftPanel.ResumeLayout(false);
            LeftPanel.PerformLayout();
            ActionPanel.ResumeLayout(false);
            ActionPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel RestageTemplatesButton;
    }
}