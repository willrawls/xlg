using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace MetX._471.Strings.Extensions;

/// <summary>Provides simple methods for retrieving tokens from a string.
/// <para>A token is a piece of a delimited string. For instance in the string "this is a test" when " " (a space) is used as a delimiter, "this" is the first token and "test" is the last (4th) token.</para>
/// <para>Asking for a token beyond the end of a string returns a blank string. Asking for the zeroth or a negative token returns a blank string.</para>
/// </summary>
public static class StringExtensions
{
    public static string AsStringFromStringArray(this char[] items, string separator = "\n", string postFix = "")
    {

        if (items == null || items.Length < 1)
            return "";

        var sb = new StringBuilder();

        foreach (var line in items)
        {
            sb.Append(line);
            if(separator.IsNotEmpty())
                sb.Append(separator);
        }

        if (postFix.IsNotEmpty())
            sb.Append(postFix);

        return sb.ToString();
    }


    public static string[] SplitInTwo(this string target)
    {
        var index = target.Length / 2;
        return new[]
        {
            target.Substring(0, index),
            target.Substring(index + 1)
        };
    }

    /*
        public static string[] SplitInXPieces(this string target, int pieces = 3)
        {
            if (target.IsEmpty() || pieces < 1)
                return new string[] { };

            if (target.IsEmpty() || pieces == 1)
                return new string[] { target };

            if (pieces == 2)
                return target.SplitInTwo();

            var index = target.Length / pieces;
            return new[]
            {
                target[..index],
                target[index..]
            };
        }
        */

    public static string InsertXEveryYCharacters(this string target, int y = 100, string toInsert = " ")
    {
        if (target.IsEmpty() || target.Length < y || toInsert == "")
            return target;

        var result = target.Left(y);
        for (var i = y; i < target.Length; i+=y)
        {
            result += toInsert;
            result += target.Mid(i, y);
        }

        return result;
    }

    public static string ImplicitReplace(this string str, string toFind, string replaceWith, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        replaceWith ??= "";
        if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(toFind) || toFind.Equals(replaceWith, comparison)) return str;

        var foundAt = 0;

