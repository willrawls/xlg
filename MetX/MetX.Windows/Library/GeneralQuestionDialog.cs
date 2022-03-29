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

        public TReturn Ask(string promptText = "Choose", string title = "USER PROMPT", TReturn defaultValue = default, int height = 110, int width = 400)
        {
            Initialize(promptText, title, defaultValue, height, width);
            ShowDialog();
            return SelectedValue;
        }

        public void Initialize(string promptText = "Choose", string title = "USER PROMPT", TReturn defaultValue = default, int height = 110, int width = 400)
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
            EntryArea.SetBounds(12, 36, width - 30, 20);
            OkButton.SetBounds(228, 72, 75, 26);
            CancelButton.SetBounds(309, 72, 75, 26);

            PromptLabel.AutoSize = true;
            EntryArea.Anchor = EntryArea.Anchor | AnchorStyles.Right;
            OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            ConstructedForm.ClientSize = new Size(width, height);
            ConstructedForm.Controls.AddRange(new Control[] { PromptLabel, EntryArea, OkButton, CancelButton });
            ConstructedForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            ConstructedForm.StartPosition = FormStartPosition.CenterScreen;
            ConstructedForm.MinimizeBox = false;
            ConstructedForm.MaximizeBox = false;
            ConstructedForm.AcceptButton = OkButton;
            ConstructedForm.CancelButton = CancelButton;

            SetupEntryArea();
        }

        public abstract void SetupEntryArea();

        public DialogResult ShowDialog()
        {
            Result = ConstructedForm.ShowDialog();
            return Result;
        }
    }
}
