namespace XLG.QuickScripts.Walker
{
    partial class Ideas1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Settings");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Global");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Helpers");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Extensions");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("SomeFile.csv");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Files", new System.Windows.Forms.TreeNode[] {
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("SomeAssembly.dll");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Assemblies", new System.Windows.Forms.TreeNode[] {
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Code", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode6,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Start (walk)");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Start (wrapper)");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Start (list)");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Start (item)");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Start (property)");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("String");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Boolean");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Date and Time");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Integer");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Bytes");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Guid");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Basic", new System.Windows.Forms.TreeNode[] {
            treeNode15,
            treeNode16,
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode20});
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Long");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Float");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Double");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Decimal");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Currency");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Byte");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Other", new System.Windows.Forms.TreeNode[] {
            treeNode22,
            treeNode23,
            treeNode24,
            treeNode25,
            treeNode26,
            treeNode27});
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Detect specialized");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("BLOB");
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Memo");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Associative Array");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("2D Associative array");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("3D Associative array");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("4D Associative array");
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("Specialized", new System.Windows.Forms.TreeNode[] {
            treeNode29,
            treeNode30,
            treeNode31,
            treeNode32,
            treeNode33,
            treeNode34,
            treeNode35});
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("Finish (property)");
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("Property", new System.Windows.Forms.TreeNode[] {
            treeNode14,
            treeNode21,
            treeNode28,
            treeNode36,
            treeNode37});
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("Add child list to item");
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("Item has no children");
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("Finish (item)");
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("Item (list member)", new System.Windows.Forms.TreeNode[] {
            treeNode13,
            treeNode38,
            treeNode39,
            treeNode40,
            treeNode41});
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("No children in list");
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("No parent above list");
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("No parent and no children in list");
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("Add list to wrapper");
            System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("Start (find)");
            System.Windows.Forms.TreeNode treeNode48 = new System.Windows.Forms.TreeNode("Construct");
            System.Windows.Forms.TreeNode treeNode49 = new System.Windows.Forms.TreeNode("Finish (find)");
            System.Windows.Forms.TreeNode treeNode50 = new System.Windows.Forms.TreeNode("Find (based on index)", new System.Windows.Forms.TreeNode[] {
            treeNode47,
            treeNode48,
            treeNode49});
            System.Windows.Forms.TreeNode treeNode51 = new System.Windows.Forms.TreeNode("Finish (list)");
            System.Windows.Forms.TreeNode treeNode52 = new System.Windows.Forms.TreeNode("List", new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode42,
            treeNode43,
            treeNode44,
            treeNode45,
            treeNode46,
            treeNode50,
            treeNode51});
            System.Windows.Forms.TreeNode treeNode53 = new System.Windows.Forms.TreeNode("Finish (wrapper)");
            System.Windows.Forms.TreeNode treeNode54 = new System.Windows.Forms.TreeNode("Wrapper (top level)", new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode52,
            treeNode53});
            System.Windows.Forms.TreeNode treeNode55 = new System.Windows.Forms.TreeNode("Finish (walk)");
            System.Windows.Forms.TreeNode treeNode56 = new System.Windows.Forms.TreeNode("Database walker", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode9,
            treeNode10,
            treeNode54,
            treeNode55});
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ScriptEditor = new MetX.Controls.QuickScriptControl();
            this.quickScriptControl1 = new MetX.Controls.QuickScriptControl();
            this.quickScriptControl2 = new MetX.Controls.QuickScriptControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(10, 10);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "SettingsNode";
            treeNode1.Text = "Settings";
            treeNode2.Name = "CodeGlobalNode";
            treeNode2.Text = "Global";
            treeNode3.Name = "CodeHelpersNode";
            treeNode3.Text = "Helpers";
            treeNode4.Name = "CodeExtensionsNode";
            treeNode4.Text = "Extensions";
            treeNode5.Name = "FilesNode0";
            treeNode5.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            treeNode5.Tag = "singlepane";
            treeNode5.Text = "SomeFile.csv";
            treeNode6.Name = "CodeFilesNode";
            treeNode6.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            treeNode6.Text = "Files";
            treeNode7.Name = "AssembliesNode0";
            treeNode7.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            treeNode7.Tag = "noedit noview";
            treeNode7.Text = "SomeAssembly.dll";
            treeNode8.ForeColor = System.Drawing.Color.Blue;
            treeNode8.Name = "CodeAssembliesNode";
            treeNode8.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            treeNode8.Tag = "isfolder";
            treeNode8.Text = "Assemblies";
            treeNode9.Name = "WalkerCodeNode";
            treeNode9.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            treeNode9.Tag = "isfolder";
            treeNode9.Text = "Code";
            treeNode10.Name = "WalkerStartNode";
            treeNode10.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            treeNode10.Text = "Start (walk)";
            treeNode11.Name = "WrapperStartNode";
            treeNode11.Text = "Start (wrapper)";
            treeNode12.Name = "ListStartNode";
            treeNode12.Text = "Start (list)";
            treeNode13.Name = "ItemStartNode";
            treeNode13.Text = "Start (item)";
            treeNode14.Name = "PropertyStartNode";
            treeNode14.Text = "Start (property)";
            treeNode15.Name = "PropertyStringNode";
            treeNode15.Text = "String";
            treeNode16.Name = "PropertyBooleanNode";
            treeNode16.Text = "Boolean";
            treeNode17.Name = "PropertyDateTimeNode";
            treeNode17.Text = "Date and Time";
            treeNode18.Name = "PropertyIntegerNode";
            treeNode18.Text = "Integer";
            treeNode19.Name = "PropertyBytesNode";
            treeNode19.Text = "Bytes";
            treeNode20.Name = "PropertyGuidNode";
            treeNode20.Text = "Guid";
            treeNode21.Name = "PropertyBasicNode";
            treeNode21.Tag = "isfolder";
            treeNode21.Text = "Basic";
            treeNode22.Name = "PropertyLongNode";
            treeNode22.Text = "Long";
            treeNode23.Name = "PropertyFloatNode";
            treeNode23.Text = "Float";
            treeNode24.Name = "PropertyDoubleNode";
            treeNode24.Text = "Double";
            treeNode25.Name = "PropertyDecimalNode";
            treeNode25.Text = "Decimal";
            treeNode26.Name = "PropertyCurrencyNode";
            treeNode26.Text = "Currency";
            treeNode27.Name = "PropertyByteNode";
            treeNode27.Text = "Byte";
            treeNode28.Name = "PropertyOtherNode";
            treeNode28.Tag = "isfolder";
            treeNode28.Text = "Other";
            treeNode29.Name = "PropertyDetectSpecializedNode";
            treeNode29.Text = "Detect specialized";
            treeNode30.Name = "PropertyBlobNode";
            treeNode30.Text = "BLOB";
            treeNode31.Name = "PropertyMemoNode";
            treeNode31.Text = "Memo";
            treeNode32.Name = "PropertyAssociativeArrayNode";
            treeNode32.Text = "Associative Array";
            treeNode33.Name = "Property2DStringNode";
            treeNode33.Text = "2D Associative array";
            treeNode34.Name = "Property3DStringNode";
            treeNode34.Text = "3D Associative array";
            treeNode35.Name = "Property4DStringNode";
            treeNode35.Text = "4D Associative array";
            treeNode36.Name = "PropertySpecializedNode";
            treeNode36.Tag = "isfolder";
            treeNode36.Text = "Specialized";
            treeNode37.Name = "PropertyFinishNode";
            treeNode37.Text = "Finish (property)";
            treeNode38.Name = "ItemPropertyNode";
            treeNode38.Tag = "isfolder";
            treeNode38.Text = "Property";
            treeNode39.Name = "ItemNewChildListNode";
            treeNode39.Text = "Add child list to item";
            treeNode40.Name = "ItemNoChildrenNode";
            treeNode40.Text = "Item has no children";
            treeNode41.Name = "ItemFinishNode";
            treeNode41.Text = "Finish (item)";
            treeNode42.Name = "ListItemNode";
            treeNode42.Tag = "isfolder";
            treeNode42.Text = "Item (list member)";
            treeNode43.Name = "ListNoChildrenNode";
            treeNode43.Text = "No children in list";
            treeNode44.Name = "ListNoParentNode";
            treeNode44.Text = "No parent above list";
            treeNode45.Name = "ListNoParentAndNoChildrenNode";
            treeNode45.Text = "No parent and no children in list";
            treeNode46.Name = "WrapperAddItemNode";
            treeNode46.Text = "Add list to wrapper";
            treeNode47.Name = "FindStartNode";
            treeNode47.Text = "Start (find)";
            treeNode48.Name = "FindConstructNode";
            treeNode48.Text = "Construct";
            treeNode49.Name = "FindFinishNode";
            treeNode49.Text = "Finish (find)";
            treeNode50.Name = "ListFindNode";
            treeNode50.Tag = "folder";
            treeNode50.Text = "Find (based on index)";
            treeNode51.Name = "ListFinish";
            treeNode51.Text = "Finish (list)";
            treeNode52.Name = "WrapperListNode";
            treeNode52.Tag = "isfolder";
            treeNode52.Text = "List";
            treeNode53.Name = "WrapperFinishNode";
            treeNode53.Text = "Finish (wrapper)";
            treeNode54.Name = "WrapperNode";
            treeNode54.Tag = "isfolder";
            treeNode54.Text = "Wrapper (top level)";
            treeNode55.Name = "WalkerFinish";
            treeNode55.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            treeNode55.Text = "Finish (walk)";
            treeNode56.Name = "WalkerNode";
            treeNode56.Tag = "isfolder";
            treeNode56.Text = "Database walker";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode56});
            this.treeView1.Size = new System.Drawing.Size(329, 826);
            this.treeView1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.459214F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 91.54079F));
            this.tableLayoutPanel1.Controls.Add(this.quickScriptControl2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.quickScriptControl1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ScriptEditor, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(339, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(662, 826);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Yellow;
            this.label2.Location = new System.Drawing.Point(3, 247);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Body";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Yellow;
            this.label3.Location = new System.Drawing.Point(3, 577);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Finish";
            // 
            // ScriptEditor
            // 
            this.ScriptEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ScriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScriptEditor.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ScriptEditor.IsIconBarVisible = true;
            this.ScriptEditor.IsReadOnly = false;
            this.ScriptEditor.Location = new System.Drawing.Point(60, 5);
            this.ScriptEditor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ScriptEditor.Name = "ScriptEditor";
            this.ScriptEditor.Size = new System.Drawing.Size(598, 237);
            this.ScriptEditor.TabIndex = 1;
            // 
            // quickScriptControl1
            // 
            this.quickScriptControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quickScriptControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickScriptControl1.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.quickScriptControl1.IsIconBarVisible = true;
            this.quickScriptControl1.IsReadOnly = false;
            this.quickScriptControl1.Location = new System.Drawing.Point(60, 252);
            this.quickScriptControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.quickScriptControl1.Name = "quickScriptControl1";
            this.quickScriptControl1.Size = new System.Drawing.Size(598, 320);
            this.quickScriptControl1.TabIndex = 2;
            // 
            // quickScriptControl2
            // 
            this.quickScriptControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quickScriptControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickScriptControl2.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.quickScriptControl2.IsIconBarVisible = true;
            this.quickScriptControl2.IsReadOnly = false;
            this.quickScriptControl2.Location = new System.Drawing.Point(60, 582);
            this.quickScriptControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.quickScriptControl2.Name = "quickScriptControl2";
            this.quickScriptControl2.Size = new System.Drawing.Size(598, 239);
            this.quickScriptControl2.TabIndex = 3;
            // 
            // Ideas1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.treeView1);
            this.Name = "Ideas1";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(1011, 846);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private MetX.Controls.QuickScriptControl quickScriptControl2;
        private MetX.Controls.QuickScriptControl quickScriptControl1;
        private MetX.Controls.QuickScriptControl ScriptEditor;
    }
}
