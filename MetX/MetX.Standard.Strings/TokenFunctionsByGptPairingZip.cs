using System;
using System.Collections.Generic;
using System.Linq;

namespace MetX.Standard.Strings;

public static class TokenFunctionsByGptPairingZip
{
    /*
       📚 Token Pairing / Zipping (5–10 functions)
          TokenPairs(), TokenPairsWithDelimiter()
          ZipTokens(otherDelimitedString)
          KeyValuePairsFromTokens(everyN = 2)
          GroupTokens(n)
    */
    public static List<(string, string)> TokenPairs(this string input, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        var result = new List<(string, string)>();
        if (string.IsNullOrEmpty(input)) return result;
        var tokens = input.Split([delimiter], StringSplitOptions.None);
        for (var i = 0; i + 1 < tokens.Length; i += 2)
        {
            result.Add((tokens[i], tokens[i + 1]));
        }

        return result;
    }

    public static string TokenPairsWithDelimiter(this string input, string pairDelimiter = ":",
        string tokenDelimiter = " ", StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        var pairs = input.TokenPairs(tokenDelimiter, compare);
        return string.Join(tokenDelimiter, pairs.Select(pair => pair.Item1 + pairDelimiter + pair.Item2));
    }

    public static List<(string, string)> ZipTokens(this string input, string other, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        var result = new List<(string, string)>();
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(other)) return result;
        var tokens1 = input.Split([delimiter], StringSplitOptions.None);
        var tokens2 = other.Split([delimiter], StringSplitOptions.None);
        var count = Math.Min(tokens1.Length, tokens2.Length);
        for (var i = 0; i < count; i++)
        {
            result.Add((tokens1[i], tokens2[i]));
        }

        return result;
    }

    public static Dictionary<string, string> KeyValuePairsFromTokens(this string input,
        string delimiter = " ", StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase.FromComparison(compare));
        if (string.IsNullOrEmpty(input)) return dict;
        var tokens = input.Split([delimiter], StringSplitOptions.None);
        for (var i = 0; i + 1 < tokens.Length; i += 2)
        {
            var key = tokens[i];
            var value = tokens[i + 1];
            dict[key] = value;
        }

        return dict;
    }

    public static List<List<string>> GroupTokens(this string input, int groupSize, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        var result = new List<List<string>>();
        if (string.IsNullOrEmpty(input) || groupSize < 1) return result;
        var tokens = input.Split([delimiter], StringSplitOptions.None);
        for (var i = 0; i < tokens.Length; i += groupSize)
        {
            var group = tokens.Skip(i).Take(groupSize).ToList();
            result.Add(group);
        }

        return result;
    }
}
