using MetX.Standard.Strings;

namespace MetX.Standard.Test.Strings.Tokens;

[TestClass]
public class TokenizerTests
{
    [TestMethod]
    public void Test_AllTokens_WithDefaultDelimiter() =>
        CollectionAssert.AreEqual(new List<string> { "this", "is", "a", "test" }, "this is a test".AllTokens());

    [TestMethod]
    public void Test_AllTokens_WithComma() =>
        CollectionAssert.AreEqual(new List<string> { "apple", "orange" }, "apple,orange".AllTokens(","));

    [TestMethod]
    public void Test_Carve_WithIndexes() =>
        CollectionAssert.AreEqual(new[] { "abc", "def", "ghi" }, "abc-def-ghi".Carve(new[] { 3, 7 }, 1));

    [TestMethod]
    public void Test_Carve_EmptyTarget() => CollectionAssert.AreEqual(Array.Empty<string>(), "".Carve(new[] { 1 }, 1));

    [TestMethod]
    public void Test_LastPathToken_SimplePath() => Assert.AreEqual("file.txt", "C:\\folder\\file.txt".LastPathToken());

    [TestMethod]
    public void Test_LastPathToken_WithTrailingSlash() => Assert.AreEqual("", "C:\\folder\\".LastPathToken());

    [TestMethod]
    public void Test_LastToken_SpaceDelimited() => Assert.AreEqual("test", "this is a test".LastToken());

    [TestMethod]
    public void Test_LastToken_CustomDelimiter() => Assert.AreEqual("last", "one|two|last".LastToken("|"));

    [TestMethod]
    public void Test_TokenAt_First() => Assert.AreEqual("this", "this is a test".TokenAt(1));

    [TestMethod]
    public void Test_TokenAt_Third() => Assert.AreEqual("a", "this is a test".TokenAt(3));

    [TestMethod]
    public void Test_TokenBetween_Parens() => Assert.AreEqual("inner", "before(inner)after".TokenBetween());

    [TestMethod]
    public void Test_TokenBetween_Custom() => Assert.AreEqual("content", "start<content>end".TokenBetween("<", ">"));

    [TestMethod]
    public void Test_EveryTokenBetween_Basic() =>
        CollectionAssert.AreEqual(new List<string> { "x", "y" }, "[x][y]".EveryTokenBetween("[", "]"));

    [TestMethod]
    public void Test_EveryTokenBetween_NoMatch() =>
        CollectionAssert.AreEqual(new List<string>(), "abc".EveryTokenBetween("<", ">"));

    [TestMethod]
    public void Test_Splice_LeftRight() => CollectionAssert.AreEqual(new List<string> { "before", "middle", "after" },
        new List<string>("before[middle]after".Splice("[", "]")));

    [TestMethod]
    public void Test_Splice_LeftRight_NoMatch() => CollectionAssert.AreEqual(new List<string> { "no brackets here" },
        new List<string>("no brackets here".Splice("[", "]")));

    [TestMethod]
    public void Test_UpdateBetweenTokens_Basic() => Assert.AreEqual("before[CHANGED]after",
        "before[original]after".UpdateBetweenTokens("[", "]", false, _ => "CHANGED"));

    [TestMethod]
    public void Test_UpdateBetweenTokens_ConsumeDelimiters() => Assert.AreEqual("beforeCHANGEDafter",
        "before[original]after".UpdateBetweenTokens("[", "]", true, _ => "CHANGED"));

    [TestMethod]
    public void Test_TokenCount_DefaultDelimiter() => Assert.AreEqual(4, "this is a test".TokenCount());

    [TestMethod]
    public void Test_TokenCount_CustomDelimiter() => Assert.AreEqual(3, "1,2,3".TokenCount(","));

    [TestMethod]
    public void Test_TokenIndex_SecondToken() => Assert.AreEqual(5, "this is a test".TokenIndex(2));

    [TestMethod]
    public void Test_TokenIndex_TokenNotFound() =>
        Assert.AreEqual("this is a test".Length, "this is a test".TokenIndex(10));

    [TestMethod]
    public void Test_TokenIndexes_Basic()
    {
        var actual = new List<int>("one two three".TokenIndexes(" "));
        CollectionAssert.AreEqual(
            new List<int> { 3, 7 },
            actual);
    }

    [TestMethod]
    public void Test_TokenIndexes_None() =>
        CollectionAssert.AreEqual(new List<int>(), new List<int>("abc".TokenIndexes("|")));

    [TestMethod]
    public void Test_TokensAfter_Second() => Assert.AreEqual("a test", "this is a test".TokensAfter(2));

    [TestMethod]
    public void Test_TokensAfter_First() => Assert.AreEqual("is a test", "this is a test".TokensAfter());

    [TestMethod]
    public void Test_TokensAfterFirst_Basic()
    {
        var actual = "this is a test".TokensAfterFirst();
        Console.WriteLine(actual);
        Assert.AreEqual(
            "is a test",
            actual);
    }

    [TestMethod]
    public void Test_TokensAfterFirst_CustomDelimiter()
    {
        var actual = "one|two|three".TokensAfterFirst("|");
        Assert.AreEqual(
            "two|three",
            actual);
    }

    [TestMethod]
    public void Test_TokensAround_Basic()
    {
        var actual = "this [is] that".TokensAround("[", "]");
        Assert.AreEqual(
            "this  that",
            actual);
    }

    [TestMethod]
    public void Test_TokensAround_NoRightDelimiter() =>
        Assert.AreEqual("[is that", "this [is that".TokensAround("[", "]"));

    [TestMethod]
    public void Test_TokensBefore_Third()
    {
        var actual = "this is a test".TokensBefore(3);
        Assert.AreEqual("this is", actual);
    }

    [TestMethod]
    public void Test_TokensBefore_First() => Assert.AreEqual("", "this is a test".TokensBefore(1));

    [TestMethod]
    public void Test_TokensBeforeLast_Space() => Assert.AreEqual("this is a", "this is a test".TokensBeforeLast());

    [TestMethod]
    public void Test_TokensBeforeLast_Custom() => Assert.AreEqual("one|two", "one|two|three".TokensBeforeLast("|"));

    [TestMethod]
    public void Test_Splice_LeftRight_Options() => CollectionAssert.AreEqual(new List<string> { "x", "y", "z" },
        new List<string>("x[[y]]z".Splice("[[", "]]")));
}