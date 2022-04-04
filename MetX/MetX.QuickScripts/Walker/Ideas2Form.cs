using System;
using System.Windows.Forms;

namespace XLG.QuickScripts.Walker
{
    public partial class Ideas2Form : Form
    {
        public Ideas1b Ideas {get; set; }
        public Ideas2Form()
        {
            InitializeComponent();
            Ideas = new Ideas1b();
            Controls.Add(Ideas);
            Ideas.Dock = DockStyle.Fill;
        }

        private void Ideas2Form_Shown(object sender, EventArgs e)
        {
            Ideas.Visible = true;
        }
    }
}
