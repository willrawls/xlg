using System;
using System.Windows.Forms;

namespace XLG.Pipeliner
{
    public partial class ClipScriptOutput : Form
    {
        public ClipScriptOutput(string title, string output)
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
