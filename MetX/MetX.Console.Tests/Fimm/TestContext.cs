using System.Collections.Generic;
using MetX.Fimm;
using MetX.Fimm.Scripts;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Console.Tests.Fimm;

public class TestContext : ContextBase
{
    private static readonly object MScriptSyncRoot = new();
    public static readonly List<ConsoleScriptOutput> OutputConsoles = new();


    public TestContext(IGenerationHost host = null) : base(Shared.Dirs.CurrentTemplateFolderPath, host)
    {
    }

    public static bool ScriptIsRunning { get; private set; }
}