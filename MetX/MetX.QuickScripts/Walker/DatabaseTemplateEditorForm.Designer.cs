namespace XLG.QuickScripts.Walker
{
    public partial class DatabaseTemplateEditorForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Global");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Helpers");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Extensions");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Code", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("No parent");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("No children");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("No parent and no children");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Functions");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Add list to wrapper");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("List", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Add child list to item");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Item has no children");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Item (list member)", new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode12});
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("String");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Boolean");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Date and Time");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Integer");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Bytes");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Guid");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Basic", new System.Windows.Forms.TreeNode[] {
            treeNode14,
            treeNode15,
            treeNode16,
            treeNode17,
            treeNode18,
            treeNode19});
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Long");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Float");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Double");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Decimal");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Currency");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Byte");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Other", new System.Windows.Forms.TreeNode[] {
            treeNode21,
            treeNode22,
            treeNode23,
            treeNode24,
            treeNode25,
            treeNode26});
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Detect specialized");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("BLOB");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Memo");
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Associative Array");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("2D Associative array");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("3D Associative array");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("4D Associative array");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("Specialized", new System.Windows.Forms.TreeNode[] {
            treeNode28,
            treeNode29,
            treeNode30,
            treeNode31,
            treeNode32,
            treeNode33,
            treeNode34});
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("Property", new System.Windows.Forms.TreeNode[] {
            treeNode20,
            treeNode27,
            treeNode35});
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("By child");
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("Lookup", new System.Windows.Forms.TreeNode[] {
            treeNode37});
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("Wrapper (top level)", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode13,
            treeNode36,
            treeNode38});
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("MetX Walker", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode39});
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.quickScriptControl2 = new MetX.Controls.QuickScriptControl();
            this.quickScriptControl1 = new MetX.Controls.QuickScriptControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.StartAreaEditor = new MetX.Controls.QuickScriptControl();
            this.BodyAreaEditor = new MetX.Controls.QuickScriptControl();
            this.FinishAreaEditor = new MetX.Controls.QuickScriptControl();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.quickScriptControl2, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // quickScriptControl2
            // 
            this.quickScriptControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quickScriptControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickScriptControl2.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.quickScriptControl2.IsIconBarVisible = true;
            this.quickScriptControl2.IsReadOnly = false;
            this.quickScriptControl2.Location = new System.Drawing.Point(50, 44);
            this.quickScriptControl2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.quickScriptControl2.Name = "quickScriptControl2";
            this.quickScriptControl2.Size = new System.Drawing.Size(146, 52);
            this.quickScriptControl2.TabIndex = 3;
            // 
            // quickScriptControl1
            // 
            this.quickScriptControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quickScriptControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickScriptControl1.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.quickScriptControl1.IsIconBarVisible = true;
            this.quickScriptControl1.IsReadOnly = false;
            this.quickScriptControl1.Location = new System.Drawing.Point(50, 4);
            this.quickScriptControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.quickScriptControl1.Name = "quickScriptControl1";
            this.quickScriptControl1.Size = new System.Drawing.Size(146, 239);
            this.quickScriptControl1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 91.54079F));
            this.tableLayoutPanel2.Controls.Add(this.StartAreaEditor, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.BodyAreaEditor, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.FinishAreaEditor, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(288, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(577, 446);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // StartAreaEditor
            // 
            this.StartAreaEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StartAreaEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartAreaEditor.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.StartAreaEditor.IsIconBarVisible = true;
            this.StartAreaEditor.IsReadOnly = false;
            this.StartAreaEditor.Location = new System.Drawing.Point(52, 4);
            this.StartAreaEditor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StartAreaEditor.Name = "StartAreaEditor";
            this.StartAreaEditor.Size = new System.Drawing.Size(521, 119);
            this.StartAreaEditor.TabIndex = 2;
            // 
            // BodyAreaEditor
            // 
            this.BodyAreaEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BodyAreaEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BodyAreaEditor.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BodyAreaEditor.IsIconBarVisible = true;
            this.BodyAreaEditor.IsReadOnly = false;
            this.BodyAreaEditor.Location = new System.Drawing.Point(52, 131);
            this.BodyAreaEditor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BodyAreaEditor.Name = "BodyAreaEditor";
            this.BodyAreaEditor.Size = new System.Drawing.Size(521, 162);
            this.BodyAreaEditor.TabIndex = 2;
            // 
            // FinishAreaEditor
            // 
            this.FinishAreaEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FinishAreaEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FinishAreaEditor.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FinishAreaEditor.IsIconBarVisible = true;
            this.FinishAreaEditor.IsReadOnly = false;
            this.FinishAreaEditor.Location = new System.Drawing.Point(52, 301);
            this.FinishAreaEditor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FinishAreaEditor.Name = "FinishAreaEditor";
            this.FinishAreaEditor.Size = new System.Drawing.Size(521, 119);
            this.FinishAreaEditor.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Yellow;
            this.label2.Location = new System.Drawing.Point(3, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Body";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Yellow;
            this.label3.Location = new System.Drawing.Point(3, 297);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Finish";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "CodeGlobalNode";
            treeNode1.Text = "Global";
            treeNode2.Name = "CodeHelpersNode";
            treeNode2.Text = "Helpers";
            treeNode3.Name = "CodeExtensionsNode";
            treeNode3.Text = "Extensions";
            treeNode4.Name = "WalkerCodeNode";
            treeNode4.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            treeNode4.Tag = "isfolder";
            treeNode4.Text = "Code";
            treeNode5.Name = "ListNoParentNode";
            treeNode5.Text = "No parent";
            treeNode6.Name = "ListNoChildrenNode";
            treeNode6.Text = "No children";
            treeNode7.Name = "ListNoParentAndNoChildrenNode";
            treeNode7.Text = "No parent and no children";
            treeNode8.Name = "ListFindNode";
            treeNode8.Tag = "folder";
            treeNode8.Text = "Functions";
            treeNode9.Name = "WrapperAddItemNode";
            treeNode9.Text = "Add list to wrapper";
            treeNode10.Name = "WrapperListNode";
            treeNode10.Tag = "isfolder";
            treeNode10.Text = "List";
            treeNode11.Name = "ItemNewChildListNode";
            treeNode11.Text = "Add child list to item";
            treeNode12.Name = "ItemNoChildrenNode";
            treeNode12.Text = "Item has no children";
            treeNode13.Name = "ListItemNode";
            treeNode13.Tag = "isfolder";
            treeNode13.Text = "Item (list member)";
            treeNode14.Name = "PropertyStringNode";
            treeNode14.Text = "String";
            treeNode15.Name = "PropertyBooleanNode";
            treeNode15.Text = "Boolean";
            treeNode16.Name = "PropertyDateTimeNode";
            treeNode16.Text = "Date and Time";
            treeNode17.Name = "PropertyIntegerNode";
            treeNode17.Text = "Integer";
            treeNode18.Name = "PropertyBytesNode";
            treeNode18.Text = "Bytes";
            treeNode19.Name = "PropertyGuidNode";
            treeNode19.Text = "Guid";
            treeNode20.Name = "PropertyBasicNode";
            treeNode20.Tag = "isfolder";
            treeNode20.Text = "Basic";
            treeNode21.Name = "PropertyLongNode";
            treeNode21.Text = "Long";
            treeNode22.Name = "PropertyFloatNode";
            treeNode22.Text = "Float";
            treeNode23.Name = "PropertyDoubleNode";
            treeNode23.Text = "Double";
            treeNode24.Name = "PropertyDecimalNode";
            treeNode24.Text = "Decimal";
            treeNode25.Name = "PropertyCurrencyNode";
            treeNode25.Text = "Currency";
            treeNode26.Name = "PropertyByteNode";
            treeNode26.Text = "Byte";
            treeNode27.Name = "PropertyOtherNode";
            treeNode27.Tag = "isfolder";
            treeNode27.Text = "Other";
            treeNode28.Name = "PropertyDetectSpecializedNode";
            treeNode28.Text = "Detect specialized";
            treeNode29.Name = "PropertyBlobNode";
            treeNode29.Text = "BLOB";
            treeNode30.Name = "PropertyMemoNode";
            treeNode30.Text = "Memo";
            treeNode31.Name = "PropertyAssociativeArrayNode";
            treeNode31.Text = "Associative Array";
            treeNode32.Name = "Property2DStringNode";
            treeNode32.Text = "2D Associative array";
            treeNode33.Name = "Property3DStringNode";
            treeNode33.Text = "3D Associative array";
            treeNode34.Name = "Property4DStringNode";
            treeNode34.Text = "4D Associative array";
            treeNode35.Name = "PropertySpecializedNode";
            treeNode35.Tag = "isfolder";
            treeNode35.Text = "Specialized";
            treeNode36.Name = "ItemPropertyNode";
            treeNode36.Tag = "isfolder";
            treeNode36.Text = "Property";
            treeNode37.Name = "Node1";
            treeNode37.Text = "By child";
            treeNode38.Name = "Node0";
            treeNode38.Text = "Lookup";
            treeNode39.Name = "WrapperNode";
            treeNode39.Tag = "isfolder";
            treeNode39.Text = "Wrapper (top level)";
            treeNode40.Name = "WalkerNode";
            treeNode40.Tag = "isfolder";
            treeNode40.Text = "MetX Walker";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode40});
            this.treeView1.Size = new System.Drawing.Size(288, 446);
            this.treeView1.TabIndex = 2;
            // 
            // DatabaseTemplateEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(865, 446);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.treeView1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DatabaseTemplateEditorForm";
            this.Text = "DatabaseTemplateEditorForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MetX.Controls.QuickScriptControl quickScriptControl2;
        private MetX.Controls.QuickScriptControl quickScriptControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MetX.Controls.QuickScriptControl StartAreaEditor;
        private MetX.Controls.QuickScriptControl BodyAreaEditor;
        private MetX.Controls.QuickScriptControl FinishAreaEditor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView treeView1;
    }
}