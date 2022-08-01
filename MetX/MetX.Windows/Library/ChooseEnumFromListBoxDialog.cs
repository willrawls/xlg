using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Strings.Extensions;
using MetX.Windows.WinApi;

namespace MetX.Windows.Library
{
    public class ChooseEnumFromListBoxDialog<TEnum> : GeneralQuestionDialog<ListBox, TEnum> where TEnum : Enum
    {
        public IntPtr WindowHandle { get; }
        public List<string> Choices;
        public Dictionary<string, TEnum> DStringToEnum = new();
        public Dictionary<TEnum, int> DEnumToIndex = new();

        public ChooseEnumFromListBoxDialog(TEnum valueToReturnOnCancel, IntPtr? windowHandle = null)
        {
            WindowHandle = windowHandle ?? ActiveWindow.GetForegroundWindow();
            ValueToReturnOnCancel = valueToReturnOnCancel;
        }

        public override TEnum SelectedValue
        {
            get
            {
                return Result == DialogResult.Cancel 
                    ? ValueToReturnOnCancel 
                    : DStringToEnum[EntryArea.Text];
            }
        }

        public TEnum Ask(TEnum valueToInitiallySelect, string promptText = "Please select one from the list", string title = "CHOOSE ONE")
        {
            Choices = new List<string>();
            
            var values = Enum.GetValues(typeof(TEnum));
            var i = -1;
            foreach(TEnum item in values)
            {
                i++;
                var name = item.Get<DescriptionAttribute>().Description;
                if (name.IsEmpty()) continue;
                Choices.Add(name);
                DStringToEnum.Add(name, item);
                DEnumToIndex.Add(item, i);
            }
            
            Initialize(promptText, title, valueToInitiallySelect, 550, 650);

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

            EntryArea.SelectedIndex = DEnumToIndex[DefaultValue];

            EntryArea.Left = 50;
            EntryArea.Top = 125;
            EntryArea.Width = ConstructedForm.Width - EntryArea.Left * 2;
            EntryArea.Height = ConstructedForm.Height - EntryArea.Top * 2;

            OkButton.Top = EntryArea.Top + EntryArea.Height + 10;
            CancelButton.Top = OkButton.Top;

            EntryArea.DoubleClick += (_, args) =>
            {
                OkButton.PerformClick();
            };
        }
    }
}