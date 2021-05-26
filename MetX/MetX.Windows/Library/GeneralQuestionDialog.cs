using System;
using System.Drawing;
using System.Windows.Forms;

namespace MetX.Windows.Library
{
    public abstract class GeneralQuestionDialog<TEntryArea, TReturn>
        where TEntryArea : Control, new()
    {
        public Button CancelButton = new();
        public Form ConstructedForm = new();
        public TReturn DefaultValue;
        public TEntryArea EntryArea;

        public Button OkButton = new();

        public Label PromptLabel = new();

        public DialogResult Result;

        public TReturn ValueToReturnOnCancel;

        public abstract TReturn SelectedValue { get; }

        /*
                public static TReturn Ask<TGeneralDialog>(string promptText = "Choose", string title = "USER PROMPT", TReturn defaultValue = default(TReturn), int height = 110, int width = 400)
                    where TGeneralDialog : GeneralQuestionDialog<TEntryArea, TReturn>, new()
                {
                    TGeneralDialog dialog = new TGeneralDialog();
                    dialog.Initialize(promptText, title, defaultValue, height, width);
                    dialog.ShowDialog();
                    return dialog.SelectedValue;
                }
        */

        public virtual TReturn Ask(string promptText = "Choose", string title = "USER PROMPT", TReturn defaultValue = default(TReturn), int height = 110, int width = 400)
        {
            Initialize(promptText, title, defaultValue, height, width);
            ShowDialog();
            return SelectedValue;
        }

        public virtual void Initialize(string promptText = "Choose", string title = "USER PROMPT", TReturn defaultValue = default(TReturn), int height = 110, int width = 400)
        {
            DefaultValue = defaultValue;
            ConstructedForm = new Form();
            CancelButton = new Button();
            EntryArea = new TEntryArea();
            OkButton = new Button();
            PromptLabel = new Label();

            ConstructedForm.Text = title;
            PromptLabel.Text = promptText;

            OkButton.Text = "OK";
            CancelButton.Text = "Cancel";
            OkButton.DialogResult = DialogResult.OK;
            CancelButton.DialogResult = DialogResult.Cancel;

            PromptLabel.SetBounds(9, 20, 372, 13);
            EntryArea.SetBounds(12, 36, 372, 20);
            OkButton.SetBounds(228, 72, 75, 23);
            CancelButton.SetBounds(309, 72, 75, 23);

            PromptLabel.AutoSize = true;
            EntryArea.Anchor = EntryArea.Anchor | AnchorStyles.Right;
            OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            SetupEntryArea();

            ConstructedForm.ClientSize = new Size(width, height);
            ConstructedForm.Controls.AddRange(
                new Control[] { PromptLabel, EntryArea, OkButton, CancelButton });
            ConstructedForm.ClientSize = new Size(
                Math.Max(300, PromptLabel.Right + 10),
                ConstructedForm.ClientSize.Height);
            ConstructedForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            ConstructedForm.StartPosition = FormStartPosition.CenterScreen;
            ConstructedForm.MinimizeBox = false;
            ConstructedForm.MaximizeBox = false;
            ConstructedForm.AcceptButton = OkButton;
            ConstructedForm.CancelButton = CancelButton;

        }

        public abstract void SetupEntryArea();

        public virtual DialogResult ShowDialog()
        {
            Result = ConstructedForm.ShowDialog();
            return Result;
        }
    }
}
