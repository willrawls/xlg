using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;

using System.IO;

namespace Metta.Web
{
	
	/// <summary>Depricated for use. Please use XslHandler. This was the original unsecured ASP.NET Page derived class allowing pages to render XML/XSL xlg style pages vs. ASP.NET style pages.</summary>
	public class XslPage : System.Web.UI.Page
	{
		/// <summary>C#CD: </summary>
		public string xsltPath = "";
		
		/// <summary>C#CD: </summary>
		private string RootName = "";
		
		/// <summary>C#CD: </summary>
		private StringBuilder sb;
		
		/// <summary>C#CD: </summary>
		private StringWriter sw;

        /// <summary>C#CD: </summary>
        /// <param name="e">C#CD: </param>
        protected override void OnLoad(System.EventArgs e)
		{
            sb = new StringBuilder();
            sw = new StringWriter(sb);
            Response.ExpiresAbsolute = System.DateTime.Now.AddDays(-2);
			Response.Cache.SetExpires(DateTime.Now.AddSeconds(60));
			Response.Cache.SetCacheability(HttpCacheability.Public);
			Response.Cache.SetValidUntilExpires(true);
			Response.Cache.VaryByParams["*"] = true;
            base.OnLoad(e);
        }

        /// <summary>C#CD: </summary>
        public void Begin()
        {
            Begin("xlgDoc", string.Empty);
        }

		/// <summary>C#CD: </summary>
		/// <param name="RootNodeName">C#CD: </param>
		public void Begin(string RootNodeName)
		{
            Begin(RootNodeName, string.Empty);
		}

		/// <summary>C#CD: </summary>
		/// <param name="RootNodeName">C#CD: </param>
		/// <param name="Attributes">C#CD: </param>
		public void Begin(string RootNodeName, string Attributes)
		{
			sw.Write("<?xml version=\"1.0\" ?>" + System.Environment.NewLine);
			if (RootNodeName.Length > 0)
			{
				RootName = ((string)(((string)(((string)(RootNodeName.Replace("<", ""))).Replace(">", ""))).Replace("\"", ""))).Replace("'", "");
				sw.Write("<" + RootName);
				if (Attributes.Length > 0)
				{
					sw.Write(" " + Attributes.Trim() + " ");
				}
				sw.Write(">" + System.Environment.NewLine);
			}
		}
		
		/// <summary>C#CD: </summary>
		private void EndDocument()
		{
			if (RootName.Length > 0)
				sw.Write("</" + RootName + ">");
		}

        /// <summary>C#CD: </summary>
        /// <param name="sToAppend">C#CD: </param>
        public void Append(string sToAppend)
        {
            sw.Write(sToAppend);
        }

		/// <summary>C#CD: </summary>
		/// <param name="sToAppend">C#CD: </param>
		public void AppendLine(string sToAppend)
		{
			sw.WriteLine(sToAppend);
		}

		/// <summary>C#CD: </summary>
		public void RawXml()
		{
			Response.ContentType = "text/xml";
			NoTransform();
		}
		
		/// <summary>C#CD: </summary>
		/// <param name="xsltPath">C#CD: </param>
		public void Transform(string xsltPath)
		{
			EndDocument();
            Response.Write(xml.xslTransform(Page.Cache, sb.ToString(), MassageXsltPath(xsltPath)));
			sw.Close();
		}

		
		/// <summary>C#CD: </summary>
		public void Transform()
		{
			if (Microsoft.VisualBasic.Strings.Len(Request["rawxml"]) > 0)
			{
				RawXml();
			}
			else if (Microsoft.VisualBasic.Strings.Len(Request["format"]) > 0)
			{
				if (Request["format"] == "excel")
				{
					Response.ContentType = "application/vnd.ms-excel";
					Response.Charset = string.Empty;
					EnableViewState = false;
					Response.Write(sTransform());
				}
			}
			else
			{
				EndDocument();
				if (xsltPath.Length == 0)
				{
                    Response.Write(xml.xslTransform(Page.Cache, sb.ToString(), MassageXsltPath(Request.ServerVariables["URL"])));
				}
				else
				{
                    Response.Write(xml.xslTransform(Page.Cache, sb.ToString(), MassageXsltPath(xsltPath)));
				}
				sw.Close();
			}
		}

		
		/// <summary>C#CD: </summary>
		/// <returns>C#CD: </returns>
		public string sTransform()
		{
			string ReturnValue = null;
			EndDocument();
			if (xsltPath.Length == 0)
			{
                ReturnValue = xml.xslTransform(Page.Cache, sb.ToString(), MassageXsltPath(Request.ServerVariables["URL"]));
			}
			else
			{
                ReturnValue = xml.xslTransform(Page.Cache, sb.ToString(), MassageXsltPath(xsltPath));
			}
			sw.Close();
			return ReturnValue;
		}

		
		/// <summary>C#CD: </summary>
		/// <returns>C#CD: </returns>
		public string sRawXml()
		{
			string ReturnValue = null;
			EndDocument();
			ReturnValue = sb.ToString();
			sw.Close();
			return ReturnValue;
		}

		
		/// <summary>C#CD: </summary>
		public void NoTransform()
		{
			EndDocument();
			Response.Write(sb.ToString());
			sw.Close();
		}
    }


} 