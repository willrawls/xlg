using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetX.Standard.Primary.Metadata;

namespace XLG.QuickScripts.Walker
{
    public partial class Ideas3 : Form
    {
        public string Filename { get; }
        public xlgDoc XlgDocument { get; set; }
        public Ideas3(string filename)
        {
            Filename = filename;
            InitializeComponent();
            XlgDocument = xlgDoc.LoadXmlFromFile(filename);
        }

        private void Ideas3_Load(object sender, System.EventArgs e)
        {
            
            foreach (TreeNode node in treeView1.Nodes)
            {
                node.ExpandAll();
            }

        }
    }
}
