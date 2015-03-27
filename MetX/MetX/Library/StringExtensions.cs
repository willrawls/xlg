using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace MetX.Library
{
    /// <summary>Provides simple methods for retrieving tokens from a string.
    /// <para>A token is a piece of a delimited string. For instance in the string "this is a test" when " " (a space) is used as a delimiter, "this" is the first token and "test" is the last (4th) token.</para>
    /// <para>Asking for a token beyond the end of a string returns a blank string. Asking for the zeroth or a negative token returns a blank string.</para>
    /// </summary>
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty<T>(this IList<T> target)
        {
            return (target == null || target.Count == 0);
        }

        public static bool IsNullOrEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }

        public static List<string> AsList(this string[] target)
        {
            return new List<string>(target);
        }

        public static string[] Tokens(this string target, string delimiter = " ", StringSplitOptions options = StringSplitOptions.None)
        {
            if (string.IsNullOrEmpty(target)) return new[] {string.Empty};
            return target.Split(new[] {delimiter}, options);
        }

        public static string[] Slice(this string target) { return target.Lines(StringSplitOptions.RemoveEmptyEntries); }
        public static string[] Dice(this string target) { return target.Tokens(" ", StringSplitOptions.RemoveEmptyEntries); }

        public static List<string> LineList(this string target, StringSplitOptions options = StringSplitOptions.None)
        {
            return target.Lines(options).AsList();
        }

        public static string[] Lines(this string target, StringSplitOptions options = StringSplitOptions.None)
        {
            if (string.IsNullOrEmpty(target)) return new[] { string.Empty };
            return target.Split(new[] { Environment.NewLine }, options);
        }

        public static string Flatten<T>(this IList<T> target)
        {
            if (target.IsNullOrEmpty()) return string.Empty;
            return string.Join(Environment.NewLine, target);
        }

        public static string AsFilename(this string target)
        {
            if (target.IsNullOrEmpty()) return string.Empty;
            return target.Replace(new[] {":", @"\", "/", "*", "\"", "?", "<", ">", "|", " ", "-"}, string.Empty);
        }

        public static string TokenBetween(this string target, string leftDelimiter, string rightDelimiter)
        {
            return String.IsNullOrEmpty(target)
                ? String.Empty
                : TokenAt(TokenAt(target, 2, leftDelimiter), 1, rightDelimiter);
        }

        public static string TokensAround(this string target, string leftDelimiter, string rightDelimiter)
        {
            if (String.IsNullOrEmpty(target)) return String.Empty;
            int leftIndex = target.IndexOf(leftDelimiter, StringComparison.OrdinalIgnoreCase);
            if (leftIndex > 0)
            {
                int rightIndex = target.IndexOf(rightDelimiter, leftIndex + leftDelimiter.Length, StringComparison.OrdinalIgnoreCase);
                if (rightIndex > leftIndex)
                {
                    return target.Left(leftIndex) + target.Substring(rightIndex + rightDelimiter.Length);
                }
                return target.Mid(leftIndex + 1 - leftDelimiter.Length);
            }
            return TokensBefore(target, 2, leftDelimiter) + TokensAfter(target, 1, rightDelimiter);
        }

        public static string Left(this string target, int length)
        {
            if (String.IsNullOrEmpty(target) || length < 1) return String.Empty;
            if (target.Length <= length) return target;
            return target.Substring(0, length);
        }

        public static string Mid(this string target, int startAt, int length)
        {
            if (String.IsNullOrEmpty(target) || length < 1 || startAt > target.Length || startAt > target.Length) return String.Empty;
            if (startAt + length > target.Length) return target.Substring(startAt);
            return target.Substring(startAt, length);
        }

        public static string Mid(this string target, int startAt)
        {
            if (String.IsNullOrEmpty(target) || startAt < 1 || startAt > target.Length) return String.Empty;
            return target.Substring(startAt);
        }

        /// <summary>Returns the number of tokens in a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        ///
        /// <example>
        /// <code>
        /// int x = Token.Count("this is a test", "is");
        /// // x = 2;
        /// x = Token.Count("this is a test", " ");
        /// // x = 4;
        /// </code>
        /// </example>
        public static int TokenCount(this string target, string delimiter)
        {
            int delimiterLength = delimiter.Length;

            //  Empty delimiter strings means only one token equal to the string
            if (delimiterLength < 1)
            {
                return 1;
            }
            //  Empty input string means no tokens
            if (target.Length == 0)
            {
                return 0;
            }

            int tokensSoFar = 0;
            do
            {
                int currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase); //  Character position of the first delimiter string
                if (currTokenLocation == -1)
                {
                    return ++tokensSoFar; // Abs(Len(Target) > 0)
                }
                tokensSoFar += 1;
                target = target.Substring(currTokenLocation + delimiterLength);
            }
            while (true);
        }

        /// <summary>Returns the number of " " (space) delimited tokens in a string. IE: returns the number of words in a string.</summary>
        /// <param name="target">The string to target</param>
        /// <returns>The number of tokens (words) in the string</returns>
        public static int TokenCount(this string target) { return TokenCount(target, " "); }

        /// <summary>Returns all tokens after the indicated token.</summary>
        /// <param name="target">The string to target</param>
        /// <param name="tokenNumber">The token number to return after</param>
        /// <param name="delimiter">The token delimiter</param>
        ///
        /// <example>
        /// <code>
        /// string x = Token.After("this is a test", 2, " ");
        /// // x = "a test"
        /// </code>
        /// </example>
        public static string TokensAfter(this string target, int tokenNumber, string delimiter)
        {
            int currTokenLocation; //  Character position of the first delimiter string
            int delimiterLength = delimiter.Length;
            if (tokenNumber < 1 || delimiterLength < 1) //  Negative or zeroth token or empty delimiter strings mean an empty token
            {
                return target;
            }
            if (tokenNumber == 1) //  Quickly extract the first token
            {
                currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
                if (currTokenLocation > 1)
                {
                    return target.Substring(currTokenLocation + delimiterLength);
                }
                if (currTokenLocation == -1)
                {
                    return String.Empty;
                }
                return target.Substring(currTokenLocation + delimiterLength);
            }
            do
            {
                currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
                if (currTokenLocation == -1)
                {
                    return String.Empty;
                }
                target = target.Substring(currTokenLocation + delimiterLength);
                tokenNumber -= 1;
            }
            while (tokenNumber > 1); //  Extract the Nth token (Which is the next token at this point)

            currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);

            if (currTokenLocation > 0)
            {
                return target.Substring(currTokenLocation + delimiterLength);
            }
            return String.Empty;
        }

        /// <summary>Returns all tokens after the indicated " " (space) delimited token</summary>
        /// <param name="target">The string to target</param>
        /// <param name="tokenNumber">The token number to return after</param>
        ///
        /// <example>
        /// <code>
        /// string x = Token.After("this is a test", 2);
        /// // x = "a test"
        /// </code>
        /// </example>
        public static string TokensAfter(this string target, int tokenNumber) { return TokensAfter(target, tokenNumber, " "); }

        /// <summary>Returns all tokens after the first " " (space) delimited token</summary>
        /// <param name="target">The string to target</param>
        /// <returns>All tokens after the first</returns>
        ///
        /// <example>
        /// <code>
        /// string x = Token.After("this is a test");
        /// // x = "is a test"
        /// </code>
        /// </example>
        public static string TokensAfter(this string target) { return TokensAfter(target, 1, " "); }

        /// <summary>Returns all tokens before the indicated token.</summary>
        /// <param name="target">The string to target</param>
        /// <param name="tokenNumber">The token number to return before</param>
        /// <param name="delimiter">The token delimiter</param>
        ///
        /// <example>
        /// <code>
        /// string x = Token.Before("this is a test", 3, " ");
        /// // x = "this is"
        /// </code>
        /// </example>
        public static string TokensBefore(this string target, long tokenNumber, string delimiter)
        {
            int delimiterLength = delimiter.Length; //  Length of the delimiter string
            if (tokenNumber < 2 || delimiterLength < 1) //  First, Zeroth, or Negative tokens or empty delimiter strings mean an empty string returned
            {
                return null;
            }
            if (tokenNumber == 2)
            {
                return TokenAt(target, 1, delimiter); //  Quickly extract the first token
            }
            //  Find the Nth token
            StringBuilder sReturned = new StringBuilder();
            bool first = true;
            do
            {
                int currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase); //  Character position of the first delimiter string
                if (currTokenLocation == -1 || tokenNumber == 1)
                {
                    if (tokenNumber > 1)
                    {
                        if (target.Length > 0)
                        {
                            sReturned.Append(delimiter);
                            sReturned.Append(target);
                        }
                    }
                    return sReturned.ToString();
                }
                if (sReturned.Length == 0 && first)
                {
                    sReturned.Append(target.Substring(0, currTokenLocation));
                    first = false;
                }
                else
                {
                    sReturned.Append(delimiter);
                    sReturned.Append(target.Substring(0, currTokenLocation));
                }
                target = target.Substring(currTokenLocation + delimiterLength);
                tokenNumber -= 1;
            }
            while (true);
        }

        /// <summary>Returns all tokens before the indicated " " (space) delimited token.</summary>
        /// <param name="target">The string to target</param>
        /// <param name="tokenNumber">The token number to return before</param>
        ///
        /// <example>
        /// <code>
        /// string x = Token.Before("this is a test", 3);
        /// // x = "this is"
        /// </code>
        /// </example>
        public static string TokensBefore(this string target, long tokenNumber) { return TokensBefore(target, tokenNumber, " "); }

        /// <summary>Returns the first delimited token in the indicated string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        ///
        /// <example>
        /// <code>
        /// string x = Token.First("this is a test", " a ");
        /// // x = "this is"
        /// </code>
        /// </example>
        public static string FirstToken(this string target, string delimiter = " ") { return TokenAt(target, 1, delimiter); }

        /// <summary>Returns a single delimited token from a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="tokenNumber">The token to return</param>
        /// <param name="delimiter">The token delimiter</param>
        public static string TokenAt(this string target, int tokenNumber, string delimiter = " ")
        {
            int currTokenLocation;
            int delimiterLength = delimiter.Length;

            //  Negative or zeroth token or empty delimiter strings mean an empty token
            if (tokenNumber < 1 || delimiterLength < 1 || String.IsNullOrEmpty(target))
            {
                return String.Empty;
            }

            //  Quickly extract the first token
            if (tokenNumber == 1)
            {
                currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
                if (currTokenLocation > 0)
                {
                    return target.Substring(0, currTokenLocation);
                }
                if (currTokenLocation == 0)
                {
                    return String.Empty;
                }
                return target;
            }
            //  Find the Nth token
            while (tokenNumber > 1)
            {
                currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
                //currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
                if (currTokenLocation == -1)
                {
                    return String.Empty;
                }
                target = target.Substring(currTokenLocation + delimiterLength);
                tokenNumber -= 1;
            }
            //  Extract the Nth token (Which is the next token at this point)
            currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
            if (currTokenLocation > 0)
            {
                return target.Substring(0, currTokenLocation);
            }
            return target; // string.Empty;
        }

        /// <summary>Returns the last delimited token from a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        public static string LastToken(this string target, string delimiter = " ") { return TokenAt(target, TokenCount(target, delimiter), delimiter); }

        public static string LastPathToken(this string target, string delimiter = @"\") { return TokenAt(target, TokenCount(target, delimiter), delimiter); }

        /// <summary>Returns everything before the last delimited token from a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        public static string TokensBeforeLast(this string target, string delimiter = " ") { return TokensBefore(target, TokenCount(target, delimiter), delimiter); }

        /// <summary>Returns everything after the first delimited token from a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        public static string TokensAfterFirst(this string target, string delimiter = " ") { return TokensAfter(target, 1, delimiter); }

        public static string Replace(this string target, string[] strings, string replacement)
        {
            if (String.IsNullOrEmpty(target)) return String.Empty;
            if (strings == null || strings.Length == 0) return target;
            return strings.Aggregate(target, (current, s) => current.Replace(s, replacement));
        }

        public static void TransformAllNotEmpty(this IList<string> target, Func<string, int, string> func)
        {
            if (target.IsNullOrEmpty()) return;

            for (int i = 0; i < target.Count; i++)
            {
                string s = target[i];
                if (s.IsNullOrEmpty()) continue;
                target[i] = func(s, i); 
            }
        }

        public static void TransformAllNotEmpty(this IList<string> target, Func<string, string> func)
        {
            if (target.IsNullOrEmpty()) return;
            for (int i = 0; i < target.Count; i++)
            {
                string s = target[i];
                if (s.IsNullOrEmpty()) continue;
                target[i] = func(s);
            }
        }
    }
}