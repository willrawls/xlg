using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
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
		private XlgUrnResolver _mUrlResolver;
		private XsltArgumentList _mUrns;
		private XlgUrn _mXlgUrn;

		/// <summary>Set this value when you wish your page's xslt compilation to be cached differently (say per theme, in this case the theme name will be sufficient).</summary>
		public string PageCacheSubKey = ".";

        public static readonly InMemoryCache<XslCompiledTransform> PageCache = new InMemoryCache<XslCompiledTransform>();

		/// <summary>Performs an XSL Transformation on an XmlDocument</summary>
		/// <param name="xmlDocument">The XmlDocument object to transform</param>
		/// <param name="sXsltDocument">The XSL document</param>
		public StringBuilder Transform(XmlDocument xmlDocument, string sXsltDocument)
		{
			var xsltDocument = new XmlTextReader(new StringReader(sXsltDocument));
			var sb = new StringBuilder();
			using (var sw = new StringWriter(sb))
			{
				try
				{
					var xslDoc = new XslCompiledTransform(true);
					xslDoc.Load(xsltDocument, new XsltSettings(false, false), new XmlUrlResolver());
					using (var xw = XmlWriter.Create(sw, xslDoc.OutputSettings))
						xslDoc.Transform(xmlDocument.CreateNavigator(), Urns, xw);
				}
				catch (Exception exp)
				{
					sb.AppendLine("<HTML><BODY><PRE>"
								+ (exp + ("</PRE><br /><HR><br /><TEXTAREA COLS=90 ROWS=15>"
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
			var xsltDocument = new XmlTextReader(new StringReader(sXsltDocument));
			try
			{
				var xslDoc = new XslCompiledTransform(true);
				xslDoc.Load(xsltDocument, new XsltSettings(false, false), new XmlUrlResolver());
				using (var xw = XmlWriter.Create(output, xslDoc.OutputSettings))
					xslDoc.Transform(xmlDocument.CreateNavigator(), Urns, xw);
			}
			catch (Exception exp)
			{
				output.WriteLine("<HTML><BODY><PRE>"
							+ (exp + ("</PRE><br /><HR><br /><TEXTAREA COLS=90 ROWS=15>"
                                      + (xmlDocument.OuterXml.Replace("><", (">" + ("\r\n" + "<"))) + "</TEXTAREA></BODY></HTML>"))));
			}
		}

		/// <summary>Performs an XSL Transformation on an XmlDocument optionally pulling the XslCompiledTransform object from the pagecache.</summary>
		/// <param name="sXmlDocument">The xml string to transform</param>
		/// <param name="xsltPath">The path and filename of the XSL file to load</param>
		/// <returns>The transformed text</returns>
		public StringBuilder Transform(StringBuilder sXmlDocument, string xsltPath)
		{
			var xmlDocument = new XmlTextReader(new StringReader(sXmlDocument.ToString()));
			var sb = new StringBuilder();
			var sw = new StringWriter(sb);
			XmlWriter xmlWriter = null;

			try
			{
				if (string.IsNullOrEmpty(PageCacheSubKey))
					PageCacheSubKey = ".";
				else if (!PageCacheSubKey.EndsWith("."))
					PageCacheSubKey += ".";
                
				XslCompiledTransform compiledTransform;
				var cacheKey = "XslCompiledTransform." + PageCacheSubKey + xsltPath;
				if (PageCache.Contains(cacheKey))
				{
					compiledTransform = PageCache[cacheKey];
				}
				else
				{
					compiledTransform = new XslCompiledTransform(false);
					var resolver = UrlResolver;
					compiledTransform.Load(xsltPath, new XsltSettings(false, false), resolver);
                    PageCache[cacheKey] = compiledTransform;
                }

				xmlWriter = XmlWriter.Create(sw, compiledTransform.OutputSettings);
				compiledTransform.Transform(new XPathDocument(xmlDocument).CreateNavigator(), Urns, xmlWriter);
			}
			catch (Exception exp)
			{
				sb.AppendLine("<HTML><BODY><PRE>" + exp.Message + "</PRE><br /><HR><br /><PRE>" + exp + "</PRE><br /><HR><br /><TEXTAREA COLS=90 ROWS=15>" + sXmlDocument.Replace("><", ">" + Environment.NewLine + "<") + "</TEXTAREA></BODY></HTML>");
			}
			finally
			{
                xmlWriter?.Close();
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
		public StringBuilder Transform(StringBuilder sXmlDocument, string xsltPath, StringBuilder xsltDocumentContent)
		{
			if (xsltDocumentContent == null || xsltDocumentContent.Length == 0)
				return Transform(sXmlDocument, xsltPath);


			var xmlDocument = new XmlTextReader(new StringReader(sXmlDocument.ToString()));
			var xsltDocument = new XmlTextReader(new StringReader(xsltDocumentContent.ToString()));
			var sb = new StringBuilder();
			var sw = new StringWriter(sb);
			XmlWriter xw = null;

			try
			{
				if (PageCacheSubKey.IsEmpty())
					PageCacheSubKey = ".";
				else if (!PageCacheSubKey.EndsWith("."))
					PageCacheSubKey += ".";

				XslCompiledTransform xslDoc;
				var cacheKey = "XslCompiledTransform.FromContent." + xsltPath;
				if (PageCache.Contains(cacheKey))
				{
					xslDoc = PageCache[cacheKey];
				}
				else
				{
					xslDoc = new XslCompiledTransform(false); //true);
					var resolver = UrlResolver;
					xslDoc.Load(xsltDocument, new XsltSettings(false, false), new XmlUrlResolver());
                    PageCache[cacheKey] = xslDoc;
                }

				xw = XmlWriter.Create(sw, xslDoc.OutputSettings);
				xslDoc.Transform(new XPathDocument(xmlDocument).CreateNavigator(), Urns, xw);
			}
			catch (Exception exp)
			{
				sb.AppendLine("<HTML><BODY><PRE>" + exp.Message + "</PRE><br /><HR><br /><PRE>" + exp + "</PRE><br /><HR><br /><TEXTAREA COLS=90 ROWS=15>" + sXmlDocument.Replace("><", ">" + Environment.NewLine + "<") + "</TEXTAREA></BODY></HTML>");
			}
			finally
			{
                xw?.Close();
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
		public XlgUrnResolver UrlResolver
		{
			get
			{
				if (_mUrlResolver == null)
					return new XlgUrnResolver();
				return _mUrlResolver;
			}
			set
			{
				_mUrlResolver = value;
			}
		}

		/// <summary>Automatically set to a new xlgUrn unless you supply your own..
		/// <para>NOTE: You can set this proprety in PreBuild() or BuildXml() to override it with your own implementation.</para>
		/// </summary>
		public XlgUrn XlgUrn
		{
			get
			{
				if (_mXlgUrn == null)
					_mXlgUrn = new XlgUrn();
				return _mXlgUrn;
			}
			set
			{
				_mXlgUrn = value;
			}
		}

		/// <summary>Returns XsltArgumentList containing a xlgUrn object and an optional object named in xlgSecurity.UrnName and xlgSecurity.UrnClass.
		/// <para>NOTE: You can set this proprety in PreBuild() or BuildXml() to override it with your own implementation.</para>
		/// </summary>
		public XsltArgumentList Urns
		{
			get
			{
				if (_mUrns != null)
					return _mUrns;

				var argsList = new XsltArgumentList();
				argsList.AddExtensionObject("urn:xlg", XlgUrn);

				var urnName = ConfigurationManager.AppSettings["xlgSecurity.UrnName"];
				var urnClass = ConfigurationManager.AppSettings["xlgSecurity.UrnClass"];
				if (urnName != null && urnClass != null && urnName.Length > 0 & urnClass.Length > 0)
				{
					Assembly a;
					if (urnClass.IndexOf(",", StringComparison.Ordinal) > -1)
					{
						var assemblyName = urnClass.Substring(urnClass.IndexOf(",", StringComparison.Ordinal) + 1).Trim();
						urnClass = urnClass.Substring(0, urnClass.IndexOf(",", StringComparison.Ordinal)).Trim();
						a = Assembly.Load(assemblyName);
					}
					else
					{
						a = Assembly.GetCallingAssembly();
					}
					var o = a.CreateInstance(urnClass);
					if (o != null)
						argsList.AddExtensionObject("urn:" + urnName, o);
				}
				return argsList;
			}
			set
			{
				_mUrns = value;
			}
		}

		public string TransformIso(string xslPath, string xmlDoc) { return TransformIso(xslPath, xmlDoc, "iso-8859-1"); }
		public string TransformIso(string xslPath, string xmlDoc, string encodingName)
		{
			var sr = new StringReader(xmlDoc);
			var xmlDocument = new XmlTextReader(sr);
			var xslt = new XslCompiledTransform(true);
			var sw = new StringWriterWithEncoding(
				new StringBuilder()
				, Encoding.GetEncoding(encodingName));
			XmlWriter xw;

			var settings = new XsltSettings(false, true);
			xslt.Load(xslPath, settings, new XmlUrlResolver());
			xw = XmlWriter.Create(sw, xslt.OutputSettings);
			var argsList = new XsltArgumentList();
			argsList.AddExtensionObject("urn:xlg", new XlgUrn());

			xslt.Transform(new XPathDocument(xmlDocument).CreateNavigator(), argsList, xw);

			return sw.ToString();
		}

		public string Transform(string xslPath, string xmlDoc)
		{
			var sr = new StringReader(xmlDoc);
			var xmlDocument = new XmlTextReader(sr);
			var xslt = new XslCompiledTransform(true);
			var sw = new StringWriter();
			XmlWriter xw;

			var settings = new XsltSettings(false, true);
			xslt.Load(xslPath, settings, new XmlUrlResolver());
			xw = XmlWriter.Create(sw, xslt.OutputSettings);
			var argsList = new XsltArgumentList();
			argsList.AddExtensionObject("urn:xlg", new XlgUrn());
			xslt.Transform(new XPathDocument(xmlDocument).CreateNavigator(), argsList, xw);
			return sw.ToString();
		}
	}
}
