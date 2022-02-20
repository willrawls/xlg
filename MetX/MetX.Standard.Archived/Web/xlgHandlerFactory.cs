using MetX.Standard.Archived.Web.Virtual;

namespace MetX.Standard.Archived.Web
{
    /// <summary>The basic class used to handle requests in an XLG web application.</summary>
    public class xlgHandlerFactory : IHttpHandlerFactory
    {
        /// <summary>Overriden in the generated inherited class, it returns the appropriate HttpHandler for the URL or null
        /// <para>NOTE: Some URLs are handled internally by Metta including: "viewtempdatafile"</para>
        /// </summary>
        /// <param name="url">The relative URL to retrieve a handler for (such as "/abc/Default.aspx")</param>
        /// <param name="pagePath">The lowercase path to the page without extension (such as "abc/default")</param>
        /// <returns>The appropriate HttpHandler for that URL</returns>
        public virtual IHttpHandler GetXlgHandler(string url, string pagePath) { return null; }

        #region IHttpHandlerFactory Members

        /// <summary>
        /// Determines if an internal handler is appropriate, otherwise returns the result of the implementing class
        /// </summary>
        /// <param name="context">The HttpContext</param>
        /// <param name="requestType">"GET", "POST", etc.</param>
        /// <param name="url">The url of the request</param>
        /// <param name="pathTranslated">Not sure.</param>
        /// <returns>The IHttpHandler for the given URL</returns>
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            int x = url.IndexOf('/',1)+1;
            string pagePath = url.Substring(x, url.Length - x - (url.Length - url.LastIndexOf('.'))).ToLower();
            IHttpHandler ret = GetXlgHandler(url, pagePath);
            if (ret == null)
            {
                switch (pagePath)
                {
                    case "security": ret = new applications(); break;
                    case "security/applications": ret = new applications(); break;
                    case "security/editapplication": ret = new editApplication(); break;
                    case "security/editpermissions": ret = new editPermissions(); break;
                    case "security/editmembers": ret = new editMembers(); break;
                    case "security/editcookie": ret = new editCookie(); break;
                    case "viewtempdatafile": ret = new ViewTempDataFile(); break;
                }
                if (ret == null) 
                    if(pagePath.IndexOf("viewtempdatafile") > -1)
                        ret = new ViewTempDataFile();
                    else
                        ret = new xlgNullHandler(url);
            }
            return ret;
        }

        /// <summary>Does nothing</summary>
        /// <param name="handler">Does nothing</param>
        public void ReleaseHandler(IHttpHandler handler)
        {
            // nothing to do
        }

        #endregion
    }

    public class xlgNullHandler : IHttpHandler
    {
        string url;
        public xlgNullHandler(string url)
        {
            this.url = url;
        }
        #region IHttpHandler Members
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.Write("<html><head><title>Bad request</title></head><body>Bad request: <b>" + url + "</b></body></html>");
        }
        #endregion
    }
}
