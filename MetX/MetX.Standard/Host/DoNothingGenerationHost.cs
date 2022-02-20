using System;
using MetX.Standard.Interfaces;

namespace MetX.Standard.Host
{
    public class DoNothingGenerationHost : IGenerationHost
    {
        public DoNothingGenerationHost(string testTextToProcess, ContextBase context)
        {
            Context = context ?? throw new ArgumentException(nameof(context));
            GetTextForProcessing = () => testTextToProcess ?? "";
            MessageBox = new DoNothingMessageBoxHost(this);

        }

        public IMessageBox MessageBox { get; set; }
        public MessageBoxResult InputBox(string title, string description, ref string itemName)
        {
            return MessageBoxResult.No;
        }

        public Func<string> GetTextForProcessing { get; set; }
        public ContextBase Context { get; set; }
        public void WaitFor(Action action)
        {
        }
    }
}