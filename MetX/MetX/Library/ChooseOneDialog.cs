namespace MetX.Library
{
    using System.Windows.Forms;

    public class ChooseOneDialog : GeneralQuestionDialog<ComboBox, int>
    {
        public string[] Choices;

        public ChooseOneDialog()
            : base()
        {
            this.ValueToReturnOnCancel = -1;
        }

        public override int SelectedValue
        {
            get
            {
                return this.Result == DialogResult.Cancel ? this.ValueToReturnOnCancel : this.EntryArea.SelectedIndex;
            }
        }

        public int Ask(
            string[] choices,
            string promptText = "Please select one from the list",
            string title = "CHOOSE ONE",
            int defaultValue = 0)
        {
            this.Choices = choices;
            return base.Ask(promptText, title, defaultValue);
        }

        public override void SetupEntryArea()
        {
            this.EntryArea.Items.Clear();
            if (this.Choices.IsEmpty())
            {
                return;
            }

            foreach (string choice in this.Choices)
            {
                this.EntryArea.Items.Add(choice);
            }

            if ((this.DefaultValue >= 0) && (this.DefaultValue < this.Choices.Length))
            {
                this.EntryArea.SelectedIndex = this.DefaultValue;
            }
        }
    }
}
