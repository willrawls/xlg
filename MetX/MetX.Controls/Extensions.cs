/*using System;
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
        #1#

    }
}*/