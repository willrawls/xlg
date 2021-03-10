using System;
using System.IO;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using MetX;
using MetX.IO;
using MetX.Urn;
using MetX.Web;
using MetX.Security;
using MetX.Data;

namespace MetX.Web.Virtual
{
    /// <summary>An unsecured HttpHandler for serving up files from some temporary path ( such as App_Data/temp )</summary>
    public class ViewTempDataFile : xlgHandler
    {
        /// <summary>Processes the request to return the contents of a temporary file.</summary>
        public override void ProcessRequest()
        {
            string filename = Request["v"];
            string format = Request["f"];
            if (filename != null && filename.Length > 0)
            {
                StringBuilder Contents = (StringBuilder) Session["vcontent"];
                Session.Remove("vcontent");
                string jf = filename;
                if (Contents != null)
                {
                    if (format == "excel")
                    {
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.Charset = string.Empty;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + jf);
                        Contents.Replace("Styles.css", "StylesExcel.css");
                        Response.Write(Contents);
                    }
                }
            }
        }
    }
}
