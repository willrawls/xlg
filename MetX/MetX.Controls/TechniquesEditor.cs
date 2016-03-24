using MetX.Techniques;
using System;
using System.Windows.Forms;

namespace MetX.Controls
{
    public partial class TechniquesEditor : ToolWindow
    {
        /*
                public void PopulateTree()
                {
                    FileTree.Nodes.Clear();
                    FileTree.Nodes.Add("Pattern").Nodes.AddRange(new[]
                    {
                        MenuActionType.QuickScriptFile.BuildTreeNode("Quick Script Files", null), //Pattern.Techniques.ToTreeNodes()),
                        MenuActionType.PipelineFile.BuildTreeNode("Pipeline Files", new []
                        {
                            MenuActionType.PipelineFile.BuildTreeNode("Examples"),
                        }),
                        MenuActionType.None.BuildTreeNode("Settings", new []
                        {
                            MenuActionType.Connection.BuildTreeNode("Data Connections"),
                            MenuActionType.Directory.BuildTreeNode("Output locations"),
                            Extensions.BuildTreeNode(MenuActionType.QuickScriptTemplate, "Quick Script Templates"), MenuActionType.XslTemplate.BuildTreeNode("XSL Templates"), MenuActionType.Provider.BuildTreeNode("Pipeline Providers"),
                        }),
                    });
                }
        */

        public TreeNode ClickedNode;
        public PatternWorks Pattern;

        public TechniquesEditor()
        {
            InitializeComponent();
        }

        private void NewTechniquesFileMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewTechniquesFile();
        }

        public void CreateNewTechniquesFile()
        {
            Pattern = new PatternWorks();
            FileTree.Nodes.Clear();
            FileTree.Nodes.Add(MenuActionType.None.BuildTreeNode("Pattern", new[]
            {
                MenuActionType.QuickScriptFiles.BuildTreeNode("Quick Script Files"),
                MenuActionType.PipelineFiles.BuildTreeNode("Pipeline Files"),
                MenuActionType.XslTemplates.BuildTreeNode("XSLT Files"),
                MenuActionType.None.BuildTreeNode("Settings", new[]
                {
                    MenuActionType.Connections.BuildTreeNode("Connections"),
                    MenuActionType.Directorys.BuildTreeNode("Directories"),
                    MenuActionType.Providers.BuildTreeNode("Providers")
                })
            }));
            FileTree.ExpandAll();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            ClickedNode = e.Node;
            ClickedNode.ContextMenuStrip.Show(FileTree, e.Location);
        }
    }
}