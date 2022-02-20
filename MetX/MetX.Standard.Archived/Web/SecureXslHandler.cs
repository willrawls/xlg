using System.Configuration;
using System.IO;
using System.Text;

namespace MetX.Standard.Archived.Web
{
    /// <summary>A SecureHandler with XslHandler rendering and SecureUserProfile functionality built in.
    /// <para>Events are fired automatically by ProcessRequest() as follows:</para>
    /// <code>
    ///     Clear();
    ///     if (OnLoad()) {
    ///         if (PreBuild()) {
    ///             Begin("xlgDoc", RootAttributes);
    ///             if (LoadMemberProfile)
    ///             // Adds the appropriate memberProfile element. If no id or username is posted, the current user is used (SecureUserProfile = memberProfile attributes).
    ///             if (BuildXml()) {
    ///                 Transform();
    ///                 PostTransform();
    ///             }
    ///         }
    ///     }
    /// </code>
    /// <para>If PreBuild returns false, no intnernal xml string or xsl transformation will occur</para>
    /// <para>If BuildXml returns false, no xsl transformation or PostBuild will occur</para>
    /// </summary>
    public class SecureXslHandler : SecureHandler
	{
        /// <summary>The SecureUserProfile automatically loaded prior to Page_Load firing.</summary>
        public SecureUserProfile memberProfile;

        /// <summary>The path to the .xsl file to render the XML against. If blank, the system uses the xsl file with the same name as the class (Default.xsl for Default.aspx for instance)</summary>
        public string xsltPath = string.Empty;

        /// <summary>When set, xsltPath is ignored and the xsl stylesheet contained in xsltContent is used to transform.</summary>
        public StringBuilder xsltContent;

        /// <summary>The value of the DebugState attribute to place in root element. Default value is pulled from the Web.config setting xlgSecurity.Debug</summary>
        public string DebugState = string.Empty;

        /// <summary>Setting this to true will load a memberProfile element below the SecureUserPage element based on the value of the request variable "id" or "username"</summary>
        public bool LoadMemberProfile = false;

        /// <summary>The additional attributes to add to the root xml element</summary>
        public string RootAttributes = string.Empty;

        /// <summary>If set, it allows for the virtualization of output normally written to Response. This allows pages to generate other pages. See BuildVirtualPage</summary>
        public StringWriter VirtualOutput;

        private StringBuilder sb;
        private string RootName = string.Empty;
        private string _OutputFormat;
        private bool _AlreadyStarted;

        /// <summary>The XSL transformation class with (MetX.xml)</summary>
        public xml Transformer = new xml();

        public void AddRootAttribute(string Name, string Value)
        {
            if (Value != null)
                RootAttributes += " " + Name + "=\"" + xml.AttributeEncode(Value) + "\"";
        }

        public void AddRootAttribute(string VariableName)
        {
            RootAttributes += " " + VariableName + "=\"" + xml.AttributeEncode(V(VariableName)) + "\"";
        }

        /// <summary>Clears the internal xml string area</summary>
        public void Clear()
		{
			sb = new StringBuilder();
		}

        /// <summary>Generates page output returning a stringbuilder of the output.</summary>
        /// <param name="context">The HttpContext to build the page from</param>
        /// <param name="TargetUserName">The user who the page should be targeted toward. Use this when you wish to not only generate another page, but you want to generate another page as if another user had viewed it (useful when sending emails).</param>
        /// <returns>A StringBuilder containing the output from the page</returns>
        public StringBuilder BuildVirtualPage(HttpContext context, string TargetUserName)
		{
			StringBuilder vsb = new StringBuilder();
			VirtualOutput = new StringWriter(vsb);
			Start(context, TargetUserName);
            ProcessRequest();
			VirtualOutput.Close();
			VirtualOutput.Dispose();
            vsb.Replace("&amp;#160;", " ");
			return vsb;
		}

