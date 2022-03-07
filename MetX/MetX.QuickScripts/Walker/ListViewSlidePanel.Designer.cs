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
            this.ChildPanel.Location = new System.Drawing.Point(0, 35);
            this.ChildPanel.Name = "ChildPanel";
            this.ChildPanel.Padding = new System.Windows.Forms.Padding(2);
            this.ChildPanel.Size = new System.Drawing.Size(232, 87);
            this.ChildPanel.TabIndex = 3;
            // 
            // ChildControl
            // 
            this.ChildControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChildControl.Location = new System.Drawing.Point(2, 2);
            this.ChildControl.Name = "ChildControl";
            this.ChildControl.Size = new System.Drawing.Size(228, 83);
            this.ChildControl.TabIndex = 0;
            this.ChildControl.UseCompatibleStateImageBehavior = false;
            this.ChildControl.View = System.Windows.Forms.View.Details;
            // 
            // TitlePanel
            // 
            this.TitlePanel.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.TitlePanel.Controls.Add(this.TitleLabel);
            this.TitlePanel.Controls.Add(this.ToggleSizeButton);
            this.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitlePanel.Location = new System.Drawing.Point(0, 0);
            this.TitlePanel.Name = "TitlePanel";
            this.TitlePanel.Padding = new System.Windows.Forms.Padding(8);
            this.TitlePanel.Size = new System.Drawing.Size(232, 35);
            this.TitlePanel.TabIndex = 2;
            // 
            // TitleLabel
            // 
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TitleLabel.Location = new System.Drawing.Point(8, 8);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(196, 19);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Title of slide panel";
            // 
            // ToggleSizeButton
            // 
            this.ToggleSizeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.ToggleSizeButton.FlatAppearance.BorderSize = 0;
            this.ToggleSizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ToggleSizeButton.Image = ((System.Drawing.Image)(resources.GetObject("ToggleSizeButton.Image")));
            this.ToggleSizeButton.Location = new System.Drawing.Point(204, 8);
            this.ToggleSizeButton.Name = "ToggleSizeButton";
            this.ToggleSizeButton.Size = new System.Drawing.Size(20, 19);
            this.ToggleSizeButton.TabIndex = 1;
            this.ToggleSizeButton.Tag = "open";
            this.ToggleSizeButton.UseVisualStyleBackColor = true;
            this.ToggleSizeButton.Click += new System.EventHandler(this.ToggleSizeButton_Click);
            // 
            // ListViewSlidePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ChildPanel);
            this.Controls.Add(this.TitlePanel);
            this.Name = "ListViewSlidePanel";
            this.Size = new System.Drawing.Size(232, 122);
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
    }
}
