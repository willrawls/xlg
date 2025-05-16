using System;
using System.Collections.Generic;

namespace MetX.Standard.Strings;

public static class TokenFunctionsByGptSearchMatch
{
    /*
           🧠 Search & Pattern Matching (10–20 functions)
               FindTokenMatching(predicate)
               AllTokensMatch(predicate)
               AnyTokenMatches(predicate)
               TokenStartsWith, TokenEndsWith, TokenContains
               TokensWithLength(n), TokensLongerThan(n)
               FindLongestToken, FindShortestToken
    */

    public static string FindTokenMatching(this string input, Func<string, bool> predicate, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter) || predicate == null) return null;
        var position = 0;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            if (predicate(token)) return token;
            if (next == input.Length) break;
            position = next + delimiter.Length;
        }

        return null;
    }

    public static bool AllTokensMatch(this string input, Func<string, bool> predicate, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter) || predicate == null) return false;
        var position = 0;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            if (!predicate(token)) return false;
            if (next == input.Length) break;
            position = next + delimiter.Length;
        }

        return true;
    }

    public static bool AnyTokenMatches(this string input, Func<string, bool> predicate, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter) || predicate == null) return false;
        var position = 0;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            if (predicate(token)) return true;
            if (next == input.Length) break;
            position = next + delimiter.Length;
        }

        return false;
    }

    public static bool TokenEndsWith(this string input, string suffix, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter) || suffix == null) return false;
        var position = 0;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            if (token.EndsWith(suffix, compare)) return true;
            if (next == input.Length) break;
            position = next + delimiter.Length;
        }

        return false;
    }

    public static bool TokenContains(this string input, string fragment, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter) || fragment == null) return false;
        var position = 0;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            if (token.IndexOf(fragment, compare) >= 0) return true;
            if (next == input.Length) break;
            position = next + delimiter.Length;
        }

        return false;
    }

    public static List<string> TokensWithLength(this string input, int length, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        var result = new List<string>();
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter)) return result;
        var position = 0;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            if (token.Length == length) result.Add(token);
            if (next == input.Length) break;
            position = next + delimiter.Length;
        }

        return result;
    }

    public static List<string> TokensLongerThan(this string input, int length, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        var result = new List<string>();
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter)) return result;
        var position = 0;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            if (token.Length > length) result.Add(token);
            if (next == input.Length) break;
            position = next + delimiter.Length;
        }

        return result;
    }

    public static string FindLongestToken(this string input, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        string longest = null;
        int maxLength = -1, position = 0;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            if (token.Length > maxLength)
            {
                longest = token;
                maxLength = token.Length;
            }

            if (next == input.Length) break;
            position = next + delimiter.Length;
        }

        return longest;
    }

    public static string FindShortestToken(this string input, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        string shortest = null;
        int minLength = int.MaxValue, position = 0;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            if (token.Length < minLength)
            {
                shortest = token;
                minLength = token.Length;
            }

            if (next == input.Length) break;
            position = next + delimiter.Length;
        }

        return shortest;
    }
}