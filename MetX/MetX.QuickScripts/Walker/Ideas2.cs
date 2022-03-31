using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using MetX.Standard.XDString;

namespace XLG.QuickScripts.Walker;

public partial class Ideas2 : UserControl
{
    public const string DefaultStartBodyFinishSoftCode = @"
~~Start:

~~Body:

~~Finish:

";

    public const string DefaultBodyOnlySoftCode = @"
~~Body:

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
        state["Files"].Value = DefaultStartBodyFinishSoftCode + "x";
        state["Wrapper"].Value = DefaultStartBodyFinishSoftCode + "y";
        States.Add(state);

        state = States["List"];
        state["No children in list"].Value = DefaultStartBodyFinishSoftCode;
        state["No parent above list"].Value = DefaultStartBodyFinishSoftCode;
        state["No parent and no children in list"].Value = DefaultStartBodyFinishSoftCode;
        state["Add list to wrapper"].Value = DefaultStartBodyFinishSoftCode;
        States.Add(state);

        state = States["Find"];
        state["Based on index"].Value = DefaultStartBodyFinishSoftCode;
        state["Based on relationship"].Value = DefaultStartBodyFinishSoftCode;
        state["Based on single column"].Value = DefaultStartBodyFinishSoftCode;
        state["Based on multiple columns"].Value = DefaultStartBodyFinishSoftCode;

        state = States["Item (list member)"];
        state["Add child list to item"].Value = DefaultStartBodyFinishSoftCode;
        state["Item has no children"].Value = DefaultStartBodyFinishSoftCode;

        state = States["Basic Property"];
        state["String"].Value = DefaultStartBodyFinishSoftCode;
        state["Boolean"].Value = DefaultStartBodyFinishSoftCode;
        state["Date and Time"].Value = DefaultStartBodyFinishSoftCode;
        state["Integer"].Value = DefaultStartBodyFinishSoftCode;
        state["Bytes"].Value = DefaultStartBodyFinishSoftCode;
        state["Guid"].Value = DefaultStartBodyFinishSoftCode;

        state = States["Other Property"];
        state["Long"].Value = DefaultStartBodyFinishSoftCode;
        state["Float"].Value = DefaultStartBodyFinishSoftCode;
        state["Double"].Value = DefaultStartBodyFinishSoftCode;
        state["Decimal"].Value = DefaultStartBodyFinishSoftCode;
        state["Currency"].Value = DefaultStartBodyFinishSoftCode;
        state["Byte"].Value = DefaultStartBodyFinishSoftCode;

        States.Add(new()
        {
            Key = "Specialized Property",
            ["Detect specialized"] = { Value = DefaultStartBodyFinishSoftCode },
            ["BLOB"] = { Value = DefaultStartBodyFinishSoftCode },
            ["Memo"] = { Value = DefaultStartBodyFinishSoftCode },
            ["Associative Array"] = { Value = DefaultStartBodyFinishSoftCode },
            ["Associative Array List"] = { Value = DefaultStartBodyFinishSoftCode },
            ["3D Associative Array"] = { Value = DefaultStartBodyFinishSoftCode },
            ["4D Associative Array"] = { Value = DefaultStartBodyFinishSoftCode },
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