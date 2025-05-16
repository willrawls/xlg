using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetX.Standard.Strings
{
    public static class TokenFunctionsByGptBasic
    {
        // ---------------------------------------------------------------
        //   ChatGPT generated functions for token manipulation
        //   in strings. Asked ChatGPT to generate a set of functions
        //   to manipulate tokens in strings, including splitting,
        //   trimming, and replacing tokens. These are what it delivered.
        // ---------------------------------------------------------------

        public static List<string> SplitAndTrim(this string input, string delimiter)
        {
            return input.Split([delimiter], StringSplitOptions.None)
                .Select(token => token.Trim())
                .ToList();
        }

        public static List<string> TokensBetween(this string input, string startDelimiter = "(",
            string endDelimiter = ")", StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            var results = new List<string>();
            var startIndex = 0;

            if (string.IsNullOrEmpty(input) || !input.Contains(startDelimiter))
                return results;

            while ((startIndex = input.IndexOf(startDelimiter, startIndex, StringComparison.Ordinal)) != -1)
            {
                startIndex += startDelimiter.Length;
                var endIndex = input.IndexOf(endDelimiter, startIndex, StringComparison.Ordinal);
                if (endIndex == -1)
                    break;

                results.Add(input.Substring(startIndex, endIndex - startIndex));
                startIndex = endIndex + endDelimiter.Length;
            }

            return results;
        }

        public static string FirstToken(this string input, string delimiter = " ",
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter))
                return string.Empty;

            var index = input.IndexOf(delimiter, compare);
            return index == -1 ? input : input.Substring(0, index);
        }

        public static List<string> TokenRange(this string input, int startIndex, int endIndex, string delimiter = " ",
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter) || startIndex < 0 ||
                endIndex <= startIndex)
                return result;

            var currentTokenIndex = 0;
            var position = 0;

            while (position <= input.Length)
            {
                var nextDelimiterPos = input.IndexOf(delimiter, position, compare);
                if (nextDelimiterPos == -1)
                    nextDelimiterPos = input.Length;

                if (currentTokenIndex >= startIndex && currentTokenIndex < endIndex)
                {
                    var token = input.Substring(position, nextDelimiterPos - position);
                    result.Add(token);
                }

                if (currentTokenIndex >= endIndex)
                    break;

                position = nextDelimiterPos + delimiter.Length;
                currentTokenIndex++;
            }

            return result;
        }

        public static string ReverseTokens(this string input, string delimiter = " ",
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter))
                return input;

            var builder = new StringBuilder();
            var end = input.Length;
            var first = true;

            while (end > 0)
            {
                var start = input.LastIndexOf(delimiter, end - 1, compare);
                int tokenStart;
                int tokenLength;

                if (start == -1)
                {
                    tokenStart = 0;
                    tokenLength = end;
                }
                else
                {
                    tokenStart = start + delimiter.Length;
                    tokenLength = end - tokenStart;
                }

                if (!first)
                {
                    builder.Append(delimiter);
                }

                builder.Append(input, tokenStart, tokenLength);

                if (start == -1)
                    break;

                end = start;
                first = false;
            }

            return builder.ToString();
        }

        public static List<string> RemoveEmptyTokens(this string input, string delimiter = " ",
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter))
                return result;

            var position = 0;

            while (position <= input.Length)
            {
                var nextDelimiter = input.IndexOf(delimiter, position, compare);
                if (nextDelimiter == -1)
                    nextDelimiter = input.Length;

                var token = input.Substring(position, nextDelimiter - position);
                if (!string.IsNullOrWhiteSpace(token))
                    result.Add(token);

                if (nextDelimiter == input.Length)
                    break;

                position = nextDelimiter + delimiter.Length;
            }

            return result;
        }

        public static string ReplaceTokenAt(this string input, string newToken, string delimiter = " ", int index = 2,
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter) || index < 0)
                return input;

            var builder = new StringBuilder();
            var position = 0;
            var currentIndex = 0;

            while (position <= input.Length)
            {
                var next = input.IndexOf(delimiter, position, compare);
                if (next == -1) next = input.Length;

                var token = input.Substring(position, next - position);
                if (currentIndex > 0) builder.Append(delimiter);

                builder.Append(currentIndex == index ? newToken : token);

                if (next == input.Length) break;

                position = next + delimiter.Length;
                currentIndex++;
            }

            return builder.ToString();
        }

        public static string TrimTokens(this string input, string delimiter = " ",
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter))
                return input;

            var builder = new StringBuilder();
            var position = 0;
            var first = true;

            while (position <= input.Length)
            {
                var next = input.IndexOf(delimiter, position, compare);
                if (next == -1) next = input.Length;

                var token = input.Substring(position, next - position).Trim();

                if (!first) builder.Append(delimiter);
                builder.Append(token);
                first = false;

                if (next == input.Length) break;
                position = next + delimiter.Length;
            }

            return builder.ToString();
        }

        public static string InsertTokenAt(this string input, string newToken, string delimiter = " ", int index = 2,
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(delimiter) || index < 0 || newToken == null)
                return input;

            var builder = new StringBuilder();
            int position = 0, currentIndex = 0;
            var inserted = false;

            while (position <= input.Length)
            {
                if (currentIndex == index)
                {
                    if (builder.Length > 0) builder.Append(delimiter);
                    builder.Append(newToken);
                    inserted = true;
                }

                var next = input.IndexOf(delimiter, position, compare);
                if (next == -1) next = input.Length;

                var token = input.Substring(position, next - position);
                if (!string.IsNullOrEmpty(token) ||
                    currentIndex != index) // avoid double inserting if inserting into empty
                {
                    if (builder.Length > 0) builder.Append(delimiter);
                    builder.Append(token);
                }

                if (next == input.Length) break;
                position = next + delimiter.Length;
                currentIndex++;
            }

            // Append at end if index is beyond existing tokens
            if (inserted || currentIndex > index) return builder.ToString();

            if (builder.Length > 0) builder.Append(delimiter);
            builder.Append(newToken);

            return builder.ToString();
        }

        public static string RemoveTokenAt(this string input, int index = 2, string delimiter = " ",
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter) || index < 0)
                return input;

            var builder = new StringBuilder();
            int position = 0, currentIndex = 0;

            while (position <= input.Length)
            {
                var next = input.IndexOf(delimiter, position, compare);
                if (next == -1) next = input.Length;

                if (currentIndex != index)
                {
                    if (builder.Length > 0) builder.Append(delimiter);
                    builder.Append(input.Substring(position, next - position));
                }

                if (next == input.Length) break;
                position = next + delimiter.Length;
                currentIndex++;
            }

            return builder.ToString();
        }

        public static int CountTokens(this string input, string delimiter = " ",
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter)) return 0;
            int count = 0, position = 0;
            while (position < input.Length)
            {
                var next = input.IndexOf(delimiter, position, compare);
                count++;
                if (next == -1) break;
                position = next + delimiter.Length;
            }

            return count;
        }

        public static bool TokenExists(this string input, string tokenToFind, string delimiter = " ",
            bool ignoreCase = false,
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter) || tokenToFind == null) return false;
            var position = 0;
            var effectiveCompare = ignoreCase ? StringComparison.OrdinalIgnoreCase : compare;
            while (position <= input.Length)
            {
                var next = input.IndexOf(delimiter, position, compare);
                if (next == -1) next = input.Length;
                var token = input.Substring(position, next - position);
                if (string.Equals(token, tokenToFind, effectiveCompare)) return true;
                if (next == input.Length) break;
                position = next + delimiter.Length;
            }

            return false;
        }

        public static string InsertTokenAt(this string input, string newToken, int index = 2, string delimiter = " ",
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(delimiter) || index < 0 || newToken == null) return input;
            var builder = new StringBuilder();
            int position = 0, currentIndex = 0;
            var inserted = false;
            while (position <= input.Length)
            {
                if (currentIndex == index)
                {
                    if (builder.Length > 0) builder.Append(delimiter);
                    builder.Append(newToken);
                    inserted = true;
                }

                var next = input.IndexOf(delimiter, position, compare);
                if (next == -1) next = input.Length;
                var token = input.Substring(position, next - position);
                if (!string.IsNullOrEmpty(token) || currentIndex != index)
                {
                    if (builder.Length > 0) builder.Append(delimiter);
                    builder.Append(token);
                }

                if (next == input.Length) break;
                position = next + delimiter.Length;
                currentIndex++;
            }

            if (!inserted && currentIndex <= index)
            {
                if (builder.Length > 0) builder.Append(delimiter);
                builder.Append(newToken);
            }

            return builder.ToString();
        }

        public static string RemoveTokenAt(this string input, string delimiter = " ", int index = 2,
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter) || index < 0) return input;
            var builder = new StringBuilder();
            int position = 0, currentIndex = 0;
            while (position <= input.Length)
            {
                var next = input.IndexOf(delimiter, position, compare);
                if (next == -1) next = input.Length;
                if (currentIndex != index)
                {
                    if (builder.Length > 0) builder.Append(delimiter);
                    builder.Append(input.Substring(position, next - position));
                }

                if (next == input.Length) break;
                position = next + delimiter.Length;
                currentIndex++;
            }

            return builder.ToString();
        }

        public static int IndexOfToken(this string input, string token, string delimiter = " ", bool ignoreCase = false,
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter) || token == null) return -1;
            int position = 0, index = 0;
            var effectiveCompare = ignoreCase ? StringComparison.OrdinalIgnoreCase : compare;
            while (position <= input.Length)
            {
                var next = input.IndexOf(delimiter, position, compare);
                if (next == -1) next = input.Length;
                var currentToken = input.Substring(position, next - position);
                if (string.Equals(currentToken, token, effectiveCompare)) return index;
                if (next == input.Length) break;
                position = next + delimiter.Length;
                index++;
            }

            return -1;
        }

        public static bool TokenStartsWith(this string input, string prefix, string delimiter = " ",
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter) || prefix == null) return false;
            var position = 0;
            while (position <= input.Length)
            {
                var next = input.IndexOf(delimiter, position, compare);
                if (next == -1) next = input.Length;
                var token = input.Substring(position, next - position);
                if (token.StartsWith(prefix, compare)) return true;
                if (next == input.Length) break;
                position = next + delimiter.Length;
            }

            return false;
        }

        public static string RemoveDuplicateTokens(this string input, string delimiter = " ",
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter)) return input;
            var seen = new HashSet<string>();
            var builder = new StringBuilder();
            var position = 0;
            while (position <= input.Length)
            {
                var next = input.IndexOf(delimiter, position, compare);
                if (next == -1) next = input.Length;
                var token = input.Substring(position, next - position);
                if (seen.Add(token))
                {
                    if (builder.Length > 0) builder.Append(delimiter);
                    builder.Append(token);
                }

                if (next == input.Length) break;
                position = next + delimiter.Length;
            }

            return builder.ToString();
        }

        public static StringComparer FromComparison(this StringComparer _, StringComparison comparison)
        {
            return comparison switch
            {
                StringComparison.Ordinal => StringComparer.Ordinal,
                StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase,
                StringComparison.CurrentCulture => StringComparer.CurrentCulture,
                StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase,
                StringComparison.InvariantCulture => StringComparer.InvariantCulture,
                StringComparison.InvariantCultureIgnoreCase => StringComparer.InvariantCultureIgnoreCase,
                _ => StringComparer.OrdinalIgnoreCase
            };
        }
    }
}
