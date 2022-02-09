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
            this.OpenInputFilePathDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveDestinationFilePathDialog = new System.Windows.Forms.SaveFileDialog();
            this.MainStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
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
            this.postToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testFuncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.RunningLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ScriptEditor = new MetX.Controls.QuickScriptControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.QuickScriptList = new System.Windows.Forms.ListView();
            this.ScriptNameColumn = new System.Windows.Forms.ColumnHeader();
            this.label5 = new System.Windows.Forms.Label();
            this.BrowseTemplateFolderPath = new System.Windows.Forms.Button();
            this.TemplateFolderPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.MainStrip.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenInputFilePathDialog
            // 
            this.OpenInputFilePathDialog.FileName = "openFileDialog1";
            // 
            // MainStrip
            // 
            this.MainStrip.BackColor = System.Drawing.Color.DarkGray;
            this.MainStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MainStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.MainStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.MainStrip.Location = new System.Drawing.Point(390, 32);
            this.MainStrip.Name = "MainStrip";
            this.MainStrip.Size = new System.Drawing.Size(877, 31);
            this.MainStrip.TabIndex = 1;
            this.MainStrip.Text = "toolStrip3";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(187, 6);
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
            this.TemplateList.Name = "TemplateList";
            this.TemplateList.Size = new System.Drawing.Size(130, 28);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(187, 6);
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
            this.toolStripSeparator11.Size = new System.Drawing.Size(187, 6);
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
            this.MainMenu.BackColor = System.Drawing.Color.DarkGray;
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.postToolStripMenuItem,
            this.findMenuItem,
            this.replaceMenuItem,
            this.testFuncToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(390, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(877, 32);
            this.MainMenu.TabIndex = 22;
            this.MainMenu.Text = "menuStrip1";
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
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
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
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
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
            this.BuildExeMenuItem.Click += new System.EventHandler(this.BuildExe_Click);
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
            this.toolStripMenuItem5.Click += new System.EventHandler(this.BuildExe_Click);
            // 
            // postToolStripMenuItem
            // 
            this.postToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("postToolStripMenuItem.Image")));
            this.postToolStripMenuItem.Name = "postToolStripMenuItem";
            this.postToolStripMenuItem.Size = new System.Drawing.Size(72, 28);
            this.postToolStripMenuItem.Text = "&Post";
            this.postToolStripMenuItem.Click += new System.EventHandler(this.postToolStripMenuItem_Click);
            // 
            // findMenuItem
            // 
            this.findMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("findMenuItem.Image")));
            this.findMenuItem.Name = "findMenuItem";
            this.findMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findMenuItem.Size = new System.Drawing.Size(73, 28);
            this.findMenuItem.Text = "Find";
            this.findMenuItem.Click += new System.EventHandler(this.FindMenuItem_Click);
            // 
            // replaceMenuItem
            // 
            this.replaceMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("replaceMenuItem.Image")));
            this.replaceMenuItem.Name = "replaceMenuItem";
            this.replaceMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceMenuItem.Size = new System.Drawing.Size(98, 28);
            this.replaceMenuItem.Text = "Replace";
            this.replaceMenuItem.Click += new System.EventHandler(this.ReplaceMenuItem_Click);
            // 
            // testFuncToolStripMenuItem
            // 
            this.testFuncToolStripMenuItem.Name = "testFuncToolStripMenuItem";
            this.testFuncToolStripMenuItem.Size = new System.Drawing.Size(79, 28);
            this.testFuncToolStripMenuItem.Text = "Test func";
            this.testFuncToolStripMenuItem.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunningLabel,
            this.toolStripStatusLabel2,
            this.ProgressLabel});
            this.statusStrip1.Location = new System.Drawing.Point(390, 621);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(877, 25);
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
            this.ScriptEditor.Location = new System.Drawing.Point(390, 303);
            this.ScriptEditor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ScriptEditor.Name = "ScriptEditor";
            this.ScriptEditor.Size = new System.Drawing.Size(877, 318);
            this.ScriptEditor.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 549F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.BrowseDestinationFilePath, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.EditDestinationFilePath, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.DestinationParam, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.DestinationList, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.InputList, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.InputParam, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.EditInputFilePath, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.BrowseInputFilePath, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.BrowseTemplateFolderPath, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.TemplateFolderPath, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.button1, 3, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(390, 63);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(877, 240);
            this.tableLayoutPanel1.TabIndex = 25;
            // 
            // BrowseDestinationFilePath
            // 
            this.BrowseDestinationFilePath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BrowseDestinationFilePath.Location = new System.Drawing.Point(795, 63);
            this.BrowseDestinationFilePath.Name = "BrowseDestinationFilePath";
            this.BrowseDestinationFilePath.Size = new System.Drawing.Size(29, 23);
            this.BrowseDestinationFilePath.TabIndex = 10;
            this.BrowseDestinationFilePath.Text = "...";
            this.BrowseDestinationFilePath.UseVisualStyleBackColor = true;
            this.BrowseDestinationFilePath.Click += new System.EventHandler(this.BrowseDestinationFilePath_Click);
            // 
            // EditDestinationFilePath
            // 
            this.EditDestinationFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditDestinationFilePath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.EditDestinationFilePath.Location = new System.Drawing.Point(745, 63);
            this.EditDestinationFilePath.Name = "EditDestinationFilePath";
            this.EditDestinationFilePath.Size = new System.Drawing.Size(44, 24);
            this.EditDestinationFilePath.TabIndex = 9;
            this.EditDestinationFilePath.Text = "Edit";
            this.EditDestinationFilePath.UseVisualStyleBackColor = true;
            this.EditDestinationFilePath.Click += new System.EventHandler(this.EditDestinationFilePath_Click);
            // 
            // DestinationParam
            // 
            this.DestinationParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DestinationParam.Location = new System.Drawing.Point(196, 63);
            this.DestinationParam.Name = "DestinationParam";
            this.DestinationParam.Size = new System.Drawing.Size(543, 20);
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
            this.DestinationList.Location = new System.Drawing.Point(88, 63);
            this.DestinationList.Name = "DestinationList";
            this.DestinationList.Size = new System.Drawing.Size(102, 21);
            this.DestinationList.TabIndex = 6;
            this.DestinationList.SelectedIndexChanged += new System.EventHandler(this.DestinationList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(3, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 30);
            this.label3.TabIndex = 5;
            this.label3.Text = "Output:";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 30);
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
            this.InputList.Location = new System.Drawing.Point(88, 33);
            this.InputList.Name = "InputList";
            this.InputList.Size = new System.Drawing.Size(102, 21);
            this.InputList.TabIndex = 1;
            this.InputList.SelectedIndexChanged += new System.EventHandler(this.InputList_SelectedIndexChanged);
            // 
            // InputParam
            // 
            this.InputParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputParam.Location = new System.Drawing.Point(196, 33);
            this.InputParam.Name = "InputParam";
            this.InputParam.Size = new System.Drawing.Size(543, 20);
            this.InputParam.TabIndex = 3;
            this.InputParam.Enter += new System.EventHandler(this.InputParam_Enter);
            this.InputParam.Leave += new System.EventHandler(this.InputParam_Leave);
            this.InputParam.MouseUp += new System.Windows.Forms.MouseEventHandler(this.InputParam_MouseUp);
            // 
            // EditInputFilePath
            // 
            this.EditInputFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditInputFilePath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.EditInputFilePath.Location = new System.Drawing.Point(745, 33);
            this.EditInputFilePath.Name = "EditInputFilePath";
            this.EditInputFilePath.Size = new System.Drawing.Size(44, 24);
            this.EditInputFilePath.TabIndex = 4;
            this.EditInputFilePath.Text = "Edit";
            this.EditInputFilePath.UseVisualStyleBackColor = true;
            this.EditInputFilePath.Click += new System.EventHandler(this.EditInputFilePath_Click);
            // 
            // BrowseInputFilePath
            // 
            this.BrowseInputFilePath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BrowseInputFilePath.Location = new System.Drawing.Point(795, 33);
            this.BrowseInputFilePath.Name = "BrowseInputFilePath";
            this.BrowseInputFilePath.Size = new System.Drawing.Size(29, 22);
            this.BrowseInputFilePath.TabIndex = 4;
            this.BrowseInputFilePath.Text = "...";
            this.BrowseInputFilePath.UseVisualStyleBackColor = true;
            this.BrowseInputFilePath.Click += new System.EventHandler(this.BrowseInputFilePath_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.17949F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.82051F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.QuickScriptList, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.2381F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(390, 646);
            this.tableLayoutPanel2.TabIndex = 26;
            // 
            // QuickScriptList
            // 
            this.QuickScriptList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ScriptNameColumn});
            this.QuickScriptList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuickScriptList.HideSelection = false;
            this.QuickScriptList.Location = new System.Drawing.Point(103, 43);
            this.QuickScriptList.Name = "QuickScriptList";
            this.QuickScriptList.Size = new System.Drawing.Size(263, 549);
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
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(3, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 30);
            this.label5.TabIndex = 14;
            this.label5.Text = "Template:";
            // 
            // BrowseTemplateFolderPath
            // 
            this.BrowseTemplateFolderPath.Location = new System.Drawing.Point(795, 93);
            this.BrowseTemplateFolderPath.Name = "BrowseTemplateFolderPath";
            this.BrowseTemplateFolderPath.Size = new System.Drawing.Size(29, 23);
            this.BrowseTemplateFolderPath.TabIndex = 17;
            this.BrowseTemplateFolderPath.Text = ",,,";
            this.BrowseTemplateFolderPath.UseVisualStyleBackColor = true;
            // 
            // TemplateFolderPath
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.TemplateFolderPath, 2);
            this.TemplateFolderPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TemplateFolderPath.Location = new System.Drawing.Point(88, 93);
            this.TemplateFolderPath.Name = "TemplateFolderPath";
            this.TemplateFolderPath.Size = new System.Drawing.Size(651, 20);
            this.TemplateFolderPath.TabIndex = 18;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(745, 93);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(44, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Clone";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // QuickScriptEditor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1267, 646);
            this.Controls.Add(this.ScriptEditor);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MainStrip);
            this.Controls.Add(this.MainMenu);
            this.Controls.Add(this.tableLayoutPanel2);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "QuickScriptEditor";
            this.Text = "Qk Scrptr";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QuickScriptEditor_FormClosing);
            this.Load += new System.EventHandler(this.QuickScriptEditor_Load);
            this.ResizeEnd += new System.EventHandler(this.QuickScriptEditor_ResizeEnd);
            this.MainStrip.ResumeLayout(false);
            this.MainStrip.PerformLayout();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog OpenInputFilePathDialog;
        private System.Windows.Forms.SaveFileDialog SaveDestinationFilePathDialog;
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
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox TemplateList;
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
        private System.Windows.Forms.ToolStripMenuItem findMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem postToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testFuncToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ListView QuickScriptList;
        private System.Windows.Forms.ColumnHeader ScriptNameColumn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BrowseTemplateFolderPath;
        private System.Windows.Forms.TextBox TemplateFolderPath;
        private System.Windows.Forms.Button button1;
    }
}