using System.Collections.Generic;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Fimm.Scripts;

public class ConsoleContext : ContextBase
{
    public static readonly List<ConsoleScriptOutput> OutputConsoles = new();
    public static bool ScriptIsRunning { get; set; }

    private static readonly object SyncRoot = new();


    public ConsoleContext(IGenerationHost host = null) : base(Shared.Dirs.CurrentTemplateFolderPath, host)
    {
    }

    public ConsoleContext(IGenerationHost host, string templateFolder) : base(templateFolder, host)
    {
    }


}