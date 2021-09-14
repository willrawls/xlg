using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MetX.Controls
{
    public static class Extensions
    {
        public static Color HalfMix(this Color one, Color two)  
        {
            return Color.FromArgb(
                (one.A + two.A) >> 1,
                (one.R + two.R) >> 1,
                (one.G + two.G) >> 1,
                (one.B + two.B) >> 1);
        }

        /*
        public static TEnumOut As<TEnumOut>(this Enum target) where TEnumOut : struct
        {
            if (Enum.TryParse(typeof(TEnumOut), target.ToString(), true, out object? translated))
            {
                return (TEnumOut?) translated ?? default(TEnumOut);
            }

            return default;
        }

        */
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