﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XLG.QuickScripts.Walker
{
    public partial class Ideas3 : Form
    {
        public Ideas3()
        {
            InitializeComponent();
            foreach (TreeNode node in treeView1.Nodes)
            {
                node.Expand();
            }
        }
    }
}