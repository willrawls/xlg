using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using MetX.Standard.XDString;

namespace XLG.QuickScripts.Walker;

public partial class DatabaseTemplateEditorControl : UserControl
{
    public Control[] ListViewSlidePanels;
    public ListViewSlidePanel MetXWalker, Files, Wrapper, List, Item, Property;

    public AssocArrayList States;

    public DatabaseTemplateEditorControl() //AssocArrayList states)
    {
        InitializeComponent();
        States = new AssocArrayList();

        var state = States["MetX Walker"];
        state["Code"].Value = "";
        state["Files"].Value = DefaultSoftCode.StartBodyFinish + "x";
        state["Wrapper"].Value = DefaultSoftCode.StartBodyFinish + "y";
        States.Add(state);

        state = States["List"];
        state["No children in list"].Value = DefaultSoftCode.StartBodyFinish;
        state["No parent above list"].Value = DefaultSoftCode.StartBodyFinish;
        state["No parent and no children in list"].Value = DefaultSoftCode.StartBodyFinish;
        state["Add list to wrapper"].Value = DefaultSoftCode.StartBodyFinish;
        States.Add(state);

        state = States["Find"];
        state["Based on index"].Value = DefaultSoftCode.StartBodyFinish;
        state["Based on relationship"].Value = DefaultSoftCode.StartBodyFinish;
        state["Based on single column"].Value = DefaultSoftCode.StartBodyFinish;
        state["Based on multiple columns"].Value = DefaultSoftCode.StartBodyFinish;

        state = States["Item (list member)"];
        state["Add child list to item"].Value = DefaultSoftCode.StartBodyFinish;
        state["Item has no children"].Value = DefaultSoftCode.StartBodyFinish;

        state = States["Basic Property"];
        state["String"].Value = DefaultSoftCode.StartBodyFinish;
        state["Boolean"].Value = DefaultSoftCode.StartBodyFinish;
        state["Date and Time"].Value = DefaultSoftCode.StartBodyFinish;
        state["Integer"].Value = DefaultSoftCode.StartBodyFinish;
        state["Bytes"].Value = DefaultSoftCode.StartBodyFinish;
        state["Guid"].Value = DefaultSoftCode.StartBodyFinish;

        state = States["Other Property"];
        state["Long"].Value = DefaultSoftCode.StartBodyFinish;
        state["Float"].Value = DefaultSoftCode.StartBodyFinish;
        state["Double"].Value = DefaultSoftCode.StartBodyFinish;
        state["Decimal"].Value = DefaultSoftCode.StartBodyFinish;
        state["Currency"].Value = DefaultSoftCode.StartBodyFinish;
        state["Byte"].Value = DefaultSoftCode.StartBodyFinish;

        States.Add(new AssocArray
        {
            Key = "Specialized Property",
            ["Detect specialized"] = {Value = DefaultSoftCode.StartBodyFinish},
            ["BLOB"] = {Value = DefaultSoftCode.StartBodyFinish},
            ["Memo"] = {Value = DefaultSoftCode.StartBodyFinish},
            ["Associative Array"] = {Value = DefaultSoftCode.StartBodyFinish},
            ["Associative Array List"] = {Value = DefaultSoftCode.StartBodyFinish},
            ["3D Associative Array"] = {Value = DefaultSoftCode.StartBodyFinish},
            ["4D Associative Array"] = {Value = DefaultSoftCode.StartBodyFinish}
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

    public bool Updating { get; set; }

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