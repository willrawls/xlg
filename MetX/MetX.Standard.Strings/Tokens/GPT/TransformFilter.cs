using System;
using System.Collections.Generic;
using System.Text;

namespace MetX.Standard.Strings.Tokens.GPT;

public static class TransformFilter
{
    /*
        🔧 Transformations & Filters (15–30 functions)
           MapTokens(Func<string, string>)
           FilterTokens(predicate)
           DistinctTokens(), SortedTokens()
           SanitizeTokens(), PadTokens(), TruncateTokens(n)
           SurroundTokens(prefix, suffix)

    */

    public static string MapTokens(this string input, Func<string, string> transformer, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        if (string.IsNullOrEmpty(input) || transformer == null) return input;
        var builder = new StringBuilder();
        var position = 0;
        var first = true;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            if (!first) builder.Append(delimiter);
            builder.Append(transformer(token));
            if (next == input.Length) break;
            position = next + delimiter.Length;
            first = false;
        }

        return builder.ToString();
    }

    public static string FilterTokens(this string input, Func<string, bool> predicate, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        if (string.IsNullOrEmpty(input) || predicate == null) return input;
        var builder = new StringBuilder();
        var position = 0;
        var first = true;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            if (predicate(token))
            {
                if (!first) builder.Append(delimiter);
                builder.Append(token);
                first = false;
            }

            if (next == input.Length) break;
            position = next + delimiter.Length;
        }

        return builder.ToString();
    }

    public static string DistinctTokens(this string input, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var comparer = StringComparer.OrdinalIgnoreCase.FromComparison(compare);
        var seen = new HashSet<string>(comparer);
        var builder = new StringBuilder();
        var position = 0;
        var first = true;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            if (seen.Add(token))
            {
                if (!first) builder.Append(delimiter);
                builder.Append(token);
                first = false;
            }

            if (next == input.Length) break;
            position = next + delimiter.Length;
        }

        return builder.ToString();
    }

    public static string SortedTokens(this string input, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var tokens = new List<string>();
        var position = 0;
        while (position <= input.Length)
        {
            var next = input.IndexOf(delimiter, position, compare);
            if (next == -1) next = input.Length;
            var token = input.Substring(position, next - position);
            tokens.Add(token);
            if (next == input.Length) break;
            position = next + delimiter.Length;
        }

        tokens.Sort(StringComparer.OrdinalIgnoreCase.FromComparison(compare));
        return string.Join(delimiter, tokens);
    }

    public static string SanitizeTokens(this string input, Func<string, string> predicate, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        return input.MapTokens(predicate, delimiter, compare);
    }

    public static string SanitizeTokens(this string input, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        if (string.IsNullOrEmpty(input)) return "";
        if (string.IsNullOrEmpty(delimiter)) return input;

        var tokens = input.AllTokens(delimiter, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(delimiter, tokens);
    }

    public static string PadTokens(this string input, int totalWidth, char paddingChar = ' ', string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        return input.MapTokens(token => token.PadRight(totalWidth, paddingChar), delimiter, compare);
    }

    public static string TruncateTokens(this string input, int maxLength, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        return input.MapTokens(token => token.Length > maxLength ? token.Substring(0, maxLength) : token, delimiter,
            compare);
    }

    public static string SurroundTokens(this string input, string prefix, string suffix, string delimiter = " ",
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        return input.MapTokens(token => prefix + token + suffix, delimiter, compare);
    }
}