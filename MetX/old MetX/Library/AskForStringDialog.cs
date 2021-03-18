namespace MetX.Library
{
    using System.Windows.Forms;

    public class AskForStringDialog : GeneralQuestionDialog<TextBox, string>
    {
        public AskForStringDialog(string defaultValue = "")
        {
            DefaultValue = defaultValue ?? string.Empty;
        }

        public override string SelectedValue => EntryArea.Text;

        public override void SetupEntryArea()
        {
            EntryArea.Text = DefaultValue;
        }
    }
}
