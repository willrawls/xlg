using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using MetX.Standard.Interfaces;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Windows.WinApi;

namespace MetX.Windows.Library
{
    public class ChooseEnumFromListBoxDialog<TEnum> : GeneralQuestionDialog<ListBox, TEnum> where TEnum : System.Enum
    {
        public IntPtr WindowHandle { get; }
        public List<string> Choices;
        public Dictionary<string, TEnum> DStringToEnum = new();
        public Dictionary<TEnum, int> DEnumToIndex = new();

        public ChooseEnumFromListBoxDialog(IntPtr? windowHandle = null)
        {
            WindowHandle = windowHandle ?? ActiveWindow.GetForegroundWindow();
            ValueToReturnOnCancel = default;
        }

        public override TEnum SelectedValue
        {
            get
            {
                if (Result == DialogResult.Cancel)
                    return ValueToReturnOnCancel;

                return DStringToEnum[EntryArea.Text];
            }
        }

        public TEnum Ask(TEnum defaultValue, string promptText = "Please select one from the list", string title = "CHOOSE ONE")
        {
            Choices = new List<string>();
            
            var values = Enum.GetValues(typeof(TEnum));
            var i = -1;
            foreach(TEnum item in values)
            {
                i++;
                var name = item.Get<DescriptionAttribute>().Description;
                // var name = Enum.GetName(typeof(TEnum), item);
                if (name.IsEmpty()) continue;
                Choices.Add(name);
                DStringToEnum.Add(name, item);
                DEnumToIndex.Add(item, i);
            }
            
            Initialize(promptText, title, defaultValue, 400, 500);
            EntryArea.Visible = true;            
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
            EntryArea.Visible = true;
        }
    }
}