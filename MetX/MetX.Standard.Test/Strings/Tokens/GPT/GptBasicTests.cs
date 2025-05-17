using MetX.Standard.Strings.Tokens.GPT;

namespace MetX.Standard.Test.Strings.Tokens.GPT;

[TestClass]
public class GptBasicTests
{
    [TestMethod]
    public void Test_NestedTokens_OneLevel() =>
        CollectionAssert.AreEqual(new List<string> { "(abc)" }, "start(abc)end".NestedTokens());

    [TestMethod]
    public void Test_NestedTokens_TwoLevels()
    {
        var actual = "before(a(b)c)after".NestedTokens(depth: 2);
        CollectionAssert.AreEqual(new List<string> { "(b)" }, actual);
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

    [TestMethod]
    public void Test_SplitAndTrim_Basic() =>
        CollectionAssert.AreEqual(new List<string> { "one", "two" }, " one , two ".SplitAndTrim(","));

    [TestMethod]
    public void Test_SplitAndTrim_MultipleSpaces() => CollectionAssert.AreEqual(new List<string> { "alpha", "beta" },
        " alpha  | beta ".SplitAndTrim("|"));

    [TestMethod]
    public void Test_TokensBetween_SinglePair() =>
        CollectionAssert.AreEqual(new List<string> { "inner" }, "start(inner)end".TokensBetween());

    [TestMethod]
    public void Test_TokensBetween_MultiplePairs() =>
        CollectionAssert.AreEqual(new List<string> { "a", "b" }, "x(a)y(b)z".TokensBetween());

    [TestMethod]
    public void Test_FirstToken_Space() => Assert.AreEqual("hello", "hello world".FirstToken());

    [TestMethod]
    public void Test_FirstToken_CustomDelimiter() => Assert.AreEqual("start", "start|middle|end".FirstToken("|"));

    [TestMethod]
    public void Test_TokenRange_ValidRange() =>
        CollectionAssert.AreEqual(new List<string> { "b", "c" }, "a b c d".TokenRange(1, 3));

    [TestMethod]
    public void Test_TokenRange_InvalidRange() =>
        CollectionAssert.AreEqual(new List<string>(), "one two".TokenRange(3, 5));

    [TestMethod]
    public void Test_ReverseTokens_Simple() => Assert.AreEqual("c b a", "a b c".ReverseTokens());

    [TestMethod]
    public void Test_ReverseTokens_CommaDelimited() => Assert.AreEqual("z,y,x", "x,y,z".ReverseTokens(","));

    [TestMethod]
    public void Test_RemoveEmptyTokens_Trimmed() =>
        CollectionAssert.AreEqual(new List<string> { "a", "b", "c" }, "a  b   c".RemoveEmptyTokens());

    [TestMethod]
    public void Test_RemoveEmptyTokens_CustomDelimiter() => CollectionAssert.AreEqual(new List<string> { "one", "two" },
        "one||two|||".RemoveEmptyTokens("|"));

    [TestMethod]
    public void Test_ReplaceTokenAt_Default() => Assert.AreEqual("a b Z d", "a b c d".ReplaceTokenAt("Z"));

    [TestMethod]
    public void Test_ReplaceTokenAt_IndexZero() => Assert.AreEqual("X b c", "a b c".ReplaceTokenAt("X", index: 0));

    [TestMethod]
    public void Test_TrimTokens_LeadingTrailing()
    {
        var actual = " a   b  c ".TrimTokens();
        Assert.AreEqual("a b c", actual);
    }

    [TestMethod]
    public void Test_TrimTokens_CustomDelimiter() => Assert.AreEqual("x|y|z", " x | y | z ".TrimTokens("|"));

    [TestMethod]
    public void Test_InsertTokenAt_Valid() =>
        Assert.AreEqual("one|TWO|two", "one|two".InsertTokenAt("TWO", delimiter: "|", index: 1));

    [TestMethod]
    public void Test_InsertTokenAt_End() =>
        Assert.AreEqual("a|b|X", "a|b".InsertTokenAt("X", delimiter: "|", index: 10));

    [TestMethod]
    public void Test_RemoveTokenAt_Second() => Assert.AreEqual("one three", "one two three".RemoveTokenAt(1));

    [TestMethod]
    public void Test_RemoveTokenAt_CustomDelimiter() =>
        Assert.AreEqual("first|third", "first|second|third".RemoveTokenAt("|", 1));

    [TestMethod]
    public void Test_CountTokens_Spaces() => Assert.AreEqual(3, "a b c".CountTokens());

    [TestMethod]
    public void Test_CountTokens_CustomDelimiter() => Assert.AreEqual(2, "x|y".CountTokens("|"));

    [TestMethod]
    public void Test_TokenExists_True() => Assert.IsTrue("one two three".TokenExists("two"));

    [TestMethod]
    public void Test_TokenExists_False() => Assert.IsFalse("x y z".TokenExists("a"));

    [TestMethod]
    public void Test_IndexOfToken_Found() => Assert.AreEqual(1, "a b c".IndexOfToken("b"));

    [TestMethod]
    public void Test_IndexOfToken_NotFound() => Assert.AreEqual(-1, "red blue green".IndexOfToken("yellow"));

    [TestMethod]
    public void Test_TokenStartsWith_True() => Assert.IsTrue("cat dog".TokenStartsWith("ca"));

    [TestMethod]
    public void Test_TokenStartsWith_False() => Assert.IsFalse("foo bar".TokenStartsWith("baz"));

    [TestMethod]
    public void Test_RemoveDuplicateTokens_Basic() => Assert.AreEqual("a b", "a b a".RemoveDuplicateTokens());

    [TestMethod]
    public void Test_RemoveDuplicateTokens_CustomDelimiter() =>
        Assert.AreEqual("1|2", "1|2|1|2".RemoveDuplicateTokens("|"));
}