using MetX.Standard.Primary;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;

namespace MetX.Five.QuickScripts;

public class Wallaby
{
    public ContextBase Context;

    public Wallaby(IGenerationHost host = null)
    {
        if (host == null)
        {
            Context = new ContextBase(Shared.Dirs.ScriptsFolderName, null);
        }

        Host = host;
    }

    public IGenerationHost Host { get; set; }

    public ActualizationResult FiverRunScript(XlgQuickScript scriptToRun)
    {
        var settings = scriptToRun.BuildSettings(false, Host);
        var result = settings.ActualizeAndCompile();
        return result;
    }

}