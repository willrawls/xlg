
using System;
using System.IO;
using System.Linq;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings;

namespace MetX.Fimm.Scripts;

public class Wallaby
{
    public ContextBase Context;

    public Wallaby(IGenerationHost host = null)
    {
        if (host == null) Context = new ContextBase(Shared.Dirs.ScriptsFolderName, null);

        Host = host;
    }

    public IGenerationHost Host { get; set; }

    public XlgQuickScript FindScript(string filePath, string scriptName)
    {
        string fileActual = null;
        if (filePath == null)
            return null;

        if (File.Exists(filePath))
        {
            fileActual = filePath;
        }
        else
        {
            fileActual = Path.Combine(filePath, scriptName);
            if (!File.Exists(fileActual))
                return null;
        }

        XlgQuickScriptFile scriptList = XlgQuickScriptFile.Load(fileActual);

        var scriptToReturn = scriptList.FirstOrDefault(x => x.TemplateName.ToLower() == scriptName);
        return scriptToReturn ?? (scriptList.Count > 0
            ? scriptList[0] // Lone Fimm 
            : null);
    }

    public XlgQuickScript FindScript(string scriptName)
    {
        if (scriptName.Contains("+")) return FindScript(scriptName.FirstToken("+"), scriptName.TokensAfterFirst("+"));


        var filePath = ResolveFimmFilePath(scriptName);
        

        if (!File.Exists(filePath))
            return null;

        var scriptList = XlgQuickScriptFile.Load(filePath);
        return scriptList[scriptName];
    }

    private string ResolveFimmFilePath(string scriptName)
    {
        if (scriptName.IsEmpty())
            return "";

        var resolvedFimmFilePath = scriptName;
        if (resolvedFimmFilePath.ToLower() != ".fimm")
            resolvedFimmFilePath += ".fimm";

        if (File.Exists(resolvedFimmFilePath))
            return resolvedFimmFilePath;

        var possibility = Path.Combine(Environment.CurrentDirectory, scriptName);
        if (File.Exists(possibility))
            return possibility;

        possibility = Path.Combine(Shared.Dirs.FimmFolderPath, scriptName);
        if (File.Exists(possibility))
            return possibility;

        return "";
    }

    public ActualizationResult RunQuickScript(string scriptName)
    {
        var scriptToRun = FindScript(scriptName);
        var settings = scriptToRun.BuildSettings(false, Host);
        var result = settings.ActualizeAndCompile();
        return result;
    }

    public ActualizationResult RunQuickScript(XlgQuickScript scriptToRun)
    {
        if (scriptToRun == null)
        {
            var actualizationSettings = new ActualizationSettings(null, false, null, true, null);
            var actualizationResult = new ActualizationResult(actualizationSettings)
            {
                CompileErrorText = "No script provided to Wallaby.RunQuickScript(), exiting."
            };
            return actualizationResult;
        }

        var settings = scriptToRun.BuildSettings(false, Host);
        var result = settings.ActualizeAndCompile();
        return result;
    }
}