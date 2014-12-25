using System;
using System.Text;

namespace MetX
{
	/// <summary>Provides simple methods for retrieving tokens from a string.
	/// <para>A token is a piece of a delimited string. For instance in the string "this is a test" when " " (a space) is used as a delimiter, "this" is the first token and "test" is the last (4th) token.</para>
	/// <para>Asking for a token beyond the end of a string returns a blank string. Asking for the zeroth or a negative token returns a blank string.</para>
	/// </summary>
	public static class Token
	{
		public static string Between(string Target, string LeftDelimiter, string RightDelimiter)
		{
			if ( string.IsNullOrEmpty(Target) )
				return string.Empty;
			return Get(Get(Target, 2, LeftDelimiter), 1, RightDelimiter);
		}

		/// <summary>Returns the number of tokens in a string</summary>
		/// <param name="Target">The string to target</param>
		/// <param name="TokenDelimiter">The token delimiter</param>
		/// 
		/// <example>
		/// <code>
		/// int x = Token.Count("this is a test", "is");
		/// // x = 2;
		/// x = Token.Count("this is a test", " ");
		/// // x = 4;
		/// </code>
		/// </example>
		public static int Count(string Target, string TokenDelimiter)
		{
			int iCurTokenLocation;					//  Character position of the first delimiter string
			int iTokensSoFar;						//  Used to keep track of how many tokens we've counted so far
			int iDelim;								//  Length of the delimiter string

			iDelim = TokenDelimiter.Length;
			if ( iDelim <  1 )
				return 1;							//  Empty delimiter strings means only one token equal to the string
			else if ( Target.Length ==  0 )
				return 0; 							//  Empty input string means no tokens
			else  									//  Count the number of tokens
			{
				iTokensSoFar = 0;
				do
				{
					iCurTokenLocation = Microsoft.VisualBasic.Strings.InStr(Target, TokenDelimiter, Microsoft.VisualBasic.CompareMethod.Text) - 1;
					if ( iCurTokenLocation ==  -1 )
					{
						return ++iTokensSoFar;	// Abs(Len(Target) > 0)
					}
					iTokensSoFar += 1;
					Target = Target.Substring(iCurTokenLocation + iDelim);
				} while ( true );
			}
		}

		/// <summary>Returns the number of " " (space) delimited tokens in a string. IE: returns the number of words in a string.</summary>
		/// <param name="Target">The string to target</param>
		/// <returns>The number of tokens (words) in the string</returns>
		public static int Count(string Target)
		{
			return Count(Target, " ");
		}

		/// <summary>Returns all tokens after the indicated token.</summary>
		/// <param name="Target">The string to target</param>
		/// <param name="TokenNumber">The token number to return after</param>
		/// <param name="TokenDelimiter">The token delimiter</param>
		/// 
		/// <example>
		/// <code>
		/// string x = Token.After("this is a test", 2, " ");
		/// // x = "a test"
		/// </code>
		/// </example>
		public static string After(string Target, int TokenNumber, string TokenDelimiter)
		{
			int iCurTokenLocation;						//  Character position of the first delimiter string
			int nDelim;									//  Length of the delimiter string
			nDelim = TokenDelimiter.Length;
			if ( TokenNumber <  1 ||  nDelim <  1 )			//  Negative or zeroth token or empty delimiter strings mean an empty token
				return Target;
			else if ( TokenNumber ==  1 )						//  Quickly extract the first token
			{
				iCurTokenLocation = Microsoft.VisualBasic.Strings.InStr(Target, TokenDelimiter, Microsoft.VisualBasic.CompareMethod.Text) - 1;
				if ( iCurTokenLocation >  1 )
					return Target.Substring(iCurTokenLocation + nDelim);
				else if ( iCurTokenLocation ==  -1 )
					return null;
				else
					return Target.Substring(iCurTokenLocation + nDelim);
			}
			else										//  Find the Nth token
			{
				do
				{
					iCurTokenLocation = Microsoft.VisualBasic.Strings.InStr(Target, TokenDelimiter, Microsoft.VisualBasic.CompareMethod.Text) - 1;
					if ( iCurTokenLocation ==  -1 )
						return null;
					else
						Target = Target.Substring(iCurTokenLocation + nDelim);
					TokenNumber -= 1;
				} while ( TokenNumber > 1 );								//  Extract the Nth token (Which is the next token at this point)

				iCurTokenLocation = Microsoft.VisualBasic.Strings.InStr(Target, TokenDelimiter, Microsoft.VisualBasic.CompareMethod.Text) - 1;

				if ( iCurTokenLocation >  0 )
					return Target.Substring(iCurTokenLocation + nDelim);
				else
					return null;
			}
		}

