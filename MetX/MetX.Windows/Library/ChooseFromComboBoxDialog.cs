using System;
using System.Windows.Forms;
using MetX.Standard.Strings;
using MetX.Windows.WinApi;

namespace MetX.Windows.Library
{
    // ReSharper disable once UnusedType.Global
    public class ChooseFromComboBoxDialog : GeneralQuestionDialog<ComboBox, int>
    {
        public IntPtr WindowHandle { get; }
        public string[] Choices;

        public ChooseFromComboBoxDialog(IntPtr? windowHandle = null)
        {
            WindowHandle = windowHandle ?? ActiveWindow.GetForegroundWindow();
            ValueToReturnOnCancel = -1;
        }

        public override int SelectedValue => Result == DialogResult.Cancel 
            ? ValueToReturnOnCancel 
            : EntryArea.SelectedIndex;

        public int Ask(
            int top, int left, 
            string[] choices,
            string promptText = "Please select one from the list",
            string title = "CHOOSE ONE",
            int defaultValue = 0)
        {
            Choices = choices;

            Initialize(top, left, promptText, title, defaultValue, 110, 400);
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
                EntryArea.Items.Add(choice.AsStringFromString());
            }

            if (DefaultValue >= 0 && DefaultValue < Choices.Length)
            {
                EntryArea.SelectedIndex = DefaultValue;
            }

            EntryArea.DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}
