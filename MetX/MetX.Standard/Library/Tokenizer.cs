using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable UnusedMember.Global

namespace MetX.Standard.Library
{
    /// <summary>
    ///     String extension methods for finding and returning a substring based on delimiter placement and position.
    /// </summary>
    public static class Tokenizer
    {
        /// <summary>
        ///     Conveniently wraps string.Split for returning a string arr
        /// </summary>
        /// <param name="target">The string to parse</param>
        /// <param name="delimiter">
        ///     The string that separates each token. For instance, In the string "Fred went home", a space ("
        ///     ") would be a common delimiter.
        /// </param>
        /// <param name="compare">See <see cref="System.StringSplitOptions" /></param>
        /// <returns>
        ///     NOTE: Never returns null. Each delimited token returned in a string array, with empty entries optionally
        ///     removed.
        /// </returns>
        public static List<string> AllTokens(this string target, string delimiter = " ", StringSplitOptions compare = StringSplitOptions.None)
        {

            var result = new List<string>();
            if (string.IsNullOrEmpty(target))
                result.Add(string.Empty);
            else
                result.AddRange(target.Split(new[] { delimiter }, compare));
            return result;
        }

        /// <summary>Returns the first delimited token in the indicated string</summary>
        /// <param name="target">The string to parse</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <param name="compare"></param>
        /// <example>
        ///     <code>
        ///  string x = FirstToken("this is a test", " a ");
        ///  // x = "this is"
        ///  </code>
        /// </example>
        public static string FirstToken(this string target, string delimiter = " ", StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            return TokenAt(target, 1, delimiter, compare);
        }

        /// <summary>
        ///     Returns everything after the last backslash (\)
        /// </summary>
        /// <param name="target">The string to parse</param>
        /// <returns></returns>
        public static string LastPathToken(this string target)
        {
            return TokenAt(target, TokenCount(target, @"\"), @"\");
        }

        /// <summary>Returns the last delimited token from a string</summary>
        /// <param name="target">The string to parse</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <param name="compare"></param>
        public static string LastToken(this string target, string delimiter = " ", StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            var tokenCount = TokenCount(target, delimiter, compare);
            return TokenAt(target, tokenCount, delimiter, compare);
        }

        /// <summary>
        ///     Scans 'target' and returns everything before the Nth (token) occurrence of 'delimiter' and after the Nth - 1
        ///     occurrence of 'delimiter'.<br />
        /// </summary>
        /// <para>
        ///     By definition, the following is true. When the target string is null or empty, TokenAt returns an empty string.
        ///     <br />
        ///     When the delimiter string is null or empty, TokenAt returns the target string.<br />
        ///     When the delimiter does not appear in the target string, TokenAt returns the target string.<br />
        ///     Otherwise, TokenAt looks for the Nth occurrence of the delimiter string (where N = token) in the target
        ///     string.<br />
        ///     It the returns everything between the Nth-1 and Nth occurrence of delimiter. If N = 1, then TokenAt just returns
        ///     everything before the first occurrence.<br />
        ///     If there is no Nth occurrence, TokenAt returns an empty string.<br />
        ///     This function will never return null and throws no exceptions.<br />
        ///     NOTE: Whenever possible, this function ignores case.
        /// </para>
        /// <code>
        /// string sample = "Fred goes home.";
        /// string delimiter = " "; // A single space
        /// string first = sample.TokenAt(1, delimiter);     // first set to "Fred"
        /// string second = sample.TokenAt(2, delimiter);    // second set to "goes"
        /// string third = sample.TokenAt(3, delimiter);     // third set to "home."
        /// string fourth = sample.TokenAt(4, delimiter);    // fourth set to ""
        /// string tenth = sample.TokenAt(10, delimiter);    // tenth set to ""
        /// string negative = sample.TokenAt(-2, delimiter); // negative set to ""
        ///
        /// sample = "Fred goes home. Fred reads his mail.";
        /// delimiter = "."; // A single space
        /// first = sample.TokenAt(1, delimiter);     // first set to "Fred goes home"
        /// second = sample.TokenAt(2, delimiter);    // second set to " Fred reads his mail"
        /// third = sample.TokenAt(3, delimiter);     // third set to ""
        /// </code>
        /// <param name="target">The string to parse</param>
        /// <param name="token">
        ///     The token to return. For instance, with a delimiter of space (" ") and a string "Fred went
        ///     home.", token could be 1, 2 or 3.
        /// </param>
        /// <param name="delimiter">The string inside 'target' which separators tokens. Defaults to a space (" ").</param>
        /// <param name="compare"> Specifies the culture, case, and sort rules to be used. Defaults to OrdinalIgnoreCase (case insensitive)</param>
        /// <returns>Returns the substring from 'target' before the Nth (token) occurrence of 'delimiter'.</returns>
        public static string TokenAt(this string target, int token, string delimiter = " ", StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            //  Empty delimiter string means an return the target or blank if target is null
            if (string.IsNullOrEmpty(delimiter))
                return target ?? string.Empty;
            //  Any token number before one or an empty target string means an empty token
            if (token < 1 || string.IsNullOrEmpty(target))
                return string.Empty;
            var index = 0;
            if (token == 1)
            {
                //  Quickly extract the first token
                index = target.IndexOf(delimiter, compare);
                if (index > 0)
                    return target.Substring(0, index);
                return index != 0
                    ? target
                    : string.Empty;
            }
            //  Find the Nth token
            for (var n = 1; n < token; n++)
            {
                index = target.IndexOf(delimiter, index, compare);
                // Check if we ran out before finding the right one
                if (index == -1)
                    return string.Empty;
                index += delimiter.Length;
            }
            //  Extract the token
            var finalIndex = target.IndexOf(delimiter, index, compare);
            return finalIndex > 0
                ? target.Substring(index, finalIndex - index)
                : target.Substring(index);
        }

        /// <summary>
        ///     Returns everything after the first occurrence of leftDelimiter and before the following occurrence of rightDelimiter
        /// </summary>
        /// <param name="target">The string to parse</param>
        /// <param name="leftDelimiter">
        ///     The first string that separates each token. For instance, In the string "(123) 456-7890",
        ///     open parenthesis("(") would be a common left delimiter.
        /// </param>
        /// <param name="rightDelimiter">
        ///     The first string that separates each token. For instance, In the string "(123) 456-7890",
        ///     close parenthesis(")") would be a common right delimiter.
        /// </param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static string TokenBetween(this string target, 
            string leftDelimiter, 
            string rightDelimiter, 
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(target))
                return string.Empty;
            var leftPart = TokenAt(target, 2, leftDelimiter, compare);
            return TokenAt(leftPart, 1, rightDelimiter, compare);
        }

