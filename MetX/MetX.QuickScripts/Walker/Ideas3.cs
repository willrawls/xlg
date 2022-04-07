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
            foreach (TreeNode node in RelationshipTreeView.Nodes)
            {
                node.ExpandAll();
            }
        }

        private void Ideas3_Shown(object sender, System.EventArgs e)
        {
            UpdateItAll();
        }

        private void UpdateItAll()
        {
            RelationshipTreeView.Nodes.Clear();

            RelatedNode = RelationshipTreeView.Nodes.Add("Related", "Related");
            LookupNode = RelationshipTreeView.Nodes.Add("Lookup", "Lookup");
            OtherNode = RelationshipTreeView.Nodes.Add("Other", "Other");

            TableListView.Items.Clear();

            foreach (var table in XlgDocument.Tables)
            {
                ListViewItem listViewItem = new ListViewItem();
                TableListView.Items.Add(listViewItem);
            }
        }

        public TreeNode RelatedNode { get; set; }
        public TreeNode LookupNode { get; set; }
        public TreeNode OtherNode { get; set; }
    }
}
