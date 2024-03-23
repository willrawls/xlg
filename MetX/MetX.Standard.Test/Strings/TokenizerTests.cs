using System.Collections.Generic;
using System.Linq;
using MetX.Standard.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.Strings;

[TestClass]
public class TokenizerTests
{
    public TestContext TestContext { get; set; }


    public static void AllTokens()
    {
        var result = "".AllTokens();
        Assert.AreEqual(0, result.Count);

        result = "1 2 3  5".AllTokens();
        Assert.AreEqual(5, result.Count);

        result = " 2 3 4  6 ".AllTokens();
        Assert.AreEqual(6, result.Count);
    }

    public static void AllTokensIgnoreCase()
    {
        var result = "".AllTokensIgnoreCase();
        Assert.AreEqual(0, result.Count);

        result = "1a2a3A4A5".AllTokensIgnoreCase();
        Assert.AreEqual(5, result.Count);

        result = "Fred1Fred2fred3".AllTokensIgnoreCase();
        Assert.AreEqual(3, result.Count);
    }

    public static void Carve()
    {
        var result = "".Carve(new int[] { 1, 3, 5}, 1);
        Assert.AreEqual(0, result.Length);

        result = "".Carve(null, 1);
        Assert.AreEqual(0, result.Length);

        result = ".1.3.5".Carve(new int[] { 1, 3, 5}, 1);
        Assert.AreEqual(new string[] { "1", "3", "5"}, result);

        result = "..1..3..5..".Carve(new int[] { 1, 3, 5}, 2);
        Assert.AreEqual(new [] { "1", "3", "5"}, result);
    }

    public static void FirstToken()
    {
        Assert.AreEqual("The", "The honorable Kim Nien".FirstToken());
    }
    public static void TokenAt()
    {
        Assert.AreEqual("Kim", "The honorable Kim Nien".TokenAt(3));
        Assert.AreEqual("Nien", "The honorable Kim Nien".TokenAt(2, "kim"));
    }

    public static void TokenBetween()
    {
        Assert.AreEqual("game", "Henry is all talk and no {game}".TokenBetween("{", "}"));
    }

    public static void EveryTokenBetween()
    {
        var result = "".EveryTokenBetween("{", "}");
        Assert.AreEqual(0, result.Count);

        result = "Henry is all talk and no {game}".EveryTokenBetween("{", "}");
        Assert.AreEqual(new List<string>() {"game"}, result);

        result = "Henry is {all talk} and no {game} and no {fun}".EveryTokenBetween("{", "}");
        Assert.AreEqual(new List<string>() {"all talk", "game"}, result);
    }
    // Write a unit test for Splice
    public static void Splice()
    {
        var result = "".Splice("{", "}");
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("", result.First());

        result = "{".Splice("{", "}");
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("", result.First());

        result = "}".Splice("{", "}");
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("", result.First());

        result = "{}".Splice("{", "}");
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("", result.First());

        result = "{a}".Splice("{", "}");
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("a", result.First());

        result = "{a}{b}".Splice("{", "}");
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual("a", result.First());
        Assert.AreEqual("b", result.Last());

        result = "{a}b{c}".Splice("{", "}");
        Assert.AreEqual(3, result.Count());
        Assert.AreEqual("a", result.First());
        Assert.AreEqual("b", result.ElementAt(1));
        Assert.AreEqual("c", result.Last());

        result = "{a}b{c}".Splice("{", "}");
        Assert.AreEqual(3, result.Count());
        Assert.AreEqual("a", result.First());
        Assert.AreEqual("b", result.ElementAt(1));
        Assert.AreEqual("c", result.Last());

        result = "{a}b{c}".Splice("{", "}");
        Assert.AreEqual(3, result.Count());
        Assert.AreEqual("a", result.First());
        Assert.AreEqual("b", result.ElementAt(1));
        Assert.AreEqual("c", result.Last());

        result = "{a}b{c}".Splice("{", "}");
        Assert.AreEqual(3, result.Count());
        Assert.AreEqual("a", result.First());
        Assert.AreEqual("b", result.ElementAt(1));
        Assert.AreEqual("c", result.Last());

        result = "{a}b{c}".Splice("{", "}");
        Assert.AreEqual(3, result.Count());
        Assert.AreEqual("a", result.First());
        Assert.AreEqual("b", result.ElementAt(1));
        Assert.AreEqual("c", result.Last());
    }