		/// <summary>Returns all tokens after the indicated " " (space) delimited token</summary>
		/// <param name="Target">The string to target</param>
		/// <param name="TokenNumber">The token number to return after</param>
		/// 
		/// <example>
		/// <code>
		/// string x = Token.After("this is a test", 2);
		/// // x = "a test"
		/// </code>
		/// </example>
		public static string After(string Target, int TokenNumber)
		{
			return After(Target, TokenNumber, " ");
		}

		/// <summary>Returns all tokens after the first " " (space) delimited token</summary>
		/// <param name="Target">The string to target</param>
		/// <returns>All tokens after the first</returns>
		/// 
		/// <example>
		/// <code>
		/// string x = Token.After("this is a test");
		/// // x = "is a test"
		/// </code>
		/// </example>
		public static string After(string Target)
		{
			return After(Target, 1, " ");
		}

		/// <summary>Returns all tokens before the indicated token.</summary>
		/// <param name="Target">The string to target</param>
		/// <param name="TokenNumber">The token number to return before</param>
		/// <param name="TokenDelimiter">The token delimiter</param>
		/// 
		/// <example>
		/// <code>
		/// string x = Token.Before("this is a test", 3, " ");
		/// // x = "this is"
		/// </code>
		/// </example>
		public static string Before(string Target, long TokenNumber, string TokenDelimiter)
		{
			int iCurTokenLocation;				    //  Character position of the first delimiter string
			int nDelim = TokenDelimiter.Length;     //  Length of the delimiter string
			if ( TokenNumber <  2 ||  nDelim <  1 )	//  First, Zeroth, or Negative tokens or empty delimiter strings mean an empty string returned
				return null;
			else if ( TokenNumber ==  2 )
				return Get(Target, 1, TokenDelimiter);  //  Quickly extract the first token
			else
			{
				//  Find the Nth token
				StringBuilder sReturned = new StringBuilder();
				do
				{
					iCurTokenLocation = Microsoft.VisualBasic.Strings.InStr(Target, TokenDelimiter, Microsoft.VisualBasic.CompareMethod.Text) - 1;
					if ( iCurTokenLocation == -1 || TokenNumber == 1 )
					{
						if ( TokenNumber > 1 )
						{
							if ( Target.Length > 0 )
							{
								sReturned.Append(TokenDelimiter);
								sReturned.Append(Target);
							}
						}
						return sReturned.ToString();
					}
					else if ( sReturned.Length == 0 )
						sReturned.Append(Target.Substring(0, iCurTokenLocation));
					else
					{
						sReturned.Append(TokenDelimiter);
						sReturned.Append(Target.Substring(0, iCurTokenLocation));
					}
					Target = Target.Substring(iCurTokenLocation + nDelim);
					TokenNumber -= 1;
				}
				while ( true );
			}
		}

		/// <summary>Returns all tokens before the indicated " " (space) delimited token.</summary>
		/// <param name="Target">The string to target</param>
		/// <param name="TokenNumber">The token number to return before</param>
		/// 
		/// <example>
		/// <code>
		/// string x = Token.Before("this is a test", 3);
		/// // x = "this is"
		/// </code>
		/// </example>
		public static string Before(string Target, long TokenNumber)
		{
			return Before(Target, TokenNumber, " ");
		}

