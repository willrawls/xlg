using System;
using MetX.Standard.Interfaces;

namespace MetX.Standard.Pipelines
{
    public abstract class GenerationHost : IGenerationHost
    {
        public IMessageBox MessageBox { get; set; }
        public Func<string> InputText { get; set; }

        public abstract MessageBoxResult InputBoxRef(string title, string description, ref string itemName);
    }
}