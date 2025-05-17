using MetX.Standard.Strings.Tokens.GPT;

namespace MetX.Standard.Test.Strings.Tokens.GPT;

[TestClass]
public class GptDebugLoggingTests
{
    [TestMethod]
    public void Test_PrintTokens_String_Numbered()
    {
        var input = "one two three";
        using var sw = new StringWriter();
        Console.SetOut(sw);
        input.PrintTokens();
        var actual = sw.ToString().Trim().Replace("\r", "").Split('\n');
        Console.SetOut(Console.Out);
        CollectionAssert.AreEqual(new[] { "1: one", "2: two", "3: three" }, actual);
    }

    [TestMethod]
    public void Test_PrintTokens_List_Unnumbered()
    {
        var tokens = new List<string> { "alpha", "beta" };
        using var writer = new StringWriter();
        Console.SetOut(writer);
        tokens.PrintTokens(numbered: false);
        var actual = writer.ToString().Trim().Replace("\r", "").Split('\n');
        Console.SetOut(Console.Out);

        CollectionAssert.AreEqual(new[] { "alpha", "beta" }, actual);
    }

    [TestMethod]
    public void Test_VisualizeTokenLengths_Basic()
    {
        var input = "hi there";
        using var sw = new StringWriter();
        Console.SetOut(sw);
        input.VisualizeTokenLengths();
        var actual = sw.ToString().Trim().Replace("\r", "").Split('\n');
        CollectionAssert.AreEqual(new[]
        {
            "1: hi | == (2)",
            "2: there | ===== (5)"
        }, actual);
    }

    [TestMethod]
    public void Test_SummaryOfTokens_Basic()
    {
        var input = "short longer longest";
        using var sw = new StringWriter();
        Console.SetOut(sw);
        input.SummaryOfTokens();
        var output = sw.ToString();
        StringAssert.Contains(output, "Token Count: 3");
        StringAssert.Contains(output, "Shortest Token: 'short' (5)");
        StringAssert.Contains(output, "Longest Token: 'longest' (7)");
        StringAssert.Contains(output, "Average Length:");
    }

    [TestMethod]
    public void Test_SummaryOfTokens_EmptyInput()
    {
        var input = "";
        using var sw = new StringWriter();
        Console.SetOut(sw);
        input.SummaryOfTokens();
        var output = sw.ToString().Trim();
        Assert.AreEqual("No input.", output);
    }
}