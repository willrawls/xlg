namespace MetX.Windows.Controls
{
    partial class OrderedSelector
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
            this.SourceList = new System.Windows.Forms.ListBox();
            this.SelectionList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.FirstButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.LastButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SourceList
            // 
            this.SourceList.FormattingEnabled = true;
            this.SourceList.Location = new System.Drawing.Point(3, 20);
            this.SourceList.Name = "SourceList";
            this.SourceList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.SourceList.Size = new System.Drawing.Size(154, 186);
            this.SourceList.TabIndex = 0;
            // 
            // SelectionList
            // 
            this.SelectionList.FormattingEnabled = true;
            this.SelectionList.Location = new System.Drawing.Point(216, 20);
            this.SelectionList.Name = "SelectionList";
            this.SelectionList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.SelectionList.Size = new System.Drawing.Size(154, 186);
            this.SelectionList.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Source";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(213, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Selection (Ordered)";
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(162, 20);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(48, 23);
            this.AddButton.TabIndex = 4;
            this.AddButton.Text = ">>";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(162, 49);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(48, 23);
            this.RemoveButton.TabIndex = 4;
            this.RemoveButton.Text = "<<";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // FirstButton
            // 
            this.FirstButton.Location = new System.Drawing.Point(376, 20);
            this.FirstButton.Name = "FirstButton";
            this.FirstButton.Size = new System.Drawing.Size(48, 23);
            this.FirstButton.TabIndex = 4;
            this.FirstButton.Text = "&First";
            this.FirstButton.UseVisualStyleBackColor = true;
            // 
            // UpButton
            // 
            this.UpButton.Location = new System.Drawing.Point(376, 49);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(48, 23);
            this.UpButton.TabIndex = 4;
            this.UpButton.Text = "Up";
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // DownButton
            // 
            this.DownButton.Location = new System.Drawing.Point(376, 78);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(48, 23);
            this.DownButton.TabIndex = 4;
            this.DownButton.Text = "Down";
            this.DownButton.UseVisualStyleBackColor = true;
            this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // LastButton
            // 
            this.LastButton.Location = new System.Drawing.Point(376, 107);
            this.LastButton.Name = "LastButton";
            this.LastButton.Size = new System.Drawing.Size(48, 23);
            this.LastButton.TabIndex = 4;
            this.LastButton.Text = "&Last";
            this.LastButton.UseVisualStyleBackColor = true;
            this.LastButton.Click += new System.EventHandler(this.LastButton_Click);
            // 
            // OrderedSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LastButton);
            this.Controls.Add(this.UpButton);
            this.Controls.Add(this.DownButton);
            this.Controls.Add(this.FirstButton);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectionList);
            this.Controls.Add(this.SourceList);
            this.Name = "OrderedSelector";
            this.Size = new System.Drawing.Size(430, 214);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox SourceList;
        private System.Windows.Forms.ListBox SelectionList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button FirstButton;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button LastButton;
    }
}
