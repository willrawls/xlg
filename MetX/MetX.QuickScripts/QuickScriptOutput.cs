using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XLG.Pipeliner
{
    public partial class QuickScriptOutput : Form
    {
        public QuickScriptOutput(string title, string output)
        {
            InitializeComponent();
            base.Text = "ClipScript Output - " + title;
            Output.Text = output;
        }

        private void ClipScriptOutput_Load(object sender, EventArgs e)
        {
            Output.SelectAll();
            Output.Focus();
        }
    }
}
