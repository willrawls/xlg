using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetX.Data.Techniques;

namespace MetX.Controls
{
    public partial class TechniquesEditor : ToolWindow
    {
        public CodeTechniques Techniques;

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

        public void PopulateTree()
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("Techniques").Nodes.AddRange(new[]
            {
                new TreeNode("Quick Script Files", Techniques.QuickScriptFiles.ToTreeNodes()),
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
        }
    }

    public static class E
    {
        public static TreeNode[] ToTreeNodes(this List<Reference> target, bool allowDefault = false)
        {
            if (target == null || target.Count == 0)
                if (allowDefault)
                    return new[] { new TreeNode("Default") { Tag = "{default" } };
                else
                    return null;
            start here
            foreach (Reference reference in target)
            {
            }
        }
    }
}