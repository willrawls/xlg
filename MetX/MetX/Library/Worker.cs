using System;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Security.Cryptography;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Web;

namespace MetX
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
                sEmail = Token.First(sEmail, "@").Trim() + "@" + Token.First(Token.After(sEmail, 1, "@").ToLower(), "<").Trim();
                return email.IsMatch(sEmail);
            }
            return false;
        }

        public static string Value(HttpRequest Request, System.Web.UI.StateBag ViewState, System.Web.SessionState.HttpSessionState Session, string Key)
        {
            string ret = null;
            if (Session != null) ret = Worker.Value(Session, Key);
            if (string.IsNullOrEmpty(ret)) ret = Request[Key];
            if (ViewState != null && string.IsNullOrEmpty(ret)) ret = Worker.Value(ViewState, Key);
            if (string.IsNullOrEmpty(ret)) ret = Request.Headers[Key];
            if (string.IsNullOrEmpty(ret))
                if (Request.Cookies[Key] != null) ret = Request.Cookies[Key].Value;
            return ret;
        }

        public static string Value(System.Web.UI.StateBag State, string Key)
        {
            string ret = string.Empty;
            if (State != null)
                try { ret = State[Key].ToString(); }
                catch { }
            return ret;
        }

        public static string Value(System.Web.SessionState.HttpSessionState State, string Key)
        {
            string ret = string.Empty;
            if (State != null)
                try { ret = State[Key].ToString(); }
                catch { }
            return ret;
        }

        /// <summary>Returns the string representation of a value, even if it's DbNull, a Guid, or null</summary>
        /// <param name="Value">The value to convert</param>
        /// <param name="DefaultValue">The value to return if Value == null or DbNull or an empty string.</param>
        /// <returns>The string representation</returns>
        public static string nzString(object Value, string DefaultValue)
        {
            if (Value == null || Value == DBNull.Value || Value.Equals(string.Empty))
                return DefaultValue;
            else if (Value is Guid)
                return System.Convert.ToString(Value);
            else
                return Value.ToString().Trim();
        }

        /// <summary>Returns the byte array representation of a value, even if it's DbNull or null</summary>
        /// <param name="Value">The value to convert</param>
        /// <returns>The byte array representation</returns>
        public static byte[] nzByteArray(object Value)
        {
            if (Value == null || Value == DBNull.Value || Value.Equals(string.Empty))
                return new byte[0];
            else if (Value is byte[])
                return (byte[])Value;
            else
                return Encoding.ASCII.GetBytes(nzString(Value));
        }

        /// <summary>Returns the TimeSpan representation of a value, even if it's DbNull or null</summary>
        /// <param name="Value">The value to convert</param>
        /// <returns>The TimeSpan representation</returns>
        public static TimeSpan nzTimeSpan(object Value)
        {
            if (Value == null || Value == DBNull.Value || Value.Equals(string.Empty))
                return new TimeSpan();
            else if (Value is TimeSpan)
                return (TimeSpan) Value;
            else if (Value is string && !string.IsNullOrEmpty((string) Value))
                try { return TimeSpan.Parse((string) Value); }
                catch { }
            return new TimeSpan();
        }

        public static string md5(string ToHash)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(ToHash, "md5").ToLower();
        }

        public static char nzChar(object Value)
        {
            if (Value == null || Value == DBNull.Value || Value.Equals(string.Empty))
                return new char();
            else if (Value is string)
                return ((string)Value)[0];
            else if (Value is char)
                return (char)Value;
            else
            {
                string ret = nzString(Value);
                if (ret.Length == 0)
                    return new char();
                return ret[0];
            }
        }

        public static byte nzByte(object Value)
        {
            if (Value is byte)
                return (byte)Value;
            if (Value is byte[])
                if (((byte[])Value).Length == 0)
                    return new byte();
                else
                    return ((byte[])Value)[0];
            else
                return (byte) (int) nzChar(Value);
        }

		/// <summary>Returns the string representation of a value, even if it's DbNull, a Guid, or null</summary>
		/// <param name="Value">The value to convert</param>
		/// <returns>The string representation</returns>
		public static string nzString(object Value)
		{
			if(Value == null || Value == DBNull.Value || Value.Equals(string.Empty))
				return string.Empty;
			else if( Value is Guid)
				return  System.Convert.ToString(Value);
			else
				return Value.ToString().Trim();
		}

        /// <summary>Returns the double representation of a value, even if it's DbNull, a Guid, or null</summary>
        /// <param name="Value">The value to convert</param>
        /// <returns>The double representation</returns>
        public static double nzDouble(object Value)
        {
            double ret = 0;
            string Text = nzString(Value);
            if (Text.Length == 0)
                ret = 0;
            else 
                try
                {
                    ret = System.Convert.ToDouble(Text);
                }
                catch { }
            return ret;
        }

        /// <summary>Returns the double representation of a value, even if it's DbNull, a Guid, or null</summary>
        /// <param name="Value">The value to convert</param>
        /// <returns>The double representation</returns>
        public static decimal nzDecimal(object Value)
        {
            decimal ret = 0;
            string Text = nzString(Value);
            if (Text.Length == 0)
                ret = 0;
            else
                try
                {
                    ret = System.Convert.ToDecimal(Text);
                }
                catch { }
            return ret;
        }

        /// <summary>Returns a SQL appropriate phrase for an object value. If the object is null or DbNull, the string "NULL" will be returned. Otherwise a single quote delimited string of the value will be created. So if Value='fred', then this function will return "'fred'"</summary>
        /// <param name="Value">The value to convert</param>
        /// <returns>The SQL appropriate phrase</returns>
        /// 
        /// <example><c>string x = "insert into x values(" + s2db(y) + ")";</c></example>
        public static string s2db(object Value)
        {
            if (Value == null || Value == DBNull.Value || (Value is DateTime && (DateTime)Value < DateTime.MinValue.AddYears(10)) || (Value is string && (string)Value == "(NULL)"))
                return "NULL";
            else
                return "'" + nzString(Value).Replace("'", "''") + "'";
        }

        /// <summary>Returns a SQL appropriate phrase for an object value. If the object is null or DbNull, the string "NULL" will be returned. Otherwise a single quote delimited string of the value will be created. So if Value='fred', then this function will return "'fred'"</summary>
        /// <param name="Value">The value to convert</param>
        /// <param name="MaxLength">The maximum length the converted string should be. Values longer will be truncated.</param>
        /// <returns>The SQL appropriate phrase</returns>
        /// 
        /// <example><c>string x = "insert into x values(" + s2db(y) + ")";</c></example>
        public static string s2db(object Value, int MaxLength)
        {
            if (Value == null || Value == DBNull.Value || (Value is DateTime && (DateTime)Value < DateTime.MinValue.AddYears(10)) || (Value is string && (string)Value == "(NULL)"))
                return "NULL";
            else
            {
                string ret = nzString(Value);
                if (MaxLength > 0 && ret.Length > MaxLength)
                    ret = ret.Substring(0, MaxLength);
                return "'" + ret.Replace("'", "''") + "'";
            }
        }

        /// <summary>Returns a SQL appropriate date/time phrase for an object value. If the object is null or DbNull, the string "NULL" will be returned. Otherwise a single quote delimited date/time string of the value will be created.</summary>
		/// <param name="Value">The value to convert</param>
		/// <returns>The SQL appropriate date/time string</returns>
		/// 
		/// <example><c>string x = "insert into x values(" + d2db(y) + ")";</c></example>
		public static string Date2Db(object Value)
		{
            if (Value == null || Value == DBNull.Value || (Value is DateTime && (DateTime)Value < DateTime.MinValue.AddYears(10)) || (Value is string && (string)Value == "(NULL)"))
				return "NULL";
			else
				return "'" + nzDateTime(Value, DateTime.Now).ToString().Replace("'", "''") + "'";
		}

        /// <summary>Returns the Guid representation of a value, even if it's DbNull, a Guid, or null</summary>
        /// <param name="Value">The value to convert</param>
        /// <returns>The Guid representation</returns>
        public static Guid nzGuid(object Value)
        {
            if (Value == null || Value == System.DBNull.Value)
                return Guid.Empty;
            else if (Value is string)
                return new Guid((string)Value);
            else
                return (Guid)Value;
        }

        /// <summary>Returns the DateTime representation of a value, even if it's DbNull or null</summary>
        /// <param name="Value">The value to convert</param>
        /// <returns>The DateTime representation</returns>
        public static System.DateTime nzDateTime(object Value)
        {
            return nzDateTime(Value, DateTime.MinValue);
        }

        /// <summary>Returns the DateTime representation of a value, even if it's DbNull or null</summary>
        /// <param name="Value">The value to convert</param>
        /// <param name="RelativeTo">If the value passed in is only a time like value, The date portion of RelativeTo is used to fill in the date.</param>
        /// <returns>The DateTime representation</returns>
        public static System.DateTime nzDateTime(object Value, DateTime RelativeTo)
        {
            DateTime ret = DateTime.MinValue;
            if (Value is DateTime)
                return (DateTime) Value;
            else if (Value != null && Value != DBNull.Value)
            {
                string s = nzString(Value).Replace("-", "/").Replace("\\", "/");
                if(!DateTime.TryParse(s, out ret))
                {
                    if (s.Length < 6)
                    {
                        if (RelativeTo == DateTime.MinValue)
                            return RelativeTo;
                        s = RelativeTo.Month + "/" + RelativeTo.Day + "/" + RelativeTo.Year + " " + s;
                    }
                    if (s.IndexOf(" ") > 7)
                    {
                        string t = Token.Last(s, " ");
                        t = t.ToLower().Replace("p", string.Empty).Replace("a", string.Empty).Replace("m", string.Empty);
                        if (t.Length == 4)
                        {
                            s = Token.First(s, " ") + " " + t.Substring(0, 2) + ":" + t.Substring(2);
                        }
                        else if (t.Length == 2)
                        {
                            s = Token.First(s, " ") + " " + t + ":00";
                        }
                        else if(t.Length == 1)
                        {
                            int i = nzInteger(t);
                            if(i > 0 && i < 7)
                                s = Token.First(s, " ") + " " + t + ":00 pm";
                            else
                                s = Token.First(s, " ") + " " + t + ":00 am";
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
		/// <param name="TableName">The table to target in the where clause</param>
		/// <param name="sField">The field to target in the where clause</param>
		/// <param name="Request">The Request to draw the value from</param>
		/// <param name="Conditions">The current where clause to add to</param>
		/// <param name="RootAttributes">The xml string of attributes to add to</param>
		/// <param name="bAllMeansNotNull">True if you want the Request value of "ALL" to add a FieldName + " NOT NULL" clause</param>
		public static void AddConditionMonthRange(string TableName, string sField, HttpRequest Request, ref string Conditions, ref string RootAttributes, bool bAllMeansNotNull)
		{
			string sValue = nzString(Request[sField]);
			if (sValue.Length > 0)
			{
				if (Conditions.Length > 0)
				{
					Conditions += " AND ";
				}
				if (sValue.ToLower() == "all")
				{
					Conditions += FieldNV(TableName + "." + sField, sValue, bAllMeansNotNull);
					RootAttributes += " " + sField + "=\"ALL\"";
				}
				else if (Microsoft.VisualBasic.Information.IsDate(sValue))
				{
                    System.DateTime StartDate = nzDateTime(sValue, DateTime.Now);
					Conditions += "(" + TableName + "." + sField + " BETWEEN '" + StartDate + "' AND '" + StartDate.AddMonths(1).AddDays(-1) + "')";
					RootAttributes += " " + sField + "=\"" + xml.AttributeEncode(sValue) + "\"";
				}
			}
			else
			{
				RootAttributes += " " + sField + "=\"ALL\"";
			}
			if (Conditions != null && Conditions.Length > 5 && Conditions.Substring(Conditions.Length - 5) == " AND ")
			{
				Conditions = Conditions.Substring(0, Conditions.Length - 5);
			}
		}


        /// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.</summary>
        /// <param name="TableName">The table to target in the where clause</param>
        /// <param name="sField">The field to target in the where clause</param>
        /// <param name="Request">The Request to draw the value from</param>
        /// <param name="Conditions">The current where clause to add to</param>
        /// <param name="bAllMeansNotNull">True if you want the Request value of "ALL" to add a FieldName + " NOT NULL" clause</param>
        public static void AddCondition(string TableName, string sField, HttpRequest Request, ref string Conditions, bool bAllMeansNotNull)
		{
			string sValue = (Request[sField] + "").Trim();
			if (sValue.Length >  0)
			{
				if (Conditions.Length >  0)
					Conditions += " AND ";
				if ( sValue.ToLower() ==  "all" )
					Conditions += FieldNV(TableName + "." + sField, sValue, bAllMeansNotNull);
				else if (sValue.Substring(0,3).ToLower() ==  "in(")
				{
					sValue = sValue.Substring(2);
					if (sValue.IndexOf("'") ==  - 1)
						sValue = sValue.Replace("'", "''").Replace("(", "('").Replace(",", "','").Replace(")", "')");
					Conditions += "(" + TableName + "." + sField + " IN " + sValue + ")";
				}
				else
					Conditions += FieldNV(TableName + "." + sField, sValue, bAllMeansNotNull);
			}
		}


        /// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.</summary>
        /// <param name="TableName">The table to target in the where clause</param>
        /// <param name="sField">The field to target in the where clause</param>
        /// <param name="Request">The Request to draw the value from</param>
        /// <param name="Conditions">The current where clause to add to</param>
        public static void AddCondition(string TableName, string sField, HttpRequest Request, ref string Conditions)
		{
			bool bAllMeansNotNull = true;
			string sValue = (Request[sField] + "").Trim();
			if (sValue.Length >  0)
			{
				if (Conditions.Length >  0)
					Conditions += " AND ";
				if (sValue.ToLower() ==  "all")
					Conditions += FieldNV(TableName + "." + sField, sValue, bAllMeansNotNull);
				else if (sValue.Substring(0, 3).ToLower() ==  "in(")
				{
					sValue = sValue.Substring(2);
					if (sValue.IndexOf("'") ==  -1)
						sValue = sValue.Replace("'", "''").Replace("(", "('").Replace(",", "','").Replace(")", "')");
					Conditions += "(" + TableName + "." + sField + " IN " + sValue + ")";
				}
				else
					Conditions += FieldNV(TableName + "." + sField, sValue, bAllMeansNotNull);
			}
		}


		/// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.</summary>
		/// <param name="TableName">The table to target in the where clause</param>
		/// <param name="sField">The field to target in the where clause</param>
		/// <param name="Request">The Request to draw the value from</param>
		/// <param name="Conditions">The current where clause to add to</param>
		/// <param name="RootAttributes">The string to add to the xml string attributes</param>
		/// <param name="bAllMeansNotNull">True if you want the Request value of "ALL" to add a FieldName + " NOT NULL" clause</param>
		/// <param name="DefaultValue">A default value to use if one isn't found in the Request</param>
		public static void AddCondition(string TableName, string sField, HttpRequest Request, ref string Conditions, ref string RootAttributes,  bool bAllMeansNotNull,  string DefaultValue)
		{
			string sValue = (Request[sField] + "").Trim();
			if ((sValue).Length ==  0)
				sValue = DefaultValue;

			if (sValue.Length >  0)
			{
				if (Conditions.Length >  0)
					Conditions += " AND ";

				if (sValue.ToLower() ==  "all")
				{
					Conditions += FieldNV(TableName + "." + sField, sValue, bAllMeansNotNull);
					RootAttributes += " " + sField + "=\"ALL\"";
				}
				else if ((sValue.Substring(0, 3).ToLower()) ==  "in(")
				{
					sValue = sValue.Substring(2);
					if (sValue.IndexOf("'") ==  - 1)
						sValue = sValue.Replace("'", "''").Replace("(", "('").Replace(",", "','").Replace(")", "')");
					Conditions += "(" + TableName + "." + sField + " IN " + sValue + ")";
					RootAttributes += " " + sField + "=\"ALL\"";
				}
				else if ((sValue.Substring(0, 8).ToLower()) ==  "between(")
				{
					sValue = sValue.Substring(8);
                    Conditions += "(" + TableName + "." + sField + " BETWEEN " + Token.Get(sValue.Replace("'", "''"), 1, ",") + " AND " + Token.Get(sValue.Replace("'", "''"), 2, ",");
					RootAttributes += " " + sField + "=\"ALL\"";
				}
				else
					Conditions += FieldNV(TableName + "." + sField, sValue, ref RootAttributes, bAllMeansNotNull);
			}
			else
				RootAttributes += " " + sField + "=\"ALL\"";
			if (Conditions.Substring(Conditions.Length - 5) ==  " AND ")
				Conditions = Conditions.Substring(0, Conditions.Length - 5);
		}



        /// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.</summary>
        /// <param name="TableName">The table to target in the where clause</param>
        /// <param name="sField">The field to target in the where clause</param>
        /// <param name="Request">The Request to draw the value from</param>
        /// <param name="Conditions">The current where clause to add to</param>
        /// <param name="RootAttributes">The string containing xml string attributes to add to</param>
        /// <param name="bAllMeansNotNull">True if you want the Request value of "ALL" to add a FieldName + " NOT NULL" clause</param>
        public static void AddCondition(string TableName, string sField, HttpRequest Request, ref string Conditions, ref string RootAttributes, bool bAllMeansNotNull)
		{
			string DefaultValue = string.Empty; 
			string sValue = Worker.nzString(Request[sField]);
			if (sValue.Length ==  0)
				sValue = DefaultValue;

			if (sValue.Length >  0)
			{
				if (Conditions.Length >  0)
					Conditions += " AND ";

				if (sValue.ToLower() ==  "all")
				{
					Conditions += FieldNV(TableName + "." + sField, sValue, bAllMeansNotNull);
					RootAttributes += " " + sField + "=\"ALL\"";
				}
				else if ((sValue.Substring(0, 3).ToLower()) ==  "in(")
				{
					sValue = sValue.Substring(2);
					if (sValue.IndexOf("'") ==  -1)
						sValue = sValue.Replace("(", "('").Replace(",", "','").Replace(")", "')");
					Conditions += "(" + TableName + "." + sField + " IN " + sValue + ")";
					RootAttributes += " " + sField + "=\"ALL\"";
				}
				else if ((sValue.Substring(0, 8).ToLower()) ==  "between(")
				{
					sValue = sValue.Substring(8);
					Conditions += "(" + TableName + "." + sField + " BETWEEN " + Token.Get(sValue, 1, ",") + " AND " + Token.Get(sValue, 2, ",");
					RootAttributes += " " + sField + "=\"ALL\"";
				}
				else
					Conditions += FieldNV(TableName + "." + sField, sValue, ref RootAttributes, bAllMeansNotNull);
			}
			else
				RootAttributes += " " + sField + "=\"ALL\"";
			if (Conditions.Substring(Conditions.Length - 5) ==  " AND ")
				Conditions = Conditions.Substring(0, Conditions.Length - 5);
		}


        /// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.</summary>
        /// <param name="TableName">The table to target in the where clause</param>
        /// <param name="sField">The field to target in the where clause</param>
        /// <param name="Request">The Request to draw the value from</param>
        /// <param name="Conditions">The current where clause to add to</param>
        /// <param name="RootAttributes">The string containing xml string attributes to add to</param>
        public static void AddCondition(string TableName, string sField, HttpRequest Request, ref string Conditions, ref string RootAttributes)
		{
			bool bAllMeansNotNull = true;
			string DefaultValue = string.Empty;
			string sValue = nzString(Request[sField]);
			if (sValue.Length ==  0)
				sValue = DefaultValue;

            if (sValue.Length > 0)
			{
				if ((Conditions).Length >  0)
					Conditions += " AND ";

                if (sValue.ToLower() == "all")
				{
					Conditions += FieldNV(TableName + "." + sField, sValue, bAllMeansNotNull);
					RootAttributes += " " + sField + "=\"ALL\"";
				}
				else if(sValue.Length > 3 && sValue.Substring(0, 3).ToLower() == "in(")
				{
					sValue = sValue.Substring(2);
					if (sValue.IndexOf("'") ==  -1)
						sValue = sValue.Replace("(", "('").Replace(",", "','").Replace(")", "')");
					Conditions += "(" + TableName + "." + sField + " IN " + sValue + ")";
					RootAttributes += " " + sField + "=\"ALL\"";
				}
				else if(sValue.Length > 8 && sValue.Substring(0, 8).ToLower() == "between(")
				{
					sValue = sValue.Substring(8);
					Conditions += "(" + TableName + "." + sField + " BETWEEN " + Token.Get(sValue, 1, ",") + " AND " + Token.Get(sValue, 2, ",");
					RootAttributes += " " + sField + "=\"ALL\"";
				}
				else
					Conditions += FieldNV(TableName + "." + sField, sValue, ref RootAttributes, bAllMeansNotNull);
			}
			else
				RootAttributes += " " + sField + "=\"ALL\"";
			if (Conditions.Length > 5 && Conditions.Substring(Conditions.Length - 5) ==  " AND ")
				Conditions = Conditions.Substring(0, Conditions.Length - 5);
		}


		/// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.
		/// <para>The target will limit the date field to a range of dates from the date specified plus 1 month.</para>
		/// </summary>
		/// <param name="sRangeField">The Request value to test</param>
		/// <param name="StartDateField">FUTURE</param>
		/// <param name="EndDateField">FUTURE</param>
		/// <param name="Field">The field to target in the where clause</param>
		/// <param name="Request">The Request to draw the value from</param>
		/// <param name="Conditions">The current where clause to add to</param>
		/// <param name="sRange">The range type to </param>
		/// <param name="DefaultRangeName">FUTURE</param>
		/// <returns>FUTURE</returns>
		public static string AddConditionDateRange(string sRangeField, string StartDateField, string EndDateField, string Field, HttpRequest Request, ref string Conditions, string sRange,  string DefaultRangeName)
		{
			DateTime StartDate;
			string ret = string.Empty;

			if ((Request[sRangeField] + "").Trim().ToLower().Length >  0)
				sRange = (Request[sRangeField] + "").Trim().ToLower();
			else if ((sRange).Length ==  0)
				sRange = DefaultRangeName;
			if ((sRange).Length >  0)
			{
				if ((Conditions).Length >  0 &&  sRange.ToLower() !=  "everything")
					Conditions += " AND ";
				if (sRange.ToLower() ==  "today" &&  DateTime.Now.Hour <  8)
					sRange = "yesterday";
				switch (sRange.ToLower())
				{
					case "yesterday":
						ret += Field + " >= '" + DateTime.Today.AddDays(-1) + "' AND " + Field + " < '" + DateTime.Today + "'";
						break;
					case "today":
						ret += Field + " >= '" + DateTime.Today + "' AND " + Field + " < '" + DateTime.Today.AddDays(1) + "'";
						break;
					case "tomorrow":
						ret += Field + " >= '" + DateTime.Today.AddDays(1) + "' AND " + Field + " < '" + DateTime.Today.AddDays(2) + "'";
						break;
					case "week":
						StartDate = DateTime.Today.AddDays(1 - (int) DateTime.Today.DayOfWeek);
						ret += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddDays(7) + "'";
						break;
					case "lastweek":
						StartDate = DateTime.Today.AddDays(1 - (int) DateTime.Today.DayOfWeek - 7);
						ret += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddDays(7) + "'";
						break;
					case "month":
						StartDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
						ret += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(1) + "'";
						break;
                    case "lastmonth":
                        StartDate = DateTime.Today.AddMonths(-1);
                        ret += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(1) + "'";
                        break;
                    case "last3months":
                        StartDate = DateTime.Today.AddMonths(-3);
                        Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(3) + "'";
                        break;
                    case "last6months":
                        StartDate = DateTime.Today.AddMonths(-6);
                        Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(6) + "'";
                        break;
                    case "nextmonth":
                        StartDate = DateTime.Today.AddDays(1 - DateTime.Today.Day).AddMonths(1);
                        Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(1) + "'";
                        break;
                    case "next3months":
                        StartDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
                        Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(3) + "'";
                        break;
                    case "next6months":
                        StartDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
                        Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(6) + "'";
                        break;
                    case "year":
						ret += " " + Field + " >= '01/01/" + DateTime.Today.Year + "' AND " + Field + " <= '12/31/" + DateTime.Today.Year + "'";
						break;
					case "everything":
						break;
				}
			}
			else
			{
				if ( (Request[StartDateField] + "").Trim().Length >  0)
				{
					if ((Conditions).Length >  0)
					{
						Conditions += " AND ";
						ret += " " + Field + " >= '" + (Request[StartDateField] + "").Trim() + "'";
						sRange = "other";
					}
					if ( (Request[EndDateField] + "").Trim().Length >  0)
					{
						if ((Conditions).Length >  0)
						{
							ret += " AND ";
							if ((Request[StartDateField] + "").Trim().Length >  0)
							{
                                if (nzDateTime(Request[StartDateField], DateTime.Now) != nzDateTime(Request[EndDateField], DateTime.Now))
								{
									ret += " " + Field + " < '" + (Request[EndDateField] + "").Trim() + "'";
									sRange = "other";
								}
								else
								{
                                    ret += " " + Field + " < '" + nzDateTime(Request[StartDateField], DateTime.Now).AddDays(1) + "'";
									sRange = "other";
								}
							}
							else
							{
								ret += " " + Field + " < '" + (Request[EndDateField] + "").Trim() + "'";
								sRange = "other";
							}
						}
					}
				}
			}
			if(ret.Length > 0)
				Conditions += ret;
			return ret;
		}



        /// <summary>Adds a condition to a where clause and to a set of xml string attributes from a specific Request value targeted at a specific table field.
        /// <para>The target will limit the date field to a range of dates from the date specified plus 1 month.</para>
        /// </summary>
        /// <param name="sRangeField">The Request value to test</param>
        /// <param name="StartDateField">FUTURE</param>
        /// <param name="EndDateField">FUTURE</param>
        /// <param name="Field">The field to target in the where clause</param>
        /// <param name="Request">The Request to draw the value from</param>
        /// <param name="Conditions">The current where clause to add to</param>
        /// <param name="sRange">The range type to </param>
        public static void AddConditionDateRange(string sRangeField, string StartDateField, string EndDateField, string Field, HttpRequest Request, ref string Conditions, string sRange)
		{
			string DefaultRangeName = "everything";
			DateTime StartDate;

			if ((Request[sRangeField] + "").Trim().ToLower().Length >  0)
				sRange = (Request[sRangeField] + "").Trim().ToLower();
			else if ((sRange).Length ==  0)
				sRange = DefaultRangeName;
			if ((sRange).Length >  0)
			{
				if ((Conditions).Length >  0 &&  sRange.ToLower() !=  "everything")
					Conditions += " AND ";
				if (sRange.ToLower() ==  "today" &&  DateTime.Now.Hour <  8)
					sRange = "yesterday";
				switch (sRange.ToLower())
				{
					case "yesterday":
						Conditions += Field + " >= '" + DateTime.Today.AddDays(-1) + "' AND " + Field + " < '" + DateTime.Today + "'";
						break;
					case "today":
						Conditions += Field + " >= '" + DateTime.Today + "' AND " + Field + " < '" + DateTime.Today.AddDays(1) + "'";
						break;
					case "tomorrow":
						Conditions += Field + " >= '" + DateTime.Today.AddDays(1) + "' AND " + Field + " < '" + DateTime.Today.AddDays(2) + "'";
						break;
					case "week":
						StartDate = DateTime.Today.AddDays(1 - (int) DateTime.Today.DayOfWeek);
						Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddDays(7) + "'";
						break;
					case "lastweek":
						StartDate = DateTime.Today.AddDays(1 - (int) DateTime.Today.DayOfWeek - 7);
						Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddDays(7) + "'";
						break;
					case "month":
						StartDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
						Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(1) + "'";
						break;
					case "lastmonth":
						StartDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
						Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(1) + "'";
						break;
					case "last3months":
                        StartDate = DateTime.Today.AddMonths(-3);
                        Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(3) + "'";
                        break;
					case "last6months":
                        StartDate = DateTime.Today.AddMonths(-6);
                        Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(6) + "'";
                        break;
                    case "nextmonth":
                        StartDate = DateTime.Today.AddDays(1 - DateTime.Today.Day).AddMonths(1);
                        Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(1) + "'";
                        break;
                    case "next3months":
                        StartDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
                        Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(3) + "'";
                        break;
                    case "next6months":
                        StartDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
                        Conditions += " " + Field + " >= '" + StartDate + "' AND " + Field + " < '" + StartDate.AddMonths(6) + "'";
                        break;
                    case "year":
						Conditions += " " + Field + " >= '01/01/" + DateTime.Today.Year + "' AND " + Field + " <= '12/31/" + DateTime.Today.Year + "'";
						break;
					case "everything":
						break;
				}
			}
			else
			{
				if ( (Request[StartDateField] + "").Trim().Length >  0)
				{
					if ((Conditions).Length >  0)
					{
						Conditions += " AND ";
						Conditions += " " + Field + " >= '" + (Request[StartDateField] + "").Trim() + "'";
						sRange = "other";
					}
					if ( (Request[EndDateField] + "").Trim().Length >  0)
					{
						if ((Conditions).Length >  0)
						{
							Conditions += " AND ";
							if ((Request[StartDateField] + "").Trim().Length >  0)
							{
                                if (nzDateTime(Request[StartDateField], DateTime.Now) != nzDateTime(Request[EndDateField], DateTime.Now))
								{
									Conditions += " " + Field + " < '" + (Request[EndDateField] + "").Trim() + "'";
									sRange = "other";
								}
								else
								{
                                    Conditions += " " + Field + " < '" + nzDateTime(Request[StartDateField], DateTime.Now).AddDays(1) + "'";
									sRange = "other";
								}
							}
							else
							{
								Conditions += " " + Field + " < '" + (Request[EndDateField] + "").Trim() + "'";
								sRange = "other";
							}
						}
					}
				}
			}
		}

		
		/// <summary>Adds a full text condition to Conditions</summary>
		/// <param name="TableName">The table to target</param>
		/// <param name="sField">The field to target</param>
		/// <param name="Request">The Request to pull the value from</param>
		/// <param name="Conditions">The where clausse to add to</param>
		public static void AddFullTextCondition(string TableName, string sField, HttpRequest Request, ref string Conditions)
		{
			string sValue = (Request[sField] + "").Trim().Replace("%", "*").Replace("'", "''");
            if (sValue.Length > 0)
			{
				if (sValue.IndexOf(" ") >  0 &&  sValue.IndexOf("\"") ==  - 1)
					sValue = "\"" + sValue + "\"";
				if ((Conditions).Length >  0)
				{
					Conditions += " AND ";
				}
                Conditions += "(contains( *, '" + sValue + "'))";
            }
		}


        /// <summary>Adds a LIKE text condition to Conditions (eg WHERE X LIKE '%y%')</summary>
        /// <param name="TableName">The table to target</param>
        /// <param name="sField">The field to target</param>
        /// <param name="Request">The Request to pull the value from</param>
        /// <param name="Conditions">The where clausse to add to</param>
        public static void AddLikeCondition(string TableName, string sField, HttpRequest Request, ref string Conditions)
		{
			string sValue = LikePattern(sField, Request);
            if (sValue.Length > 0)
			{
				if ((Conditions).Length >  0)
					Conditions += " AND ";
                Conditions += "(UPPER(" + TableName + "." + sField + ") LIKE '" + sValue.Replace("'", "''").ToUpper() + "')";
			}
		}


        /// <summary>Adds a LIKE text condition to Conditions (eg WHERE X LIKE '%y%'). The asterick (*) is interpreted like a % (to simplify url encoding)</summary>
        /// <param name="TableName">The table to target</param>
        /// <param name="sField">The field to target</param>
        /// <param name="Request">The Request to pull the value from</param>
        /// <param name="Conditions">The where clausse to add to</param>
        /// <param name="RootAttributes">The xml attribute string to add to</param>
        public static void AddLikeCondition(string TableName, string sField, HttpRequest Request, ref string Conditions, ref string RootAttributes)
        {
            string sValue = LikePattern(sField, Request);
            if (sValue.Length > 0)
            {
                RootAttributes += " " + sField + "=\"" + xml.AttributeEncode(Request[sField]) + "\"";
                if (Conditions.Length > 0)
                    Conditions += " AND ";

                if (sValue.IndexOf(" ") == -1)
                    Conditions += "(UPPER(" + TableName + "." + sField + ") LIKE " + Worker.s2db(sValue) + ")";
                else
                {
                    sValue = sValue.Replace(" ", "% %");
                    bool isFirst = true;
                    foreach (string CurrPart in sValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (isFirst)
                            isFirst = false;
                        else
                            Conditions += " OR ";
                        Conditions += "(UPPER(" + TableName + "." + sField + ") LIKE " + Worker.s2db(CurrPart) + ")";
                    }
                }
            }
            else
            {
                RootAttributes += " " + sField + "=\"ALL\"";
            }
        }
		
		
		/// <summary>Extracts a name from an email address</summary>
		/// <param name="sOriginalText">The Email address</param>
		/// <param name="DefaultName">The default name to use if none is found (or a blank email address is passed in)</param>
		/// <returns>The extracted proper case name</returns>
		public static string EmailToName(string sOriginalText, string DefaultName)
		{
			string Text = (sOriginalText + "").Trim();
			string ReturnValue;

			if (Text.IndexOf("@") > 0 )
				Text = Text.Split('@')[0];

			Text = Text.Replace(".", " ");
			Text = Text.Substring(0,1).ToUpper() + Text.Substring(1, Text.IndexOf(" ")).ToLower() + Text.Substring(Text.IndexOf(" ") + 1,1).ToUpper() + Text.Substring(Text.IndexOf(" ") + 2).ToLower();

			if( Text.Length == 0 )
				ReturnValue = DefaultName;
			else
				ReturnValue = Text;

			return Microsoft.VisualBasic.Strings.StrConv(ReturnValue, Microsoft.VisualBasic.VbStrConv.ProperCase, 0);
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
        /// <param name="RootAttributes">The xml attribute string to add to</param>
        /// <param name="bAllMeansNotNull">Controlls how "(all)" is interpreted</param>
        /// <returns>The WHERE clause component</returns>
        public static string FieldNV(string sField, string sValue, ref string RootAttributes, bool bAllMeansNotNull)
		{
			sValue = sValue.Replace("[Today]", DateTime.Today.ToString());
			if (sValue ==  "(NULL)")
			{
				RootAttributes += " " + Token.Get(sField, 2, ".") + "=\"ALL\"";
				return sField + " IS NULL";
			}
			else if (sValue ==  "not (NULL)")
			{
				RootAttributes += " " + Token.Get(sField, 2, ".") + "=\"ALL\"";
				return "(NOT " + sField + " IS NULL)";
			}
			else if (sValue ==  "(BLANK)")
			{
				RootAttributes += " " + Token.Get(sField, 2, ".") + "=\"ALL\"";
				return sField + "=''";
			}
			else if (sValue.ToLower() ==  "all")
			{
				RootAttributes += " " + Token.Get(sField, 2, ".") + "=\"ALL\"";
				if (bAllMeansNotNull)
					return "(NOT " + sField + " IS NULL)";
				else
					return "";
			}
			else if(sValue.Length > 4 && sValue.ToLower().Substring(0, 4).Equals("not "))
			{
				RootAttributes += " " + Token.Get(sField, 2, ".") + "=\"ALL\"";
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
				RootAttributes += " " + Token.Get(sField, 2, ".") + "=\"" + xml.AttributeEncode(sValue) + "\"";
                return sField + "='" + sValue.Replace("'", "''") + "'";
			}
		}

		
		/// <summary>Returns a value wrapped in percent (%) ready for use in a WHERE clause</summary>
		/// <param name="sField">The Request field to pull</param>
		/// <param name="Request">The Request to pull from</param>
		/// <returns>The WHERE ready LIKE value</returns>
		public static string LikePattern(string sField, HttpRequest Request)
		{
			string sValue;
			
			sValue = (Request[sField] + "").Trim().Replace("*", "%");
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
		public static object NV(string sValue)
		{
			if (sValue ==  "(NULL)")
				return DBNull.Value;
			else if (sValue ==  "(BLANK)")
				return string.Empty;
			else
				return sValue;
		}

		/// <summary>Converts an object to a string capable of retaining its state. DbNull or null becomes "(NULL)", an empty string becomes "(BLANK)", otherwise the value is returned</summary>
		/// <param name="Value">The value to convert</param>
		/// <returns>The converted value</returns>
		public static string NZ(object Value)
		{
            if (Value == null || DBNull.Value.Equals(Value))
				return "(NULL)";
            string x = nzString(Value);
			if(x.Length == 0)
				return "(BLANK)";
			else
				return x;
		}


        /// <summary>Converts an object to its boolean represenation. null and DbNull or blank strings return false. Failures to convert return false without raising an exception</summary>
        /// <param name="Value">The value to convert</param>
        /// <returns>The converted boolean value</returns>
        public static bool nzBoolean(object Value)
        {
            if (Value == null || Value is DBNull)
                return false;
            else if (Value is string)
            {
                string v = ((string) Value).ToLower();
                if (v.Length == 0 || v == "false" || v == "0")
                    return false;
                if (v == "true" || v == "1")
                    return true;
            }
            try
            {
                return Convert.ToBoolean(Value);
            }
            catch { }
            return false;
        }

        /// <summary>Converts an object to its integer represenation. null and DbNull or blank strings return 0. Failures to convert return 0 without raising an exception</summary>
        /// <param name="Value">The value to convert</param>
        /// <returns>The converted integer value</returns>
        public static short nzShort(object Value)
        {
			if (Value == null || Value is DBNull)
				return 0;
			else if (Value is string)
			{
				if (((string)Value).Length == 0)
					return 0;
				short ret;
				if (short.TryParse((string)Value, out ret))
					return ret;
			}
			try
            {
                return (short)Microsoft.VisualBasic.Conversion.Val(Value);
            }
            catch { }
            return 0;
        }

        /// <summary>Converts an object to its integer represenation. null and DbNull or blank strings return 0. Failures to convert return 0 without raising an exception</summary>
        /// <param name="Value">The value to convert</param>
        /// <returns>The converted integer value</returns>
        public static long nzLong(object Value)
        {
			if (Value == null || Value is DBNull)
				return 0;
			else if (Value is string)
			{
				if (((string)Value).Length == 0)
					return 0;
				long ret;
				if (long.TryParse((string)Value, out ret))
					return ret;
			}
			try
            {
                return (long)Microsoft.VisualBasic.Conversion.Val(Value);
            }
            catch { }
            return 0;
        }

        /// <summary>Converts an object to its integer represenation. null and DbNull or blank strings return 0. Failures to convert return 0 without raising an exception</summary>
		/// <param name="Value">The value to convert</param>
		/// <returns>The converted integer value</returns>
		public static int nzInteger(object Value)
		{
			if (Value == null || Value is DBNull)
				return 0;
			else if (Value is string)
			{
				if(((string)Value).Length == 0)
					return 0;
				int ret;
				if (int.TryParse((string)Value, out ret))
					return ret;
			}
            try
            {
                return (int) Microsoft.VisualBasic.Conversion.Val(Value);
            }
            catch { }
            return 0;
		}

        /// <summary>Converts an object to its integer represenation. null and DbNull or blank strings return 0. Failures to convert return 0 without raising an exception</summary>
        /// <param name="Value">The value to convert</param>
        /// <param name="DefaultValue">What to return if Value is DbNull, null, or an empty string.</param>
        /// <returns>The converted integer value</returns>
        public static int nzInteger(object Value, int DefaultValue)
        {
			if (Value == null || Value is DBNull)
				return DefaultValue;
			else if (Value is string)
			{
				if (((string)Value).Length == 0)
					return DefaultValue;
				int ret;
				if (int.TryParse((string)Value, out ret))
					return ret;
			}
			try
            {
                return Convert.ToInt32(Value);
            }
            catch { }
            return DefaultValue;
        }
    }
}
