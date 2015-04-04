using System;
using System.Windows.Forms;

namespace MetX.Controls
{
    public partial class TechniquesEditor : ToolWindow
    {
        public TechniquesEditor()
        {
            InitializeComponent();
        }

        private void NewTechniquesFileMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Add("Techniques").Nodes.AddRange(new[]
            {
                new TreeNode("Quick Script Files", new []
                {
                    new TreeNode("Default"){Tag = "{default quick script file}"},
                }),
                new TreeNode("Pipeline Files", new []
                {
                    new TreeNode("Examples"){Tag = "{default pipeline file}"},
                }),
                new TreeNode("Settings", new []
                {
                    new TreeNode("Data Connections"),
                    new TreeNode("Output locations"),
                    new TreeNode("Quick Script Templates"),
                    new TreeNode("XSL Templates"),
                    new TreeNode("Pipeline Providers"),
                }),
            });
            treeView1.ExpandAll();
        }
    }
}