using System;
using System.Configuration;
using System.IO;

namespace MetX.Standard.Archived.Web
{
    /// <summary>A basic HttpHandler that includes an automatic load of a SecureUserProfile object.
    /// <para>NOTE: For this class to be created and ProcessRequest() to be called, you must include a &lt;Virtual Name="ClassName" /> entry under &lt;Xsls> in your .xlg file</para>
    /// <para>Alternatively, you can modify the xlg.xsl file and include the object creation inside the HandlerFactory object.</para>
    /// </summary>
    public class SecureHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>The SecureUserProfile loaded immediately before ProcessRequest() is called.</summary>
        public SecureUserProfile Security = new SecureUserProfile();

        /// <summary>Override to implement your secure HttpHandler.
        /// </summary>
        public virtual void ProcessRequest() { }
        /// <summary>The HttpRequest object from Context</summary>
        public HttpRequest Request;
        /// <summary>The HttpContext object</summary>
        public HttpContext Context;
        /// <summary>The HttpResponse object from Context</summary>
        public HttpResponse Response;
        /// <summary>The HttpSession object from Context</summary>
        public HttpSessionState Session;
        /// <summary>The HttpServerUtility object from Context</summary>
        public HttpServerUtility Server;

        #region IHttpHandler Members

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            Context = context;
            Request = context.Request;
            Response = context.Response;
            Session = context.Session;
            Server = context.Server;
            Security.Start(Context);
            ProcessRequest();
        }
       #endregion

        /// <summary>Returns the System.Web.Caching.Cache object (equivalent to Context.Cache) for this request if the Web.config setting "xlgDoc.Cache" == "True"
        /// Otherwise this returns null;
        /// </summary>
        public virtual System.Web.Caching.Cache PageCache
        {
            get
            {
                if (ConfigurationManager.AppSettings["xlgDoc.Cache"] == "True")
                    return Context.Cache;
                return null;
            }
        }

        /// <summary>Attempts to remap a aspx, asp, xml, xslx, or axd page name to its xsl page name</summary>
        /// <param name="PagePath">The virtual file to remap to xsl file</param>
        /// <returns>The xsl filename matching this file</returns>
        /// 
        /// <example>
        /// <code>
        /// string x = MassageXsltPath("default.aspx")
        /// // x = "default.xsl"
        /// </code>
        /// </example>
        public string MassageXsltPath(string PagePath)
        {
            string Extension = Path.GetExtension(PagePath);
            if (Extension != null && Extension.Length > 0)
                PagePath = PagePath.Replace(Extension, ".xsl");
            else
                PagePath = PagePath.Replace(".aspx", ".xsl").Replace(".asp", ".xsl").Replace(".xml", ".xsl").Replace(".xslx", ".xsl").Replace(".axd", ".xsl");
            //PagePath = PagePath.Replace(".aspx", ".xsl").Replace(".asp", ".xsl").Replace(".xml", ".xsl").Replace(".xslx", ".xsl").Replace(".axd", ".xsl");
            if (PagePath.IndexOf(":") == -1)
            {
                PagePath = Server.MapPath(PagePath);
                if (PagePath.IndexOf(@"\xsl\") == -1)
                    PagePath = Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "xsl"), PagePath.Substring(Request.PhysicalApplicationPath.Length));
                if (!File.Exists(PagePath))
                {
                    PagePath = Path.Combine(Path.Combine(Path.GetDirectoryName(PagePath), "xsl"), Path.GetFileName(PagePath));
                }
            }
            return PagePath;
        }

        public string RequestVar(string VariableName)
        {
            return Worker.nzString(Request[VariableName]);
        }

        public string V(string VariableName)
        {
            return Worker.nzString(Request[VariableName]);
        }

        public int I(string VariableName)
        {
            return Worker.nzInteger(Request[VariableName]);
        }

        public DateTime D(string VariableName)
        {
            return Worker.nzDateTime(Request[VariableName]);
        }

        public string[] A(string VariableName)
        {
            string sV = Worker.nzString(Request[VariableName]);
            if (sV.Length > 0)
                return sV.Split(new string[] { "," }, StringSplitOptions.None);
            return new string[0];
        }

        public string X(string VariableName)
        {
            return xml.AttributeEncode(Worker.nzString(Request[VariableName]));
        }
    }
}
