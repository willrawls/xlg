namespace MetX.Library
{
    using System.Windows.Forms;

    public class ChooseManyDialog : GeneralQuestionDialog<ListBox, int[]>
    {
        public string[] Items;

        public ChooseManyDialog()
        {
            this.ValueToReturnOnCancel = null;
        }

        public override int[] SelectedValue
        {
            get
            {
                if ((this.Result == DialogResult.Cancel))
                {
                    return this.ValueToReturnOnCancel;
                }

                if (this.EntryArea.SelectedIndices.Count == 0)
                {
                    return new int[0];
                }

                int[] ret = new int[this.EntryArea.SelectedIndices.Count];
                for (int index = 0; index < this.EntryArea.SelectedIndices.Count; index++)
                {
                    if (index == -1)
                    {
                        return null;
                    }

                    int item = this.EntryArea.SelectedIndices[index];
                    ret[index] = item;
                }

                return ret;
            }
        }

        public int[] Ask(
            string[] choices,
            string promptText = "Please select one from the list",
            string title = "CHOOSE ONE",
            int[] initiallySelectedIndexes = null)
        {
            this.Items = choices;
            return this.Ask(promptText, title, initiallySelectedIndexes, 400);
        }

        public override void SetupEntryArea()
        {
            this.EntryArea.Items.Clear();

            EntryArea.SelectionMode = SelectionMode.MultiSimple;
            this.EntryArea.SetBounds(12, 106, 372, 280);

            if (this.Items.IsEmpty())
            {
                return;
            }

            foreach (string choice in this.Items)
            {
                this.EntryArea.Items.Add(choice);
            }

            if ((this.DefaultValue != null) && (this.DefaultValue.Length > 0))
            {
                foreach (int index in this.DefaultValue)
                {
                    if ((index >= 0) && (index < this.Items.Length))
                    {
                        this.EntryArea.SetSelected(index, true);
                    }
                }
            }
        }
    }
}
