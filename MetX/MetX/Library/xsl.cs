using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;

using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Data.SqlClient;
using System.Web.Caching;
using System.Reflection;

using MetX;
using MetX.IO;
using MetX.Urn;
// // using MetX.Web;
using MetX.Security;
using MetX.Data;

namespace MetX
{
	/// <summary>Helper functions for dealing with xsl transformation</summary>
	public class xsl
	{
		private xlgUrnResolver m_urlResolver;
		private XsltArgumentList m_urns;
		private XlgUrn m_xlgUrn;

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
						xslDoc.Transform(xmlDocument.CreateNavigator(), urns, xw);
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
		public void Transform(TextWriter Output, XmlDocument xmlDocument, string sXsltDocument)
		{
			XmlTextReader xsltDocument = new XmlTextReader(new StringReader(sXsltDocument));
			try
			{
				XslCompiledTransform xslDoc = new XslCompiledTransform(true);
				xslDoc.Load(xsltDocument, new XsltSettings(false, false), new XmlUrlResolver());
				using (XmlWriter xw = XmlWriter.Create(Output, xslDoc.OutputSettings))
					xslDoc.Transform(xmlDocument.CreateNavigator(), urns, xw);
			}
			catch (Exception exp)
			{
				Output.WriteLine("<HTML><BODY><PRE>"
							+ (exp.ToString() + ("</PRE><br /><HR><br /><TEXTAREA COLS=90 ROWS=15>"
							+ (xmlDocument.OuterXml.Replace("><", (">" + ("\r\n" + "<"))) + "</TEXTAREA></BODY></HTML>"))));
			}
		}

		/// <summary>Performs an XSL Transformation on an XmlDocument optionally pulling the XslCompiledTransform object from the pagecache.</summary>
		/// <param name="PageCache">The PageCache object to retreive/store the XslCompiledTrasform from/to. If null, no caching is performed</param>
		/// <param name="sXmlDocument">The xml string to transform</param>
		/// <param name="xsltPath">The path and filename of the XSL file to load</param>
		/// <returns>The transformed text</returns>
		public StringBuilder Transform(Cache PageCache, StringBuilder sXmlDocument, string xsltPath)
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
				string CacheKey = "XslCompiledTransform." + PageCacheSubKey + xsltPath;
				if (PageCache != null && PageCache[CacheKey] != null)
				{
					xslDoc = (XslCompiledTransform)(PageCache[CacheKey]);
				}
				else
				{
					xslDoc = new XslCompiledTransform(false);
					xlgUrnResolver Resolver = urlResolver;
					xslDoc.Load(xsltPath, new XsltSettings(false, false), Resolver);
					AggregateCacheDependency aggDep = new AggregateCacheDependency();
					foreach (string CurrEntity in Resolver.FileEntitys)
						aggDep.Add(new CacheDependency(CurrEntity, DateTime.Now));
					if (PageCache != null)
						PageCache.Insert(CacheKey, xslDoc, aggDep);
				}

