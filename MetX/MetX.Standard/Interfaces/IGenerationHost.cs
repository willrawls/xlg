using System;
using MetX.Standard.Pipelines;

namespace MetX.Standard.Interfaces
{
    public interface IGenerationHost
    {
        IMessageBox MessageBox { get; set; }
        MessageBoxResult InputBox(string title, string description, ref string itemName);
        Func<string> GetTextForProcessing { get; set; }
        ContextBase Context { get; set; }
        void WaitFor(Action action);
    }
}