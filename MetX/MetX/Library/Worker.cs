using System;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MetX.Library
{
	
	/// <summary>General helper functions</summary>
	public static class Worker
	{
	    public static string ConvertWildcardToRegex(string pattern)
	    {
            StringBuilder builder = new StringBuilder("^");
            pattern = pattern
                .Replace(".", @"\.")
                .Replace("*", ".*")
                .Replace("?", ".");
	        builder.Append(pattern);
	        builder.Append("$");
	        return builder.ToString();
	    }

        public static string ConvertLikeToRegex(string pattern)
        {
            // Turn "off" all regular expression related syntax in the pattern string. 
            StringBuilder builder = new StringBuilder(Regex.Escape(pattern));

            pattern = pattern.Replace("*", "%").Replace(".", "_");

            // these are needed because the .*? replacement below at the begining or end of the string is not
            // accounting for cases such as LIKE '*abc' or LIKE 'abc*'
            bool startsWith = pattern.StartsWith("%") && !pattern.EndsWith("%");
            bool endsWith = !pattern.StartsWith("%") && pattern.EndsWith("%");

            // this is a little tricky
            // ends with in like is '%abc'
            // in regex it's 'abc$'
            // so need to tanspose
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
            * equivalent regular expression metacharacters. */
            builder.Replace("%", ".*?").Replace("_", ".");

            /* The previous call to Regex.Escape actually turned off
            * too many metacharacters, i.e. those which are recognized by
            * both the regular expression engine and the SQL LIKE
            * statement ([...] and [^...]). Those metacharacters have
            * to be manually unescaped here. */
            builder.Replace(@"\[", "[").Replace(@"\]", "]").Replace(@"\^", "^");

            if (builder[0] != '^')
                builder.Insert(0, "^");
            if (builder[builder.Length - 1] != '$')
                builder.Append("$");

            return builder.ToString();
        }

        public static bool IsNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            foreach (char c in input)
                if (!char.IsNumber(c)) return false;
            return true;
        }

        public static bool IsValidEmailAddress(ref string sEmail)
        {
            if (!string.IsNullOrEmpty(sEmail) && sEmail.Contains("@"))
            {
                Regex email = (Regex)(HttpContext.Current != null ? HttpContext.Current.Cache["Regex.Email"] : new Regex(ConfigurationManager.AppSettings["Regex.Email"]));
                if (email == null && HttpContext.Current != null)
                {
                    email = new Regex(ConfigurationManager.AppSettings["Regex.Email"], RegexOptions.Compiled);
                    HttpContext.Current.Cache.Add("Regex.Email", email,
                        new System.Web.Caching.CacheDependency(HttpContext.Current.Server.MapPath("web.config")),
                        System.Web.Caching.Cache.NoAbsoluteExpiration,
                        new TimeSpan(1, 7, 11), System.Web.Caching.CacheItemPriority.AboveNormal, null);
                }
                sEmail = sEmail.Trim();
                sEmail = sEmail.FirstToken("@").Trim() + "@" + sEmail.TokensAfter(1, "@").ToLower().FirstToken("<").Trim();
                return email != null && email.IsMatch(sEmail);
            }
            return false;
        }

        public static string Value(HttpRequest request, System.Web.UI.StateBag viewState, System.Web.SessionState.HttpSessionState session, string key)
        {
            string ret = null;
            if (session != null) ret = Value(session, key);
            if (string.IsNullOrEmpty(ret)) ret = request[key];
            if (viewState != null && string.IsNullOrEmpty(ret)) ret = Value(viewState, key);
            if (string.IsNullOrEmpty(ret)) ret = request.Headers[key];
            if (string.IsNullOrEmpty(ret))
                if (request.Cookies[key] != null) ret = request.Cookies[key].Value;
            return ret;
        }

        public static string Value(System.Web.UI.StateBag state, string key)
        {
            string ret = string.Empty;
            if (state != null)
                try { ret = state[key].ToString(); }
                catch
                {
                    // ignored
                }
            return ret;
        }

        public static string Value(System.Web.SessionState.HttpSessionState state, string key)
        {
            string ret = string.Empty;
            if (state != null)
                try { ret = state[key].ToString(); }
                catch
                {
                    // ignored
                }
            return ret;
        }

        /// <summary>Returns the string representation of a value, even if it's DbNull, a Guid, or null</summary>
        /// <param name="value">The value to convert</param>
        /// <param name="defaultValue">The value to return if Value == null or DbNull or an empty string.</param>
        /// <returns>The string representation</returns>
        public static string AsString(this object value, string defaultValue = "")
        {
            if (value == null || value == DBNull.Value || value.Equals(string.Empty))
                return defaultValue;
            if (value is Guid)
                return Convert.ToString(value);
            return value.ToString().Trim();
        }

	    public static char AsChar(object value)
	    {
	        if (value == null || value == DBNull.Value || value.Equals(string.Empty))
                return new char();
	        if (value is string)
	            return ((string)value)[0];
	        if (value is char)
	            return (char)value;
	        string ret = AsString(value);
	        if (ret.Length == 0)
	            return new char();
	        return ret[0];
	    }

        /// <summary>Returns the double representation of a value, even if it's DbNull, a Guid, or null</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The double representation</returns>
        public static double NzDouble(object value)
        {
            double ret = 0;
            string text = AsString(value);
            if (text.Length == 0)
                ret = 0;
            else 
                try
                {
                    ret = Convert.ToDouble(text);
                }
                catch { }
            return ret;
        }

        /// <summary>Returns a SQL appropriate phrase for an object value. If the object is null or DbNull, the string "NULL" will be returned. Otherwise a single quote delimited string of the value will be created. So if Value='fred', then this function will return "'fred'"</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The SQL appropriate phrase</returns>
        /// 
        /// <example><c>string x = "insert into x values(" + s2db(y) + ")";</c></example>
        public static string S2DB(object value)
        {
            if (value == null || value == DBNull.Value || (value is DateTime && (DateTime)value < DateTime.MinValue.AddYears(10)) || (value is string && (string)value == "(NULL)"))
                return "NULL";
            return "'" + AsString(value).Replace("'", "''") + "'";
        }

	    /// <summary>Extracts a name from an email address</summary>
		/// <param name="sOriginalText">The Email address</param>
		/// <param name="defaultName">The default name to use if none is found (or a blank email address is passed in)</param>
		/// <returns>The extracted proper case name</returns>
		public static string EmailToName(string sOriginalText, string defaultName)
		{
			string text = (sOriginalText + "").Trim();
			string returnValue;

			if (text.IndexOf("@") > 0 )
				text = text.Split('@')[0];

			text = text.Replace(".", " ");
			text = text.Substring(0,1).ToUpper() + text.Substring(1, text.IndexOf(" ")).ToLower() + text.Substring(text.IndexOf(" ") + 1,1).ToUpper() + text.Substring(text.IndexOf(" ") + 2).ToLower();

			if( text.Length == 0 )
				returnValue = defaultName;
			else
				returnValue = text;

			return Microsoft.VisualBasic.Strings.StrConv(returnValue, Microsoft.VisualBasic.VbStrConv.ProperCase, 0);
		}

	
    }
}
