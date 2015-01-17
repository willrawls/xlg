using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Caching;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using MetX.IO;
// // using MetX.Web;

namespace MetX.Library
{
	/// <summary>Helper functions for dealing with xsl transformation</summary>
	public class Xsl
	{
		private xlgUrnResolver m_UrlResolver;
		private XsltArgumentList m_Urns;
		private XlgUrn m_XlgUrn;

		/// <summary>Set this value when you wish your page's xslt compilation to be cached differently (say per theme, in this case the theme name will be sufficient).</summary>
		public string PageCacheSubKey = ".";

		/// <summary>Performs an XSL Transformation on an XmlDocument</summary>
		/// <param name="xmlDocument">The XmlDocument object to transform</param>
		/// <param name="sXsltDocument">The XSL document</param>
		public StringBuilder Transform(XmlDocument xmlDocument, string sXsltDocument)
		{
			XmlTextReader xsltDocument = new XmlTextReader(new StringReader(sXsltDocument));
			StringBuilder sb = new StringBuilder();
			using (StringWriter sw = new StringWriter(sb))
			{
				try
				{
					XslCompiledTransform xslDoc = new XslCompiledTransform(true);
					xslDoc.Load(xsltDocument, new XsltSettings(false, false), new XmlUrlResolver());
					using (XmlWriter xw = XmlWriter.Create(sw, xslDoc.OutputSettings))
						xslDoc.Transform(xmlDocument.CreateNavigator(), Urns, xw);
				}
				catch (Exception exp)
				{
					sb.AppendLine("<HTML><BODY><PRE>"
								+ (exp.ToString() + ("</PRE><br /><HR><br /><TEXTAREA COLS=90 ROWS=15>"
								+ (xmlDocument.OuterXml.Replace("><", (">" + ("\r\n" + "<"))) + "</TEXTAREA></BODY></HTML>"))));
				}
			}
			sb.Replace("&nbsp;", " ");
			sb.Replace("&nl;", Environment.NewLine);
			return sb;
		}

		/// <summary>Performs an XSL Transformation on an XmlDocument</summary>
		/// <param name="xmlDocument">The XmlDocument object to transform</param>
		/// <param name="sXsltDocument">The XSL document</param>
		public void Transform(TextWriter output, XmlDocument xmlDocument, string sXsltDocument)
		{
			XmlTextReader xsltDocument = new XmlTextReader(new StringReader(sXsltDocument));
			try
			{
				XslCompiledTransform xslDoc = new XslCompiledTransform(true);
				xslDoc.Load(xsltDocument, new XsltSettings(false, false), new XmlUrlResolver());
				using (XmlWriter xw = XmlWriter.Create(output, xslDoc.OutputSettings))
					xslDoc.Transform(xmlDocument.CreateNavigator(), Urns, xw);
			}
			catch (Exception exp)
			{
				output.WriteLine("<HTML><BODY><PRE>"
							+ (exp.ToString() + ("</PRE><br /><HR><br /><TEXTAREA COLS=90 ROWS=15>"
							+ (xmlDocument.OuterXml.Replace("><", (">" + ("\r\n" + "<"))) + "</TEXTAREA></BODY></HTML>"))));
			}
		}

