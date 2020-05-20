namespace MetX.Controls
{
    using System.Linq;
    using System.Windows.Forms;

    using Library;

    public class ChooseOrderDialog : GeneralQuestionDialog<OrderedSelector, string[]>
    {
        public string[] Items;

        public ChooseOrderDialog()
        {
            ValueToReturnOnCancel = null;
        }

        public override string[] SelectedValue
        {
            get
            {
                if (Result == DialogResult.Cancel)
                {
                    return ValueToReturnOnCancel;
                }

                if (EntryArea.SelectedIndices.Count == 0)
                {
                    return new string[0];
                }

                object[] selection = EntryArea.CurrentSelection;
                string[] ret = new string[EntryArea.SelectedIndices.Count];
                for (int index = 0; index < selection.Length; index++)
                {
                    ret[index] = selection[index].AsString();
                }

                return ret;
            }
        }

        public string[] Ask(
            string[] choices,
            string[] selection,
            string promptText = "Please order this list",
            string title = "ORDER LIST")
        {
            Items = choices;
            string[] result = Ask(promptText, title, selection, 400, 500);
            return result;
        }

        public override void SetupEntryArea()
        {
            EntryArea.SelectionMode = SelectionMode.MultiSimple;
            EntryArea.SetBounds(12, 106, 372, 500);

            EntryArea.Initialize((object[])Items.Cast<object>(), DefaultValue);
        }
    }
}
