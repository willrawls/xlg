using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            Text = "QuickScript Output - " + title;
            Output.Text = output;
        }

        private void QuickScriptOutput_Load(object sender, EventArgs e)
        {
            Output.SelectAll();
            Output.Focus();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.FileName = Text.Replace(":", string.Empty).Replace("/", "-");
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.CheckFileExists = false;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = ".txt";
                saveFileDialog1.Filter = "All files(*.*)|*.*";
                DialogResult result = saveFileDialog1.ShowDialog(this);
                if (result == DialogResult.OK && saveFileDialog1.FileName != null)
                {
                    File.WriteAllText(saveFileDialog1.FileName, Output.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(Output.Text);
        }
    }
}
