using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.XPath;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Strings;

namespace MetX.Standard.Library.ML
{

    /// <summary>This class is automatically made available as urn:xlg while rendering xsl pages from any of the MetX.Web xsl rendering classes. Each function provides some string, date, and totaling capability as well as some basic variable storage that can survive template calls.
    /// <para>NOTE: XSL functions must return a variable. Most functions in this library normally wouldn't return anything, but to accomodate XSL, they return a blank string.</para>
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class XlgUrn
    {
        private int _mNextId;
        private int _mNextHash = 1;
        private int _mNextLayer = 10000;
        private string _mCurrRowClass = "contentDataRow2";
        private string _mNextRowClass = "contentDataRow1";
        private Dictionary<string, double> _mRunningTotals;
        private Dictionary<string, string> _mHt;
        private Dictionary<string, string> _mVars;
        private Dictionary<string, StringBuilder> _mSbVars;

        /// <summary>
        /// Returns if one or more bits are set in ToCheck
        /// </summary>
        /// <param name="toCheck">The Integer to check</param>
        /// <param name="mask">The bit mask to check</param>
        /// <returns>true if all bits set in Mask are set in ToCheck, else false</returns>
        public bool HasBit(int toCheck, int mask)
        {
            return (toCheck & mask) != 0;
        }

        /// <summary>
        /// Returns the URL encoded value of the string passed in
        /// </summary>
        /// <param name="toEncode">The string to URL encode</param>
        /// <returns>The URL encoded string</returns>
        public string UrlEncode(string toEncode)
        {
            return toEncode == null ? string.Empty : WebUtility.UrlEncode(toEncode);
        }

        /// <summary>
        /// Same as calling System.IO.File.Exists
        /// </summary>
        /// <param name="pathAndFilename">Path and filename to the file</param>
        /// <returns>True if the file exists</returns>
        public bool FileExists(string pathAndFilename)
        {
            return File.Exists(pathAndFilename);
        }


        /// <summary>Same as calling System.IO.Directory.Exists</summary>
        /// <param name="path">The path to test existence for</param>
        /// <returns>True if the folder exists</returns>
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>Same as calling System.IO.Directory.Create</summary>
        /// <param name="path">The path to create</param>
        /// <returns>True if the folder exists</returns>
        public string CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return string.Empty;
        }

