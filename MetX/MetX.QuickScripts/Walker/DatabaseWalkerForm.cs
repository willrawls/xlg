using System;
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

        private void EditInputFileButton_Click(object sender, EventArgs e)
        {
            var text = InputFileTextBox.Text;
            //if (!File.Exists(text))
             //   return;

            new RelationshipEditor(text).Show(this);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            new DatabaseTemplateEditorForm().Show(this);
        }
    }
}