        public static IEnumerable<string> Splice(this string target, string leftDelimiter, string rightDelimiter)
        {
            if (target.IsEmpty())
            {
                yield return "";
                yield break;
            }

            if (leftDelimiter.IsEmpty() || rightDelimiter.IsEmpty())
            {
                yield return "";
                yield break;
            }

            if (target == leftDelimiter || target == rightDelimiter || target == leftDelimiter + rightDelimiter)
            {
                yield return "";
                yield break;
            }

            var leftIndexes = target.TokenIndexes(leftDelimiter).ToArray();
            var rightIndexes = target.TokenIndexes(rightDelimiter).ToArray();

            if (leftIndexes.Length == 1)
            {
                if(leftIndexes[0] != 0)
                {
                    yield return target.Substring(0, leftIndexes[0]);
                }
                var firstLeft = leftIndexes[0] + leftDelimiter.Length;
                var firstLength = rightIndexes[0] - firstLeft;
                if(firstLength > 0)
                {
                    yield return target.Substring(firstLeft, firstLength);
                }
                var secondLeft = rightIndexes[0] + rightDelimiter.Length;
                yield return target.Substring(secondLeft);
                yield break;
            }
            
            var currentLocation = 0;
            
            for (int i = 0, j = 0; i < leftIndexes.Length && j < rightIndexes.Length; i++, j++)
            {
                if (currentLocation >= target.Length) break;

                if (leftIndexes[i] == 0 && currentLocation == 0) // Special case
                {
                    if (rightIndexes[i] == target.Length) // Special case
                    {
                        yield return "";
                        yield break;
                    }
                }
                
                var length = leftIndexes[i] - currentLocation;
                if(length > 0)
                {
                    yield return target.Substring(currentLocation, length);
                    currentLocation = leftIndexes[i] + leftDelimiter.Length;
                }                
                if (currentLocation >= target.Length) break;
                
                length = rightIndexes[j] - currentLocation;
                if(length > 0)
                {
                    yield return target.Substring(currentLocation, rightIndexes[j] - currentLocation);
                    currentLocation = rightIndexes[j] + rightDelimiter.Length;
                }   
                if (currentLocation >= target.Length) break;
            }
            
            if(currentLocation < target.Length)
                yield return target.Substring(currentLocation);
        }
        
