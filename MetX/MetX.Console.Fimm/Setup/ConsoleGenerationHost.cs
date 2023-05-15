using System;
using System.Drawing;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Fimm.Setup;

public class ConsoleGenerationHost : IGenerationHost
{
    public ConsoleGenerationHost(ContextBase context)
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

    public Rectangle Boundary => new Rectangle(Console.WindowLeft, Console.WindowTop, Console.WindowWidth, Console.WindowHeight);
}