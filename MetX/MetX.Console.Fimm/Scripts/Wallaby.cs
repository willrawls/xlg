
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
    public const string FimmExtension = ".fimm";
    public const string XlgExtension = ".xlg";
    public ContextBase Context;

    public Wallaby(IGenerationHost host = null)
    {
        if (host == null) Context = new ContextBase(Shared.Dirs.ScriptsFolderName, null);

        Host = host;
    }

    public IGenerationHost Host { get; set; }

    public static XlgQuickScript FindScript(string filePath, string scriptName)
    {
        string fileActual = null;
        if (filePath.IsEmpty() || scriptName.IsEmpty())
            return null;

        if (File.Exists(filePath))
        {
            fileActual = filePath;
        }
        else
        {
            fileActual = Path.Combine(filePath, scriptName);
            if (!File.Exists(fileActual))
            {
                fileActual = ResolveScriptFilePath(scriptName, XlgExtension);
                if (!File.Exists(fileActual))
                {
                    fileActual = ResolveScriptFilePath(scriptName, FimmExtension);
                    return null;
                }
            }
        }

        var scriptList = XlgQuickScriptFile.Load(fileActual);

        var scriptToReturn = scriptList.FirstOrDefault(x => x.TemplateName.ToLower() == scriptName);
        return scriptToReturn ?? (scriptList.Count > 0
            ? scriptList[0] // Lone Fimm 
            : null);
    }

    public static XlgQuickScript FindScript(string scriptName)
    {
        if (scriptName.Contains("+")) return FindScript(scriptName.FirstToken("+"), scriptName.TokensAfterFirst("+"));

        var filePath = ResolveScriptFilePath(scriptName, FimmExtension);
        if(filePath.IsEmpty())
            return null;

        var scriptList = XlgQuickScriptFile.Load(filePath);
        return scriptList[scriptName];
    }

    public static string ResolveScriptFilePath(string filename, string extension)
    {
        if (filename.IsEmpty())
            return null;

        var resolvedFimmFilePath = filename;
        if (resolvedFimmFilePath.ToLower() != extension)
            resolvedFimmFilePath += extension;

        if (File.Exists(resolvedFimmFilePath))
            return resolvedFimmFilePath;

        var possibility = Path.Combine(Environment.CurrentDirectory, filename);
        if (File.Exists(possibility))
            return possibility;

        possibility = Path.Combine(Shared.Dirs.FimmFolderPath, filename);
        if (File.Exists(possibility))
            return possibility;

        return null;
    }

    /*
    public ActualizationResult RunQuickScript(string scriptName)
    {
        var scriptToRun = FindScript(scriptName);
        var settings = scriptToRun.BuildSettings(false, Host);
        var result = settings.ActualizeAndCompile();
        return result;
    }
    */

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