    // write a unit test for UpdateBetweenTokens
    public static void UpdateBetweenTokens()
    {
        var result = "".UpdateBetweenTokens("{", "}", true, s => s.ToUpper());
        Assert.AreEqual("", result);

        result = "{".UpdateBetweenTokens("{", "}", true, s => s.ToUpper());
        Assert.AreEqual("", result);

        result = "}".UpdateBetweenTokens("{", "}", true, s => s.ToUpper());
        Assert.AreEqual("", result);

        result = "{}".UpdateBetweenTokens("{", "}", true, s => s.ToUpper());
        Assert.AreEqual("", result);

        result = "{a}".UpdateBetweenTokens("{", "}", true, s => s.ToUpper());
        Assert.AreEqual("{A}", result);

        result = "{a}{b}".UpdateBetweenTokens("{", "}", true, s => s.ToUpper());
        Assert.AreEqual("{A}{B}", result);

        result = "{a}b{c}".UpdateBetweenTokens("{", "}", true, s => s.ToUpper());
        Assert.AreEqual("{A}B{C}", result);

        result = "{a}b{c}".UpdateBetweenTokens("{", "}", true, s => s.ToUpper());
        Assert.AreEqual("{A}B{C}", result);

        result = "{a}b{c}".UpdateBetweenTokens("{", "}", true, s => s.ToUpper());
        Assert.AreEqual("{A}B{C}", result);

        result = "{a}b{c}".UpdateBetweenTokens("{", "}", true, s => s.ToUpper());
        Assert.AreEqual("{A}B{C}", result);

        result = "{a}b{c}".UpdateBetweenTokens("{", "}", true, s => s.ToUpper());
        Assert.AreEqual("{A}B{C}", result);
    }

    public static void TokenCount()
    {
        Assert.AreEqual(0, "".TokenCount());
        Assert.AreEqual(1, " ".TokenCount());
        Assert.AreEqual(1, "a".TokenCount());
        Assert.AreEqual(1, "a ".TokenCount());
        Assert.AreEqual(1, " a".TokenCount());
        Assert.AreEqual(2, "a b".TokenCount());
        Assert.AreEqual(2, "a b ".TokenCount());
        Assert.AreEqual(2, " a b".TokenCount());
        Assert.AreEqual(3, "a b c".TokenCount());
        Assert.AreEqual(3, "a b c ".TokenCount());
        Assert.AreEqual(3, " a b c".TokenCount());
        Assert.AreEqual(4, "a b c d".TokenCount());
        Assert.AreEqual(4, "a b c d ".TokenCount());
        Assert.AreEqual(4, " a b c d".TokenCount());
    }

    public static void TokenIndex()
    {
        Assert.AreEqual(-1, "".TokenIndex(1));
        Assert.AreEqual(-1, " ".TokenIndex(1));
        Assert.AreEqual(0, "a".TokenIndex(1));
        Assert.AreEqual(0, "a ".TokenIndex(1));
        Assert.AreEqual(0, " a".TokenIndex(1));
        Assert.AreEqual(1, "a b".TokenIndex(2));
        Assert.AreEqual(1, "a b ".TokenIndex(2));
        Assert.AreEqual(1, " a b".TokenIndex(2));
    }

    // Write a unit test for TokenIndexes
    public static void TokenIndexes()
    {
        IEnumerable<int>? result = "".TokenIndexes();
        Assert.AreEqual(new List<int>(), result.ToList());

        result = "a b c ".TokenIndexes().ToList();
        Assert.AreEqual(new List<int>() { 0, 2, 4, 6}, result);

    }
}