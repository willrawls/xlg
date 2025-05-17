using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MetX.Standard.Strings.Tokens.GPT;

public static class Advanced
{
    /*
        🧩 Advanced Parsing Logic (10–15 functions)
           NestedTokens(start, end, depth)
           EscapeDelimiter(delimiter), UnescapeDelimiter(delimiter)
           QuotedTokens(delimiter, quoteChar)
           SmartSplit(delimiter, escapeChar, quoteAware = true)
           MultiDelimiterSplit(params string[] delimiters)
     */

    /// <summary>
    /// Extracts substrings between matching nested start and end delimiters up to a specified nesting depth.
    /// Uses a stack to track delimiter positions without Span-based APIs.
    /// </summary>
    public static List<string> NestedTokens(this string input, string start = "(", string end = ")", int depth = 1,
        StringComparison compare = StringComparison.OrdinalIgnoreCase)
    {
        var results = new List<string>();
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end) || depth < 1)
            return results;

        var stack = new Stack<int>();
        int i = 0;
        int len = input.Length;
        int startLen = start.Length;
        int endLen = end.Length;

        while (i < len)
        {
            // Opening delimiter
            if (i + startLen <= len && string.Equals(input.Substring(i, startLen), start, compare))
            {
                stack.Push(i);
                i += startLen;
                continue;
            }

            // Closing delimiter
            if (i + endLen <= len && string.Equals(input.Substring(i, endLen), end, compare) && stack.Count > 0)
            {
                int openIndex = stack.Pop();
                if (stack.Count + 1 == depth)
                {
                    // capture from openIndex through this closing delimiter
                    results.Add(input.Substring(openIndex, i + endLen - openIndex));
                }

                i += endLen;
                continue;
            }

            i++;
        }

        return results;
    }

    /// <summary>
    /// Escapes a delimiter in the string by prefixing it with a backslash.
    /// </summary>
    public static string EscapeDelimiter(this string input, string delimiter = " ")
    {
        return input?.Replace(delimiter, $"\\{delimiter}");
    }

    /// <summary>
    /// Unescapes a previously escaped delimiter by removing the prefix backslash.
    /// </summary>
    public static string UnescapeDelimiter(this string input, string delimiter = " ")
    {
        return input?.Replace($"\\{delimiter}", delimiter);
    }

    /// <summary>
    /// Splits the string into tokens, respecting quoted sections.
    /// </summary>
    public static List<string> QuotedTokens(this string input, string delimiter = " ", char quoteChar = '"')
    {
        var result = new List<string>();
        if (string.IsNullOrEmpty(input)) return result;
        var pattern = $"{quoteChar}(.*?){quoteChar}|[^\\{delimiter}]+";
        foreach (Match match in Regex.Matches(input, pattern))
        {
            var token = match.Value.Trim(quoteChar);
            result.Add(token);
        }

        return result;
    }

    /// <summary>
    /// Splits a string by a delimiter with support for escaping and optional quote awareness.
    /// </summary>
    public static List<string> SmartSplit(this string input, string delimiter = " ", char escapeChar = '\\',
        bool quoteAware = true)
    {
        var result = new List<string>();
        if (string.IsNullOrEmpty(input)) return result;
        var current = new StringBuilder();
        var inQuotes = false;
        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (quoteAware && c == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (!inQuotes && c == delimiter[0] && (delimiter.Length == 1 || input.Substring(i).StartsWith(delimiter)))
            {
                result.Add(current.ToString());
                current.Clear();
                i += delimiter.Length - 1;
                continue;
            }

            if (c == escapeChar && i + 1 < input.Length)
            {
                i++;
                current.Append(input[i]);
                continue;
            }

            current.Append(c);
        }

        result.Add(current.ToString());
        return result;
    }

    /// <summary>
    /// Splits a string using multiple delimiters specified in an array.
    /// </summary>
    public static List<string> MultiDelimiterSplit(this string input, params string[] delimiters)
    {
        if (string.IsNullOrEmpty(input) || delimiters == null || delimiters.Length == 0)
            return new List<string> { input };
        return Regex.Split(input, string.Join("|", delimiters.Select(Regex.Escape))).ToList();
    }
}