        public static string UpdateBetweenTokens(this string target, string leftDelimiter, string rightDelimiter,
            bool consumeDelimiters, Func<string, string> tokenProcessor)
        {
            if (target.IsEmpty())
                return tokenProcessor(string.Empty);

            var result = new StringBuilder();

            var majorPieces = target.Split(new string[]{ leftDelimiter }, StringSplitOptions.None);
            result.Append(majorPieces[0]);
            foreach (var majorPiece in majorPieces.Skip(1))
            {
                if (!consumeDelimiters)
                {
                    result.Append(leftDelimiter);
                }
                var minorPieces = majorPiece.Split(new string[]{ rightDelimiter }, StringSplitOptions.None);
                result.Append(tokenProcessor(minorPieces[0]));
                if (!consumeDelimiters)
                {
                    result.Append(rightDelimiter);
                }

                if (minorPieces.Length > 1)
                {
                    result.Append(minorPieces[1]);
                }
            }
            
            return result.ToString();
        }
        
        /*
        public static string UpdateBetweenTokens(this string target, string leftDelimiter, string rightDelimiter,
            bool consumeDelimiters, Func<string, string> tokenProcessor)
        {
            if (target.IsEmpty())
                return string.Empty;

            var result = new StringBuilder();
            var odd = false;
            var pieces = target
                .Splice(leftDelimiter, rightDelimiter)
                .Where( s => s.Length > 0)
                .ToList();
            foreach (var piece in pieces)
            {
                if (!consumeDelimiters)
                {
                    result.Append(leftDelimiter);
                }
                if (odd) // Token between the current leftDelimiter and rightDelimiter
                {
                    if(piece.IsNotEmpty())
                        result.Append(tokenProcessor(piece));
                }
                else if(piece.IsNotEmpty())
                {
                    result.Append(piece);
                }
                if (!consumeDelimiters)
                {
                    result.Append(rightDelimiter);
                }

                odd = !odd;
            }
            
            return result.ToString();
        }
        */

        /// <summary>Returns the number of tokens in a string</summary>
        /// <param name="target">The string to parse</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <param name="compare"> Specifies the culture, case, and sort rules to be used. Defaults to OrdinalIgnoreCase (case insensitive)</param>
        /// <example>
        ///     <code>
        ///  int x = TokenCount("this is a test", "is");
        ///  // x = 2;
        ///  x = TokenCount("this is a test", " ");
        ///  // x = 4;
        ///  </code>
        /// </example>
        public static int TokenCount(this string target, string delimiter = " ", StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            //  Empty target means no tokens
            if (string.IsNullOrEmpty(target))
                return 0;

            //  Empty delimiter means only one token
            if (string.IsNullOrEmpty(delimiter))
                return 1;

            var found = 1;
            var index = target.IndexOf(delimiter, compare);
            while (index < target.Length)
            {
                if (index == -1)
                    return found;
                index = target.IndexOf(delimiter, index + delimiter.Length, compare);
                found++;
            }
            return found;
        }

        /// <summary>Returns the index into target of the first of the Nth token</summary>
        /// <param name="target">The string to parse</param>
        /// <param name="token">Which token to find the starting index for (N)</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <param name="compare"> Specifies the culture, case, and sort rules to be used. Defaults to OrdinalIngoreCase (case insensitive)</param>
        /// <example>
        ///     <code>
        ///  int x = TokenIndex("this is a test", 5, " ");
        ///  // x = 5;
        ///  </code>
        /// </example>
        public static int TokenIndex(this string target, int token, string delimiter = " ", StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            //  Empty target means no index
            if (string.IsNullOrEmpty(target))
                return -1;

            //  Empty delimiter means no index
            if (string.IsNullOrEmpty(delimiter))
                return -1;

            // The first token is always the first character
            // "Negative" tokens are also the first character
            if (token <= 1)
                return 0;

            var index = target.IndexOf(delimiter, compare);
            if (index == -1)
                return -1;

            var found = 1;
            while (index < target.Length && ++found < token)
            {
                // Tokens past a string are the last character of the string
                if (index == -1)
                    return target.Length;
                index = target.IndexOf(delimiter, index + delimiter.Length, compare);
            }

            if (index > -1 && index < target.Length && found <= token)
                return index + delimiter.Length;
            // Tokens past a string are the last character of the string
            return target.Length;
        }

        public static IEnumerable<int> TokenIndexes(this string target, string delimiter = " ", StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            //  Empty target means no index
            if (string.IsNullOrEmpty(target))
                yield break;

            //  Empty delimiter means no index
            if (string.IsNullOrEmpty(delimiter))
                yield break;

            var index = target.IndexOf(delimiter, compare);
            if (index == -1)
                yield break;

            yield return index;
            
            while (index < target.Length)
            {
                index = target.IndexOf(delimiter, index + delimiter.Length, compare);
                if (index < 0)
                    yield break;
                yield return index;
            }

            if (index > -1 && index < target.Length)
                yield return index + delimiter.Length;

            yield break;
        }

