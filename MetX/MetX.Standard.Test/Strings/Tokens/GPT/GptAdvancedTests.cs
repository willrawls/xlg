using MetX.Standard.Strings.Tokens.GPT;

namespace MetX.Standard.Test.Strings.Tokens.GPT;

[TestClass]
public class GptAdvancedTests
{
    [TestMethod]
    public void Test_NestedTokens_OneLevel() =>
        CollectionAssert.AreEqual(new List<string> { "(abc)" }, "start(abc)end".NestedTokens());

    [TestMethod]
    public void Test_NestedTokens_TwoLevels()
    {
        var actual = "before(a(b)c)after".NestedTokens(depth: 1);
        CollectionAssert
            .AreEqual(
                new List<string> { "(a(b)c)" },
                actual);
    }

    [TestMethod]
    public void Test_EscapeDelimiter_Space() => Assert.AreEqual("hello\\ world", "hello world".EscapeDelimiter());

    [TestMethod]
    public void Test_EscapeDelimiter_Comma() => Assert.AreEqual("apple\\,orange", "apple,orange".EscapeDelimiter(","));

    [TestMethod]
    public void Test_UnescapeDelimiter_Space() => Assert.AreEqual("hello world", "hello\\ world".UnescapeDelimiter());

    [TestMethod]
    public void Test_UnescapeDelimiter_Comma() =>
        Assert.AreEqual("apple,orange", "apple\\,orange".UnescapeDelimiter(","));

    [TestMethod]
    public void Test_QuotedTokens_Simple() =>
        CollectionAssert.AreEqual(new List<string> { "a", "b c", "d" }, "a \"b c\" d".QuotedTokens());

    [TestMethod]
    public void Test_QuotedTokens_CustomDelimiter() =>
        CollectionAssert.AreEqual(new List<string> { "one", "two three" }, "one|\"two three\"".QuotedTokens("|"));

    [TestMethod]
    public void Test_SmartSplit_EscapedDelimiter() =>
        CollectionAssert.AreEqual(new List<string> { "a", "b|c", "d" }, "a|b\\|c|d".SmartSplit("|"));

    [TestMethod]
    public void Test_SmartSplit_Quoted() =>
        CollectionAssert.AreEqual(new List<string> { "a", "b c", "d" }, "a \"b c\" d".SmartSplit());

    [TestMethod]
    public void Test_MultiDelimiterSplit_PipesAndCommas() =>
        CollectionAssert.AreEqual(new List<string> { "a", "b", "c" }, "a|b,c".MultiDelimiterSplit("|", ","));

    [TestMethod]
    public void Test_MultiDelimiterSplit_SpacesTabs() => CollectionAssert.AreEqual(new List<string> { "x", "y", "z" },
        "x y\tz".MultiDelimiterSplit(" ", "\t"));
}