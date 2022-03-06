using System;
using System.Drawing;
using System.Windows.Forms;
using MetX.Standard.Library.Extensions;
using MetX.Standard.XDString;

namespace MetX.Controls;

public class ListViewSlidePanel : SlidePanel<ListView, AssocArray>
{
    public ListView ListView;
    public Func<ListView, AssocArray, bool> OnSelectionChanged {get; set; }

    public ListViewSlidePanel(Func<ListView, AssocArray, bool> onSelectionChanged) : base()
    {
        OnSelectionChanged = onSelectionChanged;
    }

    public void SetupListViewAndPanel(string title, Size min, Size max, Size size, ListViewItem[] items)
    {
        ListView = new ListView
        {
            BorderStyle = BorderStyle.None,
            MultiSelect = false,
            FullRowSelect = true,
            HeaderStyle = ColumnHeaderStyle.None,
            Sorting = SortOrder.None,
        };
        State = new AssocArray();

        base.Setup(ListView, title, min, max, true);
        if(items.IsNotEmpty())
            ListView.Items.AddRange(items);

        ListView.SelectedIndexChanged += ListView_SelectedIndexChanged;
    }

    private void ListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnSelectionChanged?.Invoke(ListView, State);
    }
}