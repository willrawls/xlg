using System.Drawing;
using System.Windows.Forms;
using MetX.Standard.Library.Extensions;
using MetX.Standard.XDString;

namespace XLG.QuickScripts.Walker
{
    public partial class ListViewSlidePanel : UserControl
    {
        public AssocArray State;

        public ListViewSlidePanel()
        {
            InitializeComponent();
        }

        public ListViewSlidePanel Setup(int tabIndex, AssocArrayList states, string name)
        {
            State = states[name];
            TitleLabel.Text = name;
            TabIndex = tabIndex;
            Height = TitleLabel.Height + 20 + (State.Count + 1) * 50;
            Visible = true;
            ChildControl.Visible = true;
            foreach(var item in State.Items)
            {
                var listViewItem = new ListViewItem(item.Key);
                if (item.Value.IsNotEmpty())
                {
                    listViewItem.Font = new Font(Font, System.Drawing.FontStyle.Bold);
                }
                listViewItem.ForeColor = Color.Yellow;
                listViewItem.BackColor = Color.DarkBlue;

                ChildControl.Items.Add(listViewItem);
            }

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
    }
}
