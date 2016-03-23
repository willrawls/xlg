using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MetX.Techniques;

namespace MetX.Controls
{
    public partial class TechniquesEditor : ToolWindow
    {
        public PatternWorks Pattern;

        public TechniquesEditor()
        {
            InitializeComponent();
        }

        private void NewTechniquesFileMenuItem_Click(object sender, EventArgs e)
        {
            FileTree.Nodes.Clear();
            FileTree.Nodes.Add(BuildTreeNode("Pattern", ContextMenuActionType.None, new[]
            {
                BuildTreeNode("Quick Script Files", ContextMenuActionType.QuickScriptFile),
                BuildTreeNode("Pipeline Files", ContextMenuActionType.PipelineFile),
                BuildTreeNode("Settings", ContextMenuActionType.None, new[]
                {
                    BuildTreeNode("Data Connections", ContextMenuActionType.Connection),
                    BuildTreeNode("Output locations", ContextMenuActionType.Output),
                    BuildTreeNode("Quick Script Templates", ContextMenuActionType.QuickScriptTemplate),
                    BuildTreeNode("XSL Templates", ContextMenuActionType.XslTemplate),
                    BuildTreeNode("Pipeline Providers", ContextMenuActionType.PipelineProvider),
                }),
            }));
            FileTree.ExpandAll();
        }

        public void PopulateTree()
        {
            FileTree.Nodes.Clear();
            FileTree.Nodes.Add("Pattern").Nodes.AddRange(new[]
            {
                BuildTreeNode("Quick Script Files", ContextMenuActionType.QuickScriptFile, null), //Pattern.Techniques.ToTreeNodes()),
                BuildTreeNode("Pipeline Files", ContextMenuActionType.PipelineFile, new []
                {
                    BuildTreeNode("Examples", ContextMenuActionType.PipelineFile ),
                }),
                BuildTreeNode("Settings", ContextMenuActionType.None, new []
                {
                    BuildTreeNode("Data Connections", ContextMenuActionType.Connection),
                    BuildTreeNode("Output locations", ContextMenuActionType.Output),
                    BuildTreeNode("Quick Script Templates", ContextMenuActionType.QuickScriptTemplate),
                    BuildTreeNode("XSL Templates", ContextMenuActionType.XslTemplate),
                    BuildTreeNode("Pipeline Providers", ContextMenuActionType.PipelineProvider),
                }),
            });
        }

        private TreeNode BuildTreeNode(string text, ContextMenuActionType actionType, TreeNode[] children = null)
        {
            var ret = children == null ? new TreeNode(text) : new TreeNode(text, children);
            ret.ContextMenuStrip = BuildContextMenu(text, actionType);
            return ret;
        }

        public TreeNode ClickedNode;

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            ClickedNode = e.Node;
            ClickedNode.ContextMenuStrip.Show(FileTree, e.Location);
        }

        public ContextMenuStrip BuildContextMenu(string name, ContextMenuActionType menuActionType)
        {
            List<ToolStripItem> items = new List<ToolStripItem>();
            switch (menuActionType)
            {
                case ContextMenuActionType.TechniqueFile:
                    items.AddRange(BasicFileActions);
                    break;
                case ContextMenuActionType.QuickScriptFile:
                    items.AddRange(BasicFileActions);
                    break;
                case ContextMenuActionType.PipelineFile:
                    items.AddRange(BasicFileActions);
                    break;
                case ContextMenuActionType.Connection:
                    items.AddRange(BasicItemActions);
                    break;
                case ContextMenuActionType.Output:
                    items.AddRange(BasicItemActions);
                    break;
                case ContextMenuActionType.QuickScriptTemplate:
                    items.AddRange(BasicFileActions);
                    break;
                case ContextMenuActionType.XslTemplate:
                    items.AddRange(BasicFileActions);
                    break;
                case ContextMenuActionType.PipelineProvider:
                    items.AddRange(BasicItemActions);
                    items.AddRange(ProviderActions);
                    break;
            }
            var ret = BuildContextMenu(name, items);
            return ret;
        }

        public IEnumerable<ToolStripItem> BasicFileActions => BuildActionMenuItems(new[] { "Add new file", "Add existing file", "Duplicate", "Remove" });
        public IEnumerable<ToolStripItem> BasicItemActions => BuildActionMenuItems(new[] { "New item", "Add", "Copy", "Remove" });
        public IEnumerable<ToolStripItem> ProviderActions => BuildActionMenuItems(new[] { "Add", "Remove", "Test" });

        private static List<ToolStripItem> BuildActionMenuItems(string[] names)
        {
            var items = new List<ToolStripItem>();
            items.AddRange(names.Select(item => new ToolStripMenuItem(item)));
            return items;
        }

        public ContextMenuStrip BuildContextMenu(string name, List<ToolStripItem> items = null)
        {
            var ret = new ContextMenuStrip
            {
                Name = name.Replace(" ", "") + "ContextMenu", Text = name,
            };
            if (items == null || items.Count == 0)
            {
                return ret;
            }
            ret.Items.AddRange(items.ToArray());
            return ret;
        }
    }

    public enum ContextMenuActionType
    {
        Unknown,
        TechniqueFile,
        QuickScriptFile,
        PipelineFile,
        Connection,
        Output,
        QuickScriptTemplate,
        XslTemplate,
        PipelineProvider,
        None
    }

    /*
        public static class E
        {
            public static TreeNode[] ToTreeNodes(this List<Reference> target, bool allowDefault = false)
            {
                if (target == null || target.Count == 0)
                    if (allowDefault)
                        return new[] { new TreeNode("Default") { Tag = "{default}" } };
                    else
                        return null;

                foreach (Reference reference in target)
                {
                    reference.Quality
                }
            }
        }
    */
}