using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetX.Standard.XDString;

namespace XLG.QuickScripts.Walker
{
    public partial class Ideas2Form : Form
    {
        public Ideas2 Ideas {get; set; }
        public Ideas2Form()
        {
            InitializeComponent();
            Ideas = new Ideas2();
            Controls.Add(Ideas);
            Ideas.Dock = DockStyle.Fill;
        }

        private void Ideas2Form_Shown(object sender, EventArgs e)
        {
            Ideas.Visible = true;
        }
    }
}
