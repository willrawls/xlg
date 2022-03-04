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
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Modules");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Code", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Start (walk)");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Start");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Start (list)");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Start (item)");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Start (property)");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("String");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Boolean");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Date and Time");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Integer");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Bytes");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Guid");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Basic", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13,
            treeNode14,
            treeNode15});
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Long");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Float");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Double");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Decimal");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Currency");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Byte");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Other", new System.Windows.Forms.TreeNode[] {
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode20,
            treeNode21,
            treeNode22});
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Detect specialized");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("BLOB");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Memo");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Associative Array");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("2D Associative array");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("3D Associative array");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("4D Associative array");
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Specialized", new System.Windows.Forms.TreeNode[] {
            treeNode24,
            treeNode25,
            treeNode26,
            treeNode27,
            treeNode28,
            treeNode29,
            treeNode30});
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Finish (property)");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("Property", new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode16,
            treeNode23,
            treeNode31,
            treeNode32});
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("Add child list to item");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("Item has no children");
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("Finish (item)");
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("Item (list member)", new System.Windows.Forms.TreeNode[] {
            treeNode8,
            treeNode33,
            treeNode34,
            treeNode35,
            treeNode36});
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("No children in list");
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("No parent above list");
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("No parent & no children in list");
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("Finish (list)");
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("List", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode37,
            treeNode38,
            treeNode39,
            treeNode40,
            treeNode41});
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("Add item to list");
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("Finish (wrapper)");
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("Wrapper class (top level)", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode42,
            treeNode43,
            treeNode44});
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("Finish (walk)");
            System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("Database walker", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode4,
            treeNode5,
            treeNode45,
            treeNode46});
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
            treeNode3.Name = "CodeModulesNode";
            treeNode3.Text = "Modules";
            treeNode4.Name = "WalkerCodeNode";
            treeNode4.Tag = "isfolder";
            treeNode4.Text = "Code";
            treeNode5.Name = "WalkerStartNode";
            treeNode5.Text = "Start (walk)";
            treeNode6.Name = "WrapperStartNode";
            treeNode6.Text = "Start";
            treeNode7.Name = "ListStartNode";
            treeNode7.Text = "Start (list)";
            treeNode8.Name = "ItemStartNode";
            treeNode8.Text = "Start (item)";
            treeNode9.Name = "PropertyStartNode";
            treeNode9.Text = "Start (property)";
            treeNode10.Name = "PropertyStringNode";
            treeNode10.Text = "String";
            treeNode11.Name = "PropertyBooleanNode";
            treeNode11.Text = "Boolean";
            treeNode12.Name = "PropertyDateTimeNode";
            treeNode12.Text = "Date and Time";
            treeNode13.Name = "PropertyIntegerNode";
            treeNode13.Text = "Integer";
            treeNode14.Name = "PropertyBytesNode";
            treeNode14.Text = "Bytes";
            treeNode15.Name = "PropertyGuidNode";
            treeNode15.Text = "Guid";
            treeNode16.Name = "PropertyBasicNode";
            treeNode16.Tag = "isfolder";
            treeNode16.Text = "Basic";
            treeNode17.Name = "PropertyLongNode";
            treeNode17.Text = "Long";
            treeNode18.Name = "PropertyFloatNode";
            treeNode18.Text = "Float";
            treeNode19.Name = "PropertyDoubleNode";
            treeNode19.Text = "Double";
            treeNode20.Name = "PropertyDecimalNode";
            treeNode20.Text = "Decimal";
            treeNode21.Name = "PropertyCurrencyNode";
            treeNode21.Text = "Currency";
            treeNode22.Name = "PropertyByteNode";
            treeNode22.Text = "Byte";
            treeNode23.Name = "PropertyOtherNode";
            treeNode23.Tag = "isfolder";
            treeNode23.Text = "Other";
            treeNode24.Name = "PropertyDetectSpecializedNode";
            treeNode24.Text = "Detect specialized";
            treeNode25.Name = "PropertyBlobNode";
            treeNode25.Text = "BLOB";
            treeNode26.Name = "PropertyMemoNode";
            treeNode26.Text = "Memo";
            treeNode27.Name = "PropertyAssociativeArrayNode";
            treeNode27.Text = "Associative Array";
            treeNode28.Name = "Property2DStringNode";
            treeNode28.Text = "2D Associative array";
            treeNode29.Name = "Property3DStringNode";
            treeNode29.Text = "3D Associative array";
            treeNode30.Name = "Property4DStringNode";
            treeNode30.Text = "4D Associative array";
            treeNode31.Name = "PropertySpecializedNode";
            treeNode31.Tag = "isfolder";
            treeNode31.Text = "Specialized";
            treeNode32.Name = "PropertyFinishNode";
            treeNode32.Text = "Finish (property)";
            treeNode33.Name = "ItemPropertyNode";
            treeNode33.Tag = "isfolder";
            treeNode33.Text = "Property";
            treeNode34.Name = "ItemNewChildListNode";
            treeNode34.Text = "Add child list to item";
            treeNode35.Name = "ItemNoChildrenNode";
            treeNode35.Text = "Item has no children";
            treeNode36.Name = "ItemFinishNode";
            treeNode36.Text = "Finish (item)";
            treeNode37.Name = "ListItemNode";
            treeNode37.Tag = "isfolder";
            treeNode37.Text = "Item (list member)";
            treeNode38.Name = "ListNoChildrenNode";
            treeNode38.Text = "No children in list";
            treeNode39.Name = "ListNoParentNode";
            treeNode39.Text = "No parent above list";
            treeNode40.Name = "ListNoParentAndNoChildrenNode";
            treeNode40.Text = "No parent & no children in list";
            treeNode41.Name = "ListFinish";
            treeNode41.Text = "Finish (list)";
            treeNode42.Name = "WrapperListNode";
            treeNode42.Tag = "isfolder";
            treeNode42.Text = "List";
            treeNode43.Name = "WrapperAddItemNode";
            treeNode43.Text = "Add item to list";
            treeNode44.Name = "WrapperFinishNode";
            treeNode44.Text = "Finish (wrapper)";
            treeNode45.Name = "WrapperNode";
            treeNode45.Tag = "isfolder";
            treeNode45.Text = "Wrapper class (top level)";
            treeNode46.Name = "WalkerFinish";
            treeNode46.Text = "Finish (walk)";
            treeNode47.Name = "WalkerNode";
            treeNode47.Tag = "isfolder";
            treeNode47.Text = "Database walker";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode47});
            this.treeView1.Size = new System.Drawing.Size(329, 728);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(662, 728);
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
            this.label2.Location = new System.Drawing.Point(3, 218);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Body";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Yellow;
            this.label3.Location = new System.Drawing.Point(3, 509);
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
            this.ScriptEditor.Size = new System.Drawing.Size(598, 208);
            this.ScriptEditor.TabIndex = 1;
            // 
            // quickScriptControl1
            // 
            this.quickScriptControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quickScriptControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickScriptControl1.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.quickScriptControl1.IsIconBarVisible = true;
            this.quickScriptControl1.IsReadOnly = false;
            this.quickScriptControl1.Location = new System.Drawing.Point(60, 223);
            this.quickScriptControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.quickScriptControl1.Name = "quickScriptControl1";
            this.quickScriptControl1.Size = new System.Drawing.Size(598, 281);
            this.quickScriptControl1.TabIndex = 2;
            // 
            // quickScriptControl2
            // 
            this.quickScriptControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quickScriptControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickScriptControl2.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.quickScriptControl2.IsIconBarVisible = true;
            this.quickScriptControl2.IsReadOnly = false;
            this.quickScriptControl2.Location = new System.Drawing.Point(60, 514);
            this.quickScriptControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.quickScriptControl2.Name = "quickScriptControl2";
            this.quickScriptControl2.Size = new System.Drawing.Size(598, 209);
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
            this.Size = new System.Drawing.Size(1011, 748);
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
