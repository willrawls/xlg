namespace XLG.QuickScripts.Walker
{
    partial class DatabaseWalkerForm
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
            this.InputFileTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectInputFileButton = new System.Windows.Forms.Button();
            this.EditInputFileButton = new System.Windows.Forms.Button();
            this.SelectOutputFolderButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.OutputFolderTextBox = new System.Windows.Forms.TextBox();
            this.CreateOutputFolderButton = new System.Windows.Forms.Button();
            this.SelectTemplateFolderButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.TemplateFolderTextBox = new System.Windows.Forms.TextBox();
            this.CloneTemplateFolderButton = new System.Windows.Forms.Button();
            this.BuildButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.CreateTemplateFolderButton = new System.Windows.Forms.Button();
            this.EditTemplateButton = new System.Windows.Forms.Button();
            this.ExploreOutputFolderButton = new System.Windows.Forms.Button();
            this.CloneOutputFolderButton = new System.Windows.Forms.Button();
            this.CloneInputFileButton = new System.Windows.Forms.Button();
            this.CreateInputFileButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InputFileTextBox
            // 
            this.InputFileTextBox.Location = new System.Drawing.Point(107, 12);
            this.InputFileTextBox.Name = "InputFileTextBox";
            this.InputFileTextBox.Size = new System.Drawing.Size(641, 23);
            this.InputFileTextBox.TabIndex = 0;
            this.InputFileTextBox.Text = "%XLG%\\Pipes\\AdventureWorks\\AdventureWorks.Glove.xml";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input file";
            // 
            // SelectInputFileButton
            // 
            this.SelectInputFileButton.Location = new System.Drawing.Point(754, 12);
            this.SelectInputFileButton.Name = "SelectInputFileButton";
            this.SelectInputFileButton.Size = new System.Drawing.Size(33, 23);
            this.SelectInputFileButton.TabIndex = 1;
            this.SelectInputFileButton.Text = "...";
            this.SelectInputFileButton.UseVisualStyleBackColor = true;
            // 
            // EditInputFileButton
            // 
            this.EditInputFileButton.Location = new System.Drawing.Point(793, 12);
            this.EditInputFileButton.Name = "EditInputFileButton";
            this.EditInputFileButton.Size = new System.Drawing.Size(54, 23);
            this.EditInputFileButton.TabIndex = 2;
            this.EditInputFileButton.Text = "Edit";
            this.EditInputFileButton.UseVisualStyleBackColor = true;
            this.EditInputFileButton.Click += new System.EventHandler(this.EditInputFileButton_Click);
            // 
            // SelectOutputFolderButton
            // 
            this.SelectOutputFolderButton.Location = new System.Drawing.Point(754, 68);
            this.SelectOutputFolderButton.Name = "SelectOutputFolderButton";
            this.SelectOutputFolderButton.Size = new System.Drawing.Size(33, 23);
            this.SelectOutputFolderButton.TabIndex = 11;
            this.SelectOutputFolderButton.Text = "...";
            this.SelectOutputFolderButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Output folder";
            // 
            // OutputFolderTextBox
            // 
            this.OutputFolderTextBox.Location = new System.Drawing.Point(107, 69);
            this.OutputFolderTextBox.Name = "OutputFolderTextBox";
            this.OutputFolderTextBox.Size = new System.Drawing.Size(641, 23);
            this.OutputFolderTextBox.TabIndex = 10;
            this.OutputFolderTextBox.Text = "%XLG%\\Walkers\\Output\\Prexey\\";
            // 
            // CreateOutputFolderButton
            // 
            this.CreateOutputFolderButton.Location = new System.Drawing.Point(903, 67);
            this.CreateOutputFolderButton.Name = "CreateOutputFolderButton";
            this.CreateOutputFolderButton.Size = new System.Drawing.Size(54, 23);
            this.CreateOutputFolderButton.TabIndex = 14;
            this.CreateOutputFolderButton.Text = "Create";
            this.CreateOutputFolderButton.UseVisualStyleBackColor = true;
            // 
            // SelectTemplateFolderButton
            // 
            this.SelectTemplateFolderButton.Location = new System.Drawing.Point(754, 40);
            this.SelectTemplateFolderButton.Name = "SelectTemplateFolderButton";
            this.SelectTemplateFolderButton.Size = new System.Drawing.Size(33, 23);
            this.SelectTemplateFolderButton.TabIndex = 6;
            this.SelectTemplateFolderButton.Text = "...";
            this.SelectTemplateFolderButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Template folder";
            // 
            // TemplateFolderTextBox
            // 
            this.TemplateFolderTextBox.Location = new System.Drawing.Point(107, 41);
            this.TemplateFolderTextBox.Name = "TemplateFolderTextBox";
            this.TemplateFolderTextBox.Size = new System.Drawing.Size(641, 23);
            this.TemplateFolderTextBox.TabIndex = 5;
            this.TemplateFolderTextBox.Text = "%XLG%\\Walkers\\Templates\\Database\\Prexey\\";
            // 
            // CloneTemplateFolderButton
            // 
            this.CloneTemplateFolderButton.Location = new System.Drawing.Point(848, 39);
            this.CloneTemplateFolderButton.Name = "CloneTemplateFolderButton";
            this.CloneTemplateFolderButton.Size = new System.Drawing.Size(54, 23);
            this.CloneTemplateFolderButton.TabIndex = 8;
            this.CloneTemplateFolderButton.Text = "Clone";
            this.CloneTemplateFolderButton.UseVisualStyleBackColor = true;
            // 
            // BuildButton
            // 
            this.BuildButton.Location = new System.Drawing.Point(167, 98);
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(54, 23);
            this.BuildButton.TabIndex = 15;
            this.BuildButton.Text = "Build";
            this.BuildButton.UseVisualStyleBackColor = true;
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(107, 98);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(54, 23);
            this.RunButton.TabIndex = 16;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            // 
            // CreateTemplateFolderButton
            // 
            this.CreateTemplateFolderButton.Location = new System.Drawing.Point(903, 39);
            this.CreateTemplateFolderButton.Name = "CreateTemplateFolderButton";
            this.CreateTemplateFolderButton.Size = new System.Drawing.Size(54, 23);
            this.CreateTemplateFolderButton.TabIndex = 9;
            this.CreateTemplateFolderButton.Text = "Create";
            this.CreateTemplateFolderButton.UseVisualStyleBackColor = true;
            // 
            // EditTemplateButton
            // 
            this.EditTemplateButton.Location = new System.Drawing.Point(793, 39);
            this.EditTemplateButton.Name = "EditTemplateButton";
            this.EditTemplateButton.Size = new System.Drawing.Size(54, 23);
            this.EditTemplateButton.TabIndex = 7;
            this.EditTemplateButton.Text = "Edit";
            this.EditTemplateButton.UseVisualStyleBackColor = true;
            this.EditTemplateButton.Click += new System.EventHandler(this.button11_Click);
            // 
            // ExploreOutputFolderButton
            // 
            this.ExploreOutputFolderButton.Location = new System.Drawing.Point(793, 68);
            this.ExploreOutputFolderButton.Name = "ExploreOutputFolderButton";
            this.ExploreOutputFolderButton.Size = new System.Drawing.Size(54, 23);
            this.ExploreOutputFolderButton.TabIndex = 12;
            this.ExploreOutputFolderButton.Text = "Explore";
            this.ExploreOutputFolderButton.UseVisualStyleBackColor = true;
            // 
            // CloneOutputFolderButton
            // 
            this.CloneOutputFolderButton.Location = new System.Drawing.Point(848, 68);
            this.CloneOutputFolderButton.Name = "CloneOutputFolderButton";
            this.CloneOutputFolderButton.Size = new System.Drawing.Size(54, 23);
            this.CloneOutputFolderButton.TabIndex = 13;
            this.CloneOutputFolderButton.Text = "Clone";
            this.CloneOutputFolderButton.UseVisualStyleBackColor = true;
            // 
            // CloneInputFileButton
            // 
            this.CloneInputFileButton.Location = new System.Drawing.Point(848, 11);
            this.CloneInputFileButton.Name = "CloneInputFileButton";
            this.CloneInputFileButton.Size = new System.Drawing.Size(54, 23);
            this.CloneInputFileButton.TabIndex = 3;
            this.CloneInputFileButton.Text = "Clone";
            this.CloneInputFileButton.UseVisualStyleBackColor = true;
            // 
            // CreateInputFileButton
            // 
            this.CreateInputFileButton.Location = new System.Drawing.Point(903, 12);
            this.CreateInputFileButton.Name = "CreateInputFileButton";
            this.CreateInputFileButton.Size = new System.Drawing.Size(54, 23);
            this.CreateInputFileButton.TabIndex = 4;
            this.CreateInputFileButton.Text = "Create";
            this.CreateInputFileButton.UseVisualStyleBackColor = true;
            // 
            // DatabaseWalkerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(969, 127);
            this.Controls.Add(this.SelectTemplateFolderButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TemplateFolderTextBox);
            this.Controls.Add(this.SelectOutputFolderButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OutputFolderTextBox);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.CloneInputFileButton);
            this.Controls.Add(this.CloneTemplateFolderButton);
            this.Controls.Add(this.CreateInputFileButton);
            this.Controls.Add(this.CreateTemplateFolderButton);
            this.Controls.Add(this.CreateOutputFolderButton);
            this.Controls.Add(this.BuildButton);
            this.Controls.Add(this.CloneOutputFolderButton);
            this.Controls.Add(this.ExploreOutputFolderButton);
            this.Controls.Add(this.EditTemplateButton);
            this.Controls.Add(this.EditInputFileButton);
            this.Controls.Add(this.SelectInputFileButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InputFileTextBox);
            this.Name = "DatabaseWalkerForm";
            this.Text = "Walker";
            this.Load += new System.EventHandler(this.Ideas4_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox InputFileTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SelectInputFileButton;
        private System.Windows.Forms.Button EditInputFileButton;
        private System.Windows.Forms.Button SelectOutputFolderButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox OutputFolderTextBox;
        private System.Windows.Forms.Button CreateOutputFolderButton;
        private System.Windows.Forms.Button SelectTemplateFolderButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TemplateFolderTextBox;
        private System.Windows.Forms.Button CloneTemplateFolderButton;
        private System.Windows.Forms.Button BuildButton;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button CreateTemplateFolderButton;
        private System.Windows.Forms.Button EditTemplateButton;
        private System.Windows.Forms.Button ExploreOutputFolderButton;
        private System.Windows.Forms.Button CloneOutputFolderButton;
        private System.Windows.Forms.Button CloneInputFileButton;
        private System.Windows.Forms.Button CreateInputFileButton;
    }
}