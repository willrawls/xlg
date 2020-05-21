namespace MetX.Library
{
    using System.Windows.Forms;

    public class ChooseManyDialog : GeneralQuestionDialog<ListBox, int[]>
    {
        public string[] Items;

        public ChooseManyDialog()
        {
            ValueToReturnOnCancel = null;
        }

        public override int[] SelectedValue
        {
            get
            {
                if (Result == DialogResult.Cancel)
                {
                    return ValueToReturnOnCancel;
                }

                if (EntryArea.SelectedIndices.Count == 0)
                {
                    return new int[0];
                }

                var ret = new int[EntryArea.SelectedIndices.Count];
                for (var index = 0; index < EntryArea.SelectedIndices.Count; index++)
                {
                    if (index == -1)
                    {
                        return null;
                    }

                    var item = EntryArea.SelectedIndices[index];
                    ret[index] = item;
                }

                return ret;
            }
        }

        public int[] Ask(
            string[] choices,
            string promptText = "Please select one from the list",
            string title = "MULTIPLE CHOICE",
            int[] initiallySelectedIndexes = null)
        {
            Items = choices;
            return Ask(promptText, title, initiallySelectedIndexes, 400);
        }

        public override void SetupEntryArea()
        {
            EntryArea.SelectionMode = SelectionMode.MultiSimple;
            EntryArea.SetBounds(12, 106, 372, 280);

            EntryArea.Items.Clear();
            if (Items.IsEmpty())
            {
                return;
            }

            foreach (var choice in Items)
            {
                EntryArea.Items.Add(choice);
            }

            if ((DefaultValue != null) && (DefaultValue.Length > 0))
            {
                foreach (var index in DefaultValue)
                {
                    if ((index >= 0) && (index < Items.Length))
                    {
                        EntryArea.SetSelected(index, true);
                    }
                }
            }
        }
    }
}
