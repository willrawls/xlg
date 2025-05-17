using MetX.Standard.Strings.Tokens.GPT;

namespace MetX.Standard.Test.Strings.Tokens.GPT;

[TestClass]
public class GptPairingZipTests
{
    [TestMethod]
    public void Test_PrintTokens_String_Numbered()
    {
        var input = "one two three";
        using var sw = new StringWriter();
        Console.SetOut(sw);
        input.PrintTokens();
        var output = sw.ToString().Trim().Replace("\r", "").Split('\n');
        CollectionAssert.AreEqual(new[] { "1: one", "2: two", "3: three" }, output);
    }

    [TestMethod]
    public void Test_PrintTokens_List_Unnumbered()
    {
        var tokens = new List<string> { "alpha", "beta" };
        using var sw = new StringWriter();
        Console.SetOut(sw);
        tokens.PrintTokens(numbered: false);
        var output = sw.ToString().Trim().Replace("\r", "").Split('\n');
        CollectionAssert.AreEqual(new[] { "alpha", "beta" }, output);
    }

    [TestMethod]
    public void Test_VisualizeTokenLengths_Basic()
    {
        var input = "hi there";
        using var sw = new StringWriter();
        Console.SetOut(sw);
        input.VisualizeTokenLengths();
        var output = sw.ToString().Trim().Replace("\r", "").Split('\n');
        CollectionAssert.AreEqual(new[]
        {
            "1: hi | == (2)",
            "2: there | ===== (5)"
        }, output);
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

    [TestMethod]
    public void Test_TokenPairs_Basic()
    {
        var input = "one two three four";
        var expected = new List<(string, string)> { ("one", "two"), ("three", "four") };
        var result = input.TokenPairs();
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Test_TokenPairsWithDelimiter_Defaults()
    {
        var input = "a b c d";
        var expected = "a:b c:d";
        var result = input.TokenPairsWithDelimiter();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Test_ZipTokens_Simple()
    {
        var input1 = "one two three";
        var input2 = "1 2 3";
        var expected = new List<(string, string)> { ("one", "1"), ("two", "2"), ("three", "3") };
        var result = input1.ZipTokens(input2);
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Test_KeyValuePairsFromTokens_Basic()
    {
        var input = "key1 val1 key2 val2";
        var expected = new Dictionary<string, string>
        {
            { "key1", "val1" },
            { "key2", "val2" }
        };
        var result = input.KeyValuePairsFromTokens();
        CollectionAssert.AreEquivalent(expected, result);
    }

    [TestMethod]
    public void Test_GroupTokens_ThreeEach()
    {
        var input = "a b c d e f g";
        var expected = new List<List<string>>
        {
            new() { "a", "b", "c" },
            new() { "d", "e", "f" },
            new() { "g" }
        };
        var result = input.GroupTokens(3);
        Assert.AreEqual(expected.Count, result.Count);
        for (int i = 0; i < expected.Count; i++)
            CollectionAssert.AreEqual(expected[i], result[i]);
    }
}