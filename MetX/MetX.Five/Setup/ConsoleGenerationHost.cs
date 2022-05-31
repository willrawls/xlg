using System;
using MetX.Five.Scripts;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Five.Setup;

public class ConsoleGenerationHost : IGenerationHost
{
    public ConsoleGenerationHost(ConsoleContext context)
    {
        Context = context;
    }

    public IMessageBox MessageBox { get; set; } = new ConsoleMessageBoxHost();

    public MessageBoxResult InputBox(string title, string description, ref string itemName)
    {
        return MessageBoxResult.Yes;
    }

    public Func<string> GetTextForProcessing { get; set; } = () => "";

    public ContextBase Context { get; set; }

    public void WaitFor(Action action)
    {
        throw new NotImplementedException();
    }
}