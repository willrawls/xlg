using System;
using MetX.Standard.Interfaces;

namespace MetX.Standard.Host
{
    public abstract class GenerationHost : IGenerationHost
    {
        public IMessageBox MessageBox { get; set; }
        public Func<string> GetTextForProcessing { get; set; }
        public ContextBase Context { get; set; }
        public abstract void WaitFor(Action action);

        public abstract MessageBoxResult InputBox(string title, string description, ref string itemName);
    }
}