        /// <summary>Same as calling System.IO.Directory.Create</summary>
        /// <param name="path">The path to create</param>
        /// <returns>True if the folder is created, false if it already existed</returns>
        public bool InsureDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return false;
            }
            Directory.CreateDirectory(path);
            return true;
        }

        /// <summary>
        /// Appends a string to an internal string builder
        /// </summary>
        /// <param name="sbVarName">The name of the string builder to append to</param>
        /// <param name="toAppend">The string to append</param>
        /// <returns>An empty string</returns>
        public string SbAppend(string sbVarName, string toAppend)
        {
            _mSbVars ??= new Dictionary<string, StringBuilder>();
            if (_mSbVars.ContainsKey(sbVarName))
                _mSbVars[sbVarName].Append(toAppend);
            else
                _mSbVars[sbVarName] = new StringBuilder(toAppend);
            return string.Empty;
        }


        /// <summary>
        /// Appends a string to an internal string builder with a new line character at the end
        /// </summary>
        /// <param name="sbVarName">The name of the string builder to append to</param>
        /// <param name="toAppend">The string to append</param>
        /// <returns>An empty string</returns>
        public string SbAppendLine(string sbVarName, string toAppend)
        {
            _mSbVars ??= new Dictionary<string, StringBuilder>();
            if (!_mSbVars.ContainsKey(sbVarName))
                _mSbVars[sbVarName] = new StringBuilder();
            _mSbVars[sbVarName].AppendLine(toAppend);
            return string.Empty;
        }


        /// <summary>
        /// Retrieves the contents of an internal string builder
        /// </summary>
        /// <param name="sbVarName">The string builder to retrieve</param>
        /// <returns>The string builder contents</returns>
        public string SbGetVar(string sbVarName)
        {
            if (_mSbVars != null && _mSbVars.ContainsKey(sbVarName))
                return _mSbVars[sbVarName].ToString();
            return string.Empty;
        }


        /// <summary>
        /// Removes an internal string builder
        /// </summary>
        /// <param name="sbVarName">The string builder to remove</param>
        /// <returns>An empty string</returns>
        public string SbRemove(string sbVarName)
        {
            if (_mSbVars != null && _mSbVars.ContainsKey(sbVarName))
                _mSbVars.Remove(sbVarName);
            return string.Empty;
        }


        /// <summary>Removes a specific internal variable set by SetVar</summary>
        /// <param name="varName">The variable to remove</param>
        /// <returns>a blank string</returns>
        public string RemoveVar(string varName)
        {
            _mVars ??= new Dictionary<string, string>();
            if (_mVars.ContainsKey(varName))
                _mVars.Remove(varName);
            return string.Empty;
        }


        /// <summary>Clears all internal (non-xsl) variables</summary>
        /// <returns>a blank string</returns>
        public string ClearVars()
        {
            _mVars?.Clear();
            return string.Empty;
        }


        /// <summary>Sets an internal variable to some value. This value will persist until changed or until the end of rendering</summary>
        /// <param name="varName">The variable name to set</param>
        /// <param name="varValue">The value of the variable</param>
        /// <returns>a blank string</returns>
        public string SetVar(string varName, string varValue)
        {
            _mVars ??= new Dictionary<string, string>();
            _mVars[varName] = varValue;
            return string.Empty;
        }


        /// <summary>Returns the value of an internal variable or a blank string if that variable wasn't set.</summary>
        /// <param name="varName">The variable to retrieve</param>
        /// <returns>The variables value or a blank string if not set</returns>
        public string GetVar(string varName)
        {
            _mVars ??= new Dictionary<string, string>();
            if (_mVars.ContainsKey(varName))
                return _mVars[varName];
            return string.Empty;
        }


        /// <summary>Returns true if the variable has been set and if the value's length is greater than zero</summary>
        /// <param name="varName">The variable to test.</param>
        /// <returns>True if the variable has been set to a non empty string</returns>
        public bool IsVarSet(string varName)
        {
            _mVars ??= new Dictionary<string, string>();
            if (_mVars.ContainsKey(varName) && _mVars[varName] != null && _mVars[varName].Length > 0)
                return true;
            return false;
        }


        /// <summary>Creates an incrementing number for the ToHash value and stores it in a table. Each time afterward sHash is called with ToHash, the same value will be returned. Among other things, useful for generating a script block and a short unique div name.</summary>
        /// <param name="toHash">The item to hash</param>
        /// <returns>The hash value for the item</returns>
        public string sHash(string toHash)
        {
            _mHt ??= new Dictionary<string, string>();
            // ReSharper disable once InvertIf
            if (!_mHt.ContainsKey(toHash))
            {
                _mHt.Add(toHash, _mNextHash.ToString());
                _mNextHash += 1;
            }
            return _mHt[toHash];
        }


        /// <summary>Replaces one string with another. Equivalent to calling string.Replace</summary>
        /// <param name="toSearch">The text to do the replacement on</param>
        /// <param name="toFind">The text to find</param>
        /// <param name="toReplace">The text to replace</param>
        /// <returns>The string with text replaced.</returns>
        public string SReplace(string toSearch, string toFind, string toReplace)
        {
            return toSearch.Replace(toFind, toReplace);
        }


        /// <summary>Returns an xml string of the Inner text of one or more Nodes</summary>
        /// <param name="nodes">The nodes to return InnerXml for</param>
        /// <returns>The inner xml string of the nodes</returns>
        public string InnerXml(XPathNodeIterator nodes)
        {
            if (nodes == null)
            {
                return string.Empty;
            }

            nodes.MoveNext();
            var nameValueNodes = nodes.Current;
            return nameValueNodes?.InnerXml;
        }


        /// <summary>Returns an xml string of the Outer text of one or more Nodes</summary>
        /// <param name="nodes">The nodes to return OuterXml for</param>
        /// <returns>The outer xml string of the nodes</returns>
        public string OuterXml(XPathNodeIterator nodes)
        {
            if (nodes == null)
            {
                return string.Empty;
            }

            nodes.MoveNext();
            var nameValueNodes = nodes.Current;
            return nameValueNodes?.OuterXml;
        }


        /// <summary>Returns the OuterXml of Nodes in javascript format set to the javascript variable VarName. So this generates a single (usually very long) line of javascript.</summary>
        /// <param name="varName">The javascript variable name to place the outerxml into</param>
        /// <param name="nodes">The nodes to javascript code</param>
        /// <returns>One line of javascript of the form var VarName = "outerxml of Nodes";</returns>
        public string OuterXmlJson(string varName, XPathNodeIterator nodes)
        {
            var ret = new StringBuilder(OuterXml(nodes));
            if (ret.Length > 0)
            {
                ret.Replace("\"", "\\\"");
                ret.Replace(Environment.NewLine, "\\n");
                ret.Replace("\r", "\\n");
                ret.Replace("\n", "\\n");
                ret.Insert(0, "var " + varName + " = \"<?xml version=\\\"1.0\\\" encoding=\\\"UTF-8\\\" ?>\\n");
                ret.Append("\";");
            }
            return ret.ToString();
        }


        /// <summary>Returns the maximum of two dates</summary>
        /// <param name="sDate1">The first date to compare</param>
        /// <param name="sDate2">The second date to compare</param>
        /// <returns>The greater date</returns>
        public string DateMax(string sDate1, string sDate2)
        {
            if (sDate1.Length > 0)
            {
                if (sDate2.Length > 0)
                {
                    var date1 = DateTime.Parse(sDate1);
                    var date2 = DateTime.Parse(sDate2);
                    if (date1 > date2)
                        return sDate1;
                    else
                        return sDate2;
                }
                else
                    return sDate1;
            }
            return sDate2;
        }


        /// <summary>Determines if one date is between or equal to two other dates</summary>
        /// <param name="sDateToTest">The date to test</param>
        /// <param name="sDateBegin">The lower boundary to test</param>
        /// <param name="sDateEnd">The upper boundary to test</param>
        /// <returns>true if sDateToTest is between sDateBegin and sDateEnd</returns>
        public bool DateIsInRange(string sDateToTest, string sDateBegin, string sDateEnd)
        {
            if (sDateToTest.Length > 0)
            {
                if (sDateBegin.Length > 0)
                {
                    if (sDateEnd.Length > 0)
                    {
                        var dateToTest = DateTime.Parse(sDateToTest);
                        var dateBegin = DateTime.Parse(sDateBegin);
                        var dateEnd = DateTime.Parse(sDateEnd);
                        if (dateToTest >= dateBegin && dateToTest <= dateEnd)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        /// <summary>Retrieves the nth delimited token</summary>
        /// <param name="allTokens">The string to retrieve a token from</param>
        /// <param name="n">The token number to retrieve</param>
        /// <param name="delimiter">The delimiter separating each token</param>
        /// <returns>The requested token or a blank string</returns>
        public string GetToken(string allTokens, int n, string delimiter)
        {
            return allTokens.TokenAt(n, delimiter);
        }


        /// <summary>Returns the index of a string inside another string. Equivalent to string.IndexOf</summary>
        /// <param name="toSearch">The string to search</param>
        /// <param name="toFind">The string to find</param>
        /// <returns>The index of ToFind in ToSearch. -1 if nothing is found</returns>
        public int IndexOf(string toSearch, string toFind)
        {
            return toSearch.IndexOf(toFind, StringComparison.Ordinal);
        }

        /// <summary>Determines if the date passed in is today regardless of the time of day</summary>
        /// <param name="dateStringToTest">The date to test</param>
        /// <returns>true if the date is between 00:00am and 11:59pm today</returns>
        public bool IsToday(string dateStringToTest)
        {
            if (DateTime.TryParse(dateStringToTest, out var convertedDateTime))
            {
                return convertedDateTime >= DateTime.Today 
                       && convertedDateTime < DateTime.Today.AddDays(1);
            }
            return false;
        }

        /// <summary>Determines if a date is in the past</summary>
        /// <param name="dateStringToTest">The date to test</param>
        /// <returns>true if the date is in the past (even by a second)</returns>
        public bool IsInThePast(string dateStringToTest)
        {
            if (DateTime.TryParse(dateStringToTest, out var convertedDateTime))
            {
                return DateTime.Now.Subtract(convertedDateTime).TotalSeconds < 0;
            }
            return false;
        }

        /// <summary>Returns the proper case of a string (such as a name). So "this is a test" becomes "This Is A Test".</summary>
        /// <param name="text">The text to proper case</param>
        /// <returns>The proper case string</returns>
        public string ProperCase(string text)
        {
            return text.ProperCase();
        }


        /// <summary>Returns the current date and time</summary>
        /// <returns>The current date/time</returns>
        public string Today()
        {
            return DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>Returns the double representation of a string</summary>
        /// <param name="sOriginalText">The string to convert</param>
        /// <returns>The double representation of a string</returns>
        public double NzDouble(string sOriginalText)
        {
            return ForDouble.NzDouble(sOriginalText);
        }


        /// <summary>Extracts a name from an email address</summary>
        /// <param name="sOriginalText">The Email address</param>
        /// <returns>The extracted proper case name</returns>
        public string EmailToName(string sOriginalText)
        {
            string defaultName = string.Empty;
            var text = (sOriginalText + "").Trim();

            if (text.IndexOf("@", StringComparison.Ordinal) > 0 )
                text = text.Split('@')[0];

            text = text.Replace(".", " ");
            text = text.Substring(0,1).ToUpper() + text.Substring(1, text.IndexOf(" ", StringComparison.Ordinal)).ToLower() + text.Substring(text.IndexOf(" ", StringComparison.Ordinal) + 1,1).ToUpper() + text.Substring(text.IndexOf(" ", StringComparison.Ordinal) + 2).ToLower();

            var returnValue = (text.Length == 0 ? defaultName : text).ProperCase();
            return returnValue;
        }


        /// <summary>Coverts the passed string to lowercase</summary>
        /// <param name="sOriginalText">The text to convert</param>
        /// <returns>The lowercase version of the string</returns>
        public string Lower(string sOriginalText)
        {
            return sOriginalText.AsString().ToLower();
        }


        /// <summary>Coverts the passed string to uppercase</summary>
        /// <param name="sOriginalText">The text to convert</param>
        /// <returns>The uppercase version of the string</returns>
        public string Upper(string sOriginalText)
        {
            return sOriginalText.AsString().ToUpper();
        }

        public string Camel(string sOriginalText)
        {
            if (string.IsNullOrEmpty(sOriginalText))
                return string.Empty;
            var ret = sOriginalText.AsString();
            ret = ret == ret.ToUpper() 
                ? ret.ToLower() 
                : ret[0].ToString().ToLower() + ret.Substring(1);
            return ret;
        }

        public string Proper(string sOriginalText)
        {
            if (string.IsNullOrEmpty(sOriginalText))
                return string.Empty;
            var ret = sOriginalText.AsString();
            ret = ret[0].ToString().ToUpper() + ret.Substring(1);
            return ret;
        }

        /// <summary>Returns the number of days old the date passed in is or a blank string if it isn't a valid date</summary>
        /// <param name="xmlDate">The date to calculate</param>
        /// <returns>The number of days old the date is</returns>
        public string SXmlAgeOfDate(string xmlDate)
        {
            xmlDate = Convert.ToString(xmlDate + string.Empty).Trim();
            if (xmlDate.Length > 0)
            {
                var dt = Convert.ToDateTime(xmlDate);
                dt = dt.AddHours(-dt.Hour).AddMinutes(-dt.Minute);
                var ts = DateTime.Today.Subtract(dt);
                return ts.Days.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Takes a "StringLikeThis" and adds spaces at each upper case letter, except consecutive upper letters, 
        /// to make a "String Like This"
        /// </summary>
        /// <param name="target">The string to expand</param>
        /// <returns>The space expanded string</returns>
        public string Expand(string target)
        {
            if (string.IsNullOrEmpty(target))
                return string.Empty;

            var sb = new StringBuilder();
            for (var i = 0; i < target.Length; i++)
            {
                var curr = target[i];
                if (i > 0 && (curr >= 'A' && curr <= 'Z' || curr >= '0' && curr <= '9'))
                {
                    var prev = target[i - 1];
                    //if (!((prev >= 'A' && prev <= 'Z') || (prev >= '0' && prev <= '9')))
                    if(curr >= 'A' && curr <= 'Z')
                    {
                        if (!(prev >= 'A' && prev <= 'Z'))
                            sb.Append(" ");
                    }
                    else // Is a number
                    {
                        if (!(prev >= '0' && prev <= '9'))
                            sb.Append(" ");
                    }
                }
                sb.Append(curr);
            }
            return sb.ToString();
        }
        
        /*
        /// <summary>Converts an xml date/time into a javascript compatible date</summary>
        /// <param name="xmlDate">The date/time to convert</param>
        /// <param name="defaultValue">The value to return if the date passed is blank or invalid</param>
        /// <returns>The javascript date in MMMM, dd YYYY format</returns>
        public string SXmlDateOnlyForJavaScript(string xmlDate, string defaultValue)
        {
            xmlDate = Convert.ToString(xmlDate + string.Empty).Trim();
            if (xmlDate.Length > 0 & Microsoft.VisualBasic.Information.IsDate(xmlDate))
            {
                return Convert.ToDateTime(xmlDate).ToString("MMMM, dd yyyy");
            }
            return defaultValue;
        }
        */


        /*
        /// <summary>Converts an xml date/time into a displayable date (MM/dd/YYYY format)</summary>
        /// <param name="xmlDate">The date to convert</param>
        /// <returns>The displayable date</returns>
        public string SXmlDateOnly(string xmlDate)
        {
            xmlDate = Convert.ToString(xmlDate + string.Empty).Trim();
            if (xmlDate.Length > 0 & Microsoft.VisualBasic.Information.IsDate(xmlDate))
            {
                return Convert.ToDateTime(xmlDate).ToString("MM/dd/yyyy").ToLower().Replace("01/01/1900", string.Empty);
            }
            return string.Empty;
        }
        */


        /// <summary>Converts an xml date/time into a displayable date/time value ("MM/dd/YYYY hh:mm tt" format)</summary>
        /// <param name="xmlDate">The date to convert</param>
        /// <returns>The displayable date</returns>
        public string SXmlDate(string xmlDate)
        {
            xmlDate = Convert.ToString(xmlDate + string.Empty).Trim();
            if (xmlDate.Length > 0)
            {
                return Convert.ToDateTime(xmlDate).ToString("MM/dd/yyyy hh:mm tt").ToLower().Replace(" 12:00 am", string.Empty);
            }
            return string.Empty;
        }


        /// <summary>Formats a number to a particular format (see the VB Format() function).</summary>
        /// <param name="value">The value to format</param>
        /// <param name="formatString">The VB.NET format string</param>
        /// <returns>The formatted string</returns>
        public string Format(float value, string formatString)
        {
            return $"{value.ToString(formatString)}";
        }


        public string Chars(int count, string toRepeat)
        {
            if (count < 0)
                return string.Empty;
            if (string.IsNullOrEmpty(toRepeat))
                toRepeat = " ";
            if (count > 200)
                count = 200;
            var ret = new StringBuilder();
            for (var i = 0; i < count; i++) { ret.Append(toRepeat); }
            return ret.ToString();
        }


        /// <summary>Converts an xml date/time into a displayable date/time value ("MM/dd/YYYY hh:mm tt" format)</summary>
        /// <param name="xmlDate">The date to convert</param>
        /// <param name="sFormat">The VB.NET format string to format the date/time to</param>
        /// <returns>The formatted date/time string</returns>
        public string SXmlDate(string xmlDate, string sFormat)
        {
            xmlDate = Convert.ToString(xmlDate + string.Empty).Trim();
            if (xmlDate.Length > 0)
            {
                return Convert.ToDateTime(xmlDate).ToString(sFormat).ToLower();
            }
            return string.Empty;
        }

        /// <summary>Returns the first name (word) from the given string</summary>
        /// <param name="name">The name to parse</param>
        /// <returns>The first name found</returns>
        public string FirstName(string name)
        {
            return name.FirstToken();
        }

        /// <summary>Returns the last name (word) from the given string</summary>
        /// <param name="name">The name to parse</param>
        /// <returns>The last name found</returns>
        public string LastName(string name)
        {
            return name.LastToken();
        }


        /// <summary>Increments an internal counter (NextID) and returns that value.</summary>
        /// <returns>The next higher ID</returns>
        public string GetNextId()
        {
            _mNextId += 1;
            return _mNextId.ToString();
        }


        /// <summary>Subtracts one from the internal layer count (starting at 10,000) and returns that value</summary>
        /// <returns>The next lower layer</returns>
        public string GetNextLayer()
        {
            _mNextLayer -= 1;
            return _mNextLayer.ToString();
        }


        /// <summary>Oscillates between returning "contentDataRow1" and "contentDataRow2"</summary>
        /// <returns>The next row CSS class value</returns>
        public string GetNextRowClass()
        {
            var t = _mCurrRowClass;
            _mCurrRowClass = _mNextRowClass;
            _mNextRowClass = t;
            return t;
        }

        /// <summary>
        /// Clears the internal list of totals
        /// </summary>
        /// <returns></returns>
        public string ClearTotals()
        {
            _mRunningTotals = new Dictionary<string, double>();
            return string.Empty;
        }

        /// <summary>Clears the internal total to some initial value</summary>
        /// <param name="totalName">The name of the total to clear</param>
        /// <param name="sInitialValue">The value to set internal total to</param>
        /// <returns>an empty string</returns>
        public string ClearTotal(string totalName, string sInitialValue = "0")
        {
            _mRunningTotals ??= new Dictionary<string, double>();
            if (_mRunningTotals.ContainsKey(totalName))
                _mRunningTotals[totalName] = ForDouble.NzDouble(sInitialValue);
            else
                _mRunningTotals.Add(totalName, ForDouble.NzDouble(sInitialValue));
            return string.Empty;
        }


        /// <summary>Adds a value to the internal total</summary>
        /// <param name="totalName">The name of the total</param>
        /// <param name="toAdd">The value to add to the internal total</param>
        /// <returns>An empty string</returns>
        public string AddToTotal(string totalName, string toAdd)
        {
            _mRunningTotals ??= new Dictionary<string, double>();
            if (_mRunningTotals.ContainsKey(totalName))
                _mRunningTotals[totalName] += NzDouble(toAdd);
            else
                _mRunningTotals.Add(totalName, ForDouble.NzDouble(toAdd));
            return string.Empty;
        }


        /// <summary>Subtracts a value from the internal total</summary>
        /// <param name="totalName">The name of the total</param>
        /// <param name="toSubtract">The amount to subtract</param>
        /// <returns>a blank string</returns>
        public string SubtractFromTotal(string totalName, string toSubtract)
        {
            _mRunningTotals ??= new Dictionary<string, double>();
            if (_mRunningTotals.ContainsKey(totalName))
                _mRunningTotals[totalName] -= NzDouble(toSubtract);
            else
                _mRunningTotals.Add(totalName, -ForDouble.NzDouble(toSubtract));
            return string.Empty;
        }


        /// <summary>Returns the current internal total value</summary>
        /// <param name="totalName">The name of the total</param>
        /// <param name="decimalPlaces">The number of decimal places to return the internal total (normally 2 or 0)</param>
        /// <returns>The internal total formatted to the number of decimal places</returns>
        public string GetTotal(string totalName, int decimalPlaces)
        {
            _mRunningTotals ??= new Dictionary<string, double>();
            if (decimalPlaces > 0)
                if (_mRunningTotals.ContainsKey(totalName))
                    return _mRunningTotals[totalName].ToString("0." + new string('0', decimalPlaces));
                else
                    return "0." + new string('0', decimalPlaces);
            else if (_mRunningTotals.ContainsKey(totalName))
                return _mRunningTotals[totalName].ToString(CultureInfo.InvariantCulture);
            return "0";
        }

        /// <summary>
        /// Returns a distinct set of nodes
        /// </summary>
        /// <param name="nodeSet"></param>
        /// <returns></returns>
        public XPathNavigator[] Distinct(XPathNodeIterator nodeSet)
        {
            if (nodeSet.Count == 0)
                return Array.Empty<XPathNavigator>();
            var retNodes = new Dictionary<string, XPathNavigator>();
            while (nodeSet.MoveNext())
                if (nodeSet.Current != null && !retNodes.ContainsKey(nodeSet.Current.Value))
                    retNodes.Add(nodeSet.Current.Value, nodeSet.Current);
            var ret = new XPathNavigator[retNodes.Count];
            retNodes.Values.CopyTo(ret, 0);
            return ret;
        }

        // True if not empty and = true or any integer value != 0
        public bool IsTrue(string target)
        {
            if(target.IsEmpty())
                return false;

            return target.ToLower() == "true"
                   || target.AsInteger() != 0;
        }

        // True if not empty and = true or any integer value != 0
        public string IfTrue(string target, string whenTrue, string whenFalse)
        {
            if (IsTrue(target))
                return whenTrue ?? "";
            return whenFalse;
        }

        private string _lastExpandFilename;
        private string[] _lastExpansions;

        // True if not empty and = true or any integer value != 0
        public string ExpandToProperCase(string target, string filename)
        {
            if (_lastExpandFilename.IsEmpty() || filename != _lastExpandFilename)
            {
                if (!File.Exists(filename))
                    return target;

                _lastExpandFilename = filename;
                _lastExpansions = File
                    .ReadAllLines(filename)
                    .Where(l => l.IsNotEmpty())
                    .Where(l => !l.StartsWith("# "))
                    .ToArray();
            }

            if (_lastExpansions.IsEmpty())
                return target;

            var expanded = target;
            foreach (var line in _lastExpansions)
            {
                var toFind = line.TokenAt(1, ",");
                var replaceWith = line.TokensAfter(1, ",");

                if (toFind.IsNotEmpty())
                {
                    expanded = expanded.Replace(toFind, replaceWith);
                }
            }
            expanded = expanded.ProperCase();
            return expanded;
        }

        public bool IsIn(string attributeName, string toFind, XPathNodeIterator nodeSet)
        {
            if (nodeSet.Count == 0 || string.IsNullOrEmpty(attributeName) || string.IsNullOrEmpty(toFind))
                return false;

            while (nodeSet.MoveNext())
            {
                if (nodeSet.Current != null)
                {
                    var attributeValue = nodeSet.Current.GetAttribute(attributeName, string.Empty);
                    if (attributeValue == toFind)
                        return true;
                }
            }
            return false;
        }

        public XPathNavigator[] In(string attributeName, string toFind, XPathNodeIterator nodeSet)
        {
            if (nodeSet.Count == 0 || string.IsNullOrEmpty(attributeName) || string.IsNullOrEmpty(toFind))
                return Array.Empty<XPathNavigator>();

            var retNodes = new Dictionary<string, XPathNavigator>();
            while (nodeSet.MoveNext())
            {
                if (nodeSet.Current == null) continue;
                
                var attributeValue = nodeSet.Current.GetAttribute(attributeName, string.Empty);
                if (attributeValue == toFind && !retNodes.ContainsKey(nodeSet.Current.Value))
                    retNodes.Add(nodeSet.Current.Value, nodeSet.Current);
            }
            var ret = new XPathNavigator[retNodes.Count];
            retNodes.Values.CopyTo(ret, 0);
            return ret;
        }

        // xlg:AllNotIn('ColumnName',$NonPointerMasterColumns,$HistoryTable/Columns/Column)
        public XPathNavigator[] AllNotIn(string attributeName, XPathNodeIterator nodeSetToCompareAgainst, XPathNodeIterator nodeSetToPossiblyKeep)
        {
            if (nodeSetToPossiblyKeep.Count == 0 || string.IsNullOrEmpty(attributeName))
                return Array.Empty<XPathNavigator>();

            // Handle special case of when there's nothing to compare against
            // Return all possible nodes 
            if (nodeSetToCompareAgainst.Count == 0)
            {
                return XPathNodeIteratorToNavigators(nodeSetToPossiblyKeep);
            }

            // Build a list we can look over multiple times
            var compareSet = new List<string>();
            while (nodeSetToCompareAgainst.MoveNext())
            {
                if (nodeSetToCompareAgainst.Current == null) continue;
                
                var attributeValue = nodeSetToCompareAgainst.Current.GetAttribute(attributeName, string.Empty);
                if (!string.IsNullOrEmpty(attributeValue) && !compareSet.Contains(attributeValue))
                    compareSet.Add(attributeValue);
            }

            // Compare and add 
            var retNodes = new Dictionary<string, XPathNavigator>();
            while (nodeSetToPossiblyKeep.MoveNext())
            {
                if (nodeSetToPossiblyKeep.Current == null) continue;
                
                var attributeValue = nodeSetToPossiblyKeep.Current.GetAttribute(attributeName, string.Empty);
                if (!compareSet.Contains(attributeValue))
                    retNodes.Add(attributeValue, nodeSetToPossiblyKeep.Current);
            }
            var ret = new XPathNavigator[retNodes.Count];
            retNodes.Values.CopyTo(ret, 0);
            return ret;
        }

        private static XPathNavigator[] XPathNodeIteratorToNavigators(XPathNodeIterator nodeSetToPossiblyKeep)
        {
            var retNodes = new Dictionary<string, XPathNavigator>();
            while (nodeSetToPossiblyKeep.MoveNext())
            {
                if (nodeSetToPossiblyKeep.Current == null) continue;
                 
                var key = nodeSetToPossiblyKeep.Current.Value;
                if (string.IsNullOrEmpty(key)) key = Guid.NewGuid().ToString();
                retNodes.Add(key, nodeSetToPossiblyKeep.Current);
            }
            var ret = new XPathNavigator[retNodes.Count];
            retNodes.Values.CopyTo(ret, 0);
            return ret;
        }
    }
}