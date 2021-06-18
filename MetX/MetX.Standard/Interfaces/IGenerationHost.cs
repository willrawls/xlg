using System;
using MetX.Standard.Pipelines;

namespace MetX.Standard.Interfaces
{
    public interface IGenerationHost
    {
        IMessageBox MessageBox { get; set; }
        MessageBoxResult InputBoxRef(string title, string description, ref string itemName);
        Func<string> InputText { get; set; }
    }
}