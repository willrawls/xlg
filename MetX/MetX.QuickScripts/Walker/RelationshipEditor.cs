using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MetX.Fimm;

using MetX.Standard.Primary.Metadata;
using MetX.Standard.Strings.Extensions;

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

        public void RelationshipEditor_Load(object sender, System.EventArgs e)
        {
            ReloadInterfaceFromXlgDocument();
            foreach (TreeNode node in RelationshipTreeView.Nodes)
            {
                node.ExpandAll();
            }
        }

        public void Ideas3_Shown(object sender, System.EventArgs e)
        {
            ReloadInterfaceFromXlgDocument();
        }

        public void ReloadInterfaceFromXlgDocument()
        {
            ClearInterfaces();

            RelatedNode = RelationshipTreeView.Nodes.Add("Related", "Related");
            LookupNode = RelationshipTreeView.Nodes.Add("Lookup", "Lookup");
            OtherNode = RelationshipTreeView.Nodes.Add("Other", "Other");

            ReloadTableListView();
            ReloadRelationships();
        }

        public void ReloadRelationships()
        {
            ReloadDatabaseRelationships();
            ReloadCustomRelationships();
            ReloadLookupRelationships();
            ReloadOtherRelationships();
        }

        public void ReloadDatabaseRelationships()
        {
            foreach (var relationship in XlgDocument.Relationships)
            {

            }
        }

        public void ReloadCustomRelationships()
        {
            
        }

        public void ReloadLookupRelationships()
        {
            
        }

        public void ReloadOtherRelationships()
        {
            
        }

        public void ReloadTableListView()
        {
            foreach (var table in XlgDocument.Tables)
            {
                ListViewItem listViewItem = new();

                listViewItem.SubItems.Add(table.TableName);
                listViewItem.SubItems.Add(table.RowCount.AsString("0"));
                listViewItem.SubItems.Add(ChildrenOf(table)?.Count.AsString("0"));
                listViewItem.SubItems.Add(ParentOf(table));
                
                TableListView.Items.Add(listViewItem);
            }
        }

        public string ParentOf(Table table)
        {
            XlgDocument.Relationships 
            return "";
        }

        public List<Table> ChildrenOf(Table table)
        {
            return new();
        }

        public void ClearInterfaces()
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

        public void ClearRelationshipFields()
        {
            RelationshipFieldLeftComboBox1.Text = "";
            RelationshipFieldLeftComboBox2.Text = "";
            RelationshipFieldRightComboBox1.Text = "";
            RelationshipFieldRightComboBox2.Text = "";
        }


    }
}
