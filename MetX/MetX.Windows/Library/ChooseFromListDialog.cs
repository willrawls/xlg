using System;
using System.Windows.Forms;
using MetX.Standard.Strings;
using MetX.Windows.WinApi;

namespace MetX.Windows.Library
{
    public class ChooseFromListDialog : GeneralQuestionDialog<ListBox, int>
    {
        public IntPtr WindowHandle { get; }
        public string[] Choices;

        public ChooseFromListDialog(IntPtr? windowHandle = null)
        {
            WindowHandle = windowHandle ?? ActiveWindow.GetForegroundWindow();
            ValueToReturnOnCancel = -1;
        }

        public override int SelectedValue => Result == DialogResult.Cancel 
            ? ValueToReturnOnCancel 
            : EntryArea.SelectedIndex;

        public int Ask(
            string[] choices,
            string promptText = "Please select one from the list",
            string title = "CHOOSE ONE",
            int defaultValue = 0)
        {
            Choices = choices;

            Initialize(promptText, title, defaultValue, 400, 500);
            if(WindowHandle == IntPtr.Zero)
            {
                Result = ConstructedForm.ShowDialog();
            }
            else
            {
                var win32Window = new Win32Window(WindowHandle);
                Result = ConstructedForm.ShowDialog(win32Window);
            }
            return SelectedValue;
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

            EntryArea.Height = Choices.Length * 50;
            ConstructedForm.Height = EntryArea.Height + 100;
            EntryArea.Top = 50;
        }
    }
}