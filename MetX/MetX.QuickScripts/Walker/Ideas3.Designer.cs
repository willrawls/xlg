namespace XLG.QuickScripts.Walker
{
    partial class Ideas3
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
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "States",
            "50"}, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "Bob.Person",
            "1001",
            "1",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "Employee",
            "456",
            "",
            "Bob.Person"}, -1);
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Employee");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Bob.Person", new System.Windows.Forms.TreeNode[] {
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Database Name (Metadata name?)", new System.Windows.Forms.TreeNode[] {
            treeNode8});
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.label5 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.listView3 = new System.Windows.Forms.ListView();
            this.label12 = new System.Windows.Forms.Label();
            this.listView4 = new System.Windows.Forms.ListView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.listView4);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.listView2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(321, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(341, 560);
            this.panel1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(126, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Schema";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "dbo",
            "Bob"});
            this.comboBox1.Location = new System.Drawing.Point(181, 9);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(136, 23);
            this.comboBox1.TabIndex = 12;
            this.comboBox1.Text = "All";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Tables";
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.listView2.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem7,
            listViewItem8,
            listViewItem9});
            this.listView2.Location = new System.Drawing.Point(19, 53);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(300, 278);
            this.listView2.TabIndex = 10;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Table Name (Metadata name?)";
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 361);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "Table Keys";
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(11, 379);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(302, 153);
            this.listView1.TabIndex = 14;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.treeView1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(321, 560);
            this.panel2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Relationships";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 30);
            this.treeView1.Name = "treeView1";
            treeNode7.ForeColor = System.Drawing.Color.Red;
            treeNode7.Name = "Node2";
            treeNode7.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            treeNode7.Text = "Employee";
            treeNode8.Name = "Node1";
            treeNode8.Text = "Bob.Person";
            treeNode9.Name = "Node0";
            treeNode9.Text = "Database Name (Metadata name?)";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode9});
            this.treeView1.Size = new System.Drawing.Size(298, 249);
            this.treeView1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 282);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Relationship Properties";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.comboBox6);
            this.panel3.Controls.Add(this.textBox2);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.button2);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.comboBox5);
            this.panel3.Controls.Add(this.comboBox4);
            this.panel3.Controls.Add(this.comboBox3);
            this.panel3.Controls.Add(this.comboBox2);
            this.panel3.Location = new System.Drawing.Point(12, 300);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(299, 244);
            this.panel3.TabIndex = 8;
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
            // comboBox6
            // 
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Location = new System.Drawing.Point(55, 42);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(121, 23);
            this.comboBox6.TabIndex = 10;
            this.comboBox6.Text = "One to many";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(46, 209);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(240, 23);
            this.textBox2.TabIndex = 9;
            this.textBox2.Text = "hidden implied explicit  added";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1, 212);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 15);
            this.label11.TabIndex = 8;
            this.label11.Text = "Tags";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(55, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(240, 23);
            this.textBox1.TabIndex = 9;
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
            this.label7.Location = new System.Drawing.Point(175, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 15);
            this.label7.TabIndex = 7;
            this.label7.Text = "dbo.Employee";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 6;
            this.label6.Text = "Bob.Person";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(130, 125);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(39, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "-";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(130, 154);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(175, 125);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(121, 23);
            this.comboBox5.TabIndex = 3;
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(3, 125);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(121, 23);
            this.comboBox4.TabIndex = 2;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(175, 96);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 23);
            this.comboBox3.TabIndex = 1;
            this.comboBox3.Text = "PersonID";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(3, 96);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 23);
            this.comboBox2.TabIndex = 0;
            this.comboBox2.Text = "PersonID";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.listView3);
            this.panel4.Controls.Add(this.listView1);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(662, 42);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(322, 560);
            this.panel4.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 15);
            this.label10.TabIndex = 15;
            this.label10.Text = "Table Columns";
            // 
            // listView3
            // 
            this.listView3.Location = new System.Drawing.Point(8, 53);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(302, 278);
            this.listView3.TabIndex = 16;
            this.listView3.UseCompatibleStateImageBehavior = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(19, 361);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 15);
            this.label12.TabIndex = 13;
            this.label12.Text = "Table Indexes";
            // 
            // listView4
            // 
            this.listView4.Location = new System.Drawing.Point(19, 379);
            this.listView4.Name = "listView4";
            this.listView4.Size = new System.Drawing.Size(302, 153);
            this.listView4.TabIndex = 14;
            this.listView4.UseCompatibleStateImageBehavior = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.button6);
            this.panel5.Controls.Add(this.button5);
            this.panel5.Controls.Add(this.button4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1015, 42);
            this.panel5.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(272, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(39, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "+";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(7, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(181, 23);
            this.button4.TabIndex = 0;
            this.button4.Text = "Refresh (Keep overrides)";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(217, 12);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(181, 23);
            this.button5.TabIndex = 0;
            this.button5.Text = "Refresh (Keep database)";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(413, 12);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(181, 23);
            this.button6.TabIndex = 0;
            this.button6.Text = "Refresh (Implied only)";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // Ideas3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 602);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel5);
            this.Name = "Ideas3";
            this.Text = "Ideas3";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListView listView4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
    }
}