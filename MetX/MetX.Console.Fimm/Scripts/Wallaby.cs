using System;
using System.IO;
using System.Linq;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MetX.Fimm.Scripts;

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

    public ActualizationResult RunQuickScript(string scriptName)
    {
        var scriptToRun = FindScript(scriptName);
        var settings = scriptToRun.BuildSettings(false, Host);
        var result = settings.ActualizeAndCompile();
        return result;
    }

    public ActualizationResult RunQuickScript(XlgQuickScript scriptToRun)
    {
        var settings = scriptToRun.BuildSettings(false, Host);
        var result = settings.ActualizeAndCompile();
        return result;
    }

    public XlgQuickScript FindScript(string filePath, string scriptName)
    {
        if (!File.Exists(filePath))
            return null;

        var scriptList = XlgQuickScriptFile.Load(filePath);
        return scriptList[scriptName];
    }

    public XlgQuickScript FindScript(string scriptName)
    {
        if (scriptName.Contains("+"))
        {
            return FindScript(scriptName.FirstToken("+"), scriptName.TokensAfterFirst("+"));
        }
        
        
        var filePath = Shared.Dirs.LastScriptFilePath;

        if (!File.Exists(filePath))
            return null;
        
        var scriptList = XlgQuickScriptFile.Load(filePath);
        return scriptList[scriptName];
    }


}