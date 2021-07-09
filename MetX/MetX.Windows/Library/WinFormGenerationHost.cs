using System.Windows.Forms;
using MetX.Standard.Library;
using MetX.Standard.Pipelines;

namespace MetX.Windows.Library
{
    public class WinFormGenerationHost : GenerationHost
    {
        public Form Form;
        public WinFormGenerationHost(Form form)
        {
            Form = form;
        }

        public override MessageBoxResult InputBoxRef(string title, string description, ref string itemName)
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