using System;
using System.Configuration;
using System.Text;

namespace MetX.Standard.Archived.Web
{
	/// <summary>An HttpApplication allowing for automated emailing of unhandled exceptions</summary>
	public class HttpApplicationErrorHandler : HttpApplication
	{
        static string PreviousUri;
        static string AppName = "HttpApplicationErrorHandler";
        static string AppVersion = "Unknown";

        /// <summary>Basic constructor</summary>
        public HttpApplicationErrorHandler(string AppName, string AppVersion) : base()
        {
            HttpApplicationErrorHandler.AppName = AppName;
            HttpApplicationErrorHandler.AppVersion = AppVersion;
            base.Error += new EventHandler(Application_Error);
        }

        public static void QueryFields(string Title, string QueryValues, StringBuilder sb)
		{
			if (!string.IsNullOrEmpty(QueryValues))
			{
                sb.AppendLine("  <Items Title=\"" + Title + "\">");
                foreach (string CurrValue in QueryValues.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
                    if (CurrValue.IndexOf("__") != 0)
                    {
                        sb.Append("    <Item Name=\"");
                        if (CurrValue.Contains("="))
                            if (CurrValue.IndexOf('=') != CurrValue.Length - 1)
                            {
                                sb.Append(xml.AttributeEncode(Token.First(CurrValue, "=").Replace("+", " ")));
                                sb.Append("\" Value=\"");
                                sb.Append(xml.AttributeEncode(Token.After(CurrValue, 1, "=").Replace("+", " ")));
                            }
                            else
                                sb.Append(xml.AttributeEncode(Token.First(CurrValue, "=").Replace("+", " ")));
                        else
                            sb.Append(xml.AttributeEncode(CurrValue.Replace("+", " ")));
                        sb.AppendLine("\" />");
                    }
                sb.AppendLine("  </Items>");
            }
		}

        /// <summary>When an unhandled exception occurs an email is sent</summary>
        public void Application_Error(object sender, EventArgs e)
        {
            if (Context.Request.Url.AbsoluteUri != PreviousUri)
            {
                PreviousUri = Context.Request.Url.AbsoluteUri;
                SendErrorReport(AppName, null, null, null, "Unhandled Global Exception Handler", Server.GetLastError());
            }
            else
                PreviousUri = null;
        }

        /// <summary>
        /// Sends an exception error report asynchronously via email. Email body is a &lt;WebErrorReport&gt; XML node 
        /// </summary>
        /// <param name="TargetAppName">Target Web Application Name</param>
        /// <param name="FromName">Name for email From field</param>
        /// <param name="FromAddress">Email address to send the report to</param>
        /// <param name="UserID">Application specific UserID of the logged in user that was browsing (or null)</param>
        /// <param name="AdditionalNodes">Any additional XML Node(s) to add. Must be well formed.</param>
        /// <param name="AdditionalInfo">Value for the Info attribute</param>
        /// <param name="ex">The exception to report (cannot be null). Create a new exception object and pass it in if you don't have one but want to send a report anyway</param>
        public static StringBuilder SendErrorReport(string FromName, string FromAddress, string UserID, string AdditionalNodes, string AdditionalInfo, Exception ex)
        {
            StringBuilder ExceptionBody = new StringBuilder();
            try
            {
                if (ex != null && HttpContext.Current != null)
                {
                    string ToEmail = ConfigurationManager.AppSettings["Exceptions.ToEmail"];
                    if (!string.IsNullOrEmpty(ToEmail))
                    {
                        Exception baseEx = ex.GetBaseException();
                        if (baseEx != null)
                            ex = baseEx;
                        ExceptionBody.AppendLine("<WebAppErrorReport ");
                        ExceptionBody.AppendLine("          ID=\"" + Guid.NewGuid().ToString() + "\"");
                        ExceptionBody.AppendLine("        Date=\"" + DateTime.Now.ToString("s") + "\"");
                        ExceptionBody.AppendLine("      Server=\"" + xml.AttributeEncode(Environment.MachineName) + "\"");
                        ExceptionBody.AppendLine("         App=\"" + xml.AttributeEncode(HttpApplicationErrorHandler.AppName) + "\"");
                        ExceptionBody.AppendLine("     Version=\"" + xml.AttributeEncode(HttpApplicationErrorHandler.AppVersion) + "\"");
                        ExceptionBody.AppendLine("        Page=\"" + xml.AttributeEncode(HttpContext.Current.Request.ServerVariables["URL"]) + "\"");
                        ExceptionBody.AppendLine("        Type=\"" + xml.AttributeEncode(ex.GetType().FullName) + "\"");
                        ExceptionBody.AppendLine("      Source=\"" + xml.AttributeEncode(ex.Source) + "\"");

                        if(!string.IsNullOrEmpty(FromName))
                            ExceptionBody.AppendLine("        From=\"" + xml.AttributeEncode(FromName) + "\"");
                        if (!string.IsNullOrEmpty(FromAddress))
                            ExceptionBody.AppendLine("       Email=\"" + xml.AttributeEncode(FromAddress) + "\"");
                        if (!string.IsNullOrEmpty(FromAddress))
                            ExceptionBody.AppendLine("      UserID=\"" + xml.AttributeEncode(UserID) + "\"");

                        try
                        {
                            if (!string.IsNullOrEmpty(HttpContext.Current.Request.UserHostAddress))
                            {
                                ExceptionBody.AppendLine("          Ip=\"" + xml.AttributeEncode(HttpContext.Current.Request.UserHostAddress) + "\"");
                                if (!HttpContext.Current.Request.UserHostAddress.Equals(HttpContext.Current.Request.UserHostName))
                                    ExceptionBody.AppendLine("        Host=\"" + xml.AttributeEncode(HttpContext.Current.Request.UserHostName) + "\"");
                            }
                        }
                        catch { }

                        if (!string.IsNullOrEmpty(AdditionalInfo))
                            ExceptionBody.AppendLine("        Info=\"" + xml.AttributeEncode(AdditionalInfo) + "\"");
                        ExceptionBody.AppendLine("         Url=\"" + xml.AttributeEncode(HttpContext.Current.Request.Url.AbsoluteUri) + "\"");
                        ExceptionBody.AppendLine("     Message=\"" + xml.AttributeEncode(ex.Message) + "\"");

                        if (!string.IsNullOrEmpty(ex.StackTrace))
                        {
                            ExceptionBody.Append(" Trace=\"");
                            ExceptionBody.Append(xml.AttributeEncode(ex.StackTrace.Replace("   at ", System.Environment.NewLine).Replace(":line ", " : line ").Replace("]]>", "]_]_>")));
                            ExceptionBody.AppendLine("\" />");
                        }

                        if (!string.IsNullOrEmpty(AdditionalNodes))
                            ExceptionBody.AppendLine(AdditionalNodes);

                        QueryFields("Query", HttpContext.Current.Request.QueryString.ToString(), ExceptionBody);
                        QueryFields("Form", HttpContext.Current.Request.Form.ToString(), ExceptionBody);

                        try
                        {
                            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session.Count > 0)
                            {
                                ExceptionBody.AppendLine("  <Items Title=\"Session\">");
                                string[] SessionItems = new string[HttpContext.Current.Session.Count];
                                HttpContext.Current.Session.CopyTo(SessionItems, 0);
                                foreach (string CurrItem in SessionItems)
                                    if (!string.IsNullOrEmpty(CurrItem))
                                    {
                                        try
                                        {
                                            ExceptionBody.Append("    <Item Name=\"");
                                            ExceptionBody.Append(xml.AttributeEncode(CurrItem));
                                            ExceptionBody.Append("\"");
                                            object x = HttpContext.Current.Session[CurrItem];
                                            if (x != null)
                                            {
                                                ExceptionBody.Append(" Value=\"");
                                                ExceptionBody.Append(x.ToString());
                                                ExceptionBody.AppendLine("\" />");
                                            }
                                        }
                                        catch { }
                                    }
                                ExceptionBody.AppendLine("  </Items>");
                            }
                        }
                        catch { } // (Exception se) { ExceptionBody.AppendLine(se.ToString()); }


                        QueryFields("Headers", HttpContext.Current.Request.Headers.ToString(), ExceptionBody);
                        
                        try
                        {
                            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies.Count > 0)
                            {
                                ExceptionBody.AppendLine("  <Items Title=\"Cookies\">");
                                foreach (string CurrItem in HttpContext.Current.Request.Cookies)
                                {
                                    try
                                    {
                                        ExceptionBody.Append("    <Item Name=\"");
                                        ExceptionBody.Append(xml.AttributeEncode(CurrItem));
                                        ExceptionBody.Append("\" Value=\"");
                                        ExceptionBody.Append(xml.AttributeEncode(HttpContext.Current.Request.Cookies[CurrItem].Value));
                                        ExceptionBody.AppendLine("\" />");
                                    }
                                    catch { }
                                }
                                ExceptionBody.AppendLine("  </Items>");
                            }
                        }
                        catch(Exception se) { ExceptionBody.AppendLine(se.ToString()); }

                        ExceptionBody.AppendLine("</WebAppErrorReport>");

                        MetX.IO.Email.SendMail(
                            FromName, (FromAddress != null ? FromAddress : ConfigurationManager.AppSettings["Exceptions.FromEmail"]),
                            ConfigurationManager.AppSettings["Exceptions.ToName"], ConfigurationManager.AppSettings["Exceptions.ToEmail"],
                            "WebAppErrorReport", ExceptionBody.ToString());
                    }
                }
            }
            catch { } //(Exception exx) { System.Diagnostics.Debug.WriteLine(exx.ToString()); }
            return ExceptionBody;
        }
	}
}