				xw = XmlWriter.Create(sw, xslDoc.OutputSettings);
				xslDoc.Transform(new XPathDocument(xmlDocument).CreateNavigator(), urns, xw);
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
		/// <param name="PageCache">The PageCache object to retreive/store the XslCompiledTrasform from/to. If null, no caching is performed</param>
		/// <param name="sXmlDocument">The xml string to transform</param>
		/// <param name="xsltPath">The path and filename of the XSL file to load</param>
		/// <param name="xsltDocumentContent">If supplied, the xsl stylesheet is assumed to already be loaded here. xsltPath is then used as a key to cache the request</param>
		/// <returns>The transformed text</returns>
		public StringBuilder Transform(Cache PageCache, StringBuilder sXmlDocument, string xsltPath, StringBuilder xsltDocumentContent)
		{
			if (xsltDocumentContent == null || xsltDocumentContent.Length == 0)
				return Transform(PageCache, sXmlDocument, xsltPath);


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
				string CacheKey = "XslCompiledTransform.FromContent." + xsltPath;
				if (PageCache != null && PageCache[CacheKey] != null)
				{
					xslDoc = (XslCompiledTransform)(PageCache[CacheKey]);
				}
				else
				{
					xslDoc = new XslCompiledTransform(false); //true);
					xlgUrnResolver Resolver = urlResolver;
					xslDoc.Load(xsltDocument, new XsltSettings(false, false), new XmlUrlResolver());
					AggregateCacheDependency aggDep = new AggregateCacheDependency();
					foreach (string CurrEntity in Resolver.FileEntitys)
						aggDep.Add(new CacheDependency(CurrEntity, DateTime.Now));
					if (PageCache != null)
						PageCache.Insert(CacheKey, xslDoc, aggDep);
				}

				xw = XmlWriter.Create(sw, xslDoc.OutputSettings);
				xslDoc.Transform(new XPathDocument(xmlDocument).CreateNavigator(), urns, xw);
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
		public xlgUrnResolver urlResolver
		{
			get
			{
				if (m_urlResolver == null)
					return new xlgUrnResolver();
				return m_urlResolver;
			}
			set
			{
				m_urlResolver = value;
			}
		}

		/// <summary>Automatically set to a new xlgUrn unless you supply your own..
		/// <para>NOTE: You can set this proprety in PreBuild() or BuildXml() to override it with your own implementation.</para>
		/// </summary>
		public XlgUrn xlgUrn
		{
			get
			{
				if (m_xlgUrn == null)
					m_xlgUrn = new XlgUrn();
				return m_xlgUrn;
			}
			set
			{
				m_xlgUrn = value;
			}
		}

		/// <summary>Returns XsltArgumentList containing a xlgUrn object and an optional object named in xlgSecurity.UrnName and xlgSecurity.UrnClass.
		/// <para>NOTE: You can set this proprety in PreBuild() or BuildXml() to override it with your own implementation.</para>
		/// </summary>
		public XsltArgumentList urns
		{
			get
			{
				if (m_urns != null)
					return m_urns;

				XsltArgumentList argsList = new XsltArgumentList();
				argsList.AddExtensionObject("urn:xlg", xlgUrn);

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
				m_urns = value;
			}
		}

		public string TransformISO(string XslPath, string XmlDoc) { return TransformISO(XslPath, XmlDoc, "iso-8859-1"); }
		public string TransformISO(string XslPath, string XmlDoc, string EncodingName)
		{
			System.IO.StringReader sr = new System.IO.StringReader(XmlDoc);
			XmlTextReader xmlDocument = new XmlTextReader(sr);
			XslCompiledTransform xslt = new XslCompiledTransform(true);
			StringWriterWithEncoding sw = new StringWriterWithEncoding(
				new StringBuilder()
				, Encoding.GetEncoding(EncodingName));
			XmlWriter xw = null;

			XsltSettings settings = new XsltSettings(false, true);
			xslt.Load(XslPath, settings, new XmlUrlResolver());
			xw = XmlTextWriter.Create(sw, xslt.OutputSettings);
			XsltArgumentList argsList = new XsltArgumentList();
			argsList.AddExtensionObject("urn:xlg", new MetX.Urn.XlgUrn());

			xslt.Transform(new XPathDocument(xmlDocument).CreateNavigator(), argsList, xw);

			return sw.ToString();
		}

		public string Transform(string XslPath, string XmlDoc)
		{
			System.IO.StringReader sr = new System.IO.StringReader(XmlDoc);
			XmlTextReader xmlDocument = new XmlTextReader(sr);
			XslCompiledTransform xslt = new XslCompiledTransform(true);
			System.IO.StringWriter sw = new System.IO.StringWriter();
			XmlWriter xw = null;

			XsltSettings settings = new XsltSettings(false, true);
			xslt.Load(XslPath, settings, new XmlUrlResolver());
			xw = XmlTextWriter.Create(sw, xslt.OutputSettings);
			XsltArgumentList argsList = new XsltArgumentList();
			argsList.AddExtensionObject("urn:xlg", new XlgUrn());
			xslt.Transform(new XPathDocument(xmlDocument).CreateNavigator(), argsList, xw);
			return sw.ToString();
		}
	}
}
