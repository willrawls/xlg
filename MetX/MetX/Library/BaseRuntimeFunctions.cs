namespace MetX.Library
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public abstract class BaseRuntimeFunctions
    {
        public static string Ask(string title, string promptText, string defaultValue)
        {
            var value = defaultValue;
            return Ask(title, promptText, ref value) == DialogResult.Cancel ? null : value;
        }

        public static string Ask(string promptText, string defaultValue = "")
        {
            var value = defaultValue;
            return Ask("ENTER VALUE", promptText, ref value) == DialogResult.Cancel ? null : value;
        }

        public static DialogResult Ask(string title, string promptText, ref string value)
        {
            var form = new Form();
            var label = new Label();
            var textBox = new TextBox();
            var buttonOk = new Button();
            var buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            var dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        public static int Choose(
            string[] choices,
            int defaultValue = 0,
            string promptText = "Please select one from the list",
            string title = "CHOOSE ONE")
        {
            var dialog = new ChooseOneDialog();
            return dialog.Ask(choices, promptText, title, defaultValue);
        }

        public static int[] MultipleChoice(
            string[] choices,
            int[] initiallySelectedIndexes = null,
            string promptText = "Please select one or more items from the list",
            string title = "MULTIPLE CHOICE")
        {
            var dialog = new ChooseManyDialog();
            return dialog.Ask(choices, promptText, title, initiallySelectedIndexes);
        }
    }
}
