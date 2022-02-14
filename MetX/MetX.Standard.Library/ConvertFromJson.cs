﻿using System.Xml;

namespace MetX.Standard.Library;

public class ConvertFromJson
{
    public static XmlDocument ToXml(string json)
    {
        var returnXmlDoc = new XmlDocument();
        returnXmlDoc.LoadXml("<Document />");
        var rootNode = returnXmlDoc.SelectSingleNode("Document");
        var appendToNode = rootNode;

        var arrElements = json.Split('\r');
        foreach (var element in arrElements)
        {
            var processElement = element.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            if ((processElement.IndexOf("}") > -1 || processElement.IndexOf("]") > -1) && appendToNode != rootNode)
            {
                appendToNode = appendToNode.ParentNode;
            }
            else
            {
                XmlNode newNode;
                if (processElement.IndexOf("[") > -1)
                {
                    processElement = processElement.Replace(":", "").Replace("[", "").Replace("\"", "").Trim();
                    newNode = returnXmlDoc.CreateElement(processElement);
                    appendToNode.AppendChild(newNode);
                    appendToNode = newNode;
                }
                else if (processElement.IndexOf("{") > -1 && processElement.IndexOf(":") > -1)
                {
                    processElement = processElement.Replace(":", "").Replace("{", "").Replace("\"", "").Trim();
                    newNode = returnXmlDoc.CreateElement(processElement);
                    appendToNode.AppendChild(newNode);
                    appendToNode = newNode;
                }
                else
                {
                    if (processElement.IndexOf(":") > -1)
                    {
                        var arrElementData = processElement.Replace(": \"", ":").Replace("\",", "").Replace("\"", "").Split(':');
                        newNode = returnXmlDoc.CreateElement(arrElementData[0]);
                        for (var i = 1; i < arrElementData.Length; i++)
                        {
                            newNode.InnerText += arrElementData[i];
                        }

                        appendToNode.AppendChild(newNode);
                    }
                }
            }
        }

        return returnXmlDoc;
    }   
}