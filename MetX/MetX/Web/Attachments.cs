/// <summary>Allows for the retrieval of attachment information and location for a specified item.
/// Use of this requires using the xlgAttachments website. Further development is required.</summary>
/// <author>William M. Rawls</author>
using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace MetX.IO
{
	/// <summary>
	/// Summary description for Attachments.
	/// </summary>
	public class Attachments
	{
		/// <summary>The fixed name of the application</summary>
		public string AppName;
		/// <summary>A unique identifier relative to the AppName for the resource which contains attachments</summary>
		public string ParentID;
		/// <summary>The folder containing all attachments for the AppName(s)</summary>
		public string BaseFolder;
		/// <summary>Initially set to BaseFolder + AppName + "\" + ParentID + "\"</summary>
		public string DirectoryName;

		/// <summary>Default constructor</summary>
		/// <param name="BaseFolder">The folder containing the Application's attachments</param>
		/// <param name="AppName">The Application name (sub folder) containing the resource's attachments</param>
		/// <param name="ParentID">The Unique string associated with the specific set of attachments</param>
		public Attachments(string BaseFolder, string AppName, string ParentID)
		{
			this.BaseFolder = BaseFolder;
			this.AppName = AppName;
			this.ParentID = ParentID;
			DirectoryName = BaseFolder + AppName + @"\" + ParentID + @"\";
		}

		/// <summary>Retrieves an Xml containins the list of attacments for the AppName/ParentID combination. Element name is "Attachments" with each attachment being a child element named "Attachment" with "Filename" and "Link" attributes.</summary>
		public string OuterXml
		{
			get
			{
				XmlDocument xmlDoc = new XmlDocument();
				XmlNode ret = xmlDoc.CreateElement("Attachments");
				ret.Attributes["AppName"].Value = AppName;
				ret.Attributes["ParentID"].Value = ParentID;

				if(Directory.Exists(DirectoryName))
				{
					string[] FileList = Directory.GetFiles(DirectoryName);
					foreach(string CurrFile in FileList)
					{
						XmlNode CurrNode = xmlDoc.CreateElement("Attachment");
						CurrNode.Attributes["Filename"].Value = CurrFile;
						CurrNode.Attributes["Link"].Value = "http://" + ConfigurationManager.AppSettings["AttachServer"] +"/Attach/Attachments/" + AppName + "/" + ParentID + "/" + CurrFile;
						ret.AppendChild(CurrNode);
					}
				}
				return ret.OuterXml;
			}
		}
	}
}
