using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XLG.QuickScripts.Walker
{
    public partial class DatabaseWalkerForm : Form
    {
        public DatabaseWalkerForm()
        {
            InitializeComponent();
        }

        private void Ideas4_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var text = InputFileTextBox.Text;
            if (!File.Exists(text))
                return;

            new Ideas3(text).Show(this);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            new Ideas2Form().Show(this);
        }
    }
}
