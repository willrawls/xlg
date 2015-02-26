using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
// // using MetX.Web;

namespace MetX.Library
{
    /// <summary>Helper functions for dealing with xml strings</summary>
	public class Xml
    {
		/// <summary>The ?xml directive that should be at the top of each file</summary>
		public const string Declaration = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";

		private static SortedList<int, XmlSerializer> m_Serializers;

        /// <summary>Converts an XmlElement into a JSON string and appends it to Target</summary>
        /// <param name="element">The XmlElment to walk and translate to JSON</param>
        /// <param name="target">The StringBuilder to append the JSON string into</param>
        public static void ToJson(XmlElement element, StringBuilder target)
        {
            ToJson(element, target, false, string.Empty, true);
        }

        private static void ToJson(XmlElement element, StringBuilder target, bool containedInArray, string indent, bool wrapWithBrace)
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
            bool commaNeeded = false;
            if (element.HasChildNodes || element.HasAttributes)
            {
                if(containedInArray)
                    target.Append("\n" + indent + "{");
                else
                    target.Append("\n" + indent + "" + (wrapWithBrace ? "{" : string.Empty) + "\"" + element.Name + "\":/*2*/\n" + indent + "\t{");

                if (element.HasAttributes)
                {
                    foreach (XmlAttribute currAttribute in element.Attributes)
                    {
                        if (commaNeeded)
                            target.Append(",\"");
                        else
                            target.Append("\"");
                        target.Append(currAttribute.Name);
                        target.Append("\":\"");
                        target.Append(currAttribute.Value.Replace("\"", "\\\"").Replace(@"\", @"\\"));
                        target.Append("\"");
                        commaNeeded = true;
                    }
                    if (commaNeeded && element.HasChildNodes) { target.Append(",/*8*/"); commaNeeded = false; }
                }
                if (element.HasChildNodes)
                {
                    string lastNodeName = null;
                    if (element.ChildNodes.Count == 1)
                    {
                        if (element.ChildNodes[0] is XmlElement)
                        {
                            if (commaNeeded) { target.Append(",/*3*/"); commaNeeded = false; }
                            ToJson((XmlElement)element.ChildNodes[0], target, false, indent + "\t", false);
                        }
                    }
                    else
                    {
                        string opener = "{";
                        string closer = "}";
                        foreach (XmlElement currChild in element.ChildNodes)
                        {
                            if (currChild is XmlElement)
                            {
                                if (currChild.Name != lastNodeName)
                                {
                                    if (currChild.NextSibling != null)
                                    {
                                        if (lastNodeName != null) target.Append(closer + ",/*1*/");
                                        if (currChild.NextSibling.Name == currChild.Name)
                                        {
                                            opener = "[";
                                            closer = "]";
                                        }
                                    }
                                    else
                                    {
                                        if (lastNodeName != null) target.Append(closer + "/*9*/");
                                    }
                                    target.Append("\n\t" + indent + "\"" + currChild.Name + "\":/*4*/"); // + Opener);
                                    if (opener == "[")
                                        target.Append(opener);
                                    commaNeeded = false;
                                }
                                if (commaNeeded) { target.Append(",/*7*/"); commaNeeded = false;  }
                                ToJson((XmlElement)currChild, target, true, indent + "\t", true);
                                lastNodeName = currChild.Name;
                                commaNeeded = true;
                            }
                        }
                        target.Append(closer);
                    }
                }
                if(wrapWithBrace)
                    target.AppendLine("}");
                target.Append(indent);
            }
            else if (element.Value != null)
            {
                target.Append("\n" + indent + "{\"" + element.Name + "\":\"" + element.Value.Replace("\"", "\\\"").Replace(@"\", @"\\") + "\"} /*5*/");
            }
            else
            {
                target.Append("\n" + indent + "{\"" + element.Name + "\":null /*6*/} ");
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
            StringBuilder sb = new StringBuilder(strIn.AsString());
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
        /// <param name="tagName">The tag to wrap tagValue in</param>
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
        public static string Wrap(string tagName, string tagValue, string tagAttributes)
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
                    return "<" + tagName + ">\r\n" + tagValue + "</" + tagName + ">\r\n";
                // <tagName />
                else
                    return "<" + tagName + "/>\r\n";
            }
            else
            {
                if ((tagValue).Length > 0)
                    // <tagName tagAttributes ... >
                    //    <tagName .../>
                    // </tagName>
                    return "<" + tagName + " " + tagAttributes + ">\r\n" + tagValue + "</" + tagName + ">\r\n";
                // <tagName tagAttributes ... />
                else
                    return "<" + tagName + " " + tagAttributes + "/>";
            }
        }


        /// <summary>Wraps some text in a xml element. NOTE: tagValue may contain any valid text or XML</summary>
        /// <param name="tagName">The tag to wrap tagValue in</param>
        /// <param name="tagValue">The text to wrap as the text node of the wrapping tag</param>
        /// <returns>An xml string with a TagName element wrapping tagValue</returns>
        /// 
        /// <exmaple>
        /// <code>
        /// string x = Wrap("Item", "This is a test");
        /// // x = &amp;amp;amp;amp;amp;lt;Item&amp;amp;amp;amp;amp;gt;This is a test&amp;amp;amp;amp;amp;lt;/Item/&amp;amp;amp;amp;amp;gt;
        /// </code>
        /// </exmaple>
        public static string Wrap(string tagName, string tagValue)
        {
            return Wrap(tagName, tagValue, null);
        }

		/// <summary>
		/// Don't forget to close the XmlWriter or wrap this line in a using statement
		/// </summary>
		/// <param name="output">The stream to wrap</param>
		/// <returns></returns>
		public static XmlWriter Writer(Stream output)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			return XmlWriter.Create(output, settings);
		}

		/// <summary>
		/// Don't forget to close the XmlWriter or wrap this line in a using statement
		/// </summary>
		/// <param name="output">The TextWriter to wrap</param>
		/// <returns></returns>
		public static XmlWriter Writer(TextWriter output)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			return XmlWriter.Create(output, settings);
		}

