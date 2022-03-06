namespace XLG.QuickScripts.Walker;

partial class Ideas2
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.quickScriptControl2 = new MetX.Controls.QuickScriptControl();
            this.quickScriptControl1 = new MetX.Controls.QuickScriptControl();
            this.ScriptEditor = new MetX.Controls.QuickScriptControl();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.VBar = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(317, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(684, 826);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // quickScriptControl2
            // 
            this.quickScriptControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quickScriptControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickScriptControl2.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.quickScriptControl2.IsIconBarVisible = true;
            this.quickScriptControl2.IsReadOnly = false;
            this.quickScriptControl2.Location = new System.Drawing.Point(61, 582);
            this.quickScriptControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.quickScriptControl2.Name = "quickScriptControl2";
            this.quickScriptControl2.Size = new System.Drawing.Size(619, 239);
            this.quickScriptControl2.TabIndex = 3;
            // 
            // quickScriptControl1
            // 
            this.quickScriptControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quickScriptControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickScriptControl1.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.quickScriptControl1.IsIconBarVisible = true;
            this.quickScriptControl1.IsReadOnly = false;
            this.quickScriptControl1.Location = new System.Drawing.Point(61, 252);
            this.quickScriptControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.quickScriptControl1.Name = "quickScriptControl1";
            this.quickScriptControl1.Size = new System.Drawing.Size(619, 320);
            this.quickScriptControl1.TabIndex = 2;
            // 
            // ScriptEditor
            // 
            this.ScriptEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ScriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScriptEditor.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ScriptEditor.IsIconBarVisible = true;
            this.ScriptEditor.IsReadOnly = false;
            this.ScriptEditor.Location = new System.Drawing.Point(61, 5);
            this.ScriptEditor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ScriptEditor.Name = "ScriptEditor";
            this.ScriptEditor.Size = new System.Drawing.Size(619, 237);
            this.ScriptEditor.TabIndex = 1;
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
            // VBar
            // 
            this.VBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.VBar.Location = new System.Drawing.Point(10, 10);
            this.VBar.Name = "VBar";
            this.VBar.Size = new System.Drawing.Size(307, 826);
            this.VBar.TabIndex = 2;
            // 
            // Ideas2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.VBar);
            this.Name = "Ideas2";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(1011, 846);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private MetX.Controls.QuickScriptControl quickScriptControl2;
    private MetX.Controls.QuickScriptControl quickScriptControl1;
    private MetX.Controls.QuickScriptControl ScriptEditor;
    private System.Windows.Forms.FlowLayoutPanel VBar;
}