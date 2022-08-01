using System.Drawing;
using System.Windows.Forms;
using MetX.Standard.Strings.Extensions;
using MetX.Standard.Strings.Generics;

namespace XLG.QuickScripts.Walker
{
    public partial class ListViewSlidePanel : UserControl
    {
        public AssocArrayOfT<AssocArrayOfT<AssocType<string>>> State;

        public ListViewSlidePanel()
        {
            InitializeComponent();
        }

        public ListViewSlidePanel Setup(int tabIndex, AssocSheet states, string name)
        {
            State = states.FirstAxis;
            TitleLabel.Text = name;
            TabIndex = tabIndex;
            Height = TitleLabel.Height + 20 + (State.Count + 1) * Font.Height;
            Visible = true;
            ChildControl.Visible = true;
            ChildControl.View = View.Details;
            foreach(var item in State.Items())
            {
                var listViewItem = new ListViewItem(item.Key);
                if (item.Value.IsNotEmpty())
                {
                    listViewItem.Font = new Font(Font, System.Drawing.FontStyle.Bold);
                }
                ChildControl.Items.Add(listViewItem);
            }

            ChildControl.Height = (State.Count + 1) * 50;
            ChildControl.Dock = DockStyle.Fill;

            return this;
        }

        private void ToggleSizeButton_Click(object sender, System.EventArgs e)
        {
            if (ToggleSizeButton.Tag.AsString() == "open")
            {
                // Close
                Height = TitleLabel.Height + 20;
                ToggleSizeButton.Tag = "closed";
            }
            else
            {
                Height = TitleLabel.Height + 20 + (State.Count + 1) * 50;
                ToggleSizeButton.Tag = "open";
            }
        }

        private void TitleLabel_Click(object sender, System.EventArgs e)
        {
            ToggleSizeButton_Click(sender, e);
        }
    }
}