		/// <summary>Performs an XSL Transformation on an XmlDocument optionally pulling the XslCompiledTransform object from the pagecache.</summary>
		/// <param name="pageCache">The PageCache object to retreive/store the XslCompiledTrasform from/to. If null, no caching is performed</param>
		/// <param name="sXmlDocument">The xml string to transform</param>
		/// <param name="xsltPath">The path and filename of the XSL file to load</param>
		/// <returns>The transformed text</returns>
		public StringBuilder Transform(Cache pageCache, StringBuilder sXmlDocument, string xsltPath)
		{
			XmlTextReader xmlDocument = new XmlTextReader(new StringReader(sXmlDocument.ToString()));
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			XmlWriter xw = null;

			try
			{
				if (PageCacheSubKey == null || PageCacheSubKey.Length == 0)
					PageCacheSubKey = ".";
				else if (!PageCacheSubKey.EndsWith("."))
					PageCacheSubKey += ".";
				XslCompiledTransform xslDoc = null;
				string cacheKey = "XslCompiledTransform." + PageCacheSubKey + xsltPath;
				if (pageCache != null && pageCache[cacheKey] != null)
				{
					xslDoc = (XslCompiledTransform)(pageCache[cacheKey]);
				}
				else
				{
					xslDoc = new XslCompiledTransform(false);
					xlgUrnResolver resolver = UrlResolver;
					xslDoc.Load(xsltPath, new XsltSettings(false, false), resolver);
					AggregateCacheDependency aggDep = new AggregateCacheDependency();
					foreach (string currEntity in resolver.FileEntitys)
						aggDep.Add(new CacheDependency(currEntity, DateTime.Now));
					if (pageCache != null)
						pageCache.Insert(cacheKey, xslDoc, aggDep);
				}

				xw = XmlWriter.Create(sw, xslDoc.OutputSettings);
				xslDoc.Transform(new XPathDocument(xmlDocument).CreateNavigator(), Urns, xw);
			}
			catch (Exception exp)
			{
				sb.AppendLine("<HTML><BODY><PRE>" + exp.Message + "</PRE><br /><HR><br /><PRE>" + exp.ToString() + "</PRE><br /><HR><br /><TEXTAREA COLS=90 ROWS=15>" + sXmlDocument.Replace("><", ">" + System.Environment.NewLine + "<") + "</TEXTAREA></BODY></HTML>");
			}
			finally
			{
				if (xw != null)
					xw.Close();
				sw.Close();
			}
			sb.Replace(" xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\" xmlns:xlg=\"urn:xlg\"", string.Empty);
			sb.Replace("&#xA;", string.Empty);
			sb.Replace("&amp;nbsp;", "&nbsp;");
			return sb;
		}

		/// <summary>Performs an XSL Transformation on an XmlDocument optionally pulling the XslCompiledTransform object from the pagecache.</summary>
		/// <param name="pageCache">The PageCache object to retreive/store the XslCompiledTrasform from/to. If null, no caching is performed</param>
		/// <param name="sXmlDocument">The xml string to transform</param>
		/// <param name="xsltPath">The path and filename of the XSL file to load</param>
		/// <param name="xsltDocumentContent">If supplied, the xsl stylesheet is assumed to already be loaded here. xsltPath is then used as a key to cache the request</param>
		/// <returns>The transformed text</returns>
		public StringBuilder Transform(Cache pageCache, StringBuilder sXmlDocument, string xsltPath, StringBuilder xsltDocumentContent)
		{
			if (xsltDocumentContent == null || xsltDocumentContent.Length == 0)
				return Transform(pageCache, sXmlDocument, xsltPath);


			XmlTextReader xmlDocument = new XmlTextReader(new StringReader(sXmlDocument.ToString()));
			XmlTextReader xsltDocument = new XmlTextReader(new StringReader(xsltDocumentContent.ToString()));
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			XmlWriter xw = null;

			try
			{
				if (PageCacheSubKey == null || PageCacheSubKey.Length == 0)
					PageCacheSubKey = ".";
				else if (!PageCacheSubKey.EndsWith("."))
					PageCacheSubKey += ".";

				XslCompiledTransform xslDoc = null;
				string cacheKey = "XslCompiledTransform.FromContent." + xsltPath;
				if (pageCache != null && pageCache[cacheKey] != null)
				{
					xslDoc = (XslCompiledTransform)(pageCache[cacheKey]);
				}
				else
				{
					xslDoc = new XslCompiledTransform(false); //true);
					xlgUrnResolver resolver = UrlResolver;
					xslDoc.Load(xsltDocument, new XsltSettings(false, false), new XmlUrlResolver());
					AggregateCacheDependency aggDep = new AggregateCacheDependency();
					foreach (string currEntity in resolver.FileEntitys)
						aggDep.Add(new CacheDependency(currEntity, DateTime.Now));
					if (pageCache != null)
						pageCache.Insert(cacheKey, xslDoc, aggDep);
				}

				xw = XmlWriter.Create(sw, xslDoc.OutputSettings);
				xslDoc.Transform(new XPathDocument(xmlDocument).CreateNavigator(), Urns, xw);
			}
			catch (Exception exp)
			{
				sb.AppendLine("<HTML><BODY><PRE>" + exp.Message + "</PRE><br /><HR><br /><PRE>" + exp.ToString() + "</PRE><br /><HR><br /><TEXTAREA COLS=90 ROWS=15>" + sXmlDocument.Replace("><", ">" + System.Environment.NewLine + "<") + "</TEXTAREA></BODY></HTML>");
			}
			finally
			{
				if (xw != null)
					xw.Close();
				sw.Close();
			}
			sb.Replace(" xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\" xmlns:xlg=\"urn:xlg\"", string.Empty);
			sb.Replace("&#xA;", string.Empty);
			sb.Replace("&amp;nbsp;", "&nbsp;");
			return sb;
		}

