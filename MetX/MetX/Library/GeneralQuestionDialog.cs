namespace MetX.Library
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public abstract class GeneralQuestionDialog<TEntryArea, TReturn>
        where TEntryArea : Control, new()
    {
        public Button CancelButton = new Button();
        public Form ConstructedForm = new Form();
        public TReturn DefaultValue;
        public TEntryArea EntryArea;

        public Button OkButton = new Button();

        public Label PromptLabel = new Label();

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
            this.DefaultValue = defaultValue;
            this.ConstructedForm = new Form();
            this.CancelButton = new Button();
            this.EntryArea = new TEntryArea();
            this.OkButton = new Button();
            this.PromptLabel = new Label();

            this.ConstructedForm.Text = title;
            this.PromptLabel.Text = promptText;

            this.OkButton.Text = "OK";
            this.CancelButton.Text = "Cancel";
            this.OkButton.DialogResult = DialogResult.OK;
            this.CancelButton.DialogResult = DialogResult.Cancel;

            this.PromptLabel.SetBounds(9, 20, 372, 13);
            this.EntryArea.SetBounds(12, 36, 372, 20);
            this.OkButton.SetBounds(228, 72, 75, 23);
            this.CancelButton.SetBounds(309, 72, 75, 23);

            this.PromptLabel.AutoSize = true;
            this.EntryArea.Anchor = this.EntryArea.Anchor | AnchorStyles.Right;
            this.OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.CancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            this.SetupEntryArea();

            this.ConstructedForm.ClientSize = new Size(width, height);
            this.ConstructedForm.Controls.AddRange(
                new Control[] { this.PromptLabel, this.EntryArea, this.OkButton, this.CancelButton });
            this.ConstructedForm.ClientSize = new Size(
                Math.Max(300, this.PromptLabel.Right + 10),
                this.ConstructedForm.ClientSize.Height);
            this.ConstructedForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ConstructedForm.StartPosition = FormStartPosition.CenterScreen;
            this.ConstructedForm.MinimizeBox = false;
            this.ConstructedForm.MaximizeBox = false;
            this.ConstructedForm.AcceptButton = this.OkButton;
            this.ConstructedForm.CancelButton = this.CancelButton;
        }

        public abstract void SetupEntryArea();

        public virtual DialogResult ShowDialog()
        {
            Result = this.ConstructedForm.ShowDialog();
            return Result;
        }
    }
}
