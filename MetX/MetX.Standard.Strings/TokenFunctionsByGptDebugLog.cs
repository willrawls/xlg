using System;
using System.Collections.Generic;
using System.Linq;

namespace MetX.Standard.Strings;

public static class TokenFunctionsByGptDebugLog
{
    /*
        🧪 Debugging / Logging (3–5 functions)
           PrintTokens(numbered: true)
           VisualizeTokenLengths()
           SummaryOfTokens()
    */


    /// <summary>
    /// Prints each token to the console, optionally with line numbers.
    /// </summary>
    public static void PrintTokens(this string input, string delimiter = " ", bool numbered = true)
    {
        if (string.IsNullOrEmpty(input)) return;
        var tokens = input.Split([delimiter], StringSplitOptions.None);
        for (var i = 0; i < tokens.Length; i++)
        {
            Console.WriteLine(numbered ? $"{i + 1}: {tokens[i]}" : tokens[i]);
        }
    }

    /// <summary>
    /// Prints each token to the console, optionally with line numbers.
    /// </summary>
    public static void PrintTokens(this List<string> tokens, bool numbered = true)
    {
        if (tokens.IsEmpty())
        {
            Console.WriteLine("No tokens to print.");
            return;
        }

        for (var i = 0; i < tokens.Count; i++)
        {
            Console.WriteLine(numbered ? $"{i + 1}: {tokens[i]}" : tokens[i]);
        }
    }

    /// <summary>
    /// Outputs a visual representation of each token's length using bars.
    /// </summary>
    public static void VisualizeTokenLengths(this string input, string delimiter = " ")
    {
        if (string.IsNullOrEmpty(input)) return;
        var tokens = input.Split([delimiter], StringSplitOptions.None);
        for (var i = 0; i < tokens.Length; i++)
        {
            Console.WriteLine($"{i + 1}: {tokens[i]} | {new string('=', tokens[i].Length)} ({tokens[i].Length})");
        }
    }

    /// <summary>
    /// Prints a summary including count, shortest and longest token lengths.
    /// </summary>
    public static void SummaryOfTokens(this string input, string delimiter = " ")
    {
        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("No input.");
            return;
        }

        var tokens = input.Split([delimiter], StringSplitOptions.None);
        var count = tokens.Length;
        var shortest = tokens.OrderBy(t => t.Length).FirstOrDefault();
        var longest = tokens.OrderByDescending(t => t.Length).FirstOrDefault();
        var avgLength = tokens.Any() ? tokens.Average(t => t.Length) : 0;

        Console.WriteLine($"Token Count: {count}");
        Console.WriteLine($"Shortest Token: '{shortest}' ({shortest?.Length})");
        Console.WriteLine($"Longest Token: '{longest}' ({longest?.Length})");
        Console.WriteLine($"Average Length: {avgLength:F2}");
    }
}