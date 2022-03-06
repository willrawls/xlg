using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetX.Standard.Library.Extensions;
using MetX.Standard.XDString;

namespace MetX.Controls;

public partial class VerticalSlidePanelBar : UserControl
{
    public VerticalSlidePanelBar()
    {
        InitializeComponent();
    }

    public void Setup(List<AssocArray> states, Func<ListView, AssocArray, bool> onSelectionChanged)
    {
        SlidePanels = new List<ListViewSlidePanel>();

        foreach (var state in states)
        {
            var slidePanel = new ListViewSlidePanel(onSelectionChanged);
            var size = new Size(Width, 150);
            var min = new Size(Width, 35);
            var max = new Size(Width, 150);

            List<ListViewItem> items = new();
            foreach (var item in state.Items)
            {
                var listViewItem = new ListViewItem(item.Key);
                if (item.Value.IsNotEmpty())
                    listViewItem.Font = new Font(Font, FontStyle.Bold);
                items.Add(listViewItem);
            }

            slidePanel.SetupListViewAndPanel(state.Key, min, max, size, items.ToArray());
        }
    }


    public List<ListViewSlidePanel> SlidePanels { get; set; }
}