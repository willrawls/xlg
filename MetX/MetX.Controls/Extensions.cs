using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MetX.Controls
{
    public static class Extensions
    {
        public static TEnumOut As<TEnumOut>(this Enum target) where TEnumOut : struct
        {
            if (Enum.TryParse(typeof(TEnumOut), target.ToString(), true, out object? translated))
            {
                return (TEnumOut?) translated ?? default(TEnumOut);
            }

            return default(TEnumOut);
        }

        public static ContextMenuStrip BuildContextMenu(string name, List<ToolStripItem> items = null)
        {
            var ret = new ContextMenuStrip
            {
                Name = name.Replace(" ", "") + "ContextMenuStrip",
                Text = name
            };
            if (items == null || items.Count == 0)
            {
                return ret;
            }
            ret.Items.AddRange(items.ToArray());
            return ret;
        }

        /*
        public static TreeNode BuildTreeNode(this MenuActionType actionType, string text, TreeNode[] children = null)
        {
            var ret = children == null ? new TreeNode(text) : new TreeNode(text, children);
            ret.ContextMenuStrip = BuildContextMenu(actionType, text);
            return ret;
        }

        public static IEnumerable<ToolStripItem> ProviderActions(this MenuActionType actionType)
        {
            return BuildActionMenuItems(actionType, new[] { "Add", "Remove", "Test" });
        }
    */
    }
}