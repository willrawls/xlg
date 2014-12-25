using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace MetX.Web
{
    /// <summary>Base class from which all xlg style ASP.NET Page implementations derive</summary>
    public class xlgPage : System.Web.UI.Page
    {
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
//            PagePage = PagePage.Replace(".aspx", ".xsl").Replace(".asp", ".xsl").Replace(".xml", ".xsl").Replace(".xslx", ".xsl").Replace(".axd", ".xsl");
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
