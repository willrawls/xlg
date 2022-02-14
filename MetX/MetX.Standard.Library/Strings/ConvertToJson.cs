using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MetX.Standard.Library.Strings;

public static class ConvertToJson
{
    public static string Xml(string xml)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        return doc.Xml();
    }
    public static string Xml(this XmlDocument xmlDoc)
    {
        var sb = new StringBuilder();
        sb.Append("{ ");
        XmlToJsonNode(sb, xmlDoc.DocumentElement, true);
        sb.Append("}");
        return sb.ToString();
    }

    //  XmlToJsonNode:  Output an XmlElement, possibly as part of a higher array
    private static void XmlToJsonNode(StringBuilder sb, XmlElement node, bool showNodeName)
    {
        if (showNodeName)
            sb.Append("\"" + SafeJson(node.Name) + "\": ");
        sb.Append("{");
        // Build a sorted list of key-value pairs
        //  where   key is case-sensitive nodeName
        //          value is an ArrayList of string or XmlElement
        //  so that we know whether the nodeName is an array or not.
        var childNodeNames = new SortedList<string, object>();

        //  Add in all node attributes
        foreach (XmlAttribute attr in node.Attributes)
            StoreChildNode(childNodeNames, attr.Name, attr.InnerText);

        //  Add in all nodes
        foreach (XmlNode xmlNode in node.ChildNodes)
        {
            if (xmlNode is XmlText)
                StoreChildNode(childNodeNames, "value", xmlNode.InnerText);
            else if (xmlNode is XmlElement)
                StoreChildNode(childNodeNames, xmlNode.Name, xmlNode);
        }

        // Now output all stored info
        foreach (var key in childNodeNames.Keys)
        {
            var alChild = (List<object>)childNodeNames[key];
            if (alChild.Count == 1)
                OutputNode(key, alChild[0], sb, true);
            else
            {
                sb.Append(" \"" + SafeJson(key) + "\": [ ");
                foreach (var child in alChild)
                    OutputNode(key, child, sb, false);
                sb.Remove(sb.Length - 2, 2);
                sb.Append(" ], ");
            }
        }
        sb.Remove(sb.Length - 2, 2);
        sb.Append(" }");
    }

    //  StoreChildNode: Store data associated with each nodeName
    //                  so that we know whether the nodeName is an array or not.
    private static void StoreChildNode(SortedList<string, object> childNodeNames, string nodeName, object nodeValue)
    {
        // Pre-process contraction of XmlElement-s
        if (nodeValue is XmlElement)
        {
            // Convert  <aa></aa> into "aa":null
            //          <aa>xx</aa> into "aa":"xx"
            var cnode = (XmlNode)nodeValue;
            if (cnode.Attributes.Count == 0)
            {
                var children = cnode.ChildNodes;
                if (children.Count == 0)
                    nodeValue = null;
                else if (children.Count == 1 && (children[0] is XmlText))
                    nodeValue = ((XmlText)(children[0])).InnerText;
            }
        }
        // Add nodeValue to ArrayList associated with each nodeName
        // If nodeName doesn't exist then add it
        List<object> ValuesAL;

        if (childNodeNames.ContainsKey(nodeName))
        {
            ValuesAL = (List<object>)childNodeNames[nodeName];
        }
        else
        {
            ValuesAL = new List<object>();
            childNodeNames[nodeName] = ValuesAL;
        }
        ValuesAL.Add(nodeValue);
    }

    private static void OutputNode(string childName, object alChild, StringBuilder sb, bool showNodeName)
    {
        if (alChild == null)
        {
            if (showNodeName)
                sb.Append("\"" + SafeJson(childName) + "\": ");
            sb.Append("null");
        }
        else if (alChild is string)
        {
            if (showNodeName)
                sb.Append("\"" + SafeJson(childName) + "\": ");
            var sChild = (string)alChild;
            sChild = sChild.Trim();
            sb.Append("\"" + SafeJson(sChild) + "\"");
        }
        else
            XmlToJsonNode(sb, (XmlElement)alChild, showNodeName);
        sb.Append(", ");
    }

    // Make a string safe for Json
    private static string SafeJson(string sIn)
    {
        var sbOut = new StringBuilder(sIn.Length);
        foreach (var ch in sIn)
        {
            if (Char.IsControl(ch) || ch == '\'')
            {
                var ich = (int)ch;
                sbOut.Append(@"\u" + ich.ToString("x4"));
                continue;
            }
            else if (ch == '\"' || ch == '\\' || ch == '/')
            {
                sbOut.Append('\\');
            }
            sbOut.Append(ch);
        }
        return sbOut.ToString();
    }
}