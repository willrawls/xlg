using System;
using System.Drawing;
using System.Windows.Forms;
using MetX.Standard.Strings;
using MetX.Windows.Controls.Properties;

namespace XLG.QuickScripts.Walker
{
    public partial class SlidePanelBase : UserControl
    {
        protected Control ChildControl { get; private set; }
        public Size SizeWhenClosed {get; set; }
        public Size SizeWhenOpen {get; set;}

        public SlidePanelBase()
        {
            InitializeComponent();
        }

        public void Setup(Control child, string title, Size sizeWhenClosed, Size sizeWhenOpen, bool startOpen)
        {
            SizeWhenClosed = sizeWhenClosed;
            SizeWhenOpen = sizeWhenOpen;

            SuspendLayout();
            TitleLabel.Text = title;
            
            ChildPanel.Controls.Clear();
            ChildPanel.Controls.Add(ChildControl);
            
            ChildControl = child;
            ChildControl.Dock = DockStyle.Fill;
            ChildControl.Visible = true;

            MinimumSize = sizeWhenClosed;
            MaximumSize = sizeWhenOpen;
            Size = startOpen ? sizeWhenOpen : sizeWhenClosed;
            ResumeLayout();
            Refresh();
        }

        private void ToggleSizeButton_Click(object sender, EventArgs e)
        {
            if (ToggleSizeButton.Tag.AsStringFromObject().Contains("open"))
            {
                // Open going to closed
                ToggleSizeButton.Tag = "closed";
                ToggleSizeButton.Image = Resources.arrow_up_s_line;
                Size = SizeWhenClosed;
            }
            else
            {
                // Closed going to open
                ToggleSizeButton.Tag = "open";
                ToggleSizeButton.Image = Resources.arrow_down_s_line;
                Size = SizeWhenOpen;
            }
        }
    }
}
