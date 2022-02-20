using System.Configuration;
using System.IO;

namespace MetX.Standard.Archived.Web
{
    /// <summary>A basic HttpHandler (not secured). This handler acts more like a Page object giving quick access to the Request, Response, Session, and Server.
    /// <para>Useful when upgrading from Page objects or for unsecured ajax calls</para>
    /// </summary>
    public class xlgHandler : IHttpHandler, IRequiresSessionState
    {
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
            ProcessRequest();
        }

        #endregion

        /// <summary>Returns the System.Web.Caching.Cache object (equivalent to Context.Cache) for this request if the Web.config setting "xlgDoc.Cache" == "True"
        /// Otherwise this returns null;
        /// </summary>
        public System.Web.Caching.Cache PageCache
        {
            get
            {
                if (ConfigurationManager.AppSettings["xlgDoc.Cache"] == "True")
                    return Context.Cache;
                return null;
            }
        }

        /// <summary>Attempts to remap a aspx, asp, xml, xslx, or axd page name to its xsl page name</summary>
        /// <param name="PagePage">The virtual file to remap to xsl file</param>
        /// <returns>The xsl filename matching this file</returns>
        /// 
        /// <example>
        /// <code>
        /// string x = MassageXsltPath("default.aspx")
        /// // x = "default.xsl"
        /// </code>
        /// </example>
        public string MassageXsltPath(string PagePage)
        {
            string Extension = Path.GetExtension(PagePage);
            if (Extension != null && Extension.Length > 0)
                PagePage = PagePage.Replace(Extension, ".xsl");
            else
                PagePage = PagePage.Replace(".aspx", ".xsl").Replace(".asp", ".xsl").Replace(".xml", ".xsl").Replace(".xslx", ".xsl").Replace(".axd", ".xsl");
            if (PagePage.IndexOf(":") == -1)
            {
                PagePage = Server.MapPath(PagePage);
                if (!File.Exists(PagePage))
                    PagePage = Path.Combine(Path.Combine(Path.GetDirectoryName(PagePage), "xsl"), Path.GetFileName(PagePage));
            }
            return PagePage;
        }
    }
}