        while ((foundAt = str.IndexOf(toFind, foundAt, comparison)) != -1)
        {
            str = str.Remove(foundAt, toFind.Length).Insert(foundAt, replaceWith);
            foundAt += replaceWith.Length;
        }
        return str;
    }

    public static string InsertLineNumbers(this string target, List<int> keyLines)
    {
        if (target.IsEmpty())
            return string.Empty;

        var lines = target.Lines();

        var digitCount = 1;
        if (lines.Length < 10) digitCount = 2;
        else if (lines.Length < 100) digitCount = 3;
        else if (lines.Length < 1000) digitCount = 4;
        else if (lines.Length < 10000) digitCount = 5;
        else if (lines.Length < 100000) digitCount = 6;

        var formatString = new string('0', digitCount);

        var sb = new StringBuilder();
        for (var index = 0; index < lines.Length; index++)
        {
            string lineMod;
            if (keyLines.IsNotEmpty())
                lineMod = keyLines.Any(x => x == index+1) ? "|Error|" : "|     |";
            else
                lineMod = "|     |";

            sb.AppendLine($"{(index+1).ToString(formatString)} {lineMod}\t{lines[index]}");
        }

        return sb.ToString();
    }

    public static string RemoveBlankLines(this string target)
    {
        if (target.IsEmpty())
            return string.Empty;

        var result = target
            .Lines()
            .Where(l => l.Replace("\t", "").Trim().IsNotEmpty()).ToArray();

        return string.Join("\n", result);
    }

    public static string RemoveAll(this string target, string[] stringsToRemove)
    {
        if (target.IsEmpty())
            return string.Empty;
        if (stringsToRemove.IsEmpty())
            return target;
        return stringsToRemove
            .Aggregate(target, (current, stringToRemove) 
                => current
                    .Replace(stringToRemove, string.Empty));
    }

    public static string RemoveAll(this string target, char[] charsToRemove)
    {
        if (target.IsEmpty())
            return string.Empty;
        if (charsToRemove.IsEmpty())
            return target;
        return charsToRemove
            .Aggregate(target, (current, charToRemove) 
                => current
                    .Replace(charToRemove.ToString(), string.Empty));
    }

    public static string AsStringFromFormattedXml(this string target)
    {
        if (target.IsEmpty())
            return string.Empty;

        var result = target
                .Replace("><", ">\n\t<")
            ;
        return result;
    }
        
    public static string BlankToNull(this string target)
    {
        return target == string.Empty ? null : target;
    }
        
    public static string ProperCase(this string target)
    {
        if (target.IsEmpty())
            return string.Empty;
            
        var result = string.Empty;
        var isFirst = true;
        char? previousLetter = null;

        // For each char
        foreach (var letter in target.Trim())
        {
            //  If first
            if (isFirst)
            {
                //    upper case and add to output
                result += char.ToUpper(letter);
                isFirst = false;
            }
            //  Else If last was space and char isn't uppercase
            else if (previousLetter == ' ')
            {
                // upper case and add to output
                result += char.ToUpper(letter);
            }
            //  Else If char is already upper and previous was not
            else if (char.IsUpper(letter) && !char.IsUpper(previousLetter.Value))
            {
                //    add space then char
                result += ' ';
                result += letter;
            }
            //  Else
            else
            {
                //    add char
                result += letter;
            }
            previousLetter = letter;
        }

        return result;
    }

    public static bool IsEmpty<T>(this IList<T> target)
    {
        return target == null || target.Count == 0;
    }

    public static bool IsNotEmpty<T>(this IList<T> target)
    {
        return target?.Count > 0;
    }

    public static bool IsEmpty(this string target)
    {
        return target == null || 0u >= (uint)target.Length;
    }

    public static int AsInteger(this string target, int defaultValue = 0)
    {
        if (target.IsEmpty())
            return defaultValue;

        return int.TryParse(target, out var integerValue) 
            ? integerValue 
            : defaultValue;
    }

    public static bool ThrowIfEmpty(this string target, string targetsName)
    {
        if (string.IsNullOrEmpty(target))
            throw new ArgumentException(targetsName);
        return false;
    }

    public static bool IsNotEmpty(this string target)
    {
        Contract.Ensures(Contract.Result<string>() != null);
        return !string.IsNullOrEmpty(target);
    }

    public static List<string> AsList(this string[] target)
    {
        return new(target);
    }

    public static List<string> Slice(this string target) { return target.LineList(StringSplitOptions.RemoveEmptyEntries); }

    public static List<string> Dice(this string target) { return target.AllTokens(" ", StringSplitOptions.RemoveEmptyEntries); }

    public static string[] Lines(this string target, StringSplitOptions options = StringSplitOptions.None)
    {
        if (string.IsNullOrEmpty(target)) 
            return new[] { string.Empty };
            
        return target
            .Replace("\r", string.Empty)
            .Split(new[] { '\n' }, options);
    }

    public static List<string> LineList(this string target, StringSplitOptions options = StringSplitOptions.None)
    {
        return target.Lines(options).AsList();
    }

    public static string[] Words(this string target, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
    {
        if (string.IsNullOrEmpty(target)) 
            return new[] { string.Empty };
            
        return target.Trim()
            .Split(new[] { ' ' }, options);
    }

    public static List<string> WordList(this string target, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
    {
        return target.Words(options).AsList();
    }

    public static string Flatten<T>(this IList<T> target)
    {
        return target.IsEmpty() 
            ? string.Empty 
            : string.Join(Environment.NewLine, target);
    }

    public static string AsFilename(this string target, string suffix = "")
    {
        if (target.IsEmpty()) return string.Empty;
        target = target.Replace(new[] { ":", @"\", "/", "*", "\"", "?", "<", ">", "|" }, " ");
        if (suffix.IsNotEmpty())
            target += suffix;

        if (char.IsDigit(target[0]))
            target = "_" + target;

        while (target.Contains("  ")) 
            target = target.Replace("  ", " ");

        while (target.EndsWith("."))
            target = target.Substring(0, target.Length - 2);
        while (target.Contains(".."))
            target = target.Replace("..", ".");
        while (target.Contains("__"))
            target = target.Replace("__", "_");

        return target.ProperCase().Replace(" ", "");
    }

    public static string Left(this string target, int length)
    {
        if (string.IsNullOrEmpty(target) || length < 1) return string.Empty;
        if (target.Length <= length) return target;
        return target.Substring(0, length);
    }

    public static string Right(this string target, int length)
    {
        // 01234
        // ABCDE
        //    DE
        // "ABCDE".Right(2) == "DE"
        // "ABCDE".Right(0) == ""

        if (string.IsNullOrEmpty(target) || length < 1) 
            return string.Empty;

        return length >= target.Length 
            ? target 
            : target.Substring(target.Length - length);
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

    public static string Replace(this string target, string[] strings, string replacement)
    {
        if (string.IsNullOrEmpty(target)) return string.Empty;
        if (strings == null || strings.Length == 0) return target;
        return strings.Aggregate(target, (current, s) => current.Replace(s, replacement));
    }

    public static void TransformAllNotEmpty(this IList<string> target, Func<string, int, string> func)
    {
        if (target.IsEmpty()) return;

        for (var i = 0; i < target.Count; i++)
        {
            var s = target[i];
            if (s.IsEmpty()) continue;
            target[i] = func(s, i);
        }
    }

    public static void TransformAllNotEmpty(this IList<string> target, Func<string, string> func)
    {
        if (target.IsEmpty()) return;
        for (var i = 0; i < target.Count; i++)
        {
            var s = target[i];
            if (s.IsEmpty()) continue;
            target[i] = func(s);
        }
    }

    /// <summary>Returns the string representation of a value, even if it's DbNull, a Guid, or null</summary>
    /// <param name="value">The value to convert</param>
    /// <param name="defaultValue">The value to return if Value == null or DbNull or an empty string.</param>
    /// <returns>The string representation</returns>
    public static string AsStringFromObject(this object value, string defaultValue = "")
    {
        if (value == null || value == DBNull.Value || value.Equals(string.Empty))
            return defaultValue;

        return value is Guid ? Convert.ToString(value) : value.ToString()?.Trim();
    }

    /// <summary>Returns the string representation of a string value, even if it's null or a blank string</summary>
    /// <param name="value">The value to convert</param>
    /// <param name="defaultValue">The value to return if Value == null or DbNull or an empty string.</param>
    /// <returns>The string representation</returns>
    public static string AsStringFromString(this string value, string defaultValue = "")
    {
        return value.IsEmpty() 
            ? defaultValue 
            : Convert.ToString(value);
    }

    public static string AsStringFromGuid(this Guid value)
    {
        if (value == Guid.Empty)
            return "";
        return value.ToString("N");
    }

    public static string AsStringFromList(this IList<string> target, string delimiter = " ", string defaultValue = "")
    {
        if (target.IsEmpty())
            return defaultValue;
        return string.Join(delimiter, target);
    }

    /// <summary>Returns a SQL appropriate phrase for an object value.
    /// If the object is null or DbNull, the string "NULL" will be returned.
    /// Otherwise a single quote delimited string of the value will be created.
    /// So if Value='fred', then this function will return "'fred'"</summary>
    /// <param name="value">The value to convert</param>
    /// <returns>The SQL appropriate phrase</returns>
    /// 
    /// <example><c>string x = "insert into x values(" + s2db(y) + ")";</c></example>
    public static string S2Db(object value)
    {
        if (value == null || value == DBNull.Value 
                          || value is DateTime dateTime 
                          && dateTime < DateTime.MinValue.AddYears(10) 
                          || value is string and "(NULL)")
        {
            return "NULL";
        }
        
        return "'" + AsStringFromObject(value).Replace("'", "''") + "'";
    }
}