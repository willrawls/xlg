using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;

using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
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
    public class StringWriterWithEncoding : System.IO.StringWriter
    {
        Encoding m_encoding;
        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
            : base(sb)
        {
            m_encoding = encoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return m_encoding;
            }
        }
    }

	public class xhtml
	{
		public const string Declaration = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
		public const string TagHtmlBegin = "<html xmlns=\"http://www.w3.org/1999/xhtml\">";

		public void BeginDoc(TextWriter Output, XmlDocument Target)
		{
			Output.WriteLine(xhtml.Declaration);
			Output.WriteLine(xhtml.TagHtmlBegin);
			Output.WriteLine("<title>Auto Output</title>");
			Output.WriteLine("</head>");
			Output.WriteLine("<body>");
		}

		public void EndDoc(TextWriter Output, XmlDocument Target)
		{
			Output.WriteLine("</body>");
			Output.WriteLine("</html>");
		}

		public void ToDiv(TextWriter Output, XmlElement Target)
		{
			Output.WriteLine("<span class=\"" + Target.Name + "_span\">" + Target.Name + "</span>");
			Output.WriteLine("<div class=\"" + Target.Name + "_div\">");
				throw new Exception("Coding not completed. Finish or don't use me.");
			Output.WriteLine("<div>");
		}
	}

	/// <summary>Helper functions for dealing with xml strings</summary>
	public class xml
    {
		/// <summary>The ?xml directive that should be at the top of each file</summary>
		public const string Declaration = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";

		private static SortedList<int, XmlSerializer> Serializers;

        /// <summary>Converts an XmlElement into a JSON string and appends it to Target</summary>
        /// <param name="Element">The XmlElment to walk and translate to JSON</param>
        /// <param name="Target">The StringBuilder to append the JSON string into</param>
        public static void ToJson(XmlElement Element, StringBuilder Target)
        {
            ToJson(Element, Target, false, string.Empty, true);
        }

        private static void ToJson(XmlElement Element, StringBuilder Target, bool ContainedInArray, string Indent, bool WrapWithBrace)
        {
            /* Takes an XML document such as:
                <a fred="george">
                    <b x="y">
                        <c>frank</c>
                    </b>
                    <b x="z">
                        <c>mary</c>
                    </b>
                    <b>
                        
                    </b>
                </a>
             
             And generates a json string such as:
                var VariableName = {"a": {"fred":"george", "b": [ { "x": "y", "c": "frank" }, { "x": "y", "c": "mary" } } ]};
             */
            bool CommaNeeded = false;
            if (Element.HasChildNodes || Element.HasAttributes)
            {
                if(ContainedInArray)
                    Target.Append("\n" + Indent + "{");
                else
                    Target.Append("\n" + Indent + "" + (WrapWithBrace ? "{" : string.Empty) + "\"" + Element.Name + "\":/*2*/\n" + Indent + "\t{");

                if (Element.HasAttributes)
                {
                    foreach (XmlAttribute CurrAttribute in Element.Attributes)
                    {
                        if (CommaNeeded)
                            Target.Append(",\"");
                        else
                            Target.Append("\"");
                        Target.Append(CurrAttribute.Name);
                        Target.Append("\":\"");
                        Target.Append(CurrAttribute.Value.Replace("\"", "\\\"").Replace(@"\", @"\\"));
                        Target.Append("\"");
                        CommaNeeded = true;
                    }
                    if (CommaNeeded && Element.HasChildNodes) { Target.Append(",/*8*/"); CommaNeeded = false; }
                }
                if (Element.HasChildNodes)
                {
                    string LastNodeName = null;
                    if (Element.ChildNodes.Count == 1)
                    {
                        if (Element.ChildNodes[0] is XmlElement)
                        {
                            if (CommaNeeded) { Target.Append(",/*3*/"); CommaNeeded = false; }
                            ToJson((XmlElement)Element.ChildNodes[0], Target, false, Indent + "\t", false);
                        }
                    }
                    else
                    {
                        string Opener = "{";
                        string Closer = "}";
                        foreach (XmlElement CurrChild in Element.ChildNodes)
                        {
                            if (CurrChild is XmlElement)
                            {
                                if (CurrChild.Name != LastNodeName)
                                {
                                    if (CurrChild.NextSibling != null)
                                    {
                                        if (LastNodeName != null) Target.Append(Closer + ",/*1*/");
                                        if (CurrChild.NextSibling.Name == CurrChild.Name)
                                        {
                                            Opener = "[";
                                            Closer = "]";
                                        }
                                    }
                                    else
                                    {
                                        if (LastNodeName != null) Target.Append(Closer + "/*9*/");
                                    }
                                    Target.Append("\n\t" + Indent + "\"" + CurrChild.Name + "\":/*4*/"); // + Opener);
                                    if (Opener == "[")
                                        Target.Append(Opener);
                                    CommaNeeded = false;
                                }
                                if (CommaNeeded) { Target.Append(",/*7*/"); CommaNeeded = false;  }
                                ToJson((XmlElement)CurrChild, Target, true, Indent + "\t", true);
                                LastNodeName = CurrChild.Name;
                                CommaNeeded = true;
                            }
                        }
                        Target.Append(Closer);
                    }
                }
                if(WrapWithBrace)
                    Target.AppendLine("}");
                Target.Append(Indent);
            }
            else if (Element.Value != null)
            {
                Target.Append("\n" + Indent + "{\"" + Element.Name + "\":\"" + Element.Value.Replace("\"", "\\\"").Replace(@"\", @"\\") + "\"} /*5*/");
            }
            else
            {
                Target.Append("\n" + Indent + "{\"" + Element.Name + "\":null /*6*/} ");
            }
        }

        /// <summary>For use when you are building an xml string and you need to insure the value of an attribute is properly encoded.</summary>
        /// <param name="strIn">The text to encode as an xml attribute</param>
        /// <returns>The xml attribute encoded string</returns>
        /// 
        /// <example>
        /// <code>
        /// string x = "&amp;amp;amp;amp;amp;lt;Item Name=\"" + AttributeEncode("Yes &amp;amp;amp;amp;amp;amp; no") + "\" /&amp;amp;amp;amp;amp;gt;";
        /// // x = &amp;amp;amp;amp;amp;lt;Item Name="Yes &amp;amp;amp;amp;amp;amp;amp; no" /&amp;amp;amp;amp;amp;gt;
        /// </code>
        /// </example>
        public static string AttributeEncode(object strIn)
        {
            StringBuilder sb = new StringBuilder(Worker.nzString(strIn));
            sb.Replace(Convert.ToString((char)147), "&quot;");
            sb.Replace(Convert.ToString((char)148), "&quot;");
            sb.Replace(Convert.ToString((char)150), "-");
            sb.Replace(Convert.ToString((char)183), "-");
            sb.Replace("&", "&amp;");
            sb.Replace("\r\n", "&#xd;&#xa;");
            sb.Replace("\r", "&#xd;");
            sb.Replace("\n", "&#xa;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\"", "&quot;");
            sb.Replace("'", "&apos;");
            sb.Replace(Convert.ToString((char)9), "&#x9;");
            sb.Replace("’", "&apos;");
            sb.Replace(Convert.ToString((char)194), "A");
            return sb.ToString();
        }

        /// <summary>Wraps some text in a tag with optional attributes for that tag. NOTE: tagValue may contain any valid text or XML
        /// <para>Handles several scenarios when tagValue is blank.</para>
        /// </summary>
        /// <param name="TagName">The tag to wrap tagValue in</param>
        /// <param name="tagValue">The text to wrap as the text node of the wrapping tag</param>
        /// <param name="tagAttributes">The attributes of the wrapping tag (optional)</param>
        /// <returns>An xml string with a TagName element having tagAttributes as attributes wrapping tagValue</returns>
        /// 
        /// <exmaple>
        /// <code>
        /// string x = Wrap("Item", "This is a test", "Source=\"Somewhere\"");
        /// // x = &amp;amp;amp;amp;amp;lt;Item Source="Somewhere"&amp;amp;amp;amp;amp;gt;This is a test&amp;amp;amp;amp;amp;lt;/Item/&amp;amp;amp;amp;amp;gt;
        /// </code>
        /// </exmaple>
        public static string Wrap(string TagName, string tagValue, string tagAttributes)
        {
            if (tagAttributes == null)
                tagAttributes = string.Empty;
            else
                tagAttributes = tagAttributes.Trim();

            if (tagAttributes.Length == 0)
            {
                if ((tagValue).Length > 0)
                    // <tagName>
                    //    <tagName .../>
                    // </tagName>
                    return "<" + TagName + ">\r\n" + tagValue + "</" + TagName + ">\r\n";
                // <tagName />
                else
                    return "<" + TagName + "/>\r\n";
            }
            else
            {
                if ((tagValue).Length > 0)
                    // <tagName tagAttributes ... >
                    //    <tagName .../>
                    // </tagName>
                    return "<" + TagName + " " + tagAttributes + ">\r\n" + tagValue + "</" + TagName + ">\r\n";
                // <tagName tagAttributes ... />
                else
                    return "<" + TagName + " " + tagAttributes + "/>";
            }
        }


        /// <summary>Wraps some text in a xml element. NOTE: tagValue may contain any valid text or XML</summary>
        /// <param name="TagName">The tag to wrap tagValue in</param>
        /// <param name="tagValue">The text to wrap as the text node of the wrapping tag</param>
        /// <returns>An xml string with a TagName element wrapping tagValue</returns>
        /// 
        /// <exmaple>
        /// <code>
        /// string x = Wrap("Item", "This is a test");
        /// // x = &amp;amp;amp;amp;amp;lt;Item&amp;amp;amp;amp;amp;gt;This is a test&amp;amp;amp;amp;amp;lt;/Item/&amp;amp;amp;amp;amp;gt;
        /// </code>
        /// </exmaple>
        public static string Wrap(string TagName, string tagValue)
        {
            return Wrap(TagName, tagValue, null);
        }

		/// <summary>
		/// Don't forget to close the XmlWriter or wrap this line in a using statement
		/// </summary>
		/// <param name="Output">The stream to wrap</param>
		/// <returns></returns>
		public static XmlWriter Writer(Stream Output)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			return XmlWriter.Create(Output, settings);
		}

		/// <summary>
		/// Don't forget to close the XmlWriter or wrap this line in a using statement
		/// </summary>
		/// <param name="Output">The TextWriter to wrap</param>
		/// <returns></returns>
		public static XmlWriter Writer(TextWriter Output)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			return XmlWriter.Create(Output, settings);
		}

		/// <summary>
		/// Don't forget to close the XmlWriter or wrap this line in a using statement
		/// </summary>
		/// <param name="Output">The StringBuilder to wrap</param>
		/// <returns></returns>
		public static XmlWriter Writer(StringBuilder Output)
		{
			XmlWriterSettings settings = new XmlWriterSettings(); 
			settings.OmitXmlDeclaration = true; 
			settings.Indent = true;
			return XmlWriter.Create(Output, settings);
		}

		/// <summary>
		/// Turns an xml string into a object
		/// </summary>
		/// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
		/// <param name="XmlDoc">An xml string containing the serialized object</param>
		/// <returns>The deserializd object</returns>
		public static T FromXml<T>(string XmlDoc)
		{
			using (StringReader sr = new StringReader(XmlDoc))
				return (T)Serializer(typeof(T)).Deserialize(sr);
		}
		/// <summary>
		/// Turns the xml contents of a file into an object
		/// </summary>
		/// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
		/// <param name="FilePath">The file to read the xml from</param>
		/// <returns>The deserializd object</returns>
		public static T LoadFile<T>(string FilePath)
		{
			if(File.Exists(FilePath))
                using(XmlTextReader xtr = new XmlTextReader(FilePath))
                    return (T)Serializer(typeof(T)).Deserialize(xtr);
				//using (StreamReader s = System.IO.File.OpenText(FilePath))
					//return (T) Serializer(typeof(T)).Deserialize(s);
			return default(T);
		}

		/// <summary>
		/// Save a object as xml into a file. If the file is already there it is deleted then recreated with the xml contents of the supplied object.
		/// </summary>
		/// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
		/// <param name="FilePath">The file to write the xml to</param>
		/// <param name="ToSerialize">The object to serialize</param>
		public static void SaveFile<T>(string FilePath, T ToSerialize)
		{
			if (File.Exists(FilePath))
			{
				File.SetAttributes(FilePath, FileAttributes.Normal);
				File.Delete(FilePath);
			}
            using (XmlTextWriter xtw = new XmlTextWriter(FilePath, Encoding.UTF8))
                Serializer(typeof(T)).Serialize(xtw, ToSerialize);
			//using (StreamWriter sw = File.CreateText(FilePath))
				//Serializer(typeof(T)).Serialize(sw, ToSerialize);
		}

		/// <summary>
		/// Turns an object into an xml string
		/// </summary>
		/// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
		/// <param name="ToSerialize">The object to serialize</param>
		/// <returns></returns>
		public static string ToXml<T>(T ToSerialize, bool RemoveNamespaces)
		{
			StringBuilder sb = new StringBuilder();
			using (XmlWriter xw = Writer(sb))
				Serializer(typeof(T)).Serialize(xw, ToSerialize);
            if (RemoveNamespaces)
            { 
                sb.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", string.Empty);
                sb.Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
            }
			return sb.ToString();
		}

		/// <summary>
		/// Returns a XmlSerializer for the given type. Repeated calls pull the serializer previously used. Serializers are stored internally in a sorted list for quick retrieval.
		/// </summary>
		/// <param name="t">The type to return a XmlSerializer for</param>
		/// <returns>The XmlSerializer for the type</returns>
		public static XmlSerializer Serializer(Type t)
		{
			if (Serializers == null)
				Serializers = new SortedList<int, XmlSerializer>(10);
			XmlSerializer xs = null;
			int hash = t.FullName.GetHashCode();
			if (Serializers.ContainsKey(hash))
				xs = Serializers[hash];
			else
			{
				xs = new XmlSerializer(t);
				Serializers.Add(hash, xs);
			}
			return xs;
		}
	}
}
