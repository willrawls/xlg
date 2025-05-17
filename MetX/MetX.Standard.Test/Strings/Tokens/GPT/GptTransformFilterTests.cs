using MetX.Standard.Strings.Tokens.GPT;

namespace MetX.Standard.Test.Strings.Tokens.GPT;

[TestClass]
public class GptTransformFilterTests
{
    [TestMethod]
    public void Test_MapTokens_UpperCase()
    {
        var input = "a b c";
        var result = input.MapTokens(t => t.ToUpper());
        Assert.AreEqual("A B C", result);
    }

    [TestMethod]
    public void Test_FilterTokens_StartsWithA()
    {
        var input = "apple banana apricot";
        var result = input.FilterTokens(t => t.StartsWith("a"));
        Assert.AreEqual("apple apricot", result);
    }

    [TestMethod]
    public void Test_DistinctTokens_RemovesDuplicates()
    {
        var input = "a b a b c";
        var result = input.DistinctTokens();
        Assert.AreEqual("a b c", result);
    }

    [TestMethod]
    public void Test_SortedTokens_Alphabetical()
    {
        var input = "banana apple cherry";
        var result = input.SortedTokens();
        Assert.AreEqual("apple banana cherry", result);
    }

    [TestMethod]
    public void Test_SanitizeTokens_TrimsWhitespace()
    {
        var input = " apple  banana ";
        var actual = input.SanitizeTokens();
        Assert.AreEqual("apple banana", actual);
    }

    [TestMethod]
    public void Test_PadTokens_ToLength()
    {
        var input = "a bb ccc";
        var result = input.PadTokens(4, '-');
        Assert.AreEqual("a--- bb-- ccc-", result);
    }

    [TestMethod]
    public void Test_TruncateTokens_MaxLength()
    {
        var input = "abcdef ghi jk";
        var result = input.TruncateTokens(3);
        Assert.AreEqual("abc ghi jk", result);
    }

    [TestMethod]
    public void Test_SurroundTokens_WithBrackets()
    {
        var input = "a b c";
        var result = input.SurroundTokens("[", "]");
        Assert.AreEqual("[a] [b] [c]", result);
    }
}