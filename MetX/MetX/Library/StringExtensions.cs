using System;
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
        public static string TokenBetween(this string target, string leftDelimiter, string rightDelimiter)
        {
            return string.IsNullOrEmpty(target)
                ? string.Empty
                : TokenAt(TokenAt(target, 2, leftDelimiter), 1, rightDelimiter);
        }

        public static string TokensAround(this string target, string leftDelimiter, string rightDelimiter)
        {
            if (string.IsNullOrEmpty(target)) return string.Empty;
            int leftIndex = target.IndexOf(leftDelimiter, StringComparison.OrdinalIgnoreCase);
            if(leftIndex > 0)
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
            if (string.IsNullOrEmpty(target) || length < 1) return string.Empty;
            if (target.Length <= length) return target;
            return target.Substring(0, length);
        }

        public static string Mid(this string target, int startAt, int length)
        {
            if (string.IsNullOrEmpty(target) || length < 1 || startAt > target.Length || startAt > target.Length) return string.Empty;
            if (startAt + length > target.Length) return target.Substring(startAt);
            return target.Substring(startAt, length);
        }

        public static string Mid(this string target, int startAt)
        {
            if (string.IsNullOrEmpty(target) || startAt < 1 || startAt > target.Length) return string.Empty;
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
                int currTokenLocation = Strings.InStr(target, delimiter, CompareMethod.Text) - 1; //  Character position of the first delimiter string
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
                currTokenLocation = Strings.InStr(target, delimiter, CompareMethod.Text) - 1;
                if (currTokenLocation > 1)
                {
                    return target.Substring(currTokenLocation + delimiterLength);
                }
                if (currTokenLocation == -1)
                {
                    return null;
                }
                return target.Substring(currTokenLocation + delimiterLength);
            }
            do
            {
                currTokenLocation = Strings.InStr(target, delimiter, CompareMethod.Text) - 1;
                if (currTokenLocation == -1)
                {
                    return null;
                }
                target = target.Substring(currTokenLocation + delimiterLength);
                tokenNumber -= 1;
            }
            while (tokenNumber > 1); //  Extract the Nth token (Which is the next token at this point)

            currTokenLocation = Strings.InStr(target, delimiter, CompareMethod.Text) - 1;

            if (currTokenLocation > 0)
            {
                return target.Substring(currTokenLocation + delimiterLength);
            }
            return null;
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
            do
            {
                int currTokenLocation = Strings.InStr(target, delimiter, CompareMethod.Text) - 1; //  Character position of the first delimiter string
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
                if (sReturned.Length == 0)
                {
                    sReturned.Append(target.Substring(0, currTokenLocation));
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
        public static string TokenAt(this string target, int tokenNumber, string delimiter)
        {
            int currTokenLocation;
            int delimiterLength = delimiter.Length;

            //  Negative or zeroth token or empty delimiter strings mean an empty token
            if (tokenNumber < 1 || delimiterLength < 1 || string.IsNullOrEmpty(target))
            {
                return string.Empty;
            }

            //  Quickly extract the first token
            if (tokenNumber == 1)
            {
                currTokenLocation = Strings.InStr(target, delimiter, CompareMethod.Text) - 1;
                if (currTokenLocation > 0)
                {
                    return target.Substring(0, currTokenLocation);
                }
                if (currTokenLocation == 0)
                {
                    return string.Empty;
                }
                return target;
            }
            //  Find the Nth token
            while (tokenNumber > 1)
            {
                currTokenLocation = Strings.InStr(target, delimiter, CompareMethod.Text) - 1;
                if (currTokenLocation == -1)
                {
                    return string.Empty;
                }
                target = target.Substring(currTokenLocation + delimiterLength);
                tokenNumber -= 1;
            }
            //  Extract the Nth token (Which is the next token at this point)
            currTokenLocation = Strings.InStr(target, delimiter, CompareMethod.Text) - 1;
            if (currTokenLocation > 0)
            {
                return target.Substring(0, currTokenLocation);
            }
            return target;
        }

        /// <summary>Returns the last delimited token from a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        public static string LastToken(this string target, string delimiter) { return TokenAt(target, TokenCount(target, delimiter), delimiter); }

        /// <summary>Returns everything before the last delimited token from a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        public static string TokensBeforeLast(this string target, string delimiter) { return TokensBefore(target, TokenCount(target, delimiter), delimiter); }

        /// <summary>Returns everything after the first delimited token from a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        public static string TokensAfterFirst(this string target, string delimiter) { return TokensAfter(target, 1, delimiter); }
    }
}