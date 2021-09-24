using System;
using MetX.Standard.Interfaces;
using MetX.Standard.Pipelines;

namespace MetX.Standard.Generation
{
    public class DoNothingGenerationHost : IGenerationHost
    {
        public DoNothingGenerationHost(string testTextToProcess = "", ContextBase context = null)
        {
            Context = context;
            GetTextForProcessing = () => testTextToProcess;
            MessageBox = new DoNothingMessageBoxHost(this);

        }

        public IMessageBox MessageBox { get; set; }
        public MessageBoxResult InputBox(string title, string description, ref string itemName)
        {
            return MessageBoxResult.No;
        }

        public Func<string> GetTextForProcessing { get; set; }
        public ContextBase Context { get; set; }
    }
}