        /// <summary>Primes the object for manual output. Only call this method when you wish to manually build your own output (pages building pages). BuildVirtualPage calls this method.</summary>
        /// <param name="context">The HttpContenxt the page's output should be based on</param>
        /// <param name="TargetUserName">The user who the page should be targeted toward. Use this when you wish to not only generate another page, but you want to generate another page as if another user had viewed it (useful when sending emails).</param>
        public void Start(HttpContext context, string TargetUserName)
		{
            if (!_AlreadyStarted)
            {
                Clear();
                if (context != null)
                {
                    Context = context;
                    Server = context.Server;
                    Request = context.Request;
                    Response = context.Response;
                    Session = context.Session;
                }
                if (TargetUserName == null || TargetUserName.Length == 0)
                    Security.Start(context);
                else
                    Security.Start(context, TargetUserName);
                
                sb.AppendLine("<?xml version=\"1.0\"?>");
                if (Request["xslfile"] != null && Request["xslfile"].Length > 0)
                {
                    xsltPath = Request["xslfile"] + string.Empty;
                    if ((xsltPath.IndexOf(".xsl", 0) + 1) == 0)
                        xsltPath += ".xsl";
                }
                _AlreadyStarted = true;
            }
		}

        /// <summary>Writes the beginning xlgDoc root element to the internal xml string with no attributes</summary>
        public void Begin()
		{
			Begin("xlgDoc", string.Empty);
		}


        /// <summary>Writes the beginning root element to the internal xml string with no attributes</summary>
        /// <param name="RootNodeName">The root element name</param>
        public void Begin(string RootNodeName)
		{
			Begin(RootNodeName, string.Empty);
		}

        /// <summary>Writes the beginning root element to the internal xml string with attributes</summary>
        /// <param name="RootNodeName">The root element name</param>
        /// <param name="Attributes">The attributes to place in the root element</param>
        public void Begin(string RootNodeName, string Attributes)
		{
            if (RootNodeName != null && RootNodeName.Length > 0)
			{
				RootName = RootNodeName.Replace("<", string.Empty).Replace(">", string.Empty).Replace("\"", string.Empty).Replace("'", string.Empty);
                sb.Append("<" + RootName);
                if (Attributes != null && Attributes.Length > 0)
				{
                    sb.Append(" " + Attributes.Trim() + " ");
				}
                if (Request["debug"] != null && Request["debug"].Length > 0)
				{
                    DebugState = Request["debug"];
				}
				else if (ConfigurationManager.AppSettings["xlgDoc.Debug"] == "True")
				{
					DebugState = ConfigurationManager.AppSettings["xlgDoc.Debug"];
				}
                string ProbableURL = Request.Url.AbsoluteUri; //Path & Request.ServerVariables("URL") & "?" & Request.QueryString.ToString
                if (Request.QueryString.ToString().Length == 0)
                    ProbableURL += "?" + Request.Form.ToString();
                string URLPath = Token.First(ProbableURL, "?");
                URLPath = Token.Before(URLPath, Token.Count(URLPath, "/"), "/") + "/";
                string ServerPath = Token.Before(Request.Url.AbsoluteUri, 4, "/") + "/";
                string VDirPath = Token.Before(URLPath, 5, "/") + "/";
                //ServerPath = Token.Before(ServerPath, Token.Count(ServerPath, "/") - 1, "/") + "/";
				if ((ProbableURL.IndexOf("&format=", 0) + 1) > -1)
                    ProbableURL = ProbableURL.Replace("&format=" + OutputFormat, string.Empty);
                if (ProbableURL.EndsWith("?"))
                    ProbableURL = ProbableURL.Substring(0, ProbableURL.Length - 1);
                sb.AppendLine(" ServerPath=\"" + xml.AttributeEncode(ServerPath) + "\" URLPath=\"" + xml.AttributeEncode(URLPath) + "\" VDirPath=\"" + xml.AttributeEncode(VDirPath) + "\" ProbableURL=\"" + xml.AttributeEncode(ProbableURL) + "\" Format=\"" + OutputFormat + "\" Version=\"" + ConfigurationManager.AppSettings["xlgDoc.Version"] + "\" DebugState=\"" + xml.AttributeEncode(DebugState) + "\">");
                sb.Append(Security.InnerXml);
			}
		}

        /// <summary>Writes the closing element to the internal xml string using this.RootName</summary>
        private void EndDocument()
		{
            if (RootName != null && RootName.Length > 0)
                sb.Append("</" + RootName + ">");
		}

        /// <summary>Appends an xml string to the internal xml StringBuilder</summary>
        /// <param name="sToAppend">The xml string to append (usually one or more well formed elements)</param>
        public void Append(string sToAppend)
		{
            sb.Append(sToAppend);
		}

        /// <summary>Appends xml inside a StringBuilder to the internal xml StringBuilder</summary>
        /// <param name="sToAppend">The xml StringBuilder to append (usually one or more well formed elements)</param>
        public void Append(StringBuilder sToAppend)
        {
            sb.Append(sToAppend);
        }


