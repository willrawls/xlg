using System;
using System.Windows.Forms;
using MetX.Standard.Host;
using MetX.Standard.Pipelines;

namespace MetX.Windows.Library
{
    public class AskForStringDialog : GeneralQuestionDialog<TextBox, string>
    {
        public AskForStringDialog(string defaultValue = "")
        {
            DefaultValue = defaultValue ?? string.Empty;
        }

        public override string SelectedValue => EntryArea.Text;

        public MessageBoxResult MessageBoxResult
        {
            get
            {
                switch(Result)
                {
                    case DialogResult.None:
                        return MessageBoxResult.None;
                    case DialogResult.OK:
                        return MessageBoxResult.OK;
                    case DialogResult.Cancel:
                        return MessageBoxResult.Cancel;
                    case DialogResult.Abort:
                        return MessageBoxResult.Abort;
                    case DialogResult.Retry:
                        return MessageBoxResult.Retry;
                    case DialogResult.Ignore:
                        return MessageBoxResult.Ignore;
                    case DialogResult.Yes:
                        return MessageBoxResult.Yes;
                    case DialogResult.No:
                        return MessageBoxResult.No;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override void SetupEntryArea()
        {
            EntryArea.Text = DefaultValue;
        }
    }
}
