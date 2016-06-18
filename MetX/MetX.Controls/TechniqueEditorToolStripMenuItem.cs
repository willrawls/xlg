using System;
using System.Windows.Forms;

namespace MetX.Controls
{
    public class TechniqueEditorToolStripMenuItem : ToolStripMenuItem
    {
        public readonly MenuActionType ActionType;

        public TechniqueEditorToolStripMenuItem(MenuActionType actionType, string text)
            : base(text)
        {
            ActionType = actionType;
            Click += OnActionClick;
        }

        private void OnActionClick(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("{0} => {1}", ActionType, Text));
        }
    }
}