        /// <summary>Outputs the internal xml string to the Response</summary>
        public void RawXml()
		{
            Response.ContentType = "text/xml";
			NoTransform();
		}

        /// <summary>Transforms the internal xml string with the supplied xslt path and filename</summary>
        /// <param name="xsltPath">The path and filename to render the internal xml string against</param>
        public void Transform(string xsltPath)
		{
			if (OutputFormat == "xml")
			{
				RawXml();
			}
			else
			{
				EndDocument();
                WriteToOutput(Transformer.xslTransform(PageCache, sb, MassageXsltPath(xsltPath)));
			}
		}

        /// <summary>Renders the internal xml string and outputs it to the Response object. Set OutputFormat (or passing in "format" on the request query string) will override rendering options.</summary>
        public void Transform()
		{
            if (OutputFormat == "xml")
                RawXml();
            else if (OutputFormat == "excel")
                TransformToExcelDownload();
            else if (OutputFormat == "json")
            {
                EndDocument();
                xsltContent = MetX.Web.Virtual.xsl.XmlToJson.xslStringBuilder;
                Transformer.xlgUrn.SetVar("JsonVariableName", Worker.nzString(Request["variablename"], "xlgDoc"));
                WriteToOutput(Transformer.xslTransform(PageCache, sb, MassageXsltPath(xsltPath), xsltContent));
                //XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.LoadXml(sb.ToString());
                //string varName = Worker.nzString(Request["variablename"], "xlgDoc");
                //sb = new StringBuilder();
                //if (varName == "debug")
                //{
                //    sb.AppendLine("<HTML><BODY><HEAD><SCRIPT language='javascript'>");
                //}
                //if (varName != "none")
                //    sb.Append("var " + varName + " = ");
                //xml.ToJson(xmlDoc.DocumentElement, sb);
                //if (varName != "none")
                //    sb.AppendLine(";");
                //if (varName == "debug")
                //{
                //    sb.AppendLine();
                //    sb.AppendLine("var content = document.getElementById('content');");
                //    sb.AppendLine("outputNode(debug.xlgDoc);");
                //    sb.AppendLine("function outputNode(node) {");
                //    sb.AppendLine("     content.innerHTML = 'node.ServerPath = ' + node.ServerPath;");
                //    sb.AppendLine("");
                //    sb.AppendLine("}");
                //    sb.AppendLine("</SCRIPT><BODY><div id='content'></div></BODY></HTML>");
                //}
                //WriteToOutput(sb);
            }
            else
            {
                EndDocument();
                if (xsltPath == null || xsltPath.Length == 0)
                    WriteToOutput(Transformer.xslTransform(PageCache, sb, MassageXsltPath(Security.PageName), xsltContent));
                else
                    WriteToOutput(Transformer.xslTransform(PageCache, sb, MassageXsltPath(xsltPath), xsltContent));
            }
		}


        /// <summary>Renders the internal xml string as Excel 2003 readable HTML and redirects to the unsecured viewtempdatafile internal handler to allow downloading in Excel. File will be available for about 5 minutes from the temporary folder.</summary>
        public void TransformToExcelDownload()
		{
            Response.Clear();
            Response.Redirect("viewtempdatafile.aspx?f=excel&v=" + Path.GetFileName(sTransformToExcelDownload()));
		}


        /// <summary>Renders the internal xml string as Excel 2003 readable HTML and returns the results.</summary>
        /// <returns>The render Excel 2003 readable HTML string</returns>
        public string sTransformToExcelDownload()
		{
            string BatchNum = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + System.DateTime.Now.Millisecond.ToString() + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
			string xlsFilename = "s" + BatchNum + ".xls";
            string tempDir = Server.MapPath(ConfigurationManager.AppSettings["xlgSecurity.TempDataFolder"]);
			//if (!Directory.Exists(tempDir))
			//	Directory.CreateDirectory(tempDir);
			string OutputFilename = tempDir + "\\" + xlsFilename;
            //if (File.Exists(OutputFilename))
            //{
            //    File.SetAttributes(OutputFilename, FileAttributes.Normal);
            //    File.Delete(OutputFilename);
            //}
            //StringBuilder FileContents = sTransform();
            //StreamWriter sw = File.CreateText(OutputFilename);
            //sw.WriteLine(FileContents);
            //sw.Flush();
            //sw.Close();
            //sw.Dispose();
            Session["vcontent"] = sTransform();
			return OutputFilename;
		}


