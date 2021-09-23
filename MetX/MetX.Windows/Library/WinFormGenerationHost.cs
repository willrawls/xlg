using System;
using System.Windows.Forms;
using MetX.Controls;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Pipelines;

namespace MetX.Windows.Library
{
    public class WinFormGenerationHost<T> : GenerationHost where T : Form
    {
        public T Form;

        public WinFormGenerationHost(T form, Func<string> getTextForProcessing)
        {
            Form = form;
            MessageBox = new WinFormMessageBoxHost<T>(Form, this);
            GetTextForProcessing = getTextForProcessing;
        }

        public override MessageBoxResult InputBox(string title, string description, ref string itemName)
        {
            var dialog = new AskForStringDialog
            {
                ValueToReturnOnCancel = "~|~|"
            };
            var response = dialog.Ask(description, title, itemName);
            
            if (response.IsEmpty() || response == dialog.ValueToReturnOnCancel)
                return MessageBoxResult.Cancel;
            itemName = response;
            return dialog.MessageBoxResult;
        }
    }
}