		/// <summary>
		/// Don't forget to close the XmlWriter or wrap this line in a using statement
		/// </summary>
		/// <param name="output">The StringBuilder to wrap</param>
		/// <returns></returns>
		public static XmlWriter Writer(StringBuilder output)
		{
			XmlWriterSettings settings = new XmlWriterSettings(); 
			settings.OmitXmlDeclaration = true; 
			settings.Indent = true;
			return XmlWriter.Create(output, settings);
		}

		/// <summary>
		/// Turns an xml string into a object
		/// </summary>
		/// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
		/// <param name="xmlDoc">An xml string containing the serialized object</param>
		/// <returns>The deserializd object</returns>
		public static T FromXml<T>(string xmlDoc)
		{
			using (StringReader sr = new StringReader(xmlDoc))
				return (T)Serializer(typeof(T)).Deserialize(sr);
		}
		/// <summary>
		/// Turns the xml contents of a file into an object
		/// </summary>
		/// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
		/// <param name="filePath">The file to read the xml from</param>
		/// <returns>The deserializd object</returns>
		public static T LoadFile<T>(string filePath)
		{
			if(File.Exists(filePath))
                using(XmlTextReader xtr = new XmlTextReader(filePath))
                    return (T)Serializer(typeof(T)).Deserialize(xtr);
				//using (StreamReader s = System.IO.File.OpenText(FilePath))
					//return (T) Serializer(typeof(T)).Deserialize(s);
			return default(T);
		}

		/// <summary>
		/// Save a object as xml into a file. If the file is already there it is deleted then recreated with the xml contents of the supplied object.
		/// </summary>
		/// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
		/// <param name="filePath">The file to write the xml to</param>
		/// <param name="toSerialize">The object to serialize</param>
		public static void SaveFile<T>(string filePath, T toSerialize)
		{
			if (File.Exists(filePath))
			{
				File.SetAttributes(filePath, FileAttributes.Normal);
				File.Delete(filePath);
			}
            using (XmlTextWriter xtw = new XmlTextWriter(filePath, Encoding.UTF8))
                Serializer(typeof(T)).Serialize(xtw, toSerialize);
			//using (StreamWriter sw = File.CreateText(FilePath))
				//Serializer(typeof(T)).Serialize(sw, ToSerialize);
		}

		/// <summary>
		/// Turns an object into an xml string
		/// </summary>
		/// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
		/// <param name="toSerialize">The object to serialize</param>
		/// <returns></returns>
		public static string ToXml<T>(T toSerialize, bool removeNamespaces)
		{
			StringBuilder sb = new StringBuilder();
			using (XmlWriter xw = Writer(sb))
				Serializer(typeof(T)).Serialize(xw, toSerialize);
            if (removeNamespaces)
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
			if (m_Serializers == null)
				m_Serializers = new SortedList<int, XmlSerializer>(10);
			XmlSerializer xs = null;
			int hash = t.FullName.GetHashCode();
			if (m_Serializers.ContainsKey(hash))
				xs = m_Serializers[hash];
			else
			{
				xs = new XmlSerializer(t);
				m_Serializers.Add(hash, xs);
			}
			return xs;
		}
    }
}
