﻿using System;
using System.Diagnostics;
using System.IO;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.IO;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Scripts;

[TestClass]
public class XlgQuickScriptTemplateTests
{
    [TestMethod]
    public void ResolveExeTemplate()
    {
        var quickScriptTemplate = new XlgQuickScriptTemplate("TestTemplates", @"TestTemplates\TestExe");

        Assert.IsNotNull(quickScriptTemplate);
        Assert.IsNotNull(quickScriptTemplate.Assets);
        Assert.AreEqual(4, quickScriptTemplate.Assets.Count);
        Assert.AreEqual("TestExe", quickScriptTemplate.Name);

        Assert.AreEqual(4, quickScriptTemplate.Assets.Items().Length);
        CollectionAssert.AreEqual(new [] { "Program.cs", "QuickScriptProcessor.cs", "_.csproj", "_.sln" }, quickScriptTemplate.Assets.Keys());

        XlgQuickScript source = new XlgQuickScript("Freddy", "a = b;");

        BuildDoNothingGenerationHost(out var pathToTestTemplates, out var host, out var context);
        var settings = new ActualizationSettings(quickScriptTemplate, true, source, true, host);

        settings.Answers["DestinationFilePath"].Value = "AAA";
        settings.Answers["InputFilePath"].Value = "BBB";
        settings.Answers["NameInstance"].Value = "CCC";
        settings.Answers["Usings"].Value = "using DDD.D;\n";

        settings.Answers["ClassMembers"].Value = "string fred;";
        settings.Answers["Finish"].Value = "fred = \"finish\";";
        settings.Answers["ProcessLine"].Value = "fred = line;";
        settings.Answers["ReadInput"].Value = "return true;";
        settings.Answers["Start"].Value = "~~:Starting";
        settings.Answers["Project Name"].Value = settings.ProjectName;
        settings.Answers["UserName"].Value = Environment.UserName.LastToken(@"\");
        settings.Answers["Guid Config"].Value = Guid.NewGuid().ToString("N");
        settings.Answers["Guid Project 1"].Value = Guid.NewGuid().ToString("N");
        settings.Answers["Guid Project 2"].Value = Guid.NewGuid().ToString("N");
        settings.Answers["Guid Solution"].Value = Guid.NewGuid().ToString("N");

        ActualizationResult actual = quickScriptTemplate.ActualizeCode(settings);

        Assert.IsNotNull(actual);
        Assert.IsNull(actual.ActualizeErrorText);
        Assert.AreEqual(4, actual.OutputFiles.Count);
        Assert.IsFalse(actual.OutputFiles["_.csproj"].Value?.Contains("_") == false);

        if (actual.Warnings.Count == 0)
            return;

        System.Console.WriteLine(actual.Settings.ProjectFolder);
        System.Console.WriteLine();
        System.Console.WriteLine("----- Warnings ------");
        System.Console.WriteLine();
        foreach(var warning in actual.Warnings)
            System.Console.WriteLine($"{warning}");
        System.Console.WriteLine();

        bool compileResult = actual.Compile();
        Assert.IsNotNull(actual.DestinationExecutableFilePath);
        System.Console.WriteLine($"\nDestinationAssemblyFilePath is:\n{actual.DestinationExecutableFilePath}");
    }

    public static void BuildDoNothingGenerationHost(out string pathToTestTemplates,
        out DoNothingGenerationHost host, out ContextBase context)
    {
        pathToTestTemplates = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestTemplates");
        context = new ContextBase(pathToTestTemplates, null);
        host = new DoNothingGenerationHost("", context);
        context.Host = host;
        host.Context = context;
        host.Context.Templates = new XlgQuickScriptTemplateList(pathToTestTemplates);
    }

    private static void AssertConsoleExecutableOutputsProperly(ActualizationResult result, string expected)
    {
        var gatherResult = FileSystem.GatherOutputAndErrors(result.DestinationExecutableFilePath, null, out var errorOutput, result.Settings.ProjectFolder, 15, ProcessWindowStyle.Hidden);

        System.Console.WriteLine();
        System.Console.WriteLine(result.DestinationExecutableFilePath);

        if (gatherResult.IsNotEmpty())
        {
            System.Console.WriteLine();
            System.Console.WriteLine("-----[ Output ]-----");
            System.Console.WriteLine(gatherResult);
        }

        if (errorOutput.IsNotEmpty())
        {
            System.Console.WriteLine();
            System.Console.WriteLine("-----[ Errors ]-----");
            System.Console.WriteLine(errorOutput);
        }

        Assert.IsTrue(gatherResult.Contains(expected));
    }
}