		/// <summary>Returns the page specific xlgUrnResolver or a default object if not specified.
		/// <para>NOTE: You can set this proprety in PreBuild() or BuildXml() to override it with your own implementation.</para>
		/// </summary>
		public xlgUrnResolver UrlResolver
		{
			get
			{
				if (m_UrlResolver == null)
					return new xlgUrnResolver();
				return m_UrlResolver;
			}
			set
			{
				m_UrlResolver = value;
			}
		}

		/// <summary>Automatically set to a new xlgUrn unless you supply your own..
		/// <para>NOTE: You can set this proprety in PreBuild() or BuildXml() to override it with your own implementation.</para>
		/// </summary>
		public XlgUrn XlgUrn
		{
			get
			{
				if (m_XlgUrn == null)
					m_XlgUrn = new XlgUrn();
				return m_XlgUrn;
			}
			set
			{
				m_XlgUrn = value;
			}
		}

		/// <summary>Returns XsltArgumentList containing a xlgUrn object and an optional object named in xlgSecurity.UrnName and xlgSecurity.UrnClass.
		/// <para>NOTE: You can set this proprety in PreBuild() or BuildXml() to override it with your own implementation.</para>
		/// </summary>
		public XsltArgumentList Urns
		{
			get
			{
				if (m_Urns != null)
					return m_Urns;

				XsltArgumentList argsList = new XsltArgumentList();
				argsList.AddExtensionObject("urn:xlg", XlgUrn);

				string urnName = ConfigurationManager.AppSettings["xlgSecurity.UrnName"];
				string urnClass = ConfigurationManager.AppSettings["xlgSecurity.UrnClass"];
				if (urnName != null && urnClass != null && urnName.Length > 0 & urnClass.Length > 0)
				{
					Assembly a;
					if (urnClass.IndexOf(",") > -1)
					{
						string assemblyName = urnClass.Substring(urnClass.IndexOf(",") + 1).Trim();
						urnClass = urnClass.Substring(0, urnClass.IndexOf(",")).Trim();
						a = Assembly.Load(assemblyName);
					}
					else
					{
						a = Assembly.GetCallingAssembly();
					}
					object o = a.CreateInstance(urnClass);
					if (o != null)
						argsList.AddExtensionObject("urn:" + urnName, o);
				}
				return argsList;
			}
			set
			{
				m_Urns = value;
			}
		}

		public string TransformISO(string xslPath, string xmlDoc) { return TransformISO(xslPath, xmlDoc, "iso-8859-1"); }
		public string TransformISO(string xslPath, string xmlDoc, string encodingName)
		{
			System.IO.StringReader sr = new System.IO.StringReader(xmlDoc);
			XmlTextReader xmlDocument = new XmlTextReader(sr);
			XslCompiledTransform xslt = new XslCompiledTransform(true);
			StringWriterWithEncoding sw = new StringWriterWithEncoding(
				new StringBuilder()
				, Encoding.GetEncoding(encodingName));
			XmlWriter xw = null;

			XsltSettings settings = new XsltSettings(false, true);
			xslt.Load(xslPath, settings, new XmlUrlResolver());
			xw = XmlTextWriter.Create(sw, xslt.OutputSettings);
			XsltArgumentList argsList = new XsltArgumentList();
			argsList.AddExtensionObject("urn:xlg", new XlgUrn());

			xslt.Transform(new XPathDocument(xmlDocument).CreateNavigator(), argsList, xw);

			return sw.ToString();
		}

		public string Transform(string xslPath, string xmlDoc)
		{
			System.IO.StringReader sr = new System.IO.StringReader(xmlDoc);
			XmlTextReader xmlDocument = new XmlTextReader(sr);
			XslCompiledTransform xslt = new XslCompiledTransform(true);
			System.IO.StringWriter sw = new System.IO.StringWriter();
			XmlWriter xw = null;

			XsltSettings settings = new XsltSettings(false, true);
			xslt.Load(xslPath, settings, new XmlUrlResolver());
			xw = XmlTextWriter.Create(sw, xslt.OutputSettings);
			XsltArgumentList argsList = new XsltArgumentList();
			argsList.AddExtensionObject("urn:xlg", new XlgUrn());
			xslt.Transform(new XPathDocument(xmlDocument).CreateNavigator(), argsList, xw);
			return sw.ToString();
		}
	}
}
