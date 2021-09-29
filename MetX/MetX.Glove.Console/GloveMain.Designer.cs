using System.Windows.Forms;

namespace XLG.Pipeliner
{
    partial class GloveMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GloveMain));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateCheckedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regenerateCheckedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regenerateSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.autoRegenOnChangedXSLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MetadataSources = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textConnectionName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textAppXlgXsl = new System.Windows.Forms.TextBox();
            this.buttonChooseXlgFileXsl = new System.Windows.Forms.Button();
            this.buttonEditXlgXslFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.checkRegenerateOnly = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textOutputXml = new System.Windows.Forms.TextBox();
            this.buttonChoosetextOutputXml = new System.Windows.Forms.Button();
            this.buttonViewtextOutputXml = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textXlgFile = new System.Windows.Forms.TextBox();
            this.buttonChooseXlgFile = new System.Windows.Forms.Button();
            this.buttonEditXlgFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textOutput = new System.Windows.Forms.TextBox();
            this.buttonChooseOutput = new System.Windows.Forms.Button();
            this.buttonViewOutputFolder = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.comboProviderName = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textConnectionStringName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textConnectionString = new System.Windows.Forms.TextBox();
            this.buttonEditConnectionString = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.autoRegenToolbarButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.EditClipScript = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem, 
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(159, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
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
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateCheckedToolStripMenuItem,
            this.generateSelectedToolStripMenuItem,
            this.regenerateCheckedToolStripMenuItem,
            this.regenerateSelectedToolStripMenuItem,
            this.toolStripSeparator2,
            this.autoRegenOnChangedXSLToolStripMenuItem,
            this.toolStripSeparator3,
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.toolsToolStripMenuItem.Text = "S&ettings";
            // 
            // generateCheckedToolStripMenuItem
            // 
            this.generateCheckedToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("generateCheckedToolStripMenuItem.Image")));
            this.generateCheckedToolStripMenuItem.Name = "generateCheckedToolStripMenuItem";
            this.generateCheckedToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.generateCheckedToolStripMenuItem.Text = "&Generate Checked";
            this.generateCheckedToolStripMenuItem.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // generateSelectedToolStripMenuItem
            // 
            this.generateSelectedToolStripMenuItem.Enabled = false;
            this.generateSelectedToolStripMenuItem.Name = "generateSelectedToolStripMenuItem";
            this.generateSelectedToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.generateSelectedToolStripMenuItem.Text = "Generate &Selected";
            this.generateSelectedToolStripMenuItem.Click += new System.EventHandler(this.generateSelectedToolStripMenuItem_Click);
            // 
            // regenerateCheckedToolStripMenuItem
            // 
            this.regenerateCheckedToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("regenerateCheckedToolStripMenuItem.Image")));
            this.regenerateCheckedToolStripMenuItem.Name = "regenerateCheckedToolStripMenuItem";
            this.regenerateCheckedToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.regenerateCheckedToolStripMenuItem.Text = "&Regenerate Checked";
            this.regenerateCheckedToolStripMenuItem.Click += new System.EventHandler(this.buttonRegenerate_Click);
            // 
            // regenerateSelectedToolStripMenuItem
            // 
            this.regenerateSelectedToolStripMenuItem.Enabled = false;
            this.regenerateSelectedToolStripMenuItem.Name = "regenerateSelectedToolStripMenuItem";
            this.regenerateSelectedToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.regenerateSelectedToolStripMenuItem.Text = "Regenerate Selected";
            this.regenerateSelectedToolStripMenuItem.Click += new System.EventHandler(this.regenerateSelectedToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(234, 6);
            // 
            // autoRegenOnChangedXSLToolStripMenuItem
            // 
            this.autoRegenOnChangedXSLToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("autoRegenOnChangedXSLToolStripMenuItem.Image")));
            this.autoRegenOnChangedXSLToolStripMenuItem.Name = "autoRegenOnChangedXSLToolStripMenuItem";
            this.autoRegenOnChangedXSLToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.autoRegenOnChangedXSLToolStripMenuItem.Text = "Auto Regen when XSL changes";
            this.autoRegenOnChangedXSLToolStripMenuItem.Click += new System.EventHandler(this.buttonAutoGen_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(234, 6);
            // 
            // customizeToolStripMenuItem
            // 
            this.customizeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("customizeToolStripMenuItem.Image")));
            this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
            this.customizeToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.customizeToolStripMenuItem.Text = "&Add Step / Database Target";
            this.customizeToolStripMenuItem.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("optionsToolStripMenuItem.Image")));
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.optionsToolStripMenuItem.Text = "Delete Step / Database Target";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // MetadataSources
            // 
            this.MetadataSources.CheckBoxes = true;
            this.MetadataSources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.MetadataSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MetadataSources.FullRowSelect = true;
            this.MetadataSources.GridLines = true;
            this.MetadataSources.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.MetadataSources.HideSelection = false;
            this.MetadataSources.Location = new System.Drawing.Point(0, 0);
            this.MetadataSources.MultiSelect = false;
            this.MetadataSources.Name = "MetadataSources";
            this.MetadataSources.ShowGroups = false;
            this.MetadataSources.Size = new System.Drawing.Size(159, 349);
            this.MetadataSources.TabIndex = 0;
            this.MetadataSources.UseCompatibleStateImageBehavior = false;
            this.MetadataSources.View = System.Windows.Forms.View.Details;
            this.MetadataSources.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.MetadataSources_ItemChecked);
            this.MetadataSources.SelectedIndexChanged += new System.EventHandler(this.MetadataSources_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Steps / Targets";
            this.columnHeader1.Width = 150;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.toolStripContainer1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(907, 377);
            this.splitContainer1.SplitterDistance = 163;
            this.splitContainer1.TabIndex = 8;
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.MetadataSources);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(159, 349);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(159, 373);
            this.toolStripContainer1.TabIndex = 11;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel1.Controls.Add(this.label9);
            this.flowLayoutPanel1.Controls.Add(this.label8);
            this.flowLayoutPanel1.Controls.Add(this.textConnectionName);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.textAppXlgXsl);
            this.flowLayoutPanel1.Controls.Add(this.buttonChooseXlgFileXsl);
            this.flowLayoutPanel1.Controls.Add(this.buttonEditXlgXslFile);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.checkRegenerateOnly);
            this.flowLayoutPanel1.Controls.Add(this.label12);
            this.flowLayoutPanel1.Controls.Add(this.textOutputXml);
            this.flowLayoutPanel1.Controls.Add(this.buttonChoosetextOutputXml);
            this.flowLayoutPanel1.Controls.Add(this.buttonViewtextOutputXml);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.textXlgFile);
            this.flowLayoutPanel1.Controls.Add(this.buttonChooseXlgFile);
            this.flowLayoutPanel1.Controls.Add(this.buttonEditXlgFile);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.textOutput);
            this.flowLayoutPanel1.Controls.Add(this.buttonChooseOutput);
            this.flowLayoutPanel1.Controls.Add(this.buttonViewOutputFolder);
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Controls.Add(this.label13);
            this.flowLayoutPanel1.Controls.Add(this.comboProviderName);
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.flowLayoutPanel1.Controls.Add(this.textConnectionStringName);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.textConnectionString);
            this.flowLayoutPanel1.Controls.Add(this.buttonEditConnectionString);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 31);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(736, 342);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // label9
            // 
            this.flowLayoutPanel1.SetFlowBreak(this.label9, true);
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(111, 20);
            this.label9.TabIndex = 1;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(13, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 20);
            this.label8.TabIndex = 1;
            this.label8.Text = "Step Name";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textConnectionName
            // 
            this.flowLayoutPanel1.SetFlowBreak(this.textConnectionName, true);
            this.textConnectionName.Location = new System.Drawing.Point(130, 33);
            this.textConnectionName.Name = "textConnectionName";
            this.textConnectionName.Size = new System.Drawing.Size(487, 20);
            this.textConnectionName.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "XSL TemplateName";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textAppXlgXsl
            // 
            this.textAppXlgXsl.Location = new System.Drawing.Point(130, 59);
            this.textAppXlgXsl.Name = "textAppXlgXsl";
            this.textAppXlgXsl.Size = new System.Drawing.Size(487, 20);
            this.textAppXlgXsl.TabIndex = 1;
            // 
            // buttonChooseXlgFileXsl
            // 
            this.buttonChooseXlgFileXsl.Location = new System.Drawing.Point(623, 59);
            this.buttonChooseXlgFileXsl.Name = "buttonChooseXlgFileXsl";
            this.buttonChooseXlgFileXsl.Size = new System.Drawing.Size(28, 23);
            this.buttonChooseXlgFileXsl.TabIndex = 2;
            this.buttonChooseXlgFileXsl.Text = "...";
            this.buttonChooseXlgFileXsl.UseVisualStyleBackColor = true;
            this.buttonChooseXlgFileXsl.Click += new System.EventHandler(this.buttonChooseXlgFileXsl_Click);
            // 
            // buttonEditXlgXslFile
            // 
            this.flowLayoutPanel1.SetFlowBreak(this.buttonEditXlgXslFile, true);
            this.buttonEditXlgXslFile.Location = new System.Drawing.Point(657, 59);
            this.buttonEditXlgXslFile.Name = "buttonEditXlgXslFile";
            this.buttonEditXlgXslFile.Size = new System.Drawing.Size(37, 23);
            this.buttonEditXlgXslFile.TabIndex = 3;
            this.buttonEditXlgXslFile.Text = "Edit";
            this.buttonEditXlgXslFile.UseVisualStyleBackColor = true;
            this.buttonEditXlgXslFile.Click += new System.EventHandler(this.buttonEditXlgXslFile_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 16);
            this.label4.TabIndex = 1;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkRegenerateOnly
            // 
            this.flowLayoutPanel1.SetFlowBreak(this.checkRegenerateOnly, true);
            this.checkRegenerateOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkRegenerateOnly.Location = new System.Drawing.Point(130, 88);
            this.checkRegenerateOnly.Name = "checkRegenerateOnly";
            this.checkRegenerateOnly.Size = new System.Drawing.Size(354, 17);
            this.checkRegenerateOnly.TabIndex = 4;
            this.checkRegenerateOnly.Text = "Regenerate by default (don\'t overwrite Target XML)";
            this.checkRegenerateOnly.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(13, 108);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(111, 20);
            this.label12.TabIndex = 1;
            this.label12.Text = "Target XML";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textOutputXml
            // 
            this.textOutputXml.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textOutputXml.Location = new System.Drawing.Point(130, 112);
            this.textOutputXml.Name = "textOutputXml";
            this.textOutputXml.Size = new System.Drawing.Size(487, 20);
            this.textOutputXml.TabIndex = 5;
            // 
            // buttonChoosetextOutputXml
            // 
            this.buttonChoosetextOutputXml.Location = new System.Drawing.Point(623, 111);
            this.buttonChoosetextOutputXml.Name = "buttonChoosetextOutputXml";
            this.buttonChoosetextOutputXml.Size = new System.Drawing.Size(28, 23);
            this.buttonChoosetextOutputXml.TabIndex = 6;
            this.buttonChoosetextOutputXml.Text = "...";
            this.buttonChoosetextOutputXml.UseVisualStyleBackColor = true;
            this.buttonChoosetextOutputXml.Click += new System.EventHandler(this.buttonChooseTextOutputXml_Click);
            // 
            // buttonViewtextOutputXml
            // 
            this.flowLayoutPanel1.SetFlowBreak(this.buttonViewtextOutputXml, true);
            this.buttonViewtextOutputXml.Location = new System.Drawing.Point(657, 111);
            this.buttonViewtextOutputXml.Name = "buttonViewtextOutputXml";
            this.buttonViewtextOutputXml.Size = new System.Drawing.Size(37, 23);
            this.buttonViewtextOutputXml.TabIndex = 7;
            this.buttonViewtextOutputXml.Text = "Edit";
            this.buttonViewtextOutputXml.UseVisualStyleBackColor = true;
            this.buttonViewtextOutputXml.Click += new System.EventHandler(this.buttonViewtextOutputXml_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Step Settings";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textXlgFile
            // 
            this.textXlgFile.Location = new System.Drawing.Point(130, 140);
            this.textXlgFile.Name = "textXlgFile";
            this.textXlgFile.Size = new System.Drawing.Size(487, 20);
            this.textXlgFile.TabIndex = 8;
            // 
            // buttonChooseXlgFile
            // 
            this.buttonChooseXlgFile.Location = new System.Drawing.Point(623, 140);
            this.buttonChooseXlgFile.Name = "buttonChooseXlgFile";
            this.buttonChooseXlgFile.Size = new System.Drawing.Size(28, 23);
            this.buttonChooseXlgFile.TabIndex = 9;
            this.buttonChooseXlgFile.Text = "...";
            this.buttonChooseXlgFile.UseVisualStyleBackColor = true;
            this.buttonChooseXlgFile.Click += new System.EventHandler(this.buttonChooseXlgFile_Click);
            // 
            // buttonEditXlgFile
            // 
            this.flowLayoutPanel1.SetFlowBreak(this.buttonEditXlgFile, true);
            this.buttonEditXlgFile.Location = new System.Drawing.Point(657, 140);
            this.buttonEditXlgFile.Name = "buttonEditXlgFile";
            this.buttonEditXlgFile.Size = new System.Drawing.Size(37, 23);
            this.buttonEditXlgFile.TabIndex = 10;
            this.buttonEditXlgFile.Text = "Edit";
            this.buttonEditXlgFile.UseVisualStyleBackColor = true;
            this.buttonEditXlgFile.Click += new System.EventHandler(this.buttonEditXlgFile_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Output File";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textOutput
            // 
            this.textOutput.Location = new System.Drawing.Point(130, 169);
            this.textOutput.Name = "textOutput";
            this.textOutput.Size = new System.Drawing.Size(487, 20);
            this.textOutput.TabIndex = 11;
            // 
            // buttonChooseOutput
            // 
            this.buttonChooseOutput.Location = new System.Drawing.Point(623, 169);
            this.buttonChooseOutput.Name = "buttonChooseOutput";
            this.buttonChooseOutput.Size = new System.Drawing.Size(28, 23);
            this.buttonChooseOutput.TabIndex = 12;
            this.buttonChooseOutput.Text = "...";
            this.buttonChooseOutput.UseVisualStyleBackColor = true;
            this.buttonChooseOutput.Click += new System.EventHandler(this.buttonChooseOutput_Click);
            // 
            // buttonViewOutputFolder
            // 
            this.flowLayoutPanel1.SetFlowBreak(this.buttonViewOutputFolder, true);
            this.buttonViewOutputFolder.Location = new System.Drawing.Point(657, 169);
            this.buttonViewOutputFolder.Name = "buttonViewOutputFolder";
            this.buttonViewOutputFolder.Size = new System.Drawing.Size(37, 23);
            this.buttonViewOutputFolder.TabIndex = 13;
            this.buttonViewOutputFolder.Text = "Edit";
            this.buttonViewOutputFolder.UseVisualStyleBackColor = true;
            this.buttonViewOutputFolder.Click += new System.EventHandler(this.buttonViewOutputFolder_Click);
            // 
            // label5
            // 
            this.flowLayoutPanel1.SetFlowBreak(this.label5, true);
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 195);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 20);
            this.label5.TabIndex = 1;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(13, 215);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(111, 20);
            this.label13.TabIndex = 1;
            this.label13.Text = "Provider";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboProviderName
            // 
            this.comboProviderName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flowLayoutPanel1.SetFlowBreak(this.comboProviderName, true);
            this.comboProviderName.FormattingEnabled = true;
            this.comboProviderName.Items.AddRange(new object[] {
            "System.Data.SqlClient",
            "Sybase.Data.AseClient",
            "MySql.Data.MySqlClient",
            "MetX.Standard.Data.Factory.CommandLineProvider",
            "MetX.Standard.Data.Factory.PowerShellProvider",
            "MetX.Standard.Data.Factory.FileSystemProvider"});
            this.comboProviderName.Location = new System.Drawing.Point(130, 218);
            this.comboProviderName.Name = "comboProviderName";
            this.comboProviderName.Size = new System.Drawing.Size(231, 21);
            this.comboProviderName.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(13, 242);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 20);
            this.label7.TabIndex = 1;
            this.label7.Text = "Connection Name";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textConnectionStringName
            // 
            this.flowLayoutPanel1.SetFlowBreak(this.textConnectionStringName, true);
            this.textConnectionStringName.Location = new System.Drawing.Point(130, 245);
            this.textConnectionStringName.Name = "textConnectionStringName";
            this.textConnectionStringName.Size = new System.Drawing.Size(521, 20);
            this.textConnectionStringName.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 268);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 20);
            this.label6.TabIndex = 1;
            this.label6.Text = "Connection String";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textConnectionString
            // 
            this.textConnectionString.Location = new System.Drawing.Point(130, 271);
            this.textConnectionString.Multiline = true;
            this.textConnectionString.Name = "textConnectionString";
            this.textConnectionString.Size = new System.Drawing.Size(521, 43);
            this.textConnectionString.TabIndex = 16;
            // 
            // buttonEditConnectionString
            // 
            this.buttonEditConnectionString.Location = new System.Drawing.Point(657, 271);
            this.buttonEditConnectionString.Name = "buttonEditConnectionString";
            this.buttonEditConnectionString.Size = new System.Drawing.Size(37, 23);
            this.buttonEditConnectionString.TabIndex = 13;
            this.buttonEditConnectionString.Text = "Edit";
            this.buttonEditConnectionString.UseVisualStyleBackColor = true;
            this.buttonEditConnectionString.Visible = false;
            this.buttonEditConnectionString.Click += new System.EventHandler(this.buttonEditConnectionString_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton6,
            this.toolStripSeparator4,
            this.toolStripButton1,
            this.toolStripButton2,
            this.autoRegenToolbarButton,
            this.toolStripSeparator6,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripSeparator8,
            this.EditClipScript});
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(736, 31);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(59, 28);
            this.toolStripButton6.Text = "&Save";
            this.toolStripButton6.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(56, 28);
            this.toolStripButton1.Text = "&Gen";
            this.toolStripButton1.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(68, 28);
            this.toolStripButton2.Text = "&Regen";
            this.toolStripButton2.Click += new System.EventHandler(this.buttonRegenerate_Click);
            // 
            // autoRegenToolbarButton
            // 
            this.autoRegenToolbarButton.Image = ((System.Drawing.Image)(resources.GetObject("autoRegenToolbarButton.Image")));
            this.autoRegenToolbarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.autoRegenToolbarButton.Name = "autoRegenToolbarButton";
            this.autoRegenToolbarButton.Size = new System.Drawing.Size(85, 28);
            this.autoRegenToolbarButton.Text = "&Auto Gen";
            this.autoRegenToolbarButton.Click += new System.EventHandler(this.buttonAutoGen_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(83, 28);
            this.toolStripButton4.Text = "A&dd Step";
            this.toolStripButton4.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(104, 28);
            this.toolStripButton5.Text = "Remove Step";
            this.toolStripButton5.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 31);
            // 
            // EditClipScript
            // 
            this.EditClipScript.Image = ((System.Drawing.Image)(resources.GetObject("EditClipScript.Image")));
            this.EditClipScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditClipScript.Name = "EditClipScript";
            this.EditClipScript.Size = new System.Drawing.Size(101, 28);
            this.EditClipScript.Text = "&QuickScripts";
            this.EditClipScript.Click += new System.EventHandler(this.EditQuickScript_Click);
            // 
            // GloveMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 377);
            this.Controls.Add(this.splitContainer1);
            this.Name = "GloveMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XLG - XML Library Generator - Code Generation from metadata through XML and XSLT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GloveMain_FormClosing);
            this.Load += new System.EventHandler(this.GloveMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ListView MetadataSources;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem generateCheckedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regenerateCheckedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regenerateSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoRegenOnChangedXSLToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton autoRegenToolbarButton;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textXlgFile;
        private System.Windows.Forms.Button buttonChooseXlgFile;
        private System.Windows.Forms.Button buttonEditXlgFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textAppXlgXsl;
        private System.Windows.Forms.Button buttonChooseXlgFileXsl;
        private System.Windows.Forms.Button buttonEditXlgXslFile;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textOutputXml;
        private System.Windows.Forms.Button buttonChoosetextOutputXml;
        private System.Windows.Forms.Button buttonViewtextOutputXml;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textOutput;
        private System.Windows.Forms.Button buttonChooseOutput;
        private System.Windows.Forms.Button buttonViewOutputFolder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textConnectionStringName;
        private System.Windows.Forms.TextBox textConnectionName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboProviderName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textConnectionString;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkRegenerateOnly;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonEditConnectionString;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton EditClipScript;
    }
}