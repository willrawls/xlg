namespace XLG.QuickScripts.Walker
{
    partial class RelationshipEditor
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
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "States",
            "50",
            "1",
            "Lookup"}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Bob.Person",
            "1001",
            "1",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "Employee",
            "456",
            "",
            "Bob.Person"}, -1);
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Employee");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Bob.Person", new System.Windows.Forms.TreeNode[] {
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Related", new System.Windows.Forms.TreeNode[] {
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("States");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Lookup", new System.Windows.Forms.TreeNode[] {
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Logs");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Other", new System.Windows.Forms.TreeNode[] {
            treeNode13});
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "Column",
            "String",
            "255"}, -1);
            this.panel1 = new System.Windows.Forms.Panel();
            this.TableListView = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.SchemaComboBox = new System.Windows.Forms.ComboBox();
            this.AddTableButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.button15 = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.IndexListView = new System.Windows.Forms.ListView();
            this.IndexColumnName = new System.Windows.Forms.ColumnHeader();
            this.IndexColumnColumns = new System.Windows.Forms.ColumnHeader();
            this.IndexColumnStatus = new System.Windows.Forms.ColumnHeader();
            this.panel7 = new System.Windows.Forms.Panel();
            this.KeysListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.panel2 = new System.Windows.Forms.Panel();
            this.RelationshipTreeView = new System.Windows.Forms.TreeView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.AddRelaionshipButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.RelationshipTypeComboBox = new System.Windows.Forms.ComboBox();
            this.RelationshipTagsTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.RelationshipNameTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.RemoveRelaionshipFieldButton = new System.Windows.Forms.Button();
            this.AddRelaionshipFieldButton = new System.Windows.Forms.Button();
            this.RelationshipFieldRightComboBox2 = new System.Windows.Forms.ComboBox();
            this.RelationshipFieldLeftComboBox2 = new System.Windows.Forms.ComboBox();
            this.RelationshipFieldRightComboBox1 = new System.Windows.Forms.ComboBox();
            this.RelationshipFieldLeftComboBox1 = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ColumnsListView = new System.Windows.Forms.ListView();
            this.NameColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.TypeColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.SizeColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.Indexed = new System.Windows.Forms.ColumnHeader();
            this.panel13 = new System.Windows.Forms.Panel();
            this.button11 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.button13 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ShowDatabaseRelationshipsCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowCustomRelationshipsCheckBox = new System.Windows.Forms.CheckBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.RefreshFromDatabaseButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel1.Controls.Add(this.TableListView);
            this.panel1.Controls.Add(this.panel12);
            this.panel1.Controls.Add(this.panel10);
            this.panel1.Controls.Add(this.panel9);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(372, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(371, 481);
            this.panel1.TabIndex = 1;
            // 
            // TableListView
            // 
            this.TableListView.BackColor = System.Drawing.Color.Black;
            this.TableListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.TableListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableListView.ForeColor = System.Drawing.Color.White;
            this.TableListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem5,
            listViewItem6,
            listViewItem7});
            this.TableListView.Location = new System.Drawing.Point(0, 34);
            this.TableListView.Name = "TableListView";
            this.TableListView.Size = new System.Drawing.Size(371, 232);
            this.TableListView.TabIndex = 25;
            this.TableListView.UseCompatibleStateImageBehavior = false;
            this.TableListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Table Name";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Rows";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Children";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Parent";
            this.columnHeader8.Width = 120;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.label3);
            this.panel12.Controls.Add(this.SchemaComboBox);
            this.panel12.Controls.Add(this.AddTableButton);
            this.panel12.Controls.Add(this.label4);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(0, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(371, 34);
            this.panel12.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(150, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 15);
            this.label3.TabIndex = 20;
            this.label3.Text = "Schema";
            // 
            // SchemaComboBox
            // 
            this.SchemaComboBox.FormattingEnabled = true;
            this.SchemaComboBox.Items.AddRange(new object[] {
            "dbo",
            "Bob"});
            this.SchemaComboBox.Location = new System.Drawing.Point(205, 5);
            this.SchemaComboBox.Name = "SchemaComboBox";
            this.SchemaComboBox.Size = new System.Drawing.Size(136, 23);
            this.SchemaComboBox.TabIndex = 21;
            this.SchemaComboBox.Text = "All";
            // 
            // AddTableButton
            // 
            this.AddTableButton.ForeColor = System.Drawing.Color.Black;
            this.AddTableButton.Location = new System.Drawing.Point(57, 6);
            this.AddTableButton.Name = "AddTableButton";
            this.AddTableButton.Size = new System.Drawing.Size(39, 23);
            this.AddTableButton.TabIndex = 18;
            this.AddTableButton.Text = "+";
            this.AddTableButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 15);
            this.label4.TabIndex = 17;
            this.label4.Text = "Tables";
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.button15);
            this.panel10.Controls.Add(this.label12);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel10.Location = new System.Drawing.Point(0, 266);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(371, 26);
            this.panel10.TabIndex = 21;
            // 
            // button15
            // 
            this.button15.ForeColor = System.Drawing.Color.Black;
            this.button15.Location = new System.Drawing.Point(84, 2);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(39, 23);
            this.button15.TabIndex = 23;
            this.button15.Text = "+";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(0, 5);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 15);
            this.label12.TabIndex = 22;
            this.label12.Text = "Table Indexes";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.IndexListView);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(0, 292);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(371, 179);
            this.panel9.TabIndex = 20;
            // 
            // IndexListView
            // 
            this.IndexListView.BackColor = System.Drawing.Color.Black;
            this.IndexListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IndexColumnName,
            this.IndexColumnColumns,
            this.IndexColumnStatus});
            this.IndexListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IndexListView.ForeColor = System.Drawing.Color.White;
            this.IndexListView.Location = new System.Drawing.Point(0, 0);
            this.IndexListView.Name = "IndexListView";
            this.IndexListView.Size = new System.Drawing.Size(371, 179);
            this.IndexListView.TabIndex = 15;
            this.IndexListView.UseCompatibleStateImageBehavior = false;
            this.IndexListView.View = System.Windows.Forms.View.Details;
            // 
            // IndexColumnName
            // 
            this.IndexColumnName.Text = "Name";
            // 
            // IndexColumnColumns
            // 
            this.IndexColumnColumns.Text = "Columns";
            this.IndexColumnColumns.Width = 230;
            // 
            // IndexColumnStatus
            // 
            this.IndexColumnStatus.Text = "Status";
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 471);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(371, 10);
            this.panel7.TabIndex = 19;
            // 
            // KeysListView
            // 
            this.KeysListView.BackColor = System.Drawing.Color.Black;
            this.KeysListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.KeysListView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.KeysListView.ForeColor = System.Drawing.Color.White;
            this.KeysListView.Location = new System.Drawing.Point(0, 292);
            this.KeysListView.Name = "KeysListView";
            this.KeysListView.Size = new System.Drawing.Size(344, 179);
            this.KeysListView.TabIndex = 14;
            this.KeysListView.UseCompatibleStateImageBehavior = false;
            this.KeysListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Columns";
            this.columnHeader4.Width = 250;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel2.Controls.Add(this.RelationshipTreeView);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.ForeColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(0, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(372, 481);
            this.panel2.TabIndex = 2;
            // 
            // RelationshipTreeView
            // 
            this.RelationshipTreeView.BackColor = System.Drawing.Color.Black;
            this.RelationshipTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RelationshipTreeView.ForeColor = System.Drawing.Color.White;
            this.RelationshipTreeView.Location = new System.Drawing.Point(0, 26);
            this.RelationshipTreeView.Name = "RelationshipTreeView";
            treeNode8.ForeColor = System.Drawing.Color.Red;
            treeNode8.Name = "Node2";
            treeNode8.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            treeNode8.Text = "Employee";
            treeNode9.Name = "Node1";
            treeNode9.Text = "Bob.Person";
            treeNode10.Name = "Node0";
            treeNode10.Text = "Related";
            treeNode11.Name = "Node4";
            treeNode11.Text = "States";
            treeNode12.Name = "Node3";
            treeNode12.Text = "Lookup";
            treeNode13.Name = "Node6";
            treeNode13.Text = "Logs";
            treeNode14.Name = "Node5";
            treeNode14.Text = "Other";
            this.RelationshipTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode12,
            treeNode14});
            this.RelationshipTreeView.Size = new System.Drawing.Size(372, 196);
            this.RelationshipTreeView.TabIndex = 6;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.AddRelaionshipButton);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(372, 26);
            this.panel6.TabIndex = 18;
            // 
            // AddRelaionshipButton
            // 
            this.AddRelaionshipButton.ForeColor = System.Drawing.Color.Black;
            this.AddRelaionshipButton.Location = new System.Drawing.Point(105, 0);
            this.AddRelaionshipButton.Name = "AddRelaionshipButton";
            this.AddRelaionshipButton.Size = new System.Drawing.Size(39, 23);
            this.AddRelaionshipButton.TabIndex = 19;
            this.AddRelaionshipButton.Text = "+";
            this.AddRelaionshipButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(366, 15);
            this.label1.TabIndex = 18;
            this.label1.Text = "Relationships";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.DodgerBlue;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(0, 222);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(372, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Relationship Properties";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.button5);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.RelationshipTypeComboBox);
            this.panel3.Controls.Add(this.RelationshipTagsTextBox);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.RelationshipNameTextBox);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.RemoveRelaionshipFieldButton);
            this.panel3.Controls.Add(this.AddRelaionshipFieldButton);
            this.panel3.Controls.Add(this.RelationshipFieldRightComboBox2);
            this.panel3.Controls.Add(this.RelationshipFieldLeftComboBox2);
            this.panel3.Controls.Add(this.RelationshipFieldRightComboBox1);
            this.panel3.Controls.Add(this.RelationshipFieldLeftComboBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 237);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(372, 244);
            this.panel3.TabIndex = 8;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 123);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 15);
            this.label13.TabIndex = 17;
            this.label13.Text = "Fields";
            // 
            // button5
            // 
            this.button5.ForeColor = System.Drawing.Color.Black;
            this.button5.Location = new System.Drawing.Point(260, 12);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(36, 23);
            this.button5.TabIndex = 16;
            this.button5.Text = "~";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 15);
            this.label9.TabIndex = 11;
            this.label9.Text = "Type";
            // 
            // RelationshipTypeComboBox
            // 
            this.RelationshipTypeComboBox.FormattingEnabled = true;
            this.RelationshipTypeComboBox.Items.AddRange(new object[] {
            "One to many",
            "One to one",
            "Lookup"});
            this.RelationshipTypeComboBox.Location = new System.Drawing.Point(55, 42);
            this.RelationshipTypeComboBox.Name = "RelationshipTypeComboBox";
            this.RelationshipTypeComboBox.Size = new System.Drawing.Size(121, 23);
            this.RelationshipTypeComboBox.TabIndex = 10;
            this.RelationshipTypeComboBox.Text = "One to many";
            // 
            // RelationshipTagsTextBox
            // 
            this.RelationshipTagsTextBox.Location = new System.Drawing.Point(56, 71);
            this.RelationshipTagsTextBox.Name = "RelationshipTagsTextBox";
            this.RelationshipTagsTextBox.Size = new System.Drawing.Size(284, 23);
            this.RelationshipTagsTextBox.TabIndex = 9;
            this.RelationshipTagsTextBox.Text = "hidden implied explicit  added";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 74);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 15);
            this.label11.TabIndex = 8;
            this.label11.Text = "Tags";
            // 
            // RelationshipNameTextBox
            // 
            this.RelationshipNameTextBox.Location = new System.Drawing.Point(55, 13);
            this.RelationshipNameTextBox.Name = "RelationshipNameTextBox";
            this.RelationshipNameTextBox.Size = new System.Drawing.Size(203, 23);
            this.RelationshipNameTextBox.TabIndex = 9;
            this.RelationshipNameTextBox.Text = "FK_PersonID_Person_PersonID";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 15);
            this.label8.TabIndex = 8;
            this.label8.Text = "Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(226, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 15);
            this.label7.TabIndex = 7;
            this.label7.Text = "dbo.Employee";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(54, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 6;
            this.label6.Text = "Bob.Person";
            // 
            // RemoveRelaionshipFieldButton
            // 
            this.RemoveRelaionshipFieldButton.ForeColor = System.Drawing.Color.Black;
            this.RemoveRelaionshipFieldButton.Location = new System.Drawing.Point(181, 149);
            this.RemoveRelaionshipFieldButton.Name = "RemoveRelaionshipFieldButton";
            this.RemoveRelaionshipFieldButton.Size = new System.Drawing.Size(39, 23);
            this.RemoveRelaionshipFieldButton.TabIndex = 5;
            this.RemoveRelaionshipFieldButton.Text = "-";
            this.RemoveRelaionshipFieldButton.UseVisualStyleBackColor = true;
            // 
            // AddRelaionshipFieldButton
            // 
            this.AddRelaionshipFieldButton.ForeColor = System.Drawing.Color.Black;
            this.AddRelaionshipFieldButton.Location = new System.Drawing.Point(181, 178);
            this.AddRelaionshipFieldButton.Name = "AddRelaionshipFieldButton";
            this.AddRelaionshipFieldButton.Size = new System.Drawing.Size(39, 23);
            this.AddRelaionshipFieldButton.TabIndex = 4;
            this.AddRelaionshipFieldButton.Text = "+";
            this.AddRelaionshipFieldButton.UseVisualStyleBackColor = true;
            // 
            // RelationshipFieldRightComboBox2
            // 
            this.RelationshipFieldRightComboBox2.FormattingEnabled = true;
            this.RelationshipFieldRightComboBox2.Location = new System.Drawing.Point(226, 149);
            this.RelationshipFieldRightComboBox2.Name = "RelationshipFieldRightComboBox2";
            this.RelationshipFieldRightComboBox2.Size = new System.Drawing.Size(121, 23);
            this.RelationshipFieldRightComboBox2.TabIndex = 3;
            // 
            // RelationshipFieldLeftComboBox2
            // 
            this.RelationshipFieldLeftComboBox2.FormattingEnabled = true;
            this.RelationshipFieldLeftComboBox2.Location = new System.Drawing.Point(54, 149);
            this.RelationshipFieldLeftComboBox2.Name = "RelationshipFieldLeftComboBox2";
            this.RelationshipFieldLeftComboBox2.Size = new System.Drawing.Size(121, 23);
            this.RelationshipFieldLeftComboBox2.TabIndex = 2;
            // 
            // RelationshipFieldRightComboBox1
            // 
            this.RelationshipFieldRightComboBox1.FormattingEnabled = true;
            this.RelationshipFieldRightComboBox1.Location = new System.Drawing.Point(226, 120);
            this.RelationshipFieldRightComboBox1.Name = "RelationshipFieldRightComboBox1";
            this.RelationshipFieldRightComboBox1.Size = new System.Drawing.Size(121, 23);
            this.RelationshipFieldRightComboBox1.TabIndex = 1;
            this.RelationshipFieldRightComboBox1.Text = "PersonID";
            // 
            // RelationshipFieldLeftComboBox1
            // 
            this.RelationshipFieldLeftComboBox1.FormattingEnabled = true;
            this.RelationshipFieldLeftComboBox1.Location = new System.Drawing.Point(53, 120);
            this.RelationshipFieldLeftComboBox1.Name = "RelationshipFieldLeftComboBox1";
            this.RelationshipFieldLeftComboBox1.Size = new System.Drawing.Size(121, 23);
            this.RelationshipFieldLeftComboBox1.TabIndex = 0;
            this.RelationshipFieldLeftComboBox1.Text = "PersonID";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel4.Controls.Add(this.ColumnsListView);
            this.panel4.Controls.Add(this.panel13);
            this.panel4.Controls.Add(this.panel11);
            this.panel4.Controls.Add(this.KeysListView);
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.ForeColor = System.Drawing.Color.White;
            this.panel4.Location = new System.Drawing.Point(743, 42);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(344, 481);
            this.panel4.TabIndex = 3;
            // 
            // ColumnsListView
            // 
            this.ColumnsListView.BackColor = System.Drawing.Color.Black;
            this.ColumnsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumnHeader,
            this.TypeColumnHeader,
            this.SizeColumnHeader,
            this.Indexed});
            this.ColumnsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColumnsListView.ForeColor = System.Drawing.Color.White;
            this.ColumnsListView.FullRowSelect = true;
            this.ColumnsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ColumnsListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem8});
            this.ColumnsListView.Location = new System.Drawing.Point(0, 33);
            this.ColumnsListView.MultiSelect = false;
            this.ColumnsListView.Name = "ColumnsListView";
            this.ColumnsListView.Size = new System.Drawing.Size(344, 226);
            this.ColumnsListView.TabIndex = 16;
            this.ColumnsListView.UseCompatibleStateImageBehavior = false;
            this.ColumnsListView.View = System.Windows.Forms.View.Details;
            // 
            // NameColumnHeader
            // 
            this.NameColumnHeader.Text = "Name";
            // 
            // TypeColumnHeader
            // 
            this.TypeColumnHeader.Text = "Type";
            // 
            // SizeColumnHeader
            // 
            this.SizeColumnHeader.Text = "Size";
            // 
            // Indexed
            // 
            this.Indexed.Text = "Indexed";
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.button11);
            this.panel13.Controls.Add(this.label10);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel13.Location = new System.Drawing.Point(0, 0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(344, 33);
            this.panel13.TabIndex = 25;
            // 
            // button11
            // 
            this.button11.ForeColor = System.Drawing.Color.Black;
            this.button11.Location = new System.Drawing.Point(91, 3);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(39, 23);
            this.button11.TabIndex = 20;
            this.button11.Text = "+";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 15);
            this.label10.TabIndex = 19;
            this.label10.Text = "Table Columns";
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.button13);
            this.panel11.Controls.Add(this.label5);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel11.Location = new System.Drawing.Point(0, 259);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(344, 33);
            this.panel11.TabIndex = 23;
            // 
            // button13
            // 
            this.button13.ForeColor = System.Drawing.Color.Black;
            this.button13.Location = new System.Drawing.Point(67, 9);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(39, 23);
            this.button13.TabIndex = 22;
            this.button13.Text = "+";
            this.button13.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 15);
            this.label5.TabIndex = 21;
            this.label5.Text = "Table Keys";
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(0, 471);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(344, 10);
            this.panel8.TabIndex = 21;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Navy;
            this.panel5.Controls.Add(this.ShowDatabaseRelationshipsCheckBox);
            this.panel5.Controls.Add(this.ShowCustomRelationshipsCheckBox);
            this.panel5.Controls.Add(this.SaveButton);
            this.panel5.Controls.Add(this.RefreshFromDatabaseButton);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.ForeColor = System.Drawing.Color.White;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1087, 42);
            this.panel5.TabIndex = 4;
            // 
            // ShowDatabaseRelationshipsCheckBox
            // 
            this.ShowDatabaseRelationshipsCheckBox.AutoSize = true;
            this.ShowDatabaseRelationshipsCheckBox.Checked = true;
            this.ShowDatabaseRelationshipsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowDatabaseRelationshipsCheckBox.Location = new System.Drawing.Point(9, 15);
            this.ShowDatabaseRelationshipsCheckBox.Name = "ShowDatabaseRelationshipsCheckBox";
            this.ShowDatabaseRelationshipsCheckBox.Size = new System.Drawing.Size(179, 19);
            this.ShowDatabaseRelationshipsCheckBox.TabIndex = 2;
            this.ShowDatabaseRelationshipsCheckBox.Text = "Show Database Relationships";
            this.ShowDatabaseRelationshipsCheckBox.UseVisualStyleBackColor = true;
            // 
            // ShowCustomRelationshipsCheckBox
            // 
            this.ShowCustomRelationshipsCheckBox.AutoSize = true;
            this.ShowCustomRelationshipsCheckBox.Checked = true;
            this.ShowCustomRelationshipsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowCustomRelationshipsCheckBox.Location = new System.Drawing.Point(200, 15);
            this.ShowCustomRelationshipsCheckBox.Name = "ShowCustomRelationshipsCheckBox";
            this.ShowCustomRelationshipsCheckBox.Size = new System.Drawing.Size(173, 19);
            this.ShowCustomRelationshipsCheckBox.TabIndex = 2;
            this.ShowCustomRelationshipsCheckBox.Text = "Show Custom Relationships";
            this.ShowCustomRelationshipsCheckBox.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.ForeColor = System.Drawing.Color.Black;
            this.SaveButton.Location = new System.Drawing.Point(565, 11);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(148, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Save XLG document";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // RefreshFromDatabaseButton
            // 
            this.RefreshFromDatabaseButton.ForeColor = System.Drawing.Color.Black;
            this.RefreshFromDatabaseButton.Location = new System.Drawing.Point(386, 11);
            this.RefreshFromDatabaseButton.Name = "RefreshFromDatabaseButton";
            this.RefreshFromDatabaseButton.Size = new System.Drawing.Size(154, 23);
            this.RefreshFromDatabaseButton.TabIndex = 0;
            this.RefreshFromDatabaseButton.Text = "Refresh from database";
            this.RefreshFromDatabaseButton.UseVisualStyleBackColor = true;
            // 
            // RelationshipEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(1087, 523);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel5);
            this.Name = "RelationshipEditor";
            this.Text = "Relationship Editor";
            this.Load += new System.EventHandler(this.RelationshipEditor_Load);
            this.Shown += new System.EventHandler(this.Ideas3_Shown);
            this.panel1.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView KeysListView;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TreeView RelationshipTreeView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox RelationshipNameTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button RemoveRelaionshipFieldButton;
        private System.Windows.Forms.Button AddRelaionshipFieldButton;
        private System.Windows.Forms.ComboBox RelationshipFieldRightComboBox2;
        private System.Windows.Forms.ComboBox RelationshipFieldLeftComboBox2;
        private System.Windows.Forms.ComboBox RelationshipFieldRightComboBox1;
        private System.Windows.Forms.ComboBox RelationshipFieldLeftComboBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox RelationshipTypeComboBox;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ListView ColumnsListView;
        private System.Windows.Forms.TextBox RelationshipTagsTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button RefreshFromDatabaseButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.CheckBox ShowDatabaseRelationshipsCheckBox;
        private System.Windows.Forms.CheckBox ShowCustomRelationshipsCheckBox;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ColumnHeader NameColumnHeader;
        private System.Windows.Forms.ColumnHeader TypeColumnHeader;
        private System.Windows.Forms.ColumnHeader SizeColumnHeader;
        private System.Windows.Forms.ColumnHeader Indexed;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListView TableListView;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox SchemaComboBox;
        private System.Windows.Forms.Button AddTableButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.ListView IndexListView;
        private System.Windows.Forms.ColumnHeader IndexColumnName;
        private System.Windows.Forms.ColumnHeader IndexColumnColumns;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button AddRelaionshipButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.ColumnHeader IndexColumnStatus;
    }
}