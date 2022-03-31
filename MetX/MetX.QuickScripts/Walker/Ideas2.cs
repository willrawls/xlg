using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using MetX.Standard.XDString;

namespace XLG.QuickScripts.Walker;

public partial class Ideas2 : UserControl
{
    public const string DefaultStateValue = @"
~~Start:

~~Body:

~~Finish:

";

    public ListViewSlidePanel MetXWalker, Files, Wrapper, List, Item, Property;

    public AssocArrayList States;
    public Control[] ListViewSlidePanels;

    public bool Updating { get; set; }

    public Ideas2() //AssocArrayList states)
    {
        InitializeComponent();
        States = new AssocArrayList();

        var state = States["MetX Walker"];
        state["Code"].Value = "";
        state["Files"].Value = DefaultStateValue + "x";
        state["Wrapper"].Value = DefaultStateValue + "y";
        States.Add(state);

        state = States["List"];
        state["No children in list"].Value = DefaultStateValue;
        state["No parent above list"].Value = DefaultStateValue;
        state["No parent and no children in list"].Value = DefaultStateValue;
        state["Add list to wrapper"].Value = DefaultStateValue;
        States.Add(state);

        state = States["Find"];
        state["Based on index"].Value = DefaultStateValue;
        state["Based on relationship"].Value = DefaultStateValue;
        state["Based on single column"].Value = DefaultStateValue;
        state["Based on multiple columns"].Value = DefaultStateValue;

        state = States["Item (list member)"];
        state["Add child list to item"].Value = DefaultStateValue;
        state["Item has no children"].Value = DefaultStateValue;

        state = States["Basic Property"];
        state["String"].Value = DefaultStateValue;
        state["Boolean"].Value = DefaultStateValue;
        state["Date and Time"].Value = DefaultStateValue;
        state["Integer"].Value = DefaultStateValue;
        state["Bytes"].Value = DefaultStateValue;
        state["Guid"].Value = DefaultStateValue;

        state = States["Other Property"];
        state["Long"].Value = DefaultStateValue;
        state["Float"].Value = DefaultStateValue;
        state["Double"].Value = DefaultStateValue;
        state["Decimal"].Value = DefaultStateValue;
        state["Currency"].Value = DefaultStateValue;
        state["Byte"].Value = DefaultStateValue;

        States.Add(new()
        {
            Key = "Specialized Property",
            ["Detect specialized"] = { Value = DefaultStateValue },
            ["BLOB"] = { Value = DefaultStateValue },
            ["Memo"] = { Value = DefaultStateValue },
            ["Associative Array"] = { Value = DefaultStateValue },
            ["Associative Array List"] = { Value = DefaultStateValue },
            ["3D Associative Array"] = { Value = DefaultStateValue },
            ["4D Associative Array"] = { Value = DefaultStateValue },
        });

        MetXWalker = new ListViewSlidePanel().Setup(1, States, "MetX Walker");
        Files = new ListViewSlidePanel().Setup(2, States, "Files");
        Wrapper = new ListViewSlidePanel().Setup(3, States, "Wrapper");
        List = new ListViewSlidePanel().Setup(4, States, "List");
        Item = new ListViewSlidePanel().Setup(5, States, "Item (list member)");
        Property = new ListViewSlidePanel().Setup(7, States, "Property");

        ListViewSlidePanels = new Control[] 
        {
            MetXWalker, Files, Wrapper, List, Item, Property
        };
        VBar.Controls.Add(Property);
        VBar.Controls.Add(Item);
        VBar.Controls.Add(List);
        VBar.Controls.Add(Wrapper);
        VBar.Controls.Add(Files);
        VBar.Controls.Add(MetXWalker);


        ListViewSlidePanels.ToList().ForEach(control =>
        {
            control.Dock = DockStyle.Top;
            control.Visible = true;
            control.Refresh();
        });
    }

    public bool OnSelectionChanged(ListView listView, AssocArray state)
    {
        if (Updating) return true;

        try
        {
            Updating = true;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
        finally
        {
            Updating = false;
        }

        return true;
    }
}