        /// <summary>Returns all tokens after the indicated token.</summary>
        /// <param name="target">The string to parse</param>
        /// <param name="token">The token number to return after</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <param name="compare"></param>
        /// <example>
        ///     <code>
        ///  string x = .After("this is a test", 2, " ");
        ///  // x = "a test"
        ///  </code>
        /// </example>
        public static string TokensAfter(this string target, int token = 1, string delimiter = " ", StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            int index;
            var delimiterLength = delimiter.Length;

            //  Negative or zeroth token or empty delimiter strings mean an empty token
            if (token < 1 || delimiterLength < 1)
                return target;

            if (token == 1) //  Quickly extract the first token
            {
                index = target.IndexOf(delimiter, compare);
                return index != -1
                    ? target.Substring(index + delimiterLength)
                    : string.Empty;
            }

            do
            {
                index = target.IndexOf(delimiter, compare);
                if (index == -1)
                    return string.Empty;
                target = target.Substring(index + delimiterLength);
                token--;
            } while (token > 1);

            index = target.IndexOf(delimiter, compare);
            return index > 0
                ? target.Substring(index + delimiterLength)
                : string.Empty;
        }

        /// <summary>Returns everything after the first delimited token from a string</summary>
        /// <param name="target">The string to parse</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <param name="compare"></param>
        public static string TokensAfterFirst(this string target, 
            string delimiter = " ", 
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            return TokensAfter(target, 1, delimiter, compare);
        }

        /// <summary>
        /// </summary>
        /// <param name="target">The string to parse</param>
        /// <param name="leftDelimiter"></param>
        /// <param name="rightDelimiter"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static string TokensAround(this string target, string leftDelimiter, string rightDelimiter, StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(leftDelimiter) || string.IsNullOrEmpty(rightDelimiter))
                return string.Empty;

            var leftIndex = target.IndexOf(leftDelimiter, compare);
            if (leftIndex <= 0)
                return TokensBefore(target, 2, leftDelimiter) + TokensAfter(target, 1, rightDelimiter);

            var rightIndex = target.IndexOf(rightDelimiter, leftIndex + leftDelimiter.Length, compare);
            if (rightIndex > leftIndex)
                return target.Left(leftIndex) + target.Substring(rightIndex + rightDelimiter.Length);
            return target.Mid(leftIndex + 1 - leftDelimiter.Length);
        }

        /// <summary>Returns all tokens before the indicated token.</summary>
        /// <param name="target">The string to parse</param>
        /// <param name="token">The token number to return before</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <param name="compare"></param>
        /// <example>
        ///     <code>
        ///  string x = TokensBefore("this is a test", 3, " ");
        ///  // x = "this is"
        ///  </code>
        /// </example>
        public static string TokensBefore(this string target, int token, string delimiter = " ", StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            //  First, Zeroth, or Negative tokens or empty delimiter strings mean an empty string returned
            if (token < 2 || delimiter.Length < 1)
                return string.Empty;

            if (token == 2)
                return TokenAt(target, 1, delimiter);

            var index = target.TokenIndex(token, delimiter, compare);
            if (index >= target.Length)
                return target;
            if (index > 0)
                return target.Substring(0, index - delimiter.Length);
            return string.Empty;
        }

        /// <summary>Returns all tokens before the last token.</summary>
        /// <param name="target">The string to parse</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <param name="compare"></param>
        /// <example>
        ///     <code>
        ///  string x = TokensBeforeLast("this is a test", 3, " ");
        ///  // x = "this is"
        ///  </code>
        /// </example>
        public static string TokensBeforeLast(this string target, string delimiter = " ", StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            var tokenCount = target.TokenCount(delimiter, compare);
            return target.TokensBefore(tokenCount, delimiter, compare);
        }

        /// <summary>
        /// Splits the string first into a string array delimited by 'left'.
        /// Then splits each of those by 'right'.
        /// Returns a single IEnumerable of the pieces, with out the left and right delimiters
        /// So 'x[[y]]z' becomes 'x', 'y', 'z'
        /// </summary>
        /// <param name="target"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IEnumerable<string> Splice(this string target, string left, string right, StringSplitOptions options = StringSplitOptions.None)
        {
            if (target.IsEmpty())
                yield return null;

            var slices = target.Split(new []{ left }, options);
            foreach(var slice in slices)
            {
                if(!slice.ToLower().Contains(right.ToLower()))
                {
                    yield return slice;
                }
                else
                {
                    var dices = slice.Split(new []{ right }, options);
                    foreach (var dice in dices)
                        yield return dice;
                }
            }
        }
    }
}