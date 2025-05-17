using MetX.Standard.Strings.Tokens.GPT;

namespace MetX.Standard.Test.Strings.Tokens.GPT;

[TestClass]
public class GptSearchMatchTests
{
    public void Test_FindTokenMatching_MatchExists()
    {
        var input = "cat dog bird";
        var result = input.FindTokenMatching(t => t.StartsWith("d"));
        Assert.AreEqual("dog", result);
    }

    [TestMethod]
    public void Test_FindTokenMatching_NoMatch()
    {
        var input = "cat dog bird";
        var result = input.FindTokenMatching(t => t.StartsWith("x"));
        Assert.IsNull(result);
    }

    [TestMethod]
    public void Test_AllTokensMatch_True()
    {
        var input = "aaa aab aba";
        var result = input.AllTokensMatch(t => t.StartsWith("a"));
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Test_AllTokensMatch_False()
    {
        var input = "aaa aab bba";
        var result = input.AllTokensMatch(t => t.StartsWith("a"));
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Test_AnyTokenMatches_True()
    {
        var input = "cat dog bird";
        var result = input.AnyTokenMatches(t => t.StartsWith("d"));
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Test_AnyTokenMatches_False()
    {
        var input = "cat bat rat";
        var result = input.AnyTokenMatches(t => t.StartsWith("z"));
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Test_TokenEndsWith_True()
    {
        var input = "hello world moon";
        Assert.IsTrue(input.TokenEndsWith("ld"));
    }

    [TestMethod]
    public void Test_TokenEndsWith_False()
    {
        var input = "sun sky earth";
        Assert.IsFalse(input.TokenEndsWith("zz"));
    }

    [TestMethod]
    public void Test_TokenContains_True()
    {
        var input = "apple banana cherry";
        Assert.IsTrue(input.TokenContains("nan"));
    }

    [TestMethod]
    public void Test_TokenContains_False()
    {
        var input = "apple banana cherry";
        Assert.IsFalse(input.TokenContains("xyz"));
    }

    [TestMethod]
    public void Test_TokensWithLength_ThreeLetters()
    {
        var input = "one two three four five six";
        var result = input.TokensWithLength(3);
        CollectionAssert.AreEqual(new List<string> { "one", "two", "six" }, result);
    }

    [TestMethod]
    public void Test_TokensLongerThan_Four()
    {
        var input = "this is a sentence with long tokens";
        var result = input.TokensLongerThan(4);
        CollectionAssert.AreEqual(new List<string> { "sentence", "tokens" }, result);
    }

    [TestMethod]
    public void Test_FindLongestToken()
    {
        var input = "one three twentyfive";
        var result = input.FindLongestToken();
        Assert.AreEqual("twentyfive", result);
    }

    [TestMethod]
    public void Test_FindShortestToken()
    {
        var input = "one three twentyfive";
        var result = input.FindShortestToken();
        Assert.AreEqual("one", result);
    }
}