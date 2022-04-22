using System;
using System.Windows.Forms;

namespace XLG.QuickScripts.Walker
{
    public partial class DatabaseTemplateEditorForm : Form
    {
        public DatabaseTemplateEditorControl Ideas {get; set; }
        public DatabaseTemplateEditorForm()
        {
            InitializeComponent();
            Ideas = new DatabaseTemplateEditorControl();
            Controls.Add(Ideas);
            Ideas.Dock = DockStyle.Fill;
        }

        private void Ideas2Form_Shown(object sender, EventArgs e)
        {
            Ideas.Visible = true;
        }
    }
}
