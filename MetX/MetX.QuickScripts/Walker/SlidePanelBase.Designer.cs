namespace XLG.QuickScripts.Walker
{
    partial class SlidePanelBase
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
            this.TitlePanel = new System.Windows.Forms.Panel();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.ToggleSizeButton = new System.Windows.Forms.Button();
            this.ChildPanel = new System.Windows.Forms.Panel();
            this.TitlePanel.SuspendLayout();
            this.SuspendLayout();
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
            this.TitlePanel.Size = new System.Drawing.Size(359, 35);
            this.TitlePanel.TabIndex = 0;
            // 
            // TitleLabel
            // 
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TitleLabel.Location = new System.Drawing.Point(8, 8);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(323, 19);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Title of slide panel";
            // 
            // ToggleSizeButton
            // 
            this.ToggleSizeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.ToggleSizeButton.FlatAppearance.BorderSize = 0;
            this.ToggleSizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ToggleSizeButton.Image = global::MetX.Controls.Properties.Resources.arrow_up_s_line;
            this.ToggleSizeButton.Location = new System.Drawing.Point(331, 8);
            this.ToggleSizeButton.Name = "ToggleSizeButton";
            this.ToggleSizeButton.Size = new System.Drawing.Size(20, 19);
            this.ToggleSizeButton.TabIndex = 1;
            this.ToggleSizeButton.Tag = "open";
            this.ToggleSizeButton.UseVisualStyleBackColor = true;
            this.ToggleSizeButton.Click += new System.EventHandler(this.ToggleSizeButton_Click);
            // 
            // ChildPanel
            // 
            this.ChildPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChildPanel.Location = new System.Drawing.Point(0, 35);
            this.ChildPanel.Name = "ChildPanel";
            this.ChildPanel.Padding = new System.Windows.Forms.Padding(2);
            this.ChildPanel.Size = new System.Drawing.Size(359, 115);
            this.ChildPanel.TabIndex = 1;
            // 
            // SlidePanelBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ChildPanel);
            this.Controls.Add(this.TitlePanel);
            this.Name = "SlidePanelBase";
            this.Size = new System.Drawing.Size(359, 150);
            this.TitlePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TitlePanel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Button ToggleSizeButton;
        private System.Windows.Forms.Panel ChildPanel;
    }
}