		/// <summary>Returns the first token in the indicated string</summary>
		/// <param name="Target">The string to target</param>
		/// <returns>The first token (word) in the string</returns>
		/// 
		/// <example>
		/// <code>
		/// string x = Token.First("this is a test");
		/// // x = "this"
		/// </code>
		/// </example>
		public static string First(string Target)
		{
			return Get(Target, 1, " ");
		}

		/// <summary>Returns the first delimited token in the indicated string</summary>
		/// <param name="Target">The string to target</param>
		/// <param name="TokenDelimiter">The token delimiter</param>
		/// 
		/// <example>
		/// <code>
		/// string x = Token.First("this is a test", " a ");
		/// // x = "this is"
		/// </code>
		/// </example>
		public static string First(string Target, string TokenDelimiter)
		{
			return Get(Target, 1, TokenDelimiter);
		}

		/// <summary>Returns a single delimited token from a string</summary>
		/// <param name="Target">The string to target</param>
		/// <param name="TokenNumber">The token to return</param>
		/// <param name="TokenDelimiter">The token delimiter</param>
		public static string Get(string Target, int TokenNumber, string TokenDelimiter)
		{
			int iCurTokenLocation;
			int nDelim = TokenDelimiter.Length;
			if ( TokenNumber < 1 || nDelim < 1 || Target == null || Target.Length == 0 )
				//  Negative or zeroth token or empty delimiter strings mean an empty token
				return string.Empty;
			//  Quickly extract the first token
			else if ( TokenNumber ==  1 )
			{

				iCurTokenLocation = Microsoft.VisualBasic.Strings.InStr(Target, TokenDelimiter, Microsoft.VisualBasic.CompareMethod.Text) - 1;
				if ( iCurTokenLocation > 0 )
					return Target.Substring(0, iCurTokenLocation);
				else if ( iCurTokenLocation == 0 )
					return string.Empty;
				else
					return Target;
			}
			//  Find the Nth token
			else
			{
				while ( TokenNumber > 1 )
				{
					iCurTokenLocation = Microsoft.VisualBasic.Strings.InStr(Target, TokenDelimiter, Microsoft.VisualBasic.CompareMethod.Text) - 1;
					if ( iCurTokenLocation == -1 )
						return string.Empty;
					else
						Target = Target.Substring(iCurTokenLocation + nDelim);
					TokenNumber -= 1;
				}
				//  Extract the Nth token (Which is the next token at this point)
				iCurTokenLocation = Microsoft.VisualBasic.Strings.InStr(Target, TokenDelimiter, Microsoft.VisualBasic.CompareMethod.Text) - 1;
				if ( iCurTokenLocation >  0 )
					return Target.Substring(0, iCurTokenLocation);
				else
					return Target;
			}
		}

        /// <summary>Returns the last delimited token from a string</summary>
        /// <param name="Target">The string to target</param>
        /// <param name="TokenDelimiter">The token delimiter</param>
        public static string Last(string Target, string TokenDelimiter)
        {
            return Get(Target, Count(Target, TokenDelimiter), TokenDelimiter);
        }

        /// <summary>Returns everything before the last delimited token from a string</summary>
        /// <param name="Target">The string to target</param>
        /// <param name="TokenDelimiter">The token delimiter</param>
        public static string BeforeLast(string Target, string TokenDelimiter)
        {
            return Before(Target, Count(Target, TokenDelimiter), TokenDelimiter);
        }

        /// <summary>Returns everything after the first delimited token from a string</summary>
		/// <param name="Target">The string to target</param>
		/// <param name="TokenDelimiter">The token delimiter</param>
		public static string AfterFirst(string Target, string TokenDelimiter)
		{
			return After(Target, 1, TokenDelimiter);
		}

	}
}
