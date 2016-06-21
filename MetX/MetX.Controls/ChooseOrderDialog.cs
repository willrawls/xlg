namespace MetX.Controls
{
    using System.Linq;
    using System.Windows.Forms;

    using MetX.Library;

    public class ChooseOrderDialog : GeneralQuestionDialog<OrderedSelector, string[]>
    {
        public string[] Items;

        public ChooseOrderDialog()
        {
            this.ValueToReturnOnCancel = null;
        }

        public override string[] SelectedValue
        {
            get
            {
                if (this.Result == DialogResult.Cancel)
                {
                    return this.ValueToReturnOnCancel;
                }

                if (this.EntryArea.SelectedIndices.Count == 0)
                {
                    return new string[0];
                }

                object[] selection = this.EntryArea.CurrentSelection;
                string[] ret = new string[this.EntryArea.SelectedIndices.Count];
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
            this.Items = choices;
            string[] result = this.Ask(promptText, title, selection, 400, 500);
            return result;
        }

        public override void SetupEntryArea()
        {
            this.EntryArea.SelectionMode = SelectionMode.MultiSimple;
            this.EntryArea.SetBounds(12, 106, 372, 500);

            this.EntryArea.Initialize((object[])this.Items.Cast<object>(), this.DefaultValue);
        }
    }
}
