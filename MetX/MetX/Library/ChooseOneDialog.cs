namespace MetX.Library
{
    using System.Windows.Forms;

    public class ChooseOneDialog : GeneralQuestionDialog<ComboBox, int>
    {
        public string[] Choices;

        public ChooseOneDialog()
        {
            ValueToReturnOnCancel = -1;
        }

        public override int SelectedValue => Result == DialogResult.Cancel ? ValueToReturnOnCancel : EntryArea.SelectedIndex;

        public int Ask(
            string[] choices,
            string promptText = "Please select one from the list",
            string title = "CHOOSE ONE",
            int defaultValue = 0)
        {
            Choices = choices;
            return Ask(promptText, title, defaultValue);
        }

        public override void SetupEntryArea()
        {
            EntryArea.Items.Clear();
            if (Choices.IsEmpty())
            {
                return;
            }

            foreach (var choice in Choices)
            {
                EntryArea.Items.Add(choice.AsString());
            }

            if (DefaultValue >= 0 && DefaultValue < Choices.Length)
            {
                EntryArea.SelectedIndex = DefaultValue;
            }
        }
    }
}
