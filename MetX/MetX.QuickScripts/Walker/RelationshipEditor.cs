using System.IO;
using System.Windows.Forms;
using MetX.Fimm;

using MetX.Standard.Primary.Metadata;

namespace XLG.QuickScripts.Walker
{
    public partial class RelationshipEditor : Form
    {
        public bool UpdatingInterface;
        public string Filename;
        public xlgDoc XlgDocument;

        public TreeNode RelatedNode;
        public TreeNode LookupNode;
        public TreeNode OtherNode;

        public RelationshipEditor(string filename)
        {
            Filename = Shared.Dirs.ResolveVariables(filename);
            InitializeComponent();
            if (File.Exists(Filename))
                XlgDocument = xlgDoc.LoadXmlFromFile(Filename).MakeViable();
            else
                XlgDocument = xlgDoc.Empty(Filename);
        }

        private void RelationshipEditor_Load(object sender, System.EventArgs e)
        {
            ReloadInterfaceFromXlgDocument();
            foreach (TreeNode node in RelationshipTreeView.Nodes)
            {
                node.ExpandAll();
            }
        }

        private void Ideas3_Shown(object sender, System.EventArgs e)
        {
            ReloadInterfaceFromXlgDocument();
        }

        private void ReloadInterfaceFromXlgDocument()
        {
            ClearInterfaces();


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

        private void ClearInterfaces()
        {
            if (UpdatingInterface)
                return;

            UpdatingInterface = true;
            
            RelationshipTreeView.Nodes.Clear();
            RelationshipNameTextBox.Text = "";
            RelationshipTypeComboBox.Text = "";
            RelationshipTagsTextBox.Text = "";

            ClearRelationshipFields();

            SchemaComboBox.Text = "";
            SchemaComboBox.Items.Clear();
            TableListView.Items.Clear();
            ColumnsListView.Items.Clear();
            IndexListView.Items.Clear();
            KeysListView.Items.Clear();

            UpdatingInterface = false;
        }

        private void ClearRelationshipFields()
        {
            RelationshipFieldLeftComboBox1.Text = "";
            RelationshipFieldLeftComboBox2.Text = "";
            RelationshipFieldRightComboBox1.Text = "";
            RelationshipFieldRightComboBox2.Text = "";
        }


    }
}
