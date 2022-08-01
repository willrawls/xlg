using System;
using MetX.Standard.Strings.Extensions;

// ReSharper disable UnusedMember.Global

namespace MetX.Standard.Library.Extensions
{
	
	/// <summary>General helper functions</summary>
	public static class ForDouble
	{

        /*
        public static string ConvertLikeToRegex(string pattern)
        {
            // Turn "off" all regular expression related syntax in the pattern string. 
            var builder = new StringBuilder(Regex.Escape(pattern));

            pattern = pattern.Replace("*", "%").Replace(".", "_");

            // these are needed because the .*? replacement below at the beginning or end of the string is not
            // accounting for cases such as LIKE '*abc' or LIKE 'abc*'
            var startsWith = pattern.StartsWith("%") && !pattern.EndsWith("%");
            var endsWith = !pattern.StartsWith("%") && pattern.EndsWith("%");

            // this is a little tricky
            // ends with in like is '%abc'
            // in regex it's 'abc$'
            // so need to transpose
            if (startsWith)
            {
                builder.Replace("%", "", 0, 1);
                builder.Append("$");
            }

            // same but inverse here
            if (endsWith)
            {
                builder.Replace("%", "", pattern.Length - 1, 1);
                builder.Insert(0, "^");
            }

            /* Replace the SQL LIKE wildcard metacharacters with the
            * equivalent regular expression metacharacters. #1#
            builder.Replace("%", ".*?").Replace("_", ".");

            /* The previous call to Regex.Escape actually turned off
            * too many metacharacters, i.e. those which are recognized by
            * both the regular expression engine and the SQL LIKE
            * statement ([...] and [^...]). Those metacharacters have
            * to be manually unescaped here. #1#
            builder.Replace(@"\[", "[").Replace(@"\]", "]").Replace(@"\^", "^");

            if (builder[0] != '^')
                builder.Insert(0, "^");
            if (builder[^1] != '$')
                builder.Append("$");

            return builder.ToString();
        }
        */

        /*
        public static bool IsNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            foreach (var c in input)
                if (!char.IsNumber(c)) return false;
            return true;
        }
        */

        /*
	    public static char AsChar(object value)
	    {
	        if (value == null || value == DBNull.Value || value.Equals(string.Empty))
                return new char();
	        switch (value)
            {
                case string stringValue:
                    return stringValue[0];
                case char charValue:
                    return charValue;
            }

            var ret = AsString(value);
            return ret.Length == 0 ? new char() : ret[0];
        }
        */

        /// <summary>Returns the double representation of a value,
        /// even if it's DbNull, a Guid, or null</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The double representation</returns>
        public static double NzDouble(object value)
        {
            double ret = 0;
            var text = value.AsString();
            if (text.Length <= 0) return ret;

            try
            {
                ret = Convert.ToDouble(text);
            }
            catch
            {
                // ignored
            }

            return ret;
        }
    }
}
