using MetX.Five;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests;

[TestClass]
public class FiverParserTests
{
    private const string Fiver_TemplateAreaBeginProcessingArea = @"
~~Template:
    string x = ""Ding"";

~~Area: Fred
    string y = ""Fred"";
    y = y.Trim();

~~BeginProcessing: %Input%

~~Area: George
    string z = ""George"";
    z += ""z"";
    z += ""Mary"";
";

    [TestMethod]
    public void Parse_Simple()
    {
        var data = new FiverParser();
        data.Parse(@"
~~Template:
    string x = ""Ding"";
");
        Assert.AreEqual(1, data.Templates.Count);
        Assert.AreEqual("Default", data.Templates[0].Name);
        Assert.AreEqual(0, data.Areas.Count);
        Assert.AreEqual(InstructionType.Template, data.Templates[0].InstructionType);
        Assert.AreEqual(1, data.Templates[0].Lines.Count);
        Assert.IsTrue(data.Templates[0].Lines[0].Contains(@"string x = ""Ding"";"));
    }

    [TestMethod]
    public void Parse_Simple_OneTemplateAnd2Areas()
    {
        var data = new FiverParser();
        data.Parse(@"
~~Template:
    string x = ""Ding"";
~~Area: Fred
    string y = ""Fred"";
    y = y.Trim();
~~Area: George
    string z = ""George"";
    z += ""z"";
    z += ""Mary"";
");
        Assert.AreEqual(1, data.Templates.Count);
        Assert.AreEqual(2, data.Areas.Count);

        Assert.AreEqual("Default", data.Templates[0].Name);
        Assert.AreEqual("Fred", data.Areas[0].Name);
        Assert.AreEqual("George", data.Areas[1].Name);
            
        Assert.AreEqual(1, data.Templates[0].Lines.Count);
        Assert.AreEqual(2, data.Areas[0].Lines.Count);
        Assert.AreEqual(3, data.Areas[1].Lines.Count);
            
        Assert.IsTrue(data.Areas[0].Lines[0].Contains("Fred"));
        Assert.IsTrue(data.Areas[1].Lines[0].Contains("George"));
    }
        
    [TestMethod]
    public void Parse_Simple_AreasAfterBeginProcessingAreMarkedAsProcessingAreas()
    {
        var data = new FiverParser();
        data.Parse(Fiver_TemplateAreaBeginProcessingArea);
        Assert.AreEqual(1, data.Areas.Count);
        Assert.AreEqual(2, data.Activities.Count);
        Assert.AreEqual(1, data.Templates.Count);
        Assert.IsFalse(data.Areas[0].IsAreaForProcessing);
        Assert.IsTrue(data.Activities[0].IsAreaForProcessing);
        Assert.IsTrue(data.Activities[1].IsAreaForProcessing);
    }
        
    [TestMethod]
    public void Parse_Simple_AreasAfterBeginProcessingArePutInActivities()
    {
        var data = new FiverParser();
        data.Parse(Fiver_TemplateAreaBeginProcessingArea);
        Assert.AreEqual(2, data.Activities.Count);
        Assert.AreEqual(InstructionType.BeginProcessing, data.Activities[0].InstructionType);
        Assert.AreEqual(InstructionType.Area, data.Activities[1].InstructionType);
    }

        
}