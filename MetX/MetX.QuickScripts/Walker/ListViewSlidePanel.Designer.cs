namespace XLG.QuickScripts.Walker
{
    partial class ListViewSlidePanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListViewSlidePanel));
            this.ChildPanel = new System.Windows.Forms.Panel();
            this.ChildControl = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.TitlePanel = new System.Windows.Forms.Panel();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.ToggleSizeButton = new System.Windows.Forms.Button();
            this.ChildPanel.SuspendLayout();
            this.TitlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChildPanel
            // 
            this.ChildPanel.Controls.Add(this.ChildControl);
            this.ChildPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChildPanel.Location = new System.Drawing.Point(0, 26);
            this.ChildPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ChildPanel.Name = "ChildPanel";
            this.ChildPanel.Padding = new System.Windows.Forms.Padding(2);
            this.ChildPanel.Size = new System.Drawing.Size(203, 66);
            this.ChildPanel.TabIndex = 3;
            // 
            // ChildControl
            // 
            this.ChildControl.BackColor = System.Drawing.Color.Cornsilk;
            this.ChildControl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ChildControl.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ChildControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChildControl.ForeColor = System.Drawing.Color.Black;
            this.ChildControl.FullRowSelect = true;
            this.ChildControl.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ChildControl.Location = new System.Drawing.Point(2, 2);
            this.ChildControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ChildControl.MultiSelect = false;
            this.ChildControl.Name = "ChildControl";
            this.ChildControl.Size = new System.Drawing.Size(199, 62);
            this.ChildControl.TabIndex = 0;
            this.ChildControl.UseCompatibleStateImageBehavior = false;
            this.ChildControl.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 200;
            // 
            // TitlePanel
            // 
            this.TitlePanel.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.TitlePanel.Controls.Add(this.TitleLabel);
            this.TitlePanel.Controls.Add(this.ToggleSizeButton);
            this.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitlePanel.Location = new System.Drawing.Point(0, 0);
            this.TitlePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TitlePanel.Name = "TitlePanel";
            this.TitlePanel.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.TitlePanel.Size = new System.Drawing.Size(203, 26);
            this.TitlePanel.TabIndex = 2;
            // 
            // TitleLabel
            // 
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TitleLabel.Location = new System.Drawing.Point(7, 6);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(171, 14);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Title of slide panel";
            this.TitleLabel.Click += new System.EventHandler(this.TitleLabel_Click);
            // 
            // ToggleSizeButton
            // 
            this.ToggleSizeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.ToggleSizeButton.FlatAppearance.BorderSize = 0;
            this.ToggleSizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ToggleSizeButton.Image = ((System.Drawing.Image)(resources.GetObject("ToggleSizeButton.Image")));
            this.ToggleSizeButton.Location = new System.Drawing.Point(178, 6);
            this.ToggleSizeButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ToggleSizeButton.Name = "ToggleSizeButton";
            this.ToggleSizeButton.Size = new System.Drawing.Size(18, 14);
            this.ToggleSizeButton.TabIndex = 1;
            this.ToggleSizeButton.Tag = "open";
            this.ToggleSizeButton.UseVisualStyleBackColor = true;
            this.ToggleSizeButton.Click += new System.EventHandler(this.ToggleSizeButton_Click);
            // 
            // ListViewSlidePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ChildPanel);
            this.Controls.Add(this.TitlePanel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ListViewSlidePanel";
            this.Size = new System.Drawing.Size(203, 92);
            this.ChildPanel.ResumeLayout(false);
            this.TitlePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ChildPanel;
        private System.Windows.Forms.ListView ChildControl;
        private System.Windows.Forms.Panel TitlePanel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Button ToggleSizeButton;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
