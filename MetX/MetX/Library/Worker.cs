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
                sEmail = StringExtensions.FirstToken(sEmail, "@").Trim() + "@" + StringExtensions.FirstToken(StringExtensions.TokensAfter(sEmail, 1, "@").ToLower(), "<").Trim();
                return email.IsMatch(sEmail);
            }
            return false;
        }

        public static string Value(HttpRequest request, System.Web.UI.StateBag viewState, System.Web.SessionState.HttpSessionState session, string key)
        {
            string ret = null;
            if (session != null) ret = Worker.Value(session, key);
            if (string.IsNullOrEmpty(ret)) ret = request[key];
            if (viewState != null && string.IsNullOrEmpty(ret)) ret = Worker.Value(viewState, key);
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
                catch { }
            return ret;
        }

        public static string Value(System.Web.SessionState.HttpSessionState state, string key)
        {
            string ret = string.Empty;
            if (state != null)
                try { ret = state[key].ToString(); }
                catch { }
            return ret;
        }

        /// <summary>Returns the string representation of a value, even if it's DbNull, a Guid, or null</summary>
        /// <param name="value">The value to convert</param>
        /// <param name="defaultValue">The value to return if Value == null or DbNull or an empty string.</param>
        /// <returns>The string representation</returns>
        public static string nzString(object value, string defaultValue)
        {
            if (value == null || value == DBNull.Value || value.Equals(string.Empty))
                return defaultValue;
            else if (value is Guid)
                return System.Convert.ToString(value);
            else
                return value.ToString().Trim();
        }

        /// <summary>Returns the byte array representation of a value, even if it's DbNull or null</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The byte array representation</returns>
        public static byte[] NzByteArray(object value)
        {
            if (value == null || value == DBNull.Value || value.Equals(string.Empty))
                return new byte[0];
            else if (value is byte[])
                return (byte[])value;
            else
                return Encoding.ASCII.GetBytes(nzString(value));
        }

        /// <summary>Returns the TimeSpan representation of a value, even if it's DbNull or null</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The TimeSpan representation</returns>
        public static TimeSpan NzTimeSpan(object value)
        {
            if (value == null || value == DBNull.Value || value.Equals(string.Empty))
                return new TimeSpan();
            else if (value is TimeSpan)
                return (TimeSpan) value;
            else if (value is string && !string.IsNullOrEmpty((string) value))
                try { return TimeSpan.Parse((string) value); }
                catch { }
            return new TimeSpan();
        }

        public static string Md5(string toHash)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(toHash, "md5").ToLower();
        }

        public static char NzChar(object value)
        {
            if (value == null || value == DBNull.Value || value.Equals(string.Empty))
                return new char();
            else if (value is string)
                return ((string)value)[0];
            else if (value is char)
                return (char)value;
            else
            {
                string ret = nzString(value);
                if (ret.Length == 0)
                    return new char();
                return ret[0];
            }
        }

        public static byte NzByte(object value)
        {
            if (value is byte)
                return (byte)value;
            if (value is byte[])
                if (((byte[])value).Length == 0)
                    return new byte();
                else
                    return ((byte[])value)[0];
            else
                return (byte) (int) NzChar(value);
        }

		/// <summary>Returns the string representation of a value, even if it's DbNull, a Guid, or null</summary>
		/// <param name="value">The value to convert</param>
		/// <returns>The string representation</returns>
		public static string nzString(object value)
		{
			if(value == null || value == DBNull.Value || value.Equals(string.Empty))
				return string.Empty;
			else if( value is Guid)
				return  System.Convert.ToString(value);
			else
				return value.ToString().Trim();
		}

        /// <summary>Returns the double representation of a value, even if it's DbNull, a Guid, or null</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The double representation</returns>
        public static double NzDouble(object value)
        {
            double ret = 0;
            string text = nzString(value);
            if (text.Length == 0)
                ret = 0;
            else 
                try
                {
                    ret = System.Convert.ToDouble(text);
                }
                catch { }
            return ret;
        }

        /// <summary>Returns the double representation of a value, even if it's DbNull, a Guid, or null</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The double representation</returns>
        public static decimal NzDecimal(object value)
        {
            decimal ret = 0;
            string text = nzString(value);
            if (text.Length == 0)
                ret = 0;
            else
                try
                {
                    ret = System.Convert.ToDecimal(text);
                }
                catch { }
            return ret;
        }

        /// <summary>Returns a SQL appropriate phrase for an object value. If the object is null or DbNull, the string "NULL" will be returned. Otherwise a single quote delimited string of the value will be created. So if Value='fred', then this function will return "'fred'"</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The SQL appropriate phrase</returns>
        /// 
        /// <example><c>string x = "insert into x values(" + s2db(y) + ")";</c></example>
        public static string s2db(object value)
        {
            if (value == null || value == DBNull.Value || (value is DateTime && (DateTime)value < DateTime.MinValue.AddYears(10)) || (value is string && (string)value == "(NULL)"))
                return "NULL";
            else
                return "'" + nzString(value).Replace("'", "''") + "'";
        }

        /// <summary>Returns a SQL appropriate phrase for an object value. If the object is null or DbNull, the string "NULL" will be returned. Otherwise a single quote delimited string of the value will be created. So if Value='fred', then this function will return "'fred'"</summary>
        /// <param name="value">The value to convert</param>
        /// <param name="maxLength">The maximum length the converted string should be. Values longer will be truncated.</param>
        /// <returns>The SQL appropriate phrase</returns>
        /// 
        /// <example><c>string x = "insert into x values(" + s2db(y) + ")";</c></example>
        public static string s2db(object value, int maxLength)
        {
            if (value == null || value == DBNull.Value || (value is DateTime && (DateTime)value < DateTime.MinValue.AddYears(10)) || (value is string && (string)value == "(NULL)"))
                return "NULL";
            else
            {
                string ret = nzString(value);
                if (maxLength > 0 && ret.Length > maxLength)
                    ret = ret.Substring(0, maxLength);
                return "'" + ret.Replace("'", "''") + "'";
            }
        }

        /// <summary>Returns a SQL appropriate date/time phrase for an object value. If the object is null or DbNull, the string "NULL" will be returned. Otherwise a single quote delimited date/time string of the value will be created.</summary>
		/// <param name="value">The value to convert</param>
		/// <returns>The SQL appropriate date/time string</returns>
		/// 
		/// <example><c>string x = "insert into x values(" + d2db(y) + ")";</c></example>
		public static string Date2Db(object value)
		{
            if (value == null || value == DBNull.Value || (value is DateTime && (DateTime)value < DateTime.MinValue.AddYears(10)) || (value is string && (string)value == "(NULL)"))
				return "NULL";
			else
				return "'" + nzDateTime(value, DateTime.Now).ToString().Replace("'", "''") + "'";
		}

        /// <summary>Returns the Guid representation of a value, even if it's DbNull, a Guid, or null</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The Guid representation</returns>
        public static Guid NzGuid(object value)
        {
            if (value == null || value == System.DBNull.Value)
                return Guid.Empty;
            else if (value is string)
                return new Guid((string)value);
            else
                return (Guid)value;
        }

        /// <summary>Returns the DateTime representation of a value, even if it's DbNull or null</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The DateTime representation</returns>
        public static System.DateTime nzDateTime(object value)
        {
            return nzDateTime(value, DateTime.MinValue);
        }

        /// <summary>Returns the DateTime representation of a value, even if it's DbNull or null</summary>
        /// <param name="value">The value to convert</param>
        /// <param name="relativeTo">If the value passed in is only a time like value, The date portion of RelativeTo is used to fill in the date.</param>
        /// <returns>The DateTime representation</returns>
        public static System.DateTime nzDateTime(object value, DateTime relativeTo)
        {
            DateTime ret = DateTime.MinValue;
            if (value is DateTime)
                return (DateTime) value;
            else if (value != null && value != DBNull.Value)
            {
                string s = nzString(value).Replace("-", "/").Replace("\\", "/");
                if(!DateTime.TryParse(s, out ret))
                {
                    if (s.Length < 6)
                    {
                        if (relativeTo == DateTime.MinValue)
                            return relativeTo;
                        s = relativeTo.Month + "/" + relativeTo.Day + "/" + relativeTo.Year + " " + s;
                    }
                    if (s.IndexOf(" ") > 7)
                    {
                        string t = StringExtensions.LastToken(s, " ");
                        t = t.ToLower().Replace("p", string.Empty).Replace("a", string.Empty).Replace("m", string.Empty);
                        if (t.Length == 4)
                        {
                            s = StringExtensions.FirstToken(s, " ") + " " + t.Substring(0, 2) + ":" + t.Substring(2);
                        }
                        else if (t.Length == 2)
                        {
                            s = StringExtensions.FirstToken(s, " ") + " " + t + ":00";
                        }
                        else if(t.Length == 1)
                        {
                            int i = nzInteger(t);
                            if(i > 0 && i < 7)
                                s = StringExtensions.FirstToken(s, " ") + " " + t + ":00 pm";
                            else
                                s = StringExtensions.FirstToken(s, " ") + " " + t + ":00 am";
                        }
                        else
                            return DateTime.MinValue;
						if (!DateTime.TryParse(s, out ret))
                            ret = DateTime.MinValue;
                    }
                    else ret = DateTime.MinValue;
                }
            }
            return ret;
        }

        ///// <summary></summary>
        ///// <param name="strIn"></param>
        ///// <returns></returns>
        //public static string NormalizeText(object strIn)
        //{
        //    string strText = nzString(strIn);
        //    for (int CurrChar = 1; CurrChar <= strText.Length; CurrChar++)
        //        if ((int)strText[CurrChar - 1] > 127)
        //            strText = strText.Substring(0, CurrChar - 1) + "-" + strText.Substring(CurrChar);
        //    return strText;
        //}
		

		/// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.
		/// <para>The target will limit the date field to a range of dates from the date specified plus 1 month.</para>
		/// </summary>
		/// <param name="tableName">The table to target in the where clause</param>
		/// <param name="sField">The field to target in the where clause</param>
		/// <param name="request">The Request to draw the value from</param>
		/// <param name="conditions">The current where clause to add to</param>
		/// <param name="rootAttributes">The xml string of attributes to add to</param>
		/// <param name="bAllMeansNotNull">True if you want the Request value of "ALL" to add a FieldName + " NOT NULL" clause</param>
		public static void AddConditionMonthRange(string tableName, string sField, HttpRequest request, ref string conditions, ref string rootAttributes, bool bAllMeansNotNull)
		{
			string sValue = nzString(request[sField]);
			if (sValue.Length > 0)
			{
				if (conditions.Length > 0)
				{
					conditions += " AND ";
				}
				if (sValue.ToLower() == "all")
				{
					conditions += FieldNV(tableName + "." + sField, sValue, bAllMeansNotNull);
					rootAttributes += " " + sField + "=\"ALL\"";
				}
				else if (Microsoft.VisualBasic.Information.IsDate(sValue))
				{
                    System.DateTime startDate = nzDateTime(sValue, DateTime.Now);
					conditions += "(" + tableName + "." + sField + " BETWEEN '" + startDate + "' AND '" + startDate.AddMonths(1).AddDays(-1) + "')";
					rootAttributes += " " + sField + "=\"" + Xml.AttributeEncode(sValue) + "\"";
				}
			}
			else
			{
				rootAttributes += " " + sField + "=\"ALL\"";
			}
			if (conditions != null && conditions.Length > 5 && conditions.Substring(conditions.Length - 5) == " AND ")
			{
				conditions = conditions.Substring(0, conditions.Length - 5);
			}
		}


        /// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.</summary>
        /// <param name="tableName">The table to target in the where clause</param>
        /// <param name="sField">The field to target in the where clause</param>
        /// <param name="request">The Request to draw the value from</param>
        /// <param name="conditions">The current where clause to add to</param>
        /// <param name="bAllMeansNotNull">True if you want the Request value of "ALL" to add a FieldName + " NOT NULL" clause</param>
        public static void AddCondition(string tableName, string sField, HttpRequest request, ref string conditions, bool bAllMeansNotNull)
		{
			string sValue = (request[sField] + "").Trim();
			if (sValue.Length >  0)
			{
				if (conditions.Length >  0)
					conditions += " AND ";
				if ( sValue.ToLower() ==  "all" )
					conditions += FieldNV(tableName + "." + sField, sValue, bAllMeansNotNull);
				else if (sValue.Substring(0,3).ToLower() ==  "in(")
				{
					sValue = sValue.Substring(2);
					if (sValue.IndexOf("'") ==  - 1)
						sValue = sValue.Replace("'", "''").Replace("(", "('").Replace(",", "','").Replace(")", "')");
					conditions += "(" + tableName + "." + sField + " IN " + sValue + ")";
				}
				else
					conditions += FieldNV(tableName + "." + sField, sValue, bAllMeansNotNull);
			}
		}


        /// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.</summary>
        /// <param name="tableName">The table to target in the where clause</param>
        /// <param name="sField">The field to target in the where clause</param>
        /// <param name="request">The Request to draw the value from</param>
        /// <param name="conditions">The current where clause to add to</param>
        public static void AddCondition(string tableName, string sField, HttpRequest request, ref string conditions)
		{
			bool bAllMeansNotNull = true;
			string sValue = (request[sField] + "").Trim();
			if (sValue.Length >  0)
			{
				if (conditions.Length >  0)
					conditions += " AND ";
				if (sValue.ToLower() ==  "all")
					conditions += FieldNV(tableName + "." + sField, sValue, bAllMeansNotNull);
				else if (sValue.Substring(0, 3).ToLower() ==  "in(")
				{
					sValue = sValue.Substring(2);
					if (sValue.IndexOf("'") ==  -1)
						sValue = sValue.Replace("'", "''").Replace("(", "('").Replace(",", "','").Replace(")", "')");
					conditions += "(" + tableName + "." + sField + " IN " + sValue + ")";
				}
				else
					conditions += FieldNV(tableName + "." + sField, sValue, bAllMeansNotNull);
			}
		}


		/// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.</summary>
		/// <param name="tableName">The table to target in the where clause</param>
		/// <param name="sField">The field to target in the where clause</param>
		/// <param name="request">The Request to draw the value from</param>
		/// <param name="conditions">The current where clause to add to</param>
		/// <param name="rootAttributes">The string to add to the xml string attributes</param>
		/// <param name="bAllMeansNotNull">True if you want the Request value of "ALL" to add a FieldName + " NOT NULL" clause</param>
		/// <param name="defaultValue">A default value to use if one isn't found in the Request</param>
		public static void AddCondition(string tableName, string sField, HttpRequest request, ref string conditions, ref string rootAttributes,  bool bAllMeansNotNull,  string defaultValue)
		{
			string sValue = (request[sField] + "").Trim();
			if ((sValue).Length ==  0)
				sValue = defaultValue;

			if (sValue.Length >  0)
			{
				if (conditions.Length >  0)
					conditions += " AND ";

				if (sValue.ToLower() ==  "all")
				{
					conditions += FieldNV(tableName + "." + sField, sValue, bAllMeansNotNull);
					rootAttributes += " " + sField + "=\"ALL\"";
				}
				else if ((sValue.Substring(0, 3).ToLower()) ==  "in(")
				{
					sValue = sValue.Substring(2);
					if (sValue.IndexOf("'") ==  - 1)
						sValue = sValue.Replace("'", "''").Replace("(", "('").Replace(",", "','").Replace(")", "')");
					conditions += "(" + tableName + "." + sField + " IN " + sValue + ")";
					rootAttributes += " " + sField + "=\"ALL\"";
				}
				else if ((sValue.Substring(0, 8).ToLower()) ==  "between(")
				{
					sValue = sValue.Substring(8);
                    conditions += "(" + tableName + "." + sField + " BETWEEN " + StringExtensions.TokenAt(sValue.Replace("'", "''"), 1, ",") + " AND " + StringExtensions.TokenAt(sValue.Replace("'", "''"), 2, ",");
					rootAttributes += " " + sField + "=\"ALL\"";
				}
				else
					conditions += FieldNV(tableName + "." + sField, sValue, ref rootAttributes, bAllMeansNotNull);
			}
			else
				rootAttributes += " " + sField + "=\"ALL\"";
			if (conditions.Substring(conditions.Length - 5) ==  " AND ")
				conditions = conditions.Substring(0, conditions.Length - 5);
		}



        /// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.</summary>
        /// <param name="tableName">The table to target in the where clause</param>
        /// <param name="sField">The field to target in the where clause</param>
        /// <param name="request">The Request to draw the value from</param>
        /// <param name="conditions">The current where clause to add to</param>
        /// <param name="rootAttributes">The string containing xml string attributes to add to</param>
        /// <param name="bAllMeansNotNull">True if you want the Request value of "ALL" to add a FieldName + " NOT NULL" clause</param>
        public static void AddCondition(string tableName, string sField, HttpRequest request, ref string conditions, ref string rootAttributes, bool bAllMeansNotNull)
		{
			string defaultValue = string.Empty; 
			string sValue = Worker.nzString(request[sField]);
			if (sValue.Length ==  0)
				sValue = defaultValue;

			if (sValue.Length >  0)
			{
				if (conditions.Length >  0)
					conditions += " AND ";

				if (sValue.ToLower() ==  "all")
				{
					conditions += FieldNV(tableName + "." + sField, sValue, bAllMeansNotNull);
					rootAttributes += " " + sField + "=\"ALL\"";
				}
				else if ((sValue.Substring(0, 3).ToLower()) ==  "in(")
				{
					sValue = sValue.Substring(2);
					if (sValue.IndexOf("'") ==  -1)
						sValue = sValue.Replace("(", "('").Replace(",", "','").Replace(")", "')");
					conditions += "(" + tableName + "." + sField + " IN " + sValue + ")";
					rootAttributes += " " + sField + "=\"ALL\"";
				}
				else if ((sValue.Substring(0, 8).ToLower()) ==  "between(")
				{
					sValue = sValue.Substring(8);
					conditions += "(" + tableName + "." + sField + " BETWEEN " + StringExtensions.TokenAt(sValue, 1, ",") + " AND " + StringExtensions.TokenAt(sValue, 2, ",");
					rootAttributes += " " + sField + "=\"ALL\"";
				}
				else
					conditions += FieldNV(tableName + "." + sField, sValue, ref rootAttributes, bAllMeansNotNull);
			}
			else
				rootAttributes += " " + sField + "=\"ALL\"";
			if (conditions.Substring(conditions.Length - 5) ==  " AND ")
				conditions = conditions.Substring(0, conditions.Length - 5);
		}


        /// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.</summary>
        /// <param name="tableName">The table to target in the where clause</param>
        /// <param name="sField">The field to target in the where clause</param>
        /// <param name="request">The Request to draw the value from</param>
        /// <param name="conditions">The current where clause to add to</param>
        /// <param name="rootAttributes">The string containing xml string attributes to add to</param>
        public static void AddCondition(string tableName, string sField, HttpRequest request, ref string conditions, ref string rootAttributes)
		{
			bool bAllMeansNotNull = true;
			string defaultValue = string.Empty;
			string sValue = nzString(request[sField]);
			if (sValue.Length ==  0)
				sValue = defaultValue;

            if (sValue.Length > 0)
			{
				if ((conditions).Length >  0)
					conditions += " AND ";

                if (sValue.ToLower() == "all")
				{
					conditions += FieldNV(tableName + "." + sField, sValue, bAllMeansNotNull);
					rootAttributes += " " + sField + "=\"ALL\"";
				}
				else if(sValue.Length > 3 && sValue.Substring(0, 3).ToLower() == "in(")
				{
					sValue = sValue.Substring(2);
					if (sValue.IndexOf("'") ==  -1)
						sValue = sValue.Replace("(", "('").Replace(",", "','").Replace(")", "')");
					conditions += "(" + tableName + "." + sField + " IN " + sValue + ")";
					rootAttributes += " " + sField + "=\"ALL\"";
				}
				else if(sValue.Length > 8 && sValue.Substring(0, 8).ToLower() == "between(")
				{
					sValue = sValue.Substring(8);
					conditions += "(" + tableName + "." + sField + " BETWEEN " + StringExtensions.TokenAt(sValue, 1, ",") + " AND " + StringExtensions.TokenAt(sValue, 2, ",");
					rootAttributes += " " + sField + "=\"ALL\"";
				}
				else
					conditions += FieldNV(tableName + "." + sField, sValue, ref rootAttributes, bAllMeansNotNull);
			}
			else
				rootAttributes += " " + sField + "=\"ALL\"";
			if (conditions.Length > 5 && conditions.Substring(conditions.Length - 5) ==  " AND ")
				conditions = conditions.Substring(0, conditions.Length - 5);
		}


		/// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.
		/// <para>The target will limit the date field to a range of dates from the date specified plus 1 month.</para>
		/// </summary>
		/// <param name="sRangeField">The Request value to test</param>
		/// <param name="startDateField">FUTURE</param>
		/// <param name="endDateField">FUTURE</param>
		/// <param name="field">The field to target in the where clause</param>
		/// <param name="request">The Request to draw the value from</param>
		/// <param name="conditions">The current where clause to add to</param>
		/// <param name="sRange">The range type to </param>
		/// <param name="defaultRangeName">FUTURE</param>
		/// <returns>FUTURE</returns>
		public static string AddConditionDateRange(string sRangeField, string startDateField, string endDateField, string field, HttpRequest request, ref string conditions, string sRange,  string defaultRangeName)
		{
			DateTime startDate;
			string ret = string.Empty;

			if ((request[sRangeField] + "").Trim().ToLower().Length >  0)
				sRange = (request[sRangeField] + "").Trim().ToLower();
			else if ((sRange).Length ==  0)
				sRange = defaultRangeName;
			if ((sRange).Length >  0)
			{
				if ((conditions).Length >  0 &&  sRange.ToLower() !=  "everything")
					conditions += " AND ";
				if (sRange.ToLower() ==  "today" &&  DateTime.Now.Hour <  8)
					sRange = "yesterday";
				switch (sRange.ToLower())
				{
					case "yesterday":
						ret += field + " >= '" + DateTime.Today.AddDays(-1) + "' AND " + field + " < '" + DateTime.Today + "'";
						break;
					case "today":
						ret += field + " >= '" + DateTime.Today + "' AND " + field + " < '" + DateTime.Today.AddDays(1) + "'";
						break;
					case "tomorrow":
						ret += field + " >= '" + DateTime.Today.AddDays(1) + "' AND " + field + " < '" + DateTime.Today.AddDays(2) + "'";
						break;
					case "week":
						startDate = DateTime.Today.AddDays(1 - (int) DateTime.Today.DayOfWeek);
						ret += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddDays(7) + "'";
						break;
					case "lastweek":
						startDate = DateTime.Today.AddDays(1 - (int) DateTime.Today.DayOfWeek - 7);
						ret += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddDays(7) + "'";
						break;
					case "month":
						startDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
						ret += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(1) + "'";
						break;
                    case "lastmonth":
                        startDate = DateTime.Today.AddMonths(-1);
                        ret += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(1) + "'";
                        break;
                    case "last3months":
                        startDate = DateTime.Today.AddMonths(-3);
                        conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(3) + "'";
                        break;
                    case "last6months":
                        startDate = DateTime.Today.AddMonths(-6);
                        conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(6) + "'";
                        break;
                    case "nextmonth":
                        startDate = DateTime.Today.AddDays(1 - DateTime.Today.Day).AddMonths(1);
                        conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(1) + "'";
                        break;
                    case "next3months":
                        startDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
                        conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(3) + "'";
                        break;
                    case "next6months":
                        startDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
                        conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(6) + "'";
                        break;
                    case "year":
						ret += " " + field + " >= '01/01/" + DateTime.Today.Year + "' AND " + field + " <= '12/31/" + DateTime.Today.Year + "'";
						break;
					case "everything":
						break;
				}
			}
			else
			{
				if ( (request[startDateField] + "").Trim().Length >  0)
				{
					if ((conditions).Length >  0)
					{
						conditions += " AND ";
						ret += " " + field + " >= '" + (request[startDateField] + "").Trim() + "'";
						sRange = "other";
					}
					if ( (request[endDateField] + "").Trim().Length >  0)
					{
						if ((conditions).Length >  0)
						{
							ret += " AND ";
							if ((request[startDateField] + "").Trim().Length >  0)
							{
                                if (nzDateTime(request[startDateField], DateTime.Now) != nzDateTime(request[endDateField], DateTime.Now))
								{
									ret += " " + field + " < '" + (request[endDateField] + "").Trim() + "'";
									sRange = "other";
								}
								else
								{
                                    ret += " " + field + " < '" + nzDateTime(request[startDateField], DateTime.Now).AddDays(1) + "'";
									sRange = "other";
								}
							}
							else
							{
								ret += " " + field + " < '" + (request[endDateField] + "").Trim() + "'";
								sRange = "other";
							}
						}
					}
				}
			}
			if(ret.Length > 0)
				conditions += ret;
			return ret;
		}



        /// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.
        /// <para>The target will limit the date field to a range of dates from the date specified plus 1 month.</para>
        /// </summary>
        /// <param name="sRangeField">The Request value to test</param>
        /// <param name="startDateField">FUTURE</param>
        /// <param name="endDateField">FUTURE</param>
        /// <param name="field">The field to target in the where clause</param>
        /// <param name="request">The Request to draw the value from</param>
        /// <param name="conditions">The current where clause to add to</param>
        /// <param name="sRange">The range type to </param>
        public static void AddConditionDateRange(string sRangeField, string startDateField, string endDateField, string field, HttpRequest request, ref string conditions, string sRange)
		{
			string defaultRangeName = "everything";
			DateTime startDate;

			if ((request[sRangeField] + "").Trim().ToLower().Length >  0)
				sRange = (request[sRangeField] + "").Trim().ToLower();
			else if ((sRange).Length ==  0)
				sRange = defaultRangeName;
			if ((sRange).Length >  0)
			{
				if ((conditions).Length >  0 &&  sRange.ToLower() !=  "everything")
					conditions += " AND ";
				if (sRange.ToLower() ==  "today" &&  DateTime.Now.Hour <  8)
					sRange = "yesterday";
				switch (sRange.ToLower())
				{
					case "yesterday":
						conditions += field + " >= '" + DateTime.Today.AddDays(-1) + "' AND " + field + " < '" + DateTime.Today + "'";
						break;
					case "today":
						conditions += field + " >= '" + DateTime.Today + "' AND " + field + " < '" + DateTime.Today.AddDays(1) + "'";
						break;
					case "tomorrow":
						conditions += field + " >= '" + DateTime.Today.AddDays(1) + "' AND " + field + " < '" + DateTime.Today.AddDays(2) + "'";
						break;
					case "week":
						startDate = DateTime.Today.AddDays(1 - (int) DateTime.Today.DayOfWeek);
						conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddDays(7) + "'";
						break;
					case "lastweek":
						startDate = DateTime.Today.AddDays(1 - (int) DateTime.Today.DayOfWeek - 7);
						conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddDays(7) + "'";
						break;
					case "month":
						startDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
						conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(1) + "'";
						break;
					case "lastmonth":
						startDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
						conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(1) + "'";
						break;
					case "last3months":
                        startDate = DateTime.Today.AddMonths(-3);
                        conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(3) + "'";
                        break;
					case "last6months":
                        startDate = DateTime.Today.AddMonths(-6);
                        conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(6) + "'";
                        break;
                    case "nextmonth":
                        startDate = DateTime.Today.AddDays(1 - DateTime.Today.Day).AddMonths(1);
                        conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(1) + "'";
                        break;
                    case "next3months":
                        startDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
                        conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(3) + "'";
                        break;
                    case "next6months":
                        startDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
                        conditions += " " + field + " >= '" + startDate + "' AND " + field + " < '" + startDate.AddMonths(6) + "'";
                        break;
                    case "year":
						conditions += " " + field + " >= '01/01/" + DateTime.Today.Year + "' AND " + field + " <= '12/31/" + DateTime.Today.Year + "'";
						break;
					case "everything":
						break;
				}
			}
			else
			{
				if ( (request[startDateField] + "").Trim().Length >  0)
				{
					if ((conditions).Length >  0)
					{
						conditions += " AND ";
						conditions += " " + field + " >= '" + (request[startDateField] + "").Trim() + "'";
						sRange = "other";
					}
					if ( (request[endDateField] + "").Trim().Length >  0)
					{
						if ((conditions).Length >  0)
						{
							conditions += " AND ";
							if ((request[startDateField] + "").Trim().Length >  0)
							{
                                if (nzDateTime(request[startDateField], DateTime.Now) != nzDateTime(request[endDateField], DateTime.Now))
								{
									conditions += " " + field + " < '" + (request[endDateField] + "").Trim() + "'";
									sRange = "other";
								}
								else
								{
                                    conditions += " " + field + " < '" + nzDateTime(request[startDateField], DateTime.Now).AddDays(1) + "'";
									sRange = "other";
								}
							}
							else
							{
								conditions += " " + field + " < '" + (request[endDateField] + "").Trim() + "'";
								sRange = "other";
							}
						}
					}
				}
			}
		}

		
		/// <summary>Adds a full text condition to Conditions</summary>
		/// <param name="tableName">The table to target</param>
		/// <param name="sField">The field to target</param>
		/// <param name="request">The Request to pull the value from</param>
		/// <param name="conditions">The where clausse to add to</param>
		public static void AddFullTextCondition(string tableName, string sField, HttpRequest request, ref string conditions)
		{
			string sValue = (request[sField] + "").Trim().Replace("%", "*").Replace("'", "''");
            if (sValue.Length > 0)
			{
				if (sValue.IndexOf(" ") >  0 &&  sValue.IndexOf("\"") ==  - 1)
					sValue = "\"" + sValue + "\"";
				if ((conditions).Length >  0)
				{
					conditions += " AND ";
				}
                conditions += "(contains( *, '" + sValue + "'))";
            }
		}


        /// <summary>Adds a LIKE text condition to Conditions (eg WHERE X LIKE '%y%')</summary>
        /// <param name="tableName">The table to target</param>
        /// <param name="sField">The field to target</param>
        /// <param name="request">The Request to pull the value from</param>
        /// <param name="conditions">The where clausse to add to</param>
        public static void AddLikeCondition(string tableName, string sField, HttpRequest request, ref string conditions)
		{
			string sValue = LikePattern(sField, request);
            if (sValue.Length > 0)
			{
				if ((conditions).Length >  0)
					conditions += " AND ";
                conditions += "(UPPER(" + tableName + "." + sField + ") LIKE '" + sValue.Replace("'", "''").ToUpper() + "')";
			}
		}


        /// <summary>Adds a LIKE text condition to Conditions (eg WHERE X LIKE '%y%'). The asterick (*) is interpreted like a % (to simplify url encoding)</summary>
        /// <param name="tableName">The table to target</param>
        /// <param name="sField">The field to target</param>
        /// <param name="request">The Request to pull the value from</param>
        /// <param name="conditions">The where clausse to add to</param>
        /// <param name="rootAttributes">The xml attribute string to add to</param>
        public static void AddLikeCondition(string tableName, string sField, HttpRequest request, ref string conditions, ref string rootAttributes)
        {
            string sValue = LikePattern(sField, request);
            if (sValue.Length > 0)
            {
                rootAttributes += " " + sField + "=\"" + Xml.AttributeEncode(request[sField]) + "\"";
                if (conditions.Length > 0)
                    conditions += " AND ";

                if (sValue.IndexOf(" ") == -1)
                    conditions += "(UPPER(" + tableName + "." + sField + ") LIKE " + Worker.s2db(sValue) + ")";
                else
                {
                    sValue = sValue.Replace(" ", "% %");
                    bool isFirst = true;
                    foreach (string currPart in sValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (isFirst)
                            isFirst = false;
                        else
                            conditions += " OR ";
                        conditions += "(UPPER(" + tableName + "." + sField + ") LIKE " + Worker.s2db(currPart) + ")";
                    }
                }
            }
            else
            {
                rootAttributes += " " + sField + "=\"ALL\"";
            }
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

		
		/// <summary>Builds a WHERE clause appropriate phrase for a single field and value</summary>
		/// <param name="sField">The field to target</param>
		/// <param name="sValue">The value. Special cases include "(NULL)" [FieldName + " IS NULL"], "not (NULL)" [FieldName + " IS NOT NULL"], "(BLANK)" [FieldName + "=''"], "(all)" if bAllMeansNotNull=true then [FieldName + " IS NOT NULL"] otherwise a blank string is returned</param>
		/// <param name="bAllMeansNotNull">Controlls how "(all)" is interpreted</param>
		/// <returns>The WHERE clause component</returns>
		public static string FieldNV(string sField, string sValue, bool bAllMeansNotNull)
		{
			if(sValue.Equals("(NULL)"))
				return sField + " IS NULL";
			else if(sValue.Equals("not (NULL)"))
				return "(NOT " + sField + " IS NULL)";
			else if (sValue.Equals("(BLANK)"))
				return sField + "=''";
			else if(sValue.ToLower().Equals("all"))
			{
				if (bAllMeansNotNull)
					return "(NOT " + sField + " IS NULL)";
				else
					return "";
			}
			else if (sValue.Length > 4 && sValue.ToLower().Substring(0, 4).Equals("not "))
                return "(NOT " + sField + "='" + sValue.Replace("'", "''").Substring(4) + "')";
			else
                return sField + "='" + sValue.Replace("'", "''") + "'";
		}


        /// <summary>Builds a WHERE clause appropriate phrase for a single field and value</summary>
        /// <param name="sField">The field to target</param>
        /// <param name="sValue">The value. Special cases include "(NULL)" [FieldName + " IS NULL"], "not (NULL)" [FieldName + " IS NOT NULL"], "(BLANK)" [FieldName + "=''"], "(all)" if bAllMeansNotNull=true then [FieldName + " IS NOT NULL"] otherwise a blank string is returned</param>
        /// <param name="rootAttributes">The xml attribute string to add to</param>
        /// <param name="bAllMeansNotNull">Controlls how "(all)" is interpreted</param>
        /// <returns>The WHERE clause component</returns>
        public static string FieldNV(string sField, string sValue, ref string rootAttributes, bool bAllMeansNotNull)
		{
			sValue = sValue.Replace("[Today]", DateTime.Today.ToString());
			if (sValue ==  "(NULL)")
			{
				rootAttributes += " " + StringExtensions.TokenAt(sField, 2, ".") + "=\"ALL\"";
				return sField + " IS NULL";
			}
			else if (sValue ==  "not (NULL)")
			{
				rootAttributes += " " + StringExtensions.TokenAt(sField, 2, ".") + "=\"ALL\"";
				return "(NOT " + sField + " IS NULL)";
			}
			else if (sValue ==  "(BLANK)")
			{
				rootAttributes += " " + StringExtensions.TokenAt(sField, 2, ".") + "=\"ALL\"";
				return sField + "=''";
			}
			else if (sValue.ToLower() ==  "all")
			{
				rootAttributes += " " + StringExtensions.TokenAt(sField, 2, ".") + "=\"ALL\"";
				if (bAllMeansNotNull)
					return "(NOT " + sField + " IS NULL)";
				else
					return "";
			}
			else if(sValue.Length > 4 && sValue.ToLower().Substring(0, 4).Equals("not "))
			{
				rootAttributes += " " + StringExtensions.TokenAt(sField, 2, ".") + "=\"ALL\"";
                return "(NOT " + sField + "='" + sValue.Replace("'", "''").Substring(4) + "')";
			}
			else if (sValue.Length > 3 && sValue.ToLower().Substring(0, 3) ==  "lt ")
				return "(" + sField + " < '" + sValue.Substring(3).Replace("'", "''") + "')";
			else if (sValue.Length > 3 && sValue.ToLower().Substring(0, 3) ==  "gt ")
                return "(" + sField + " > '" + sValue.Substring(3).Replace("'", "''") + "')";
			else if (sValue.Length > 3 && sValue.ToLower().Substring(0, 3) ==  "le ")
                return "(" + sField + " <= '" + sValue.Substring(3).Replace("'", "''") + "')";
			else if (sValue.Length > 3 && sValue.ToLower().Substring(0, 3) ==  "ge ")
                return "(" + sField + " >= '" + sValue.Substring(3).Replace("'", "''") + "')";
			else
			{
				rootAttributes += " " + StringExtensions.TokenAt(sField, 2, ".") + "=\"" + Xml.AttributeEncode(sValue) + "\"";
                return sField + "='" + sValue.Replace("'", "''") + "'";
			}
		}

		
		/// <summary>Returns a value wrapped in percent (%) ready for use in a WHERE clause</summary>
		/// <param name="sField">The Request field to pull</param>
		/// <param name="request">The Request to pull from</param>
		/// <returns>The WHERE ready LIKE value</returns>
		public static string LikePattern(string sField, HttpRequest request)
		{
			string sValue;
			
			sValue = (request[sField] + "").Trim().Replace("*", "%");
			if (sValue.Length >  0)
			{
				if (sValue.Substring(0, 1) !=  "%" &&  sValue.Substring(sValue.Length - 1, 1) !=  "%")
					sValue = "%" + sValue + "%";
			}
			return sValue;
		}
		
		/// <summary>Translates a string back into a database value where "(NULL)" will return DbNull.Value and "(BLANK)" will return string.Empty </summary>
		/// <param name="sValue">The value to convert</param>
		/// <returns>The converted value</returns>
		public static object Nv(string sValue)
		{
			if (sValue ==  "(NULL)")
				return DBNull.Value;
			else if (sValue ==  "(BLANK)")
				return string.Empty;
			else
				return sValue;
		}

		/// <summary>Converts an object to a string capable of retaining its state. DbNull or null becomes "(NULL)", an empty string becomes "(BLANK)", otherwise the value is returned</summary>
		/// <param name="value">The value to convert</param>
		/// <returns>The converted value</returns>
		public static string Nz(object value)
		{
            if (value == null || DBNull.Value.Equals(value))
				return "(NULL)";
            string x = nzString(value);
			if(x.Length == 0)
				return "(BLANK)";
			else
				return x;
		}


        /// <summary>Converts an object to its boolean represenation. null and DbNull or blank strings return false. Failures to convert return false without raising an exception</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The converted boolean value</returns>
        public static bool NzBoolean(object value)
        {
            if (value == null || value is DBNull)
                return false;
            else if (value is string)
            {
                string v = ((string) value).ToLower();
                if (v.Length == 0 || v == "false" || v == "0")
                    return false;
                if (v == "true" || v == "1")
                    return true;
            }
            try
            {
                return Convert.ToBoolean(value);
            }
            catch { }
            return false;
        }

        /// <summary>Converts an object to its integer represenation. null and DbNull or blank strings return 0. Failures to convert return 0 without raising an exception</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The converted integer value</returns>
        public static short NzShort(object value)
        {
			if (value == null || value is DBNull)
				return 0;
			else if (value is string)
			{
				if (((string)value).Length == 0)
					return 0;
				short ret;
				if (short.TryParse((string)value, out ret))
					return ret;
			}
			try
            {
                return (short)Microsoft.VisualBasic.Conversion.Val(value);
            }
            catch { }
            return 0;
        }

        /// <summary>Converts an object to its integer represenation. null and DbNull or blank strings return 0. Failures to convert return 0 without raising an exception</summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The converted integer value</returns>
        public static long NzLong(object value)
        {
			if (value == null || value is DBNull)
				return 0;
			else if (value is string)
			{
				if (((string)value).Length == 0)
					return 0;
				long ret;
				if (long.TryParse((string)value, out ret))
					return ret;
			}
			try
            {
                return (long)Microsoft.VisualBasic.Conversion.Val(value);
            }
            catch { }
            return 0;
        }

        /// <summary>Converts an object to its integer represenation. null and DbNull or blank strings return 0. Failures to convert return 0 without raising an exception</summary>
		/// <param name="value">The value to convert</param>
		/// <returns>The converted integer value</returns>
		public static int nzInteger(object value)
		{
			if (value == null || value is DBNull)
				return 0;
			else if (value is string)
			{
				if(((string)value).Length == 0)
					return 0;
				int ret;
				if (int.TryParse((string)value, out ret))
					return ret;
			}
            try
            {
                return (int) Microsoft.VisualBasic.Conversion.Val(value);
            }
            catch { }
            return 0;
		}

        /// <summary>Converts an object to its integer represenation. null and DbNull or blank strings return 0. Failures to convert return 0 without raising an exception</summary>
        /// <param name="value">The value to convert</param>
        /// <param name="defaultValue">What to return if Value is DbNull, null, or an empty string.</param>
        /// <returns>The converted integer value</returns>
        public static int nzInteger(object value, int defaultValue)
        {
			if (value == null || value is DBNull)
				return defaultValue;
			else if (value is string)
			{
				if (((string)value).Length == 0)
					return defaultValue;
				int ret;
				if (int.TryParse((string)value, out ret))
					return ret;
			}
			try
            {
                return Convert.ToInt32(value);
            }
            catch { }
            return defaultValue;
        }
    }
}
