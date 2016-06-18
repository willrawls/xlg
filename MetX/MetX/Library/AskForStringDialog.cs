namespace MetX.Library
{
    using System.Windows.Forms;

    public class AskForStringDialog : GeneralQuestionDialog<TextBox, string>
    {
        public AskForStringDialog(string defaultValue = "")
        {
            this.DefaultValue = defaultValue ?? string.Empty;
        }

        public override string SelectedValue
        {
            get
            {
                return this.EntryArea.Text;
            }
        }

        public override void SetupEntryArea()
        {
            this.EntryArea.Text = this.DefaultValue;
        }
    }
}
