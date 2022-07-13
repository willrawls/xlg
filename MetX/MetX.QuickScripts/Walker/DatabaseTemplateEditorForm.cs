using System;
using System.Linq;
using System.Windows.Forms;
using MetX.Five;
using MetX.Five.Scripts;
using MetX.Standard.XDString;
using MetX.Standard.XDString.Generics;

namespace XLG.QuickScripts.Walker
{
    public partial class DatabaseTemplateEditorForm : Form
    {
        public Control[] ListViewSlidePanels;
        public ListViewSlidePanel MetXWalker, Files, Wrapper, List, Item, Property;

        public AssocSheet States;

        public bool Updating { get; set; }


        public DatabaseTemplateEditorForm()
        {
            InitializeComponent();
            treeView1.ExpandAll();

            States = new AssocSheet();

            States["MetX Walker", "Code"].Value = "";
            States["MetX Walker", "Files"].Value = DefaultSoftCode.StartBodyFinish + "x";
            States["MetX Walker", "Wrapper"].Value = DefaultSoftCode.StartBodyFinish + "y";

            States["List", "No children in list"].Value = DefaultSoftCode.StartBodyFinish;
            States["List", "No parent above list"].Value = DefaultSoftCode.StartBodyFinish;
            States["List", "No parent and no children in list"].Value = DefaultSoftCode.StartBodyFinish;
            States["List", "Add list to wrapper"].Value = DefaultSoftCode.StartBodyFinish;

            States["Find", "Based on index"].Value = DefaultSoftCode.StartBodyFinish;
            States["Find", "Based on relationship"].Value = DefaultSoftCode.StartBodyFinish;
            States["Find", "Based on single column"].Value = DefaultSoftCode.StartBodyFinish;
            States["Find", "Based on multiple columns"].Value = DefaultSoftCode.StartBodyFinish;

            States["Item (list member)", "Add child list to item"].Value = DefaultSoftCode.StartBodyFinish;
            States["Item (list member)", "Item has no children"].Value = DefaultSoftCode.StartBodyFinish;

            States["Basic Property", "String"].Value = DefaultSoftCode.StartBodyFinish;
            States["Basic Property", "Boolean"].Value = DefaultSoftCode.StartBodyFinish;
            States["Basic Property", "Date and Time"].Value = DefaultSoftCode.StartBodyFinish;
            States["Basic Property", "Integer"].Value = DefaultSoftCode.StartBodyFinish;
            States["Basic Property", "Bytes"].Value = DefaultSoftCode.StartBodyFinish;
            States["Basic Property", "Guid"].Value = DefaultSoftCode.StartBodyFinish;

            States["Other Property", "Long"].Value = DefaultSoftCode.StartBodyFinish;
            States["Other Property", "Float"].Value = DefaultSoftCode.StartBodyFinish;
            States["Other Property", "Double"].Value = DefaultSoftCode.StartBodyFinish;
            States["Other Property", "Decimal"].Value = DefaultSoftCode.StartBodyFinish;
            States["Other Property", "Currency"].Value = DefaultSoftCode.StartBodyFinish;
            States["Other Property", "Byte"].Value = DefaultSoftCode.StartBodyFinish;

            States["Specialized Property", "Detect specialized"].Value = DefaultSoftCode.StartBodyFinish;
            States["Specialized Property", "BLOB"].Value = DefaultSoftCode.StartBodyFinish;
            States["Specialized Property", "Memo"].Value = DefaultSoftCode.StartBodyFinish;
            States["Specialized Property", "Associative Array"].Value = DefaultSoftCode.StartBodyFinish;
            States["Specialized Property", "Associative Array List"].Value = DefaultSoftCode.StartBodyFinish;
            States["Specialized Property", "3D Associative Array"].Value = DefaultSoftCode.StartBodyFinish;
            States["Specialized Property", "4D Associative Array"].Value = DefaultSoftCode.StartBodyFinish;

            /*
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


            ListViewSlidePanels.ToList().ForEach(control =>
            {
                control.Dock = DockStyle.Top;
                control.Visible = true;
                control.Refresh();
            });
        */
        }
    }
}
