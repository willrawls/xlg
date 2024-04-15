using System;
using System.Net;
using MetX.Console.Tests.Fimm;
using MetX.Fimm;
using MetX.Fimm.Glove.Pipelines;
using MetX.Fimm.Scripts;
using MetX.Fimm.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests;

[TestClass]
public class DatabaseWalking
{
    [TestMethod]
    public void GenerateXlgDocFromWalkerTemplateFolderTest1()
    {
        var host = new TestHost("");
        var fimmSettings = new ArgumentSettings
        {
            Name = "Fred",
            Verb = ArgumentVerb.Walk,
            Noun = ArgumentNoun.TemplateFolder,
            Path = @"E:\OneDrive\Documents\XLG\Walkers\Test 1",
            Host = host,
        };

        var actor = new TemplateActor(fimmSettings.Verb, fimmSettings.Noun);
        Assert.IsNotNull(actor);

        var processingFunction = actor.GetProcessingFunction(fimmSettings);
        var result = processingFunction(fimmSettings);



        /*
        Assert.AreEqual(InstructionType.Template, area.InstructionType);
        Assert.AreEqual(2, area.Arguments.Count);
        Assert.AreEqual(TemplateType.Table, area.TemplateType);
        Assert.AreEqual("?", area.Target);
    */
    }

}