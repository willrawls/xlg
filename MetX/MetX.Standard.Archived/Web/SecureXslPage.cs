using System;
using System.Configuration;
using System.Text;

namespace MetX.Standard.Archived.Web
{
	/// <summary>An xlgPage with XslPage rendering and SecureUserProfile functionality built in.</summary>
	public class SecureXslPage : xlgPage
	{
		/// <summary>The SecureUserProfile automatically loaded prior to Page_Load firing.</summary>
		public SecureUserProfile Security = new SecureUserProfile();
		
		/// <summary>The path to the .xsl file to render the XML against. If blank, the system uses the xsl file with the same name as the class (Default.xsl for Default.aspx for instance)</summary>
		public string xsltPath = string.Empty;

		/// <summary>The value of the DebugState attribute to place in root element. Default value is pulled from the Web.config setting xlgSecurity.Debug</summary>
		public string DebugState = string.Empty;
		
		private StringBuilder sb;
		private string RootName = string.Empty;
		private string _OutputFormat;

        /// <summary>The XSL transformation class with (MetX.xml)</summary>
        public xml Transformer = new xml();

        /// <summary>Used to target a manual page creation at a specific user.</summary>
        public string TargetUserName;
		
		/// <summary>Clears the internal xml string area</summary>
		public void Clear()
		{
			sb = new StringBuilder();
		}

        /// <summary>The OnLoad event fired that causes the SecureUserProfile to load.</summary>
        /// <param name="e">Standard OnLoad event arguments</param>
        protected override void OnLoad(System.EventArgs e)
        {
            Clear();
            if (TargetUserName == null || TargetUserName.Length == 0)
                Security.Start(HttpContext.Current);
            else
                Security.Start(HttpContext.Current, TargetUserName);

            sb.AppendLine("<?xml version=\"1.0\"?>");
            if (Request["xslfile"] != null && Request["xslfile"].Length > 0)
            {
                xsltPath = Request["xslfile"] + string.Empty;
                if ((xsltPath.IndexOf(".xsl", 0) + 1) == 0)
                    xsltPath += ".xsl";
            }
            base.OnLoad(e);
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
				{
					ProbableURL += "?" + Request.Form.ToString();
				}
                string URLPath = Token.First(ProbableURL, "?");
                URLPath = Token.Before(URLPath, Token.Count(URLPath, "/"), "/") + "/";
                string ServerPath = Token.Before(URLPath, Token.Count(URLPath, "/") - 1, "/") + "/";
                string VDirPath = Token.Before(URLPath, 5, "/") + "/";
                if ((ProbableURL.IndexOf("&format=", 0) + 1) > -1)
					ProbableURL = ProbableURL.Replace("&format=" + Request["format"], string.Empty);
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

		
		/// <summary>Appends an xml string to the internal xml string</summary>
		/// <param name="sToAppend">The xml string to append (usually one or more well formed elements)</param>
		public void Append(string sToAppend)
		{
            sb.Append(sToAppend);
		}

		
		/// <summary>Outputs the internal xml string to the Response</summary>
		public void RawXml()
		{
			Response.ContentType = "text/xml";
			NoTransform();
		}

		
		/// <summary>Returns the Cache object if the Web.config settings "xlgDoc.Cache" = "True"</summary>
		public System.Web.Caching.Cache PageCache
		{
            get
            {
                if (ConfigurationManager.AppSettings["xlgDoc.Cache"] == "True")
                {
                    return Cache;
                }
                return null;
            }
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
			else
			{
				EndDocument();
                if (xsltPath == null || xsltPath.Length == 0)
				{
                    WriteToOutput(Transformer.xslTransform(PageCache, sb, MassageXsltPath(Security.PageName)));
				}
				else
				{
                    WriteToOutput(Transformer.xslTransform(PageCache, sb, MassageXsltPath(xsltPath)));
				}
			}
		}


        /// <summary>Renders the internal xml string as Excel 2003 readable HTML and redirects to the unsecured viewtempdatafile internal handler to allow downloading in Excel. File will be available for about 5 minutes from the temporary folder.</summary>
        public void TransformToExcelDownload()
		{
			Response.Clear();
			Session["vfilename"] = sTransformToExcelDownload();
			Session["format"] = "excel";
			Response.Redirect("viewtempdatafile.aspx");
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
            Session["vcontent"] = sTransform();
            //StreamWriter sw = File.CreateText(OutputFilename);
            //sw.WriteLine(FileContents);
            //sw.Flush();
            //sw.Close();
            //sw.Dispose();
			return OutputFilename;
		}


		/// <summary>Renders the internal xml string and returns the output. OutputFormat will not override this transformation.</summary>
		/// <returns>The rendered output</returns>
		public StringBuilder sTransform()
		{
			EndDocument();
            if (xsltPath == null || xsltPath.Length == 0)
                return Transformer.xslTransform(PageCache, sb, MassageXsltPath(Security.PageName));
            return Transformer.xslTransform(PageCache, sb, MassageXsltPath(xsltPath));
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

		
		/// <summary>Performs garbage collection on page unloda</summary>
		/// <param name="e">Stadnard event arguments</param>
		override protected void OnUnload(System.EventArgs e)
		{
			base.OnUnload(e);
			GC.Collect();
		}

		
		/// <summary>Writes a string to the Response object.</summary>
		/// <param name="ToWrite">The string to write</param>
		private void WriteToOutput(ref string ToWrite)
		{
			Response.Write(ToWrite);
		}


        /// <summary>Writes a string to the Response object.</summary>
        /// <param name="ToWrite">The StringBuilder to write</param>
        private void WriteToOutput(StringBuilder ToWrite)
        {
            Response.Write(ToWrite);
        }
        
        /// <summary>The format to render the page as. If not set, it will be set to the Request variable "format"</summary>
        public string OutputFormat
		{
			get
			{
                if (_OutputFormat == null || _OutputFormat.Length == 0)
				{
					_OutputFormat = Request["format"];
				}
				return _OutputFormat;
			}
			set
			{
				_OutputFormat = value;
			}
		}
    }
} 