        /// <summary>Renders the internal xml string and returns the output. OutputFormat will not override this transformation.</summary>
        /// <returns>The rendered output</returns>
        public StringBuilder sTransform()
		{
			EndDocument();
            if (xsltPath == null || xsltPath.Length == 0)
                return Transformer.xslTransform(PageCache, sb, MassageXsltPath(Security.PageName), xsltContent);
			else
                return Transformer.xslTransform(PageCache, sb, MassageXsltPath(xsltPath), xsltContent);
		}


        /// <summary>Returns the contents of the internal xml string</summary>
        /// <returns>The internal xml string contents</returns>
        public string sRawXml()
		{
			string ReturnValue = null;
			EndDocument();
			ReturnValue = sb.ToString();
			return ReturnValue;
		}


        /// <summary>Causes the internal xml string to be written to the output</summary>
        public void NoTransform()
		{
			EndDocument();
			WriteToOutput(sb);
		}

        /// <summary>Writes a string to the Response object.</summary>
        /// <param name="ToWrite">The string to write</param>
        private void WriteToOutput(ref string ToWrite)
		{
			if (VirtualOutput == null)
			{
                Response.Write(ToWrite);
			}
			else
			{
				VirtualOutput.Write(ToWrite);
			}
		}


        /// <summary>Writes a string to the Response object.</summary>
        /// <param name="ToWrite">The StringBuilder to write</param>
        private void WriteToOutput(StringBuilder ToWrite)
        {
            if (VirtualOutput == null)
            {
                Response.Write(ToWrite);
            }
            else
            {
                VirtualOutput.Write(ToWrite);
            }
        }


        /// <summary>The format to render the page as. If not set, it will be set to the Request variable "format"</summary>
        public string OutputFormat
		{
			get
			{
                if (_OutputFormat == null || _OutputFormat.Length == 0)
                    _OutputFormat = Request["format"];
				return _OutputFormat;
			}
			set
			{
				_OutputFormat = value;
			}
		}


        /// <summary>Override to Append your own elements to the internal xml string prior to xsl transformation.
        /// <para>NOTE: This function has no implementation except to return true</para>
        /// </summary>
        /// <returns>Returns true</returns>
        public virtual bool BuildXml()
        {
            return true;
        }


        /// <summary>Override to handle situations that need to be handled before anything else is done.
        /// <para>NOTE: This function has no implementation except to return true</para>
        /// </summary>
        /// <returns>Returns true</returns>
        public virtual bool OnLoad()
        {
            return true;
        }


        /// <summary>Override to handle situations that need to be handled after everything else is done.
        /// <para>NOTE: This function has no implementation</para>
        /// </summary>
        public virtual void OnFinish()
        {
        }


        /// <summary>Override to handle situations (such as handling POST) that need to be handled before the xml string is begun. This is where RootAttributes would be set.
        /// <para>NOTE: This function has no implementation except to return true</para>
        /// </summary>
        /// <returns>Returns true</returns>
        public virtual bool PreBuild()
        {
            return true;
        }


        /// <summary>Occurs after transformation and before the Response is returned to the client.
        /// <para>NOTE: This function has no implementation except to return true</para>
        /// </summary>
        /// <returns>Returns true</returns>
        public virtual bool PostTransform()
        {
            return true;
        }


        /// <summary>This is the HttpHandler event that triggers the PreBuild, BuildXml, PostBuild events.</summary>
        public override void ProcessRequest()
        {
            Clear();
            if (OnLoad())
            {
                if (PreBuild())
                {
                    Begin("xlgDoc", RootAttributes);
                    if (LoadMemberProfile)
                    {
                        string username = Request["username"];
                        string NotificationID = Request["NotificationID"];
                        if (NotificationID != null && NotificationID.Length > 0)
                            username = sql.RetrieveSingleStringValue("SELECT UserName FROM [User] WHERE UserID='" + NotificationID + "'", "xlgSecurity");

                        if (username == null || username.Length == 0)
                        {
                            memberProfile = Security;
                        }
                        else
                        {
                            memberProfile = new SecureUserProfile();
                            if (!memberProfile.Load(Security.ApplicationName, Security.PageName, username))
                                memberProfile = Security;
                        }
                        Append(memberProfile.InnerXml.Replace("SecureUserPage", "memberProfile"));
                    }
                    if (BuildXml())
                    {
                        Transform();
                        PostTransform();
                    }
                }
            }
            OnFinish();
        }
    }
} 