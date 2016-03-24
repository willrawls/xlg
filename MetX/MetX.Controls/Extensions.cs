using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MetX.Controls
{
    public static class Extensions
    {
        public static TreeNode BuildTreeNode(this MenuActionType actionType, string text, TreeNode[] children = null)
        {
            var ret = children == null ? new TreeNode(text) : new TreeNode(text, children);
            ret.ContextMenuStrip = BuildContextMenu(actionType, text);
            return ret;
        }

        public static ContextMenuStrip BuildContextMenu(this MenuActionType actionType, string name)
        {
            var items = new List<ToolStripItem>();
            switch (actionType)
            {
                case MenuActionType.TechniqueFile:
                    items.AddRange(BasicFileActions(actionType));
                    break;

                case MenuActionType.QuickScriptFiles:
                case MenuActionType.QuickScriptFile:
                    items.AddRange(BasicFileActions(actionType));
                    break;

                case MenuActionType.PipelineFiles:
                case MenuActionType.PipelineFile:
                    items.AddRange(BasicFileActions(actionType));
                    break;

                case MenuActionType.Connections:
                case MenuActionType.Connection:
                    items.AddRange(BasicItemActions(actionType));
                    break;

                case MenuActionType.Directorys:
                case MenuActionType.Directory:
                    items.AddRange(BasicItemActions(actionType));
                    break;

                case MenuActionType.XslTemplates:
                case MenuActionType.XslTemplate:
                    items.AddRange(BasicFileActions(actionType));
                    break;

                case MenuActionType.Providers:
                case MenuActionType.Provider:
                    items.AddRange(BasicItemActions(actionType));
                    items.AddRange(ProviderActions(actionType));
                    break;
            }
            var ret = BuildContextMenu(name, items);
            return ret;
        }

        public static IEnumerable<ToolStripItem> BasicFileActions(this MenuActionType actionType)
            => BuildActionMenuItems(actionType, new[] { "Add new file", "Add existing file", "Duplicate", "Remove" });

        public static IEnumerable<ToolStripItem> BasicItemActions(this MenuActionType actionType)
            => BuildActionMenuItems(actionType, new[] { "New item", "Add", "Copy", "Remove" });

        public static IEnumerable<ToolStripItem> ProviderActions(this MenuActionType actionType)
            => BuildActionMenuItems(actionType, new[] { "Add", "Remove", "Test" });

        public static IEnumerable<ToolStripItem> BuildActionMenuItems(this MenuActionType actionType, string[] names)
        {
            return names.Select(item => new TechniqueEditorToolStripMenuItem(actionType, item));
        }

        public static ContextMenuStrip BuildContextMenu(string name, List<ToolStripItem> items = null)
        {
            var ret = new ContextMenuStrip
            {
                Name = name.Replace(" ", "") + "ContextMenu",
                Text = name
            };
            if (items == null || items.Count == 0)
            {
                return ret;
            }
            ret.Items.AddRange(items.ToArray());
            return ret;
        }
    }
}