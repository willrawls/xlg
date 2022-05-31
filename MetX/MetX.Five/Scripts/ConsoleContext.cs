using System.Collections.Generic;
using System.Text;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Five.Scripts;


public class ConsoleScriptOutput
{
    public StringBuilder StringBuilder = new StringBuilder();
}

public class ConsoleContext : ContextBase
{
    public static readonly List<ConsoleScriptOutput> OutputConsoles = new();
    public static bool ScriptIsRunning { get; private set; }

    private static readonly object MScriptSyncRoot = new();


    public ConsoleContext(IGenerationHost host = null) : base(Shared.Dirs.CurrentTemplateFolderPath, host